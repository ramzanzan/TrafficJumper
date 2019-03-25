using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelViewerTest : MonoBehaviour
{

	public GameObject A;
	public GameObject B;
	public ModelViewer MV;

	public void InitMV()
	{
		
		MV.Init(A.transform);
	}
	
	public void Test()
	{
		MV.ShowNext(false, B.transform);
	}
}
