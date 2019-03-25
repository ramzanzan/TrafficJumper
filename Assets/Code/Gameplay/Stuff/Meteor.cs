using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
	public GameObject Projector;
	
	//todo boom 
	private void OnTriggerEnter(Collider other)
	{
		switch (other.gameObject.tag)
		{
			case "Car":
				other.gameObject.GetComponent<Vehicle>().Behaviour.Stop();
				goto case "Road";
			case "Road":
				GetComponent<Rigidbody>().velocity=Vector3.zero;
				Projector.SetActive(false);
				break;
		}
		
	}
}
