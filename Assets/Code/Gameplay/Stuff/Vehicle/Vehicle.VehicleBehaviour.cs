using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class Vehicle : MonoBehaviour
{
    public abstract class VehicleBehaviour
    {
        protected const float DynamicFriction = .1f;
        protected Vehicle Vehicle;
        protected bool IsEdible;
        protected bool On;
        protected Rigidbody Rbody;
        protected Vector3 Velocity;
        public Vector3 GetVelocity() => Velocity;
        public abstract string Tag { get; }

        protected VehicleBehaviour(bool edible)
        {
            IsEdible = edible;
        }

        public virtual void SetVehicle(Vehicle vehicle)
        {
            Vehicle = vehicle;
            Rbody = vehicle.GetComponent<Rigidbody>();
        }

        public virtual void TurnOn()
        {
            On = true;
            Vehicle.GetComponent<Collider>().material.dynamicFriction = 0;
        }
        public virtual void TurnOff()
        {
            On = false;
            Vehicle.GetComponent<Collider>().material.dynamicFriction = DynamicFriction;
        }
        public virtual void Stop()
        {
            TurnOff();
            Velocity=Vector3.zero;
            Rbody.velocity=Velocity;
        }
        
        public virtual void Destruct()
        {
            Vehicle._behaviour = null;
            Vehicle = null;
        }
        
        public abstract bool HandleEating();
        public abstract void HandleCollisionEnter(Collision other);
        public abstract void HandleTriggerEnter(Collider other);
        public abstract void HandleAdopting();
        public abstract void HandleBreakeDown();
        public abstract void Affect();

    }
}

