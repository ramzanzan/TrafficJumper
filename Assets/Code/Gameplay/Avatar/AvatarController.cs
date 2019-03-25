using System;
using UnityEngine;

public class AvatarController : MonoBehaviour
{
    private static AvatarController _instance;

    public static AvatarController GetInstance()
    {
        if(_instance==null) throw new Exception("Сlasses initialization order have violated");
        return _instance;
    }
    
    public JoystickBase Joystick
    {
        set
        {
            if (_joystick != null)
            {
                _joystick.OnTick = null;
                _joystick.OnHolding = null;
                _joystick.OnRelease = null;
                _joystick.OnDoubleTap = null;
                Avatar.OnDeath -= _joystick.Rezet;
            }
            _joystick = value;
            _joystick.OnTick = Tick;
            _joystick.OnHolding = Holding;
            _joystick.OnRelease = Release;
            _joystick.OnDoubleTap = DoubleTap;
            Avatar.OnDeath += _joystick.Rezet;
        }
        get { return _joystick;}
    }    
    public PointerController Pointer;
    public PlayerAvatar Avatar;

    private JoystickBase _joystick;
    private Vector3 _avatarRotation;
    private bool _needUpdateAvatarRotation;
    private Transform _avatarTransform;
    
    private void Start()
    {
        if(_instance!=null) throw new Exception("Second singleton");
        _instance = this;
        
        Pointer.transform.SetParent(Avatar.transform);
        Pointer.transform.localPosition = Vector3.zero;
        //todo redo
        Pointer.transform.localPosition= new Vector3(0,-.3f,0);
       _avatarTransform = Avatar.transform;
        Avatar.OnDeath += Pointer.Reset;

    }

    private void Tick(float time)
    {
        if (Avatar.IsControllable)
        {
            float power = time <= Avatar.PowerTime ? time / Avatar.PowerTime: 1;
            Pointer.SetLoad(power);
        }
    }

    private void Holding(Vector2 rotating, float time)
    {
        if (Avatar.IsControllable)
        {
            Pointer.gameObject.SetActive(true);
            var power = time <= Avatar.PowerTime ? time / Avatar.PowerTime : 1;
            _avatarRotation= new Vector3(0, Mathf.Sign(rotating.x) * Vector2.Angle(Vector2.up, rotating), 0);
            _needUpdateAvatarRotation = true;
            Pointer.SetLoad(power);
        }
    }

    private void Release(Vector2 dir, float time)
    {
        if (Avatar.IsControllable)
        {
            Pointer.gameObject.SetActive(false);
            var power = time <= Avatar.PowerTime ? time / Avatar.PowerTime : 1;
            Avatar.Jump(power,dir);
        }       
    }

    private void DoubleTap()
    {
        if (Avatar.IsControllable)
        {
            Avatar.Eat();
        }
    }

    private void Update()
    {
        if (_needUpdateAvatarRotation)
        {
            _avatarTransform.eulerAngles = _avatarRotation;
            _needUpdateAvatarRotation = false;
        }
    }
}
