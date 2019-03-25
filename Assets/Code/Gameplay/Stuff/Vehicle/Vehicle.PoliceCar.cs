using System;
using UnityEngine;
using Random = System.Random;

public partial class Vehicle
{
    public class PoliceCar : CarBehaviour
    {
        private enum State
        {
            Idle,
            BackAttack,
            SideAttack,
            ChangeLine,
        }

        private const float StartSideSpeed = 1;
        private const float SideMoveAngle = 15 ;
        private const float BackAttackMultipler = 2;
            
        private State _state;
        private State _attack;
        private int _targetLine;
        private float _targetX;
        private bool _sideward;
        private float _sideSpeed;
        private float _scanRadius;
        private float _speedZ;
        private Transform _vehTransform;

        private readonly float _acceleration;
        private readonly float _rotationSpeed;
        private float _middle;
        private float _sideSign;
        private bool _startCountdown;
        private float _timeLeft, _time;
        public override string Tag => "Police";

        public PoliceCar(float speed, float sideSpeed, float scanRadius) : base(true)
        {
            Avatar.OnLanding += AvatarHasLanded;
            _speedZ = speed;
            Velocity.z = _speedZ;
            _sideSpeed = sideSpeed;
            _scanRadius = scanRadius;
            _acceleration = (StartSideSpeed + _sideSpeed)*(-StartSideSpeed + _sideSpeed) / Road.LineWidth ;
            _rotationSpeed = SideMoveAngle*(StartSideSpeed + _sideSpeed)/ Road.LineWidth;
            //todo Что-то адекватное
            _time = 1.5f;
        }

        public PoliceCar(PoliceDescriptor desc) :
            this(desc.Velocity, desc.SideSpeed, desc.ScanRadius){}

        public override void SetVehicle(Vehicle vehicle)
        {
            base.SetVehicle(vehicle);
            _vehTransform = Vehicle.transform;
        }

        public override void TurnOn()
        {
            base.TurnOn();
            _state = State.Idle;
            _attack = State.Idle;
            _timeLeft = _time;
            Rbody.velocity = Velocity;
        }

        private bool Scan(float radius, out bool atSideNotBehind)
        {
            var avaFrontZ = Avatar.Vehicle.FrontPosZ();
            var avaBackZ = Avatar.Vehicle.BackPosZ();
            var backZ = Vehicle.BackPosZ();
            var frontZ = Vehicle.FrontPosZ();
            if (Avatar.OnCar && avaFrontZ > backZ - radius && avaBackZ < frontZ)
            {
                var avaP = Avatar.Vehicle.CenterZ();
                var p = Vehicle.CenterZ();
                atSideNotBehind = Math.Abs(p - avaP) < Vehicle.Length();
                return true;
            }
            atSideNotBehind = false;
            return false;
        }

        private int AvatarLine()
        {
            return Road.LineNumFromPosX(Avatar.transform.position.x);
        }

        private bool IsPathFree(int targetLine)
        {
            var x = _vehTransform.position.x;
            var avaX = Avatar.transform.position.x;
            var ownLine = Road.LineNumFromPosX(x);
            var offset = (targetLine > ownLine ? .5f : -.5f) * Road.LineWidth;
            var A = new Vector3(x + offset,Vehicle.BlockHeight()/2,Vehicle.CenterZ());
            var B = new Vector3(avaX - offset,Vehicle.BlockHeight()/2,A.z);
            var res = Physics.Linecast(A, B);
            if (res) return false;
            
            A.z = Vehicle.FrontPosZ();
            B.z = A.z;
            res = Physics.Linecast(A,B);
            if (res) return false;
            
            A.z = Vehicle.BackPosZ();
            B.z = A.z;
            return !Physics.Linecast(A, B);
        }
        
        public override void Affect()
        {
            if (!On) return;
            if (_startCountdown)
                _timeLeft -= Time.fixedDeltaTime;
            switch (_state)
            {
                case State.Idle:
                    if (_timeLeft < 0 && Avatar.OnCar)
                        _state = _attack;
                    break;
           
                case State.ChangeLine:
                    var x = _vehTransform.position.x;
                    if (Math.Abs(_targetX-x)<0.05f)
                    {
                        _vehTransform.rotation = Quaternion.identity;
                        Velocity.x = 0;
                        _state = State.Idle;
                    }
                    else
                    {
                        int sign = x < _middle ? 1 : -1;
                        Velocity.x += sign * _acceleration * Time.fixedDeltaTime;
                        _vehTransform.Rotate(0,sign* _rotationSpeed * Time.fixedDeltaTime,0);
                    }

                    Rbody.velocity = Velocity;
                    break;
                
                case State.BackAttack:
                    Velocity.z -= _speedZ * Time.fixedDeltaTime * BackAttackMultipler;
                    Rbody.velocity = Velocity;
                    break;
                
                case State.SideAttack:
                    if (Mathf.Abs(Velocity.x) < _sideSpeed)
                    {
                        Velocity.x += _sideSign * _acceleration*2 * Time.fixedDeltaTime;
                        _vehTransform.Rotate(0,_sideSign * _rotationSpeed*2* Time.fixedDeltaTime,0);
                    }
                    Rbody.velocity = Velocity;
                    break;
            }
           
        }

        private void AvatarHasLanded()
        {
            bool sNb;
            if(Avatar.Vehicle==Vehicle) return;
            if (Scan(_scanRadius,out sNb))
            {
                TurnOnFlasher();
                int ownLine = Road.LineNumFromPosX(Vehicle.transform.position.x);
                int targetLine = AvatarLine();
                if (sNb)
                {
                    if (IsPathFree(targetLine))
                    {
                        _startCountdown = true;
                        _attack = State.SideAttack;
                        _sideSign = ownLine < targetLine ? 1 : -1;
                        _targetX = Road.LinePosXFromNum(targetLine);
                    }
                    else
                        _attack = State.Idle;
                }
                else
                {
                    if (ownLine == targetLine || IsPathFree(ownLine < targetLine ? targetLine + 1 : targetLine - 1))
                    {
                        _startCountdown = true;
                        _attack = State.BackAttack;
                        if (ownLine == targetLine) return;
                        _state = State.ChangeLine;
                        _sideSign = ownLine < targetLine ? 1 : -1;
                        _targetX = Road.LinePosXFromNum(targetLine);
                        _middle = (_vehTransform.position.x + _targetX) / 2;
                    }
                    else
                        _attack = State.Idle;
                }
            }
            else
            {
                _startCountdown = false;
                _timeLeft = _time;
                _state = _attack = State.Idle;
            }

        }

        public override void HandleAdopting()
        {
            base.HandleAdopting();
            _timeLeft = .3f; //SessionSettings.SS.PowerTime;
            _attack = State.BackAttack;
        }

        //todo
        private void TurnOnFlasher()
        {
            
        }
        
        public override void Destruct()
        {
            base.Destruct();
            _vehTransform = null;
            Avatar.OnLanding -= AvatarHasLanded;
        }
    }
}