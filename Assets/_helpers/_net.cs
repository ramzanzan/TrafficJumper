using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _net : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Avatar"))
		{
			GetComponent<Rigidbody>().velocity=Vector3.zero;
			Debug.Log("trigg net");
		}
	}
}
