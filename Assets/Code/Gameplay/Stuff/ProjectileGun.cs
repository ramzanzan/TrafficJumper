using UnityEngine;

public class ProjectileGun : MonoBehaviour
{
    private const float IndicatorTime = 2;
    private static PlayerAvatar _avatar;
    private static Transform _avaTransform;
    public static GOPool ProjectilePool;
    private float _rotationSpeed;
    private float _reloadTime, _timer;
    private float _shellVelocity;
    private bool _indicator, _isBroken;
    private Vector3 _dir;

    public void Init(float reloadTime, float shellSpeed, float rotationSpeed)
    {
        _reloadTime = reloadTime;
        _timer = reloadTime;
        _shellVelocity = shellSpeed;
        _rotationSpeed = rotationSpeed;
        _isBroken = false;
        On = false;
    }
    
    public static void SetAvatar(PlayerAvatar avatar)
    {
        _avatar = avatar;
        _avaTransform = _avatar.transform;
        Projectile.Avatar = _avatar;
    }

    public bool On;

    private Vector2 _aimDir = new Vector2(0,0);
    
    private void FixedUpdate()
    {
        if (On && !_isBroken)
        {
            var curDir = transform.forward.MyVec3ToVec2();
            _aimDir = _avaTransform.position.MyVec3ToVec2() - transform.position.MyVec3ToVec2();
            var angDelta = Vector2.SignedAngle(curDir, _aimDir);
            var rotation = _rotationSpeed * Time.deltaTime;
            if (Mathf.Abs(angDelta) < rotation)
            {
                transform.Rotate(0,angDelta,0);
                _dir = transform.forward.normalized;
                if (_timer < 0 && _avatar.OnCar)
                {
                    Fire();
                    _timer = _reloadTime;
                    TurnOffIndicator();
                }
            }
            else
            {
                transform.Rotate(0, -Mathf.Sign(angDelta)*rotation,0);
            }
        }

        if (_timer>=0)
            _timer -= Time.fixedDeltaTime;
        
        if (!_indicator && _timer < IndicatorTime)
        {
            TurnOnIndicator();
        }

    }

    private void Fire()
    {
        var go = ProjectilePool.Pop();
        go.transform.rotation = transform.rotation;
        //todo
        go.transform.position = transform.position + transform.rotation*new Vector3(0,0.2f,.32f); //Vector3(0,.82f,.6f);
        var resVelocity = _dir * _shellVelocity;
        resVelocity.z += _avatar.Vehicle.Velocity.z; 
        go.GetComponent<Rigidbody>().velocity = resVelocity;
    }

    //todo
    private void TurnOnIndicator()
    {
        _indicator = true;
    }
    
    private void TurnOffIndicator()
    {
        _indicator = false;
    }

    public void Break()
    {
        _isBroken = true;
    }
}
