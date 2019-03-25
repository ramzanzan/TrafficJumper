using UnityEngine;

namespace Code.Gameplay.Descriptors
{
    public class WobblingCarDescriptor : CarDescriptor
    {
        public bool OnPointerContact;
        public float HalfPeriodLength;
        public float MinCarVelocity;
        
        
        public WobblingCarDescriptor(Vector2 position, float velocity, int size,
            float halfPeriodLength, float minCarVelocity, bool onPointerContact) : 
            base(position, velocity, size)
        {
            HalfPeriodLength = halfPeriodLength;
            OnPointerContact = onPointerContact;
            MinCarVelocity = minCarVelocity;
        }
    }
}