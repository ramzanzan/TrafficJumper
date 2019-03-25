using UnityEngine;

public class PoliceDescriptor : CarDescriptor
{
    public float SideSpeed;
    public float ScanRadius;
    
    public PoliceDescriptor(Vector2 position, float velocity, float sideSpeed, float scanRadius) 
        : base(position, velocity, 1)
    {
        IsModifable = false;
        SideSpeed = sideSpeed;
        ScanRadius = scanRadius;
    }
}
