
using UnityEngine;

public class MilitaryDescriptor : CarDescriptor
{
    public float ReloadTime;
    public float ShellSpeed;
    public float RotationSpeed;
    public float Radius;

    public MilitaryDescriptor(Vector2 pos, float velocity, float radius, float reloadTime, float rotSpeed, float shellSpeed) : 
        base(pos,velocity,1)
    {
        Radius = radius;
        ReloadTime = reloadTime;
        RotationSpeed = rotSpeed;
        ShellSpeed = shellSpeed;
    }

}
