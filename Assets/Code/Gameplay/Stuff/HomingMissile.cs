using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    private static PlayerAvatar _avatar;
    private static Transform _avaTransform;
    public static GOPool SelfPool;
    private const float LifeTime = 10f;
    private float _rotationVelocity;
    private float _absVelocity;
    private float _sqrLimit;
    private Rigidbody _rbody;
    private Vector3 _velocity;
    private bool _homing;
    private float _timer;

    public static void SetAvatar(PlayerAvatar avatar)
    {
        _avatar = avatar;
        _avaTransform = avatar.transform;
    }
    
    private void Start()
    {
        _rbody = GetComponent<Rigidbody>();
    }

    public void Init(float absVelocity, float rotVelocity, float limit)
    {
        _rotationVelocity = rotVelocity;
        _absVelocity = absVelocity;
        _sqrLimit = limit*limit;
        _homing = true;
        _timer = LifeTime;
    }

    
    private void FixedUpdate()
    {
        if (_homing)
        {
            Homing();
            _homing = _sqrLimit < (transform.position - _avaTransform.position).sqrMagnitude;
        }

        if (_timer > 0)
            _timer -= Time.fixedDeltaTime;
        else
            Destroy();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Avatar"))
            Explode();
    }

    private void Homing()
    {
        var curDir = transform.forward.MyVec3ToVec2();
        var aimDir = _avaTransform.position.MyVec3ToVec2() - transform.position.MyVec3ToVec2();
        var angDelta = Vector2.SignedAngle(curDir, aimDir);
        var rotation = _rotationVelocity * Time.deltaTime;
        if (Mathf.Abs(angDelta) < rotation)
        {
            transform.Rotate(0,angDelta,0);
        }
        else
        {
            transform.Rotate(0, -Mathf.Sign(angDelta)*rotation,0);
        }
        _rbody.velocity = transform.forward.normalized * _absVelocity;
    }

    //todo
    private void Explode()
    {
        //boom
        _rbody.velocity = Vector3.zero;
    }

    private void Destroy()
    {
        SelfPool.Push(gameObject);
    }
}
