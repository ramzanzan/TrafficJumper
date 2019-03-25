using System;
using Code.Gameplay.Descriptors;
using UnityEngine;
using Random = UnityEngine.Random;

public partial class Vehicle
{
    public class WobblingCar : CarBehaviour
    {
        private const float MaxRotatingAngle = 25;

        private bool _mightWobble;
        private bool _currentSideRnL;
        private readonly bool _onPointerContact;
        private readonly float _freq;
        private float _amplitude;
        private readonly float _halfPeriodTime;
        private readonly float _halfPeriodLength;
        private float _timer;
        private int _halfPeriodsCount;
        private float _dirRnL;
        private float _minCarVelocity;

        public override string Tag => "Wobbling";

        public WobblingCar(float speedZ, float halfPeriodLength, float minCarVel, bool onPointerContact = false)
            : base(true)
        {
            Velocity.z = speedZ;
            _onPointerContact = onPointerContact;
            _halfPeriodLength = halfPeriodLength;
            _halfPeriodTime = halfPeriodLength / speedZ;
            _freq = Mathf.PI /_halfPeriodTime;
            _amplitude = Road.LineWidth*_freq/2;    //подстройка для интегральной суммы -> перемещения
            _minCarVelocity = minCarVel;
        }

        public WobblingCar(WobblingCarDescriptor desc):
            this(desc.Velocity, desc.HalfPeriodLength, desc.MinCarVelocity, desc.OnPointerContact){}
        
        /// <summary>
        /// Возвращает true если удалось выбрать правую или левую сторону.
        /// Имеет побочный эффект - _currentSideRnL - устанавливает текущую сторону.
        /// И _dirRnL - направление движения.
        /// </summary>
        /// <returns></returns>
        private bool ChooseSide()
        {
            bool b = Random.Range(0, 1f) > .5f;
            _dirRnL = b ? 1 : -1;
            if (IsSideFree(b) && IsSideExist(b))
            {
                _currentSideRnL = !b;
                return true;
            }
            if (IsSideFree(!b) && IsSideExist(!b))
            {
                _dirRnL *= -1;
                _currentSideRnL = b;
                return true;
            }
            return false;
        }

        private bool IsSideExist(bool rightNotLeft)
        {
            if (rightNotLeft)
                return Road.PositionX + Road.LineWidth*Road.LinesCount
                       > Vehicle.transform.position.x +Road.LineWidth;
            
            return Road.PositionX < Vehicle.transform.position.x - Road.LineWidth;
        }

        private bool IsSideFree(bool rightNotLeft)
        {
            Vector3 A;
            float m = rightNotLeft ? 1 : -1;
            float x;
            try
            {
                x = Road.LinePosXFromNum(Road.LineNumFromPosX(Vehicle.transform.position.x));
            }
            catch
            {
                return false;
            }
            A.x = x + m * Road.LineWidth/2;
            A.y = Vehicle.BlockHeight()/2;
            A.z = Vehicle.BackPosZ()-Vehicle.BlockLength()/2;
            Vector3 B = A;
            B.x += m * Road.LineWidth;
            B.z += Vehicle.Length() + _halfPeriodLength - _minCarVelocity * _halfPeriodTime;
            return !Physics.Linecast(A, B, LayerMask.GetMask("Default"));
        }

        public override void Affect()
        {
            if (!On) return;
            
            if (_mightWobble)
            {
                if (Mathf.FloorToInt(_timer / _halfPeriodTime) > _halfPeriodsCount)
                {
                    _halfPeriodsCount++;
                    _currentSideRnL=!_currentSideRnL;
                    _mightWobble = IsSideFree(!_currentSideRnL);
                    //коррекция амплитуды для попадания в центр полосы следующей её смене
                    var side = _currentSideRnL ? -1 : 1;
                    var posX = Vehicle.transform.position.x;
                    var nextLine = Road.LineNumFromPosX(posX) + side;
                    _amplitude = side * (Road.LinePosXFromNum(nextLine) - posX) * _freq / 2;
                    
                    if (!_mightWobble)
                    {
                        _dirRnL = 0;
                        Rbody.rotation = Quaternion.identity;
                    }
                }

                float sin = Mathf.Sin(_timer * _freq);
                Rbody.rotation = Quaternion.Euler(0,_dirRnL*sin*MaxRotatingAngle,0);
                _timer += Time.fixedDeltaTime;
                Velocity.x = _dirRnL * _amplitude * sin;
            }
            else
            {
                Velocity.x = 0;
            }

            Rbody.velocity = Velocity;

        }

        public override void HandleAdopting()
        {
            if(!_mightWobble)
                _mightWobble = ChooseSide();
        }

        private void Wobble()
        {
            if(!_mightWobble && _onPointerContact)
                _mightWobble = ChooseSide();

        }

        public override void HandleTriggerEnter(Collider other)
        {
            base.HandleTriggerEnter(other);
            if(other.gameObject.CompareTag("PointerToucher"))
                Wobble();
        }
    }
}