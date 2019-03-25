using UnityEngine;

[CreateAssetMenu(fileName = "AssetContainer", menuName = "AssetContainer", order = 51)]
public class CommonAssets : ScriptableObject
{
    public Mesh[] Car1_mesh, CrushedCar1_mesh;
    public Mesh[] Car2_mesh, CrushedCar2_mesh;
    public Mesh[] Car3_mesh, CrushedCar3_mesh;
    public Mesh[] P_mesh, CrushedP_mesh;
    public Mesh CrushedT_mesh;
    public Mesh CrushedM_mesh;
    public GameObject OilEnd;
}
