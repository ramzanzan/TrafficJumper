using UnityEngine;

public class OilPuddleDescriptor : IDescriptorWithID
{
    private const string Tag = "OilPuddle";
    public string GetTag()
    {
        return Tag;
    }
    
    private int _id;
    public enum PosState
    {
        First,
        Transitional,
        Last
    }

    public PosState State;
    public float Length;
    public float Width;
    public Vector2 Position;
    
    public OilPuddleDescriptor(Vector2 pos, float length, float width, PosState state=PosState.Transitional)
    {
        Position = pos;
        State = state;
        Length = length;
        Width = width;
    }
    
    public int GetID()
    {
        return _id;
    }

    public void SetID(int id)
    {
        _id = id;
    }

    public OilPuddleDescriptor Clone()
    {
        return new OilPuddleDescriptor(Position,Length,Width,State);
    }
}
