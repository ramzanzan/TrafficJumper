using System;
using Code.Gameplay.Descriptors;
using UnityEngine;
using Random = UnityEngine.Random;

public partial class Vehicle
{
    public class SlidingCar : CarBehaviour
    {
        public override string Tag => "Sliding";
        
        public SlidingCar(float speedZ, float minAngle, float maxAngle,
            float minSideSpeed, float maxSideSpeed, bool byTouch=false) : base(true)
        {
            if(minSideSpeed<=0 || maxSideSpeed>3) throw new ArgumentOutOfRangeException();
            if(minAngle<=0 || maxAngle>90) throw new ArgumentOutOfRangeException();
            _maxAngle = maxAngle;
            _minAngle = minAngle;
            Velocity.z = speedZ;
            _minSideSpeed = minSideSpeed;
            _maxSideSpeed = maxSideSpeed;
            _state = !byTouch ? State.OnOil : State.StraightLineMotion;
        }
        
        public SlidingCar(SlidingCarDescriptor desc) : 
            this(desc.Velocity,desc.MinAngle,desc.MaxAngle,desc.MinSideSpeed,desc.MaxSideSpeed,desc.ByTouch)
        {}

        public override void SetVehicle(Vehicle vehicle)
        {
            base.SetVehicle(vehicle);
            _transform = Vehicle.transform;
        }

        private enum State
        {
            StraightLineMotion,
            OnOil,
            AfterOil,
        }

        private State _state;
        private const float DeltaAngle = 3f;
        private const float DeltaVelocity = .1f;
        private const float DeltaMovement = .1f;
        private const float Deg2Rad = Mathf.Deg2Rad;
        private const float AngularDrag = .1f;
        private const float AfterOilAngVel = 15f;
        private float _maxAngle;
        private float _minAngle;
        private Vector3 _angVelocity;
        private float _minSideSpeed;
        private float _maxSideSpeed;
        private Transform _transform;
        private float _posTarget;
        private float _acceleration;
        private float _posMiddle;
        private float _rotTarget;
        private float _rotAcceleration;
        private float _rotMiddle;
        private bool _c1, _c2, _ready;
        
        public override void Affect()
        {
            if (!On || _state==State.StraightLineMotion) return;
            
            var pos = _transform.position.x;
            var ang = _transform.rotation.eulerAngles.y;
            ang = ang < 180 ? ang : ang - 360;
            var fdt = Time.fixedDeltaTime;
            
            switch (_state)
            {
                case State.OnOil:
                    if (!(_c1 || _c2) && Math.Abs(Velocity.x) < DeltaVelocity)
                    {
                        _posTarget = GetPosTarget(ang, pos);
                        _rotTarget = GetRotTarget(ang);
                        float movDelta = Math.Abs(_posTarget - pos);
                        float time = movDelta / Random.Range(_minSideSpeed,_maxSideSpeed);
                        _posMiddle = (pos + _posTarget) / 2;
                        _acceleration = Math.Sign(ang) * movDelta / (time * time);
                        float rotDelta = Math.Abs(ang) + Math.Abs(_rotTarget);
                        _rotAcceleration = -Math.Sign(ang) * rotDelta / (time * time);
                        _rotMiddle = (_rotTarget + ang) / 2;
                        _c1 = _c2 = true;
                        Velocity.x = 0;
                        _angVelocity.y = 0;
                    }

                    if (_c1 && Math.Abs(_rotMiddle - ang) < DeltaAngle)
                    {
                        _rotAcceleration *= -1;
                        _c1 = false;
                    }

                    if (_c2 && Math.Abs(_posMiddle - pos) < DeltaMovement)
                    {
                        _acceleration *= -1;
                        _c2 = false;
                    }

                    Velocity.x += _acceleration * fdt * 1.05f;
                    _angVelocity.y += _rotAcceleration * fdt * 1.05f;
                    break;
                
                case State.AfterOil:
                    if (Math.Abs(ang) < DeltaAngle)
                    {
                        _angVelocity.y = 0;
                        _state = State.StraightLineMotion;
                        Velocity.x = 0;
                    }
                    else
                    {
                        _angVelocity.y = -Math.Sign(ang) * AfterOilAngVel;
                        Velocity.x = _transform.forward.normalized.x * Velocity.z;

                    }
                    break;
            }
            
            Rbody.velocity = Velocity;
            Rbody.angularVelocity = _angVelocity * Deg2Rad;

        }

        private float GetRotTarget(float curRot)
        {
            return curRot > 0 ? Random.Range(-_maxAngle, -_minAngle) : Random.Range(_minAngle, _maxAngle);
        }
        
        private static float GetPosTarget(float curRot, float curPos)
        {
            var lLine = Road.LinePosXFromNum(0);
            var rLine = Road.LinePosXFromNum(Road.LinesCount - 1);
            return curRot > 0 ? 
                Random.Range(curPos + Road.LineWidth, rLine) : 
                Random.Range(lLine, curPos - Road.LineWidth);
        }
        
        public override void TurnOn()
        {
            base.TurnOn();
            if (_state==State.OnOil)
            {
                _transform.rotation = 
                    Quaternion.Euler(0,_transform.position.x > Road.MiddlePosition ? -_minAngle : _minAngle, 0);
            }
            Rbody.velocity = Velocity;
        }
        
        public override void HandleTriggerEnter(Collider other)
        {
            base.HandleTriggerEnter(other);
            if (other.gameObject.CompareTag("OilPuddle") && _state!=State.OnOil)
            {
                _state = State.OnOil;
                _transform.rotation = 
                    Quaternion.Euler(0,_transform.position.x > Road.MiddlePosition ? -1 : 1, 0);
            }
            if (other.gameObject.CompareTag("OilEnd"))
            {
                _state = State.AfterOil;
            }

        }

        public override void TurnOff()
        {
            if (_state != State.OnOil)
            {
                base.TurnOff();
                Rbody.angularDrag = AngularDrag;
            }
            else
                On = false;
        }

        public override void Destruct()
        {
            base.Destruct();
            _transform = null;
        }
    }
}