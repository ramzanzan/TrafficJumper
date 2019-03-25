using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarNPointerCtor : MonoBehaviour {

	public float _maxJumpDistance = 4;
	public float _minJumpDistance = 1;
	public float _horizontalVelocity = 2;
	public float _powerTime = 4;
	public float maxJumpHeight = 3;

	public AvatarController AvaCtrl;
	
	// Use this for initialization
	void Start ()
	{
		StartCoroutine(TuneAll());
	}

	IEnumerator TuneAll()
	{
		yield return new WaitForSeconds(.2f);
		AvaCtrl.Pointer.Init(4, 0.5f, 1);
		AvaCtrl.Avatar.Init(_maxJumpDistance, _minJumpDistance, _horizontalVelocity,  _powerTime, maxJumpHeight);
	}
}
