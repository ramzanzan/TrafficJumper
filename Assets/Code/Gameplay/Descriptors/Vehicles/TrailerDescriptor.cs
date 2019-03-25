using UnityEngine;

public class TrailerDescriptor : VehicleDescriptor
{
    public TrailerDescriptor(Vector2 position, float velocity) : base(position, velocity)
    {
        IsModifable = false;
    }
}
