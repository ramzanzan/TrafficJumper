public partial class Vehicle
{
    public class MilitaryCar : CarBehaviour
    {
        private float _radius;
        private float _reloadTime;
        private float _rotSpeed;
        private float _shellSpeed;
        private ProjectileGun _gun;
        private bool _isGunBroken;
        public override string Tag => "Military";
        
        public MilitaryCar(float velocity,float radius, float reloadTime, float rotSpeed, float shellSpeed) : 
            base(true)
        {
            Velocity.z = velocity;
            _radius = radius;
            _reloadTime = reloadTime;
            _rotSpeed = rotSpeed;
            _shellSpeed = shellSpeed;
        }
        
        public MilitaryCar(MilitaryDescriptor desc) : 
            this(desc.Velocity,desc.Radius,desc.ReloadTime,desc.RotationSpeed,desc.ShellSpeed)
        {}

        public override void TurnOn()
        {
            base.TurnOn();
            Rbody.velocity = Velocity;
            _gun = Vehicle.GetComponentInChildren<ProjectileGun>();
            _gun.Init(_reloadTime,_shellSpeed,_rotSpeed);
        }

        private bool Scan()
        {
            var z = Vehicle.transform.position.z;
            var avaZ = Avatar.transform.position.z;
            return z + _radius > avaZ && z - _radius < avaZ;
        }

        public override void Affect()
        {
            if(!_isGunBroken)
                _gun.On = Scan();
        }

        public override void HandleAdopting()
        {
            base.HandleAdopting();
            _gun.Break();
            _isGunBroken = true;
        }

        public override bool HandleEating()
        {
            return base.HandleEating();
        }
    }
}