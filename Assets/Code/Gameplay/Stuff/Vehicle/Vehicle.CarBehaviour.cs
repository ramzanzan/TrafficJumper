using System;
using UnityEngine;

public partial class Vehicle
{
    public abstract class CarBehaviour : VehicleBehaviour
    {
        private const float CrashTreshhold = 1;

        protected CarBehaviour(bool edible) : base(edible){}
        
        public override bool HandleEating()
        {
            if (Vehicle.ComputeBlockNum(Vehicle.Avatar.transform.localPosition) == Vehicle.Size - 1
                && IsEdible)
            {
                IsEdible = false;
                TurnOff();
                return true;
            }
            return false;
        }
        
        public override void HandleAdopting(){}

        public override void HandleBreakeDown(){}

        //todo
        public override void HandleCollisionEnter(Collision other)
        {
            switch (other.gameObject.tag)
            {
                case "Car":    
                    if (other.relativeVelocity.sqrMagnitude < CrashTreshhold)
                    {
                        if(Vehicle.transform.position.z<other.transform.position.z)
                            TurnOff();
                        Rbody.angularVelocity=Vector3.zero;
                    }
                    else
                    {
                        TurnOff();
                        //не убивать при столкновении
//                        if(Vehicle._isBusy)
//                            Vehicle.DropAvatar(Rbody.velocity);
                    }
                    break;
            }
        }

        public override void HandleTriggerEnter(Collider other)
        {
            
        }

        public override void Affect()
        {
            
        }

        
    }
}
