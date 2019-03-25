using UnityEngine;

public class Projectile : MonoBehaviour
{
    public static PlayerAvatar Avatar;
    public static GOPool SelfPool;
    public bool NoHarm; //todo redo

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Avatar") && Avatar.IsAlive && !NoHarm)
        {
            Avatar.PushDown(GetComponent<Rigidbody>().velocity/2);
        }
        //todo vfx
        Destroy();
    }

    //todo pool
    private void Destroy()
    {
        SelfPool.Push(gameObject);
    }
}
