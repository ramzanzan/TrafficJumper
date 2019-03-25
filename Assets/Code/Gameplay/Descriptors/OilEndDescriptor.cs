using UnityEngine;

public class OilEndDescriptor : IDescriptorWithID
{
    private const string Tag = "OilEnd";
    public string GetTag()
    {
        return Tag;
    }
    
    private int _id;
    public Vector2 Position;
    public OilEndDescriptor(Vector2 pos)
    {
        Position = pos;
    }
    
    public int GetID()
    {
        return _id;
    }

    public void SetID(int id)
    {
        _id = id;
    }
}
