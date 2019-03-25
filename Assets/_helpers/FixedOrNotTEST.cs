using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedOrNotTEST : MonoBehaviour {

	public bool isFixed;
	public Vector3 speed;
	private bool on = true;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(!isFixed && on)
			GetComponent<Rigidbody>().velocity=speed;
	}

	private void FixedUpdate()
	{
		if (isFixed && on)
			GetComponent<Rigidbody>().velocity = speed;
	}

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log(this);
		GetComponent<Rigidbody>().velocity = Vector3.zero;
		on = false;
//		speed = 0;

	}

	private void OnCollisionEnter(Collision other)
	{
		Debug.Log(other.relativeVelocity);
		Debug.Log(other.impulse);
		Debug.Log(GetComponent<Rigidbody>().velocity);
		//GetComponent<Rigidbody>().velocity = speed =Vector3.zero;
//		GetComponent<Rigidbody>().isKinematic = true;
		on = false;
	}

	private void OnCollisionStay(Collision other)
	{
		if(isFixed)
		Debug.Log(other.impulse);
	}
}
