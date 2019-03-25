using UnityEngine;

namespace Code.Gameplay.Descriptors
{
    public class SlidingCarDescriptor : CarDescriptor
    {
        public float MinAngle;
        public float MaxAngle;
        public float MinSideSpeed;
        public float MaxSideSpeed;
        public bool ByTouch;
        
        public SlidingCarDescriptor(Vector2 position, float speedZ, float minAngle, float maxAngle,
            float minSideSpeed, float maxSideSpeed, bool byTouch=false) : base(position, speedZ, 1)
        {
            MinAngle = minAngle;
            MaxAngle = maxAngle;
            MinSideSpeed = minSideSpeed;
            MaxSideSpeed = maxSideSpeed;
            ByTouch = byTouch;
        }
    }
}