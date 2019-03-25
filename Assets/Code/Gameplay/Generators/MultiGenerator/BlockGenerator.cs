using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GenUtils;

public class BlockGenerator : IMiniGenerator
{
	public bool HasMore { get; private set; }
	private readonly RandomItem<int> _carSizeDistr;
	private readonly RandomItem<int> _carAmntDistr;
	private readonly float[] _topBounds;
	private float _lowerBound;
	private readonly float[] _topBoundsFtr;
	private float _lowerBoundFtr;
	private readonly PlaySettings SS;
	private byte _mask;
	
	public BlockGenerator(PlaySettings ss,RandomItem<int> carSizeDistribution, 
		RandomItem<int> carAmountDistribution, byte mask=0)
	{
		SS=ss;
		_carSizeDistr = carSizeDistribution;
		if(_carSizeDistr.Length>3 || _carSizeDistr.Items.Max()>3 || _carSizeDistr.Items.Min()<1)
			throw new ArgumentException("Bad blockLengthDistribution");
		_carAmntDistr = carAmountDistribution;
		if(carAmountDistribution!=null 
		   && (carAmountDistribution.Items.Min() < 1 || carAmountDistribution.Items.Max() > SS.Road.LinesCount)) 
			throw new ArgumentException("Bad carAmountDistribution");
		_topBounds = new float[SS.Road.LinesCount];
		_topBoundsFtr = new float[SS.Road.LinesCount];
		_mask = mask;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="list"></param> 
	/// <param name="lineMask"></param> from left - zero line - to right. 0x28 - 00101000 - masks 2th and 4th lines;
	/// <param name="count"></param> using if !<=0 insdeat amountDist
	public void GenerateBlock(LinkedList<IDescriptorWithID> list, byte lineMask, int count=0, float lowBound=0)
	{
		float tmp;
		_lowerBound = _lowerBound > lowBound ? _lowerBound : lowBound;
		
		foreach (var e in list)
		{
			CarDescriptor car;
			if (e is CarDescriptor) car = (CarDescriptor)e;
			else continue;
			
			tmp = car.Position.y + Vehicle.LengthStatic(car.Size) + SS.MinCarGap;
			_lowerBound = _lowerBound > tmp ? _lowerBound : tmp;
			tmp += car.Velocity * SS.TimeWindow;
			_lowerBoundFtr = _lowerBoundFtr > tmp ? _lowerBoundFtr : tmp;
			for (int line = 0; line < SS.Road.LinesCount; line++)
			{
				if(IsLineMasked(line,lineMask)) continue;
				tmp = car.Position.y + SS.TopBound(SS.Road.LineNumFromPosX(car.Position.x), line, car.Size);
				_topBounds[line] = _topBounds[line] > tmp ? _topBounds[line] : tmp;
				tmp += car.Velocity * SS.TimeWindow;
				_topBoundsFtr[line] = _topBoundsFtr[line] > tmp ? _topBoundsFtr[line] : tmp;
			}
		}
		
		for(int i=0;i<SS.Road.LinesCount;i++)
			if (_topBounds[i] <= _lowerBound || _topBoundsFtr[i] <= _lowerBoundFtr)
				lineMask |= MaskLine(i);

		int carCnt = count > 0 ? count : _carAmntDistr.Next();
		carCnt = carCnt <= FreeLines(SS.Road.LinesCount, lineMask) 
			? carCnt : FreeLines(SS.Road.LinesCount, lineMask);
		
#if UNITY_EDITOR
        if (carCnt <= 0)
        {
            Debug.LogError("CAR CNT MINI GEN = 0");
            Debug.LogError(lineMask);
            Debug.LogError(_lowerBound);
            Debug.LogError(_lowerBoundFtr);
            for (int i = 0; i < _topBounds.Length; i++)
            {
                Debug.LogError(_topBounds[i]);
                Debug.LogError(_topBoundsFtr[i]);
            }

            foreach (var v in list)
            {
                Debug.LogError(v);
            }
        }
#endif
		
		list.Clear();
		
		int lineNum;
		float minVel, maxVel;
		Vector2 pos;
		for (int i = 0; i < carCnt; i++)
		{
			do{
				lineNum = UnityEngine.Random.Range(0, SS.Road.LinesCount);
			} while (IsLineMasked(lineNum, lineMask));
			lineMask |= MaskLine(lineNum);
	
			minVel = (_lowerBoundFtr - _topBounds[lineNum]) / SS.TimeWindow;
			minVel = minVel >= SS.MinCarSpeed ? minVel : SS.MinCarSpeed;
			maxVel = (_topBoundsFtr[lineNum] - _lowerBound) / SS.TimeWindow;
			maxVel = maxVel <= SS.MaxCarSpeed ? maxVel : SS.MaxCarSpeed;
			tmp = UnityEngine.Random.Range(minVel, maxVel);
			pos.x = SS.Road.LinePosXFromNum(lineNum);
			pos.y = _lowerBound + (_topBounds[lineNum] - _lowerBound) * ( 1 - (tmp-minVel)/(maxVel-minVel));
			list.AddLast(new CarDescriptor(pos,tmp,_carSizeDistr.Next()));
		}

		HasMore = false;
	}

	public void GenerateBlock(LinkedList<IDescriptorWithID> list)
	{
		GenerateBlock(list,_mask);
	}
	
	public bool WasBlockModifable => true;

	public void Reset()
	{
		HasMore = true;
		_lowerBound = _lowerBoundFtr = 0;
		for (int i = 0; i < _topBounds.Length; i++)
			_topBounds[i] = _topBoundsFtr[i] = 0;
	}

	public Func<IDictionary<int, GameObject>, LinkedList<IDescriptorWithID>, float, bool> ReadinessTestHandler => null;
}
