using UnityEngine;

public class CopterDescriptor : IDescriptorWithID
{
    private const string Tag = "Copter";
    private int _id;
    public float ReloadTime;
    public float MissileVelocity;
    public float MissileRotationVelocity;
    public float MissileLimit;
    public float MiddlePosX;
    
    public CopterDescriptor(float reloadTime, float missileVel, float missileRotVel, 
        float missileLimit, float middlePosX)
    {
        ReloadTime = reloadTime;
        MissileVelocity = missileVel;
        MissileRotationVelocity = missileRotVel;
        MissileLimit = missileLimit;
        MiddlePosX = middlePosX;
    }
    
    public int GetID()
    {
        return _id;
    }

    public void SetID(int id)
    {
        _id = id;
    }

    public string GetTag()
    {
        return Tag;
    }
}
