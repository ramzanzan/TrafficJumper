using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Copter : MonoBehaviour
{
	private const float IndicatorTime = 1.5f;
	private const float Attitude = 4f;
	private const float MaxScreenBoundDiff = 2f;
	private const float MaxDelta = 1f;
	private const float Acceleration = 2f;
	public static GOPool MissilePool;
	
	private float _missileVelocity;
	private float _missileRotVelocity;
	private float _missileLimit;
	private float _reloadTime;
	private float _timer;
	private float _sideSpeed;
	private Vector3 _velocity;
	private float _middlePosX;
		
	public static Transform AvatarTransform;
	public static PlayerAvatar Ava;
	public static PursuitController PursuitController;


	public void Init(float reloadTime, float missileVel, float missileRotVel, 
		float missileLimit, float middlePosX)
	{
		_reloadTime = reloadTime;
		_missileLimit = missileLimit;
		_missileRotVelocity = missileRotVel;
		_missileVelocity = missileVel;
		_middlePosX = middlePosX;
	}

	public void TakePosition()
	{
		var pos = new Vector3(_middlePosX,2);
		pos.z = PursuitController.End_.transform.position.z;
		transform.position = pos;
	}

	private void FixedUpdate()
	{
		if (_timer > 0)
			_timer -= Time.fixedDeltaTime;
		else
		{
			_timer = _reloadTime;
			Fire();
		}
			
	}

	private void LateUpdate()
	{
		var pos = new Vector3(_middlePosX,2);
		pos.z = PursuitController.End_.transform.position.z-MaxScreenBoundDiff;
		transform.position = pos;
	}

	private void Movement()
	{
		
	}
	
	private void Fluctuate()
	{
		
	}

	public void Recall(CopterBuilder builder)
	{
		StartCoroutine(RecallCountdown(builder));
	}

	private IEnumerator RecallCountdown(CopterBuilder builder)
	{
		yield return new WaitForSeconds(5);
		if(gameObject.activeInHierarchy)
			builder.Disassemble(gameObject);
	}
	
	//todo red0
	private void Fire()
	{
		var angle = Vector3.SignedAngle(
			Vector3.forward, AvatarTransform.position - transform.position, Vector3.up );
		Debug.Log(angle+" angle");
		var pos = transform.position;
		pos.y = .717f;
		var missile = MissilePool.Pop();
		missile.GetComponent<HomingMissile>().Init(_missileVelocity,_missileRotVelocity,_missileLimit);
	}

}
