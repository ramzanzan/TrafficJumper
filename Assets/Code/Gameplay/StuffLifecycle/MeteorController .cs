using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorController : IBuilder
{

	private const float ProjectorStartY = 10;
	private const float ProjectorEndY = 5;
	private const float HitVelocity = 25;
	private StyleAssets _style;
	private Transform _launcher;
	private GOPool _meteorPool, _projPool;
	public Dictionary<int, GameObject> ForDisassemble;

	public MeteorController(Transform launcher)
	{
		_launcher = launcher;
		_meteorPool = new GOPool();
		_projPool = new GOPool();
	}
	
	public GameObject Build(IDescriptorWithID desc)
	{
		if(!(desc is MeteorDescriptor)) throw new ArgumentException();
		var d = (MeteorDescriptor) desc;
		CreateAndFire(d.To,d.Time);
		return null;
	}

	public void RecallAll()
	{
		_meteorPool.RecallAll();
		_projPool.RecallAll();
	}

	public void SetStyle(StyleAssets style)
	{
		_style = style;
		_meteorPool.ResetTemplate(_style.Meteor);
		_projPool.ResetTemplate(_style.MeteorProjector);
	}

	public void Disassemble(GameObject go)
	{
		var meteor = go.GetComponent<Meteor>(); 
		_projPool.Push(meteor.Projector);
		meteor.Projector = null;
		_meteorPool.Push(go);
	}

	public void CreateAndFire(Vector2 to, float time)
	{
		CreateAndFire(_launcher.position.MyVec3ToVec2(),to,time);
	}
	
	public void CreateAndFire(Vector2 from, Vector2 to, float time)
	{
		var meteor = _meteorPool.Pop();
		var projector = _projPool.Pop();
		ForDisassemble.Add(meteor.GetHashCode(),meteor);			//костылище
//		ForDisassemble.Add(projector.GetHashCode(),projector);		//	
		_style.StylizeMeteor(meteor,projector);

		var pos = new Vector3(to.x, HitVelocity * time , to.y);
		meteor.transform.position = pos;
		meteor.GetComponent<Meteor>().Projector = projector;
		meteor.GetComponent<Rigidbody>().velocity=new Vector3(0,-HitVelocity,0);
		
		pos.x = from.x;
		pos.z = from.y;
		pos.y = ProjectorStartY;
		projector.transform.position = pos;
		var distance = new Vector3(to.x - from.x, 0, to.y - from.y);
		var velocity = distance / time;
		projector.GetComponent<Rigidbody>().velocity = velocity;
		projector.GetComponent<ConstantForce>().force = new Vector2(0,-(ProjectorStartY-ProjectorEndY)*2/(time*time));
	}


}
