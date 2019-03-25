using UnityEngine;

[CreateAssetMenu(fileName = "CasualStyle", menuName = "CasualStyle", order = 51)]
public class CasualStyle : StyleAssets
{
    private int _ColorID, _MainTexID;

    public override void InitializeStyle()
    {
        _ColorID = Shader.PropertyToID("_Color");
        _MainTexID = Shader.PropertyToID("_MainTex");
    }

    public override void StylizeCar1(GameObject go)
    {
        int num = Random.Range(0, CommonAssets.Car1_mesh.Length);
        go.GetComponent<MeshFilter>().mesh = CommonAssets.Car1_mesh[num];
    }

    public override void StylizeCar2(GameObject go)
    {
        int num = Random.Range(0, CommonAssets.Car2_mesh.Length);
        go.GetComponent<MeshFilter>().mesh = CommonAssets.Car2_mesh[num];
    }

    public override void StylizeCar3(GameObject go)
    {
        int num = Random.Range(0, CommonAssets.Car3_mesh.Length);
        go.GetComponent<MeshFilter>().mesh = CommonAssets.Car3_mesh[num];
    }

    public override void StylizePolice(GameObject go)
    {
        int num = Random.Range(0, CommonAssets.P_mesh.Length);
        go.GetComponent<MeshFilter>().mesh = CommonAssets.P_mesh[num];
    }

    public override void StylizeTransitPuddle(GameObject go)
    {
        //todo
    }
}
