using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;

public class PursuitController : MonoBehaviour 
{
	private static PursuitController _instance;

	public static PursuitController GetInstance()
	{
		if(_instance==null) throw new Exception("Сlasses initialization order have violated");
		return _instance;
	}
	
	//todo
	public GameObject Start_;
	public GameObject End_;

	public Action Catched;
	public float MinVelocity { get; set; }
	public float MaxVelocity { get; set; }
	private float _velocity;
	private Vector3 _movement;
	private float _screenBottomOffset;
	private float _screenTopOffset;
	private float _screenLength;

	[SerializeField]
	private Camera _camera;
	public Transform AvatarTf;
	private bool _on;
	
	private void Start ()
	{
		if(_instance!=null) throw new Exception("Second singleton");
		_instance = this;
		
		_screenLength = 2 * _camera.orthographicSize /
						Mathf.Cos(_camera.transform.rotation.eulerAngles.x * Mathf.Deg2Rad);
		_screenBottomOffset = _camera.transform.position.y *
						   Mathf.Tan((90 - _camera.transform.rotation.eulerAngles.x) * Mathf.Deg2Rad) - 
		                   _screenLength / 2;
		_screenTopOffset = _screenBottomOffset + _screenLength;
		
		//todo redo
		Start_.transform.localPosition+=new Vector3(0,0,_screenBottomOffset);
		End_.transform.localPosition+=new Vector3(0,0,_screenTopOffset);
	}

	private float _topPursuitBound;
	private float _bottomPursuitBound;
	
	public void SetPursuitBoundsFactors(float bottom, float top)
	{
		if(bottom<0 || top>1 || bottom>top) throw new ArgumentException();
		_topPursuitBound = _screenBottomOffset + _screenLength * top;
		_bottomPursuitBound = _screenBottomOffset + _screenLength * bottom;

	}

	private void FixedUpdate ()
	{
		if(!_on) return;
		var z = transform.position.z;
		var avaZ = AvatarTf.position.z;
		if (avaZ < z + _bottomPursuitBound)
			_velocity = MinVelocity;
		else if (avaZ > z + _topPursuitBound)
			_velocity = MaxVelocity;
		else
			_velocity = MinVelocity + (avaZ - z - _bottomPursuitBound) / 
			            (_topPursuitBound - _bottomPursuitBound) * (MaxVelocity-MinVelocity);
		
		_movement.z = _velocity * Time.fixedDeltaTime;
		transform.position += _movement;
		
		if(avaZ<z+_screenBottomOffset)
			Catched.Invoke();
			
	}

	//todo
	public void LevelStarted()
	{
		_on = true;
	}

	//todo
	public void LevelEnded()
	{
		_on = false;
		transform.position = new Vector3(0,0,-5);
	}
	
	public float ScreenTop()
	{
		return transform.position.z + _screenTopOffset;
	}

	public float ScreenBottom()
	{
		return transform.position.z + _screenBottomOffset;
	}
	
	
}


