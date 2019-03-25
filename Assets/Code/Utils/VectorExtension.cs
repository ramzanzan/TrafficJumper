using UnityEngine;
public static class VectorExtension
{
    public static Vector3 MyVec2ToVec3(this Vector2 v, float y=0)
    {
        return new Vector3(v.x,y,v.y);
    }

    public static Vector2 MyVec3ToVec2(this Vector3 v)
    {
        return new Vector2(v.x,v.z);
    }
}
