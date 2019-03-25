using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Analytics;

public partial class Vehicle
{
    public class SimpleCar : CarBehaviour
    {
        public override string Tag => "Simple";

        public SimpleCar(float velocity) : base(true)
        {
            Velocity.z = velocity;
        }

        public SimpleCar(CarDescriptor desc) : this(desc.Velocity){}

        public override void TurnOn()
        {
            base.TurnOn();
            Velocity = Vehicle.transform.forward.normalized * Velocity.z;
            Rbody.velocity = Velocity;
        }

        public override void Affect()
        {
            if (!On) return;
            Rbody.velocity = Velocity;
        }
    }
}