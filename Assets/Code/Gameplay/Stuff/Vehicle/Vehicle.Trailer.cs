using UnityEngine;

public partial class Vehicle
{
    public class Trailer : CarBehaviour
    {
        public override string Tag => "Trailer";

        public Trailer(float velocity) : base(false)
        {
            Velocity.z = velocity;
        }
        
        public Trailer(TrailerDescriptor desc):this(desc.Velocity){}

        public override void TurnOn()
        {
            base.TurnOn();
            Velocity = Vehicle.transform.forward.normalized * Velocity.z;
            Rbody.velocity = Velocity;
        }

        public override void HandleAdopting()
        {
            TurnOff();
        }

    }
}