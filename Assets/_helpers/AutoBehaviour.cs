using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoBehaviour : MonoBehaviour {

	public enum Behs
	{
		Simpe,
		Wobbling,
		Police,
		Slide,
		Military
	}
	
	public Vehicle.BlockType blockType = 0;
	public int blockCount = 1;
	public float speed = 2;
	public float sideSpeed = 2;
	public float scanRadius = 4;
	public float halfPeriod = 5;
	public bool initLeftNotRight = true;
	public float maxAngle = 80;
	public float minAngle = 20;
	public float sminSideSpeed = 1;
	public float smaxSideSpeed = 3;

	public Behs behav = Behs.Simpe;
	public PlayerAvatar avatar;
	public bool Stop;

	private Vehicle v;
	static AutoBehaviour()
	{
		Vehicle.Road = new Road(1,-2.5f,5);
		
	}
	
	void Start ()
	{
		v = GetComponent<Vehicle>();
		if (avatar != null)
		{
			Vehicle.Avatar = avatar;
			ProjectileGun.SetAvatar(avatar);
			Projectile.Avatar = avatar;
		}
		switch (behav)
		{
			case Behs.Simpe:
				v.InitBehaviour(new Vehicle.SimpleCar(speed));
				break;
			case Behs.Police:
				v.InitBehaviour(new Vehicle.PoliceCar(speed,sideSpeed,scanRadius));
				break;
			case Behs.Wobbling:
				v.InitBehaviour(new Vehicle.WobblingCar(speed,halfPeriod,2.5f,true));
				break;
			case Behs.Slide:
				v.InitBehaviour(new Vehicle.SlidingCar(speed,minAngle,maxAngle,sminSideSpeed,smaxSideSpeed));
				break;
			case Behs.Military:
				v.InitBehaviour(new Vehicle.MilitaryCar(speed,5,2,30,3.5f));
				break;
			default:
				Debug.LogWarning("something wrong");
				break;
		}
		v.InitSize(blockType,blockCount);
	}
	
	// Update is called once per frame
	void Update () {
		if(Stop)
			v.Behaviour.Stop();
			
	}
}
