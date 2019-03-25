using System;
using System.Collections.Generic;
using System.Linq;
using Code.Gameplay.Descriptors;
using JetBrains.Annotations;
using UnityEngine;
using static GenUtils;
using Random = UnityEngine.Random;

public class SlidingCarGenerator : IMiniGenerator
{
	private const int InitialCarsCount = 3;
	private const float MaxDistanceMultiplier = .7f;
	private const float PuddleLength = 3;
	private float _maxAngle;
	private float _minAngle;
	private float _minSideSpeed;
	private float _maxSideSpeed;
	private int _minCount;
	private int _maxCount;
	private int _count;
	private int _i;
	private float _lowerBound;
	private float _topBound;
	private PlaySettings _ss;
	private float _nextDist;
	
	private CarDescriptor _carBuffer;
	private OilPuddleDescriptor _puddleBuffer;
	private bool _isNextCar;
	private bool _isNextPuddle;
	
	public SlidingCarGenerator(PlaySettings ss, int minCount, int maxCount, float minAngle, float maxAngle,
		float minSideSpeed, float maxSideSpeed)
	{
		_ss = ss;
		_minCount = minCount + InitialCarsCount;
		_maxCount = maxCount + InitialCarsCount;
		_minAngle = minAngle;
		_maxAngle = maxAngle;
		_minSideSpeed = minSideSpeed;
		_maxSideSpeed = maxSideSpeed;
	}

	public bool HasMore => _i != _count;
	
	public void GenerateBlock(LinkedList<IDescriptorWithID> list)
	{
		if (_i == 0)
		{
			CarDescriptor car = null;
			foreach (var e in list)
			{
				var descriptor = e as CarDescriptor;
				if (descriptor != null && (car == null || car.Position.y < descriptor.Position.y))
					car = descriptor;
			}
			list.Clear();
			car.Position.y += Random.Range(_ss.MinCarGap, _ss.MaxJumpDistance * MaxDistanceMultiplier)+
			                  Vehicle.LengthStatic(car.Size);
			_carBuffer = new SlidingCarDescriptor(car.Position, (car.Velocity + _ss.NormalCarSpeed) / 2, _minAngle,
				_maxAngle, _minSideSpeed, _maxSideSpeed, true);
			list.AddLast(_carBuffer);
			++_i;
		}
		else
		{
			list.Clear();
			if (_isNextCar)
			{
				Vector2 pos = _carBuffer.Position;
				pos.x = _ss.Road.LinePosXFromNum(Random.Range(0, _ss.Road.LinesCount));
				pos.y += Random.Range(_ss.MinCarGap, _ss.MaxJumpDistance * MaxDistanceMultiplier) +
				         Vehicle.LengthStatic();
				_carBuffer = new SlidingCarDescriptor(pos, _ss.NormalCarSpeed, _minAngle, _maxAngle,
					_minSideSpeed, _maxSideSpeed, _i < InitialCarsCount);
				list.AddLast(_carBuffer);
				_isNextCar = false;
				++_i;
			}
			
			if (_isNextPuddle)
			{
				_puddleBuffer = new OilPuddleDescriptor(
					new Vector2(_ss.Road.MiddlePosition, _puddleBuffer.Position.y + _puddleBuffer.Length),
					PuddleLength, _ss.Road.Width,
					_i < _count ? OilPuddleDescriptor.PosState.Transitional : OilPuddleDescriptor.PosState.Last);
				list.AddFirst(_puddleBuffer);
				_isNextPuddle = false;
			}
			else if (_i - 1 == InitialCarsCount)
			{
				_puddleBuffer = new OilPuddleDescriptor(
					new Vector2(_ss.Road.MiddlePosition, _carBuffer.Position.y+PuddleLength/2),
					PuddleLength, _ss.Road.Width, OilPuddleDescriptor.PosState.First);
				list.AddFirst(_puddleBuffer);
			}

			//todo декорацию масловоза + len?
			if (_i == _count)
			{
				var carY = ((CarDescriptor) list.Last.Value).Position.y;
				while (_puddleBuffer.Position.y+_puddleBuffer.Length/2 - carY < _ss.MaxJumpDistance)
				{
					_puddleBuffer = _puddleBuffer.Clone();
					_puddleBuffer.Position.y += _puddleBuffer.Length;
					list.AddLast(_puddleBuffer);
				}
				_puddleBuffer.State = OilPuddleDescriptor.PosState.Last;
				
				list.AddLast(new OilEndDescriptor(new Vector2(_ss.Road.MiddlePosition,
					_puddleBuffer.Position.y + _puddleBuffer.Length/2)));
			}
		}
		
	}

	public bool WasBlockModifable => false;
	
	public void Reset()
	{
		_count = Random.Range(_minCount, _maxCount + 1);
		_isNextCar = _isNextPuddle = false;
		_carBuffer = null;
		_puddleBuffer = null;
		_i = 0;
	}

	private bool ReadinessTest(IDictionary<int, GameObject> items, LinkedList<IDescriptorWithID> list, float boundary)
	{
		if (_i == 0) return MultiGeneratorProvider.DefaultReadinessTest(items, list, boundary);

		GameObject obj;
		items.TryGetValue(_carBuffer.GetID(),out obj);
		_isNextCar = obj.GetComponent<Vehicle>().FrontPosZ() + _ss.MinCarGap < boundary;
		if (_isNextCar)
			_carBuffer.Position.y = obj.GetComponent<Vehicle>().BackPosZ();
		
		if (_i - 1 >= InitialCarsCount)
		{
//			items.TryGetValue(_puddleBuffer.GetID(), out obj);
//			_isNextPuddle = obj.transform.position.z + obj.transform.localScale.z / 2 < boundary;
			_isNextPuddle = _puddleBuffer.Position.y + _puddleBuffer.Length / 2 < boundary;
		}

		return _isNextCar||_isNextPuddle;
	}

	public Func<IDictionary<int, GameObject>, LinkedList<IDescriptorWithID>, float, bool> ReadinessTestHandler => ReadinessTest;


}
