using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerController : MonoBehaviour
{

	public CanvasPointer CanvPointer;
	public ProjectorPointer ProjPointer;
	public Transform Toucher;
	
	private float _headHeight;
	private float _distance;

	public void Init(float distance,float width,float headHeight)
	{
		_distance = distance;
		_headHeight = headHeight;
		CanvPointer.Init(distance,width,headHeight);
		ProjPointer.Init(distance,width,headHeight);
	}

	public void SetRotation(float angle)
	{
		transform.eulerAngles = new Vector3(0,angle,0);
	}

	public void SetLoad(float load)
	{
		load = load > 1 ? 1 : load;
		CanvPointer.SetLoad(load);
		ProjPointer.SetLoad(load);
		Toucher.localPosition = new Vector3(0,0,(_distance-_headHeight)*load+_headHeight);
	}

	public void Reset()
	{
		ProjPointer.Resetting();
		CanvPointer.Resetting();
		Toucher.localPosition = Vector3.zero;	
	}
	
	private void OnDisable()
	{
		Reset();	
	}
}
