using UnityEngine;

public partial class Vehicle
{
    public class AutoMeteorCar : CarBehaviour
    {
        public static MeteorController MeteorCtrl;
        private Vector3 _increasedVelocity;
        private float _distance;
        private float _time;
        public override string Tag => "SoloMeteor";
        
        public AutoMeteorCar(float vel, float addVel, float distance, float time) : base(false)
        {
            Velocity.z = vel;
            _increasedVelocity.z = vel + addVel;
            _distance = distance;
            _time = time;
        }
        
        public AutoMeteorCar(AutoMeteorCarDescriptor desc) 
            : this(desc.Velocity, desc.AddVelocity, desc.Distance, desc.Time){}

        public override void TurnOn()
        {
            base.TurnOn();
            Rbody.velocity = Velocity;
        }

        public override void HandleAdopting()
        {
            var to = new Vector2(Vehicle.transform.position.x,Vehicle.transform.position.z+_distance); 
            MeteorCtrl.CreateAndFire(to, _time);
            Rbody.velocity = _increasedVelocity;

        }

    }
}
