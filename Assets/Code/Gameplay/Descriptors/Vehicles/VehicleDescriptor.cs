using UnityEngine;

public class VehicleDescriptor : IDescriptorWithID
{
    protected const string Tag = "Car";
    public string GetTag()
    {
        return Tag;
    }
    
    private int _id;
    public Vector2 Position;
    public float Velocity;
    public bool IsModifable = true;

    public VehicleDescriptor(Vector2 position, float velocity)
    {
        Position = position;
        Velocity = velocity;
        _id = -1;
    }

    public int GetID()
    {
        return _id;
    }

    public void SetID(int id)
    {
        _id = id;
    }

    public override string ToString()
    {
        return "ID: " + _id + " Pos: " + Position + " Vel.: " + Velocity;
    }
}
