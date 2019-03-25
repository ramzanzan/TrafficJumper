using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class WallGenerator : IMiniGenerator
{

    private PlaySettings _ss;
    private float _wallLenMin;
    private float _wallLenMax;
    private float _gapLenMin;
    private float _gapLenMax;
    private float _wallLen;
    private float _gapLen;
    private int _minCount;
    private int _maxCount;
    private int _count, _i;
    private float _timeWindow;
    private float _minCarSpeed;
    private float _maxCarSpeed;
    private LinkedList<IDescriptorWithID> _bufferList;
    private WallDescriptor _wallBuffer;
    private bool _isNextCar, _isNextWall, _modifable;
    private bool _setFirstWall, _isFirstWallSet;
    private bool _isBufferListDirty;
    private BlockGenerator _blockGen;
    
    public WallGenerator(PlaySettings ss, RandomItem<int> carSizeDistr, RandomItem<int> carCountDistr,
        float wallLenMin, float wallLenMax, float gapLenMin, float gapLenMax,
        float vehVelDelta, int minCount, int maxCount, bool modifable)
    {
        _ss = ss;
        _wallLenMax = wallLenMax;
        _wallLenMin = wallLenMin;
        _gapLenMax = gapLenMax;
        _gapLenMin = gapLenMin;
        _minCarSpeed= _ss.NormalCarSpeed - vehVelDelta;
        _maxCarSpeed = _ss.NormalCarSpeed + vehVelDelta;
        _maxCount = maxCount;
        _minCount = minCount;
        _bufferList = new LinkedList<IDescriptorWithID>();
        _blockGen = new BlockGenerator(ss,carSizeDistr,carCountDistr,0x20);
        _modifable = modifable;
    }

    public bool HasMore => _i < _count;
    
    public void GenerateBlock(LinkedList<IDescriptorWithID> list)
    {
        if (_isNextCar)
        {
            if (_isBufferListDirty)
            {
                list.Clear();
                foreach (var e in _bufferList)
                    list.AddLast(e);
                _bufferList.Clear();
                _isBufferListDirty = false;
            }

            float tmp1, tmp2, tmp3;
            tmp1 = _ss.TimeWindow;
            tmp2 = _ss.MinCarSpeed;
            tmp3 = _ss.MaxCarSpeed;
            _ss.TimeWindow = _timeWindow;
            _ss.MinCarSpeed = _minCarSpeed;
            _ss.MaxCarSpeed = _maxCarSpeed;
            _blockGen.GenerateBlock(list);
            _ss.TimeWindow = tmp1;
            _ss.MinCarSpeed = tmp2;
            _ss.MaxCarSpeed = tmp3;
            
            if (!_isFirstWallSet && _setFirstWall)
            {
                _isFirstWallSet = true;
                _wallBuffer = new WallDescriptor(
                    new Vector2(_ss.Road.MiddlePosition, ((CarDescriptor)list.Last.Value).Position.y+_wallLen), 
                    _wallLen);
                list.AddLast(_wallBuffer);
            }
            
            ++_i;
            _isNextCar = false;
        }
        else
        {
            list.Clear();
        }

        if (_isNextWall)
        {
            _wallBuffer.Position.y += _wallLen + _gapLen;
            list.AddLast(_wallBuffer);
            _isNextWall = false;
        }
    }

    public bool WasBlockModifable => _modifable;

    private bool ReadinessTest(IDictionary<int, GameObject> items, LinkedList<IDescriptorWithID> list, float boundary)
    {
        if (_isFirstWallSet && _wallBuffer.Position.y + _wallLen / 2 + _gapLen<boundary)
        {
            _isNextWall = true;
            if (!_isBufferListDirty)
            {
                foreach (var e in list)
                    _bufferList.AddLast(e);
                _isBufferListDirty = true;
            }
        }

        _isNextCar = MultiGeneratorProvider.
            DefaultReadinessTest(items, _isBufferListDirty ? _bufferList : list, boundary);
        
        if (_isNextCar && !_isFirstWallSet)
        {
            _setFirstWall = true;
            foreach (var e in list)
            {
                var midNum = _ss.Road.LineNumFromPosX(_ss.Road.MiddlePosition);
                if(e is CarDescriptor)
                    _setFirstWall &= midNum!=_ss.Road.LineNumFromPosX(((CarDescriptor)e).Position.x);
            }
        }
        
        if (_isNextCar && _isBufferListDirty)
        {
            foreach (var e in _bufferList)
            {
                var carDesc = e as CarDescriptor; 
                if(carDesc==null) continue;
                GameObject car;
                if (items.TryGetValue(e.GetID(), out car))
                    carDesc.Position.y = car.transform.position.z;
            }
        }

        return _isNextCar || _isNextWall;
    }

    public Func<IDictionary<int, GameObject>, LinkedList<IDescriptorWithID>, float, bool> ReadinessTestHandler => ReadinessTest;

    public void Reset()
    {
        _blockGen.Reset();
        _wallLen = Random.Range(_wallLenMin, _wallLenMax);
        _gapLen = Random.Range(_gapLenMin, _gapLenMax);
        _count = Random.Range(_minCount, _maxCount + 1);
        _bufferList.Clear();
        _isFirstWallSet = _setFirstWall = false;
        _isBufferListDirty = false;
        _wallBuffer = null;
        _i = 0;
        
        _timeWindow = (_wallLen + _gapLen) * 2 / _ss.NormalCarSpeed;
        _timeWindow = _timeWindow > _ss.TimeWindow ? _timeWindow : _ss.TimeWindow;

    }
}
