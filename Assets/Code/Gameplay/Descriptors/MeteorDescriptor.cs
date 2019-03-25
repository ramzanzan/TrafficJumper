using UnityEngine;

public class MeteorDescriptor : IDescriptorWithID
{
    private const string Tag = "Meteor";
    public string GetTag()
    {
        return Tag;
    }
    
    private int _id;
    public Vector2 From;
    public Vector2 To;
    public float Time;

    public MeteorDescriptor(Vector2 from, Vector2 to, float time)
    {
        From = from;
        To = to;
        Time = time;
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