using UnityEngine;

public class CarDescriptor : VehicleDescriptor
{
    public readonly int Size;
    
    public CarDescriptor(Vector2 position, float velocity,int size) : base(position, velocity)
    {
        Size = size;
    }

    public override string ToString()
    {
        return base.ToString() + " Size: " + Size;
    }
}
