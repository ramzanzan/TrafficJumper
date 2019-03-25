using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatar : MonoBehaviour {
    
    private float _maxJumpDistance;
    private float _minJumpDistance;
    private float _horizontalVelocity;
    private float _maxJumpHeigth;
    private Rigidbody _rbody;
    private ConstantForce _cforce;
    private bool _inJump;
    public Vehicle Vehicle;
    public float PowerTime { get; private set; }
    public bool OnCar { get; private set; }
    public bool IsAlive { get; private set; }
    public bool IsControllable { get; private set; }
    public Action OnDeath;
    public Action OnLanding;
    public Action OnSuccsefulEating;


    public void Init(float maxJumpDist, float minJumpDist, float horizVelocity, float powerTime, float maxJumpHeight)
    {
        _maxJumpDistance = maxJumpDist;
        _minJumpDistance = minJumpDist;
        _horizontalVelocity = horizVelocity;
        PowerTime = powerTime;
        _maxJumpHeigth = maxJumpHeight;
        IsControllable = true;
    }
    
    private void Start()
    {
        _rbody = GetComponent<Rigidbody>();
        _cforce = GetComponent<ConstantForce>();
    }

    private void OnCollisionEnter(Collision other)
    {
        var go = other.gameObject;
        if (!IsAlive) return;
        switch (go.tag)
        {
            case "Car":
                Vehicle = go.GetComponent<Vehicle>();
                if (TryLand())
                {
                    _rbody.isKinematic = true;
                    IsControllable = true;
                    Vehicle.Adopt();
                    //get local port ....
                    Centring();
                    transform.rotation= Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0));
                    _rbody.constraints = RigidbodyConstraints.FreezePosition;
                    OnLanding?.Invoke();
                }
                else
                {
                    Vehicle = null;
                }
                break;
            case "Meteor":
                //todo
                Die();
                break;
            case "Projectile":
                //todo отбрасывание, анимацию связывание
                Die();
                break;
            case "Missile":
                break;
            case "Wall":
            case "Road":
                Die();
                break;
            default:
                throw new ArgumentException(go.tag);
                    
        }
    }

    //todo анимация-состояния смерти
    public virtual void Die()
    {
        BreakDown();
        IsControllable = false;
        IsAlive = false;
        OnDeath.Invoke(); 
    }

    public void PushDown(Vector3 velocity)
    {
        Die(); 
        _rbody.velocity = velocity;
    }
    
    private bool TryLand()
    {
        OnCar = Vehicle.DoesComprisePoint();
        return OnCar;
    }
    
    //todo redo
    //ret time needed to centring?
    private void Centring()
    {
        //forbid control
        transform.localPosition = Vehicle.GetLocalPort();
        //ret control
    }

    //todo animashku
    public bool Eat()
    {
        var res = Vehicle != null && Vehicle.TryEat();
        if(res && OnSuccsefulEating!=null) OnSuccsefulEating.Invoke();
        return res;
    }

    public void BreakDown()
    {
        if (Vehicle != null)
        {
            Vehicle.BreakeDown();
            Vehicle = null;
            transform.parent = null;
            OnCar = false;
        }
        _rbody.constraints = RigidbodyConstraints.None;
        _rbody.isKinematic = false;
    }
    
    public void Jump(float power, Vector2 dir)
    {
        var vehVelocityZ = Vehicle == null ? 0 : Vehicle.Velocity.z;
        BreakDown();
        
        if(_minJumpDistance/_maxJumpDistance > power) power = _minJumpDistance/_maxJumpDistance;
        IsControllable = false;
        var time = power * _maxJumpDistance / _horizontalVelocity;
        var vertVelocity = _maxJumpHeigth * 4 / time;
        var gAcceleration = vertVelocity * 2 / time;

        _cforce.force=new Vector3(0,-gAcceleration,0);
        dir = dir.normalized;
        var vel = new Vector3(dir.x * _horizontalVelocity, vertVelocity, dir.y * _horizontalVelocity + vehVelocityZ);
        _rbody.velocity = vel;

//        _inJump = true;
    }

    public void Rezet()
    {
        BreakDown();
        IsAlive = true;
        IsControllable = true;
        if (_rbody != null)
        {
            _rbody.angularVelocity = Vector3.zero;
            _rbody.velocity = Vector3.zero;
            _rbody.useGravity = false;
        }
        transform.rotation=Quaternion.identity;
    }

    private void FixedUpdate()
    {
//        if(OnCar && _rbody.velocity.y<0)
            
    }
}
