using UnityEngine;

public class AutoMeteorCarDescriptor : CarDescriptor
{
    public float AddVelocity;
    public float Distance;
    public float Time;
    
    public AutoMeteorCarDescriptor(Vector2 position, float velocity, float addVelocity,
         float distance, float time) : base(position, velocity, 1)
    {
        AddVelocity = addVelocity;
        Distance = distance;
        Time = time;
    }
}
