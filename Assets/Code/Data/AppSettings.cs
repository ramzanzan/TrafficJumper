using System;
using System.Linq;
using UnityEngine;

[Serializable]
public sealed class AppSettings
{
    private static AppSettings _instance;

    public static AppSettings GetInstance()
    {
        //todo проверка, первая ли загрузка игры, и нужно ли загружать или создавать прогресс
        return _instance ?? (_instance = new AppSettings());
    }

    private AppSettings()
    {
        _floatingJoystick = GUIController.GetInstance().Playing.GetComponentInChildren<FloatingJoystick>(true);
        _fixedJoystick = GUIController.GetInstance().Playing.GetComponentInChildren<Joystick>(true);
        SetJoystick(false);
//        SetAvatar("a1");
    }

    private void SaveChanges()
    {
        //todo
    }

    private readonly FloatingJoystick _floatingJoystick;
    private readonly Joystick _fixedJoystick;
    public readonly Vector2 DefaultJoystickPos = new Vector2(220,70);
    
    public Vector2 FixedJoystickPos;
    public bool JoystickFixedNotFloating;
    public string AvatarSkinName;

    public void SetAvatar(string name)
    {
        var skin = SkinsContainer.GetInstance().Skins.FirstOrDefault(sp => name == sp.Skin.name);
        if(skin==null) throw new ArgumentException("bad "+name);
        AvatarSkinName = name;
        var ava = AvatarController.GetInstance().Avatar.gameObject;
        ava.GetComponent<MeshFilter>().mesh = skin.Skin.GetComponent<MeshFilter>().mesh;
        ava.GetComponent<MeshRenderer>().material = skin.Skin.GetComponent<MeshRenderer>().material;
    }
    
    public void SetJoystick(bool type)
    {
        JoystickFixedNotFloating = type;
        _fixedJoystick.gameObject.SetActive(type);
        _floatingJoystick.gameObject.SetActive(!type);
        AvatarController.GetInstance().Joystick = type ? (JoystickBase)_fixedJoystick : _floatingJoystick;
    }

    public void SetJoystick(Vector2 pos)
    {
        _fixedJoystick.transform.position = pos;
        _fixedJoystick.Rezet();
        SetJoystick(true);
    }
    
}
