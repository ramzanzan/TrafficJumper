using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speed : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}

	public float speed = 2;
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.position += new Vector3(0,0,speed*Time.deltaTime);
	}
}
