using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MeteorsGenerator : IMiniGenerator
{
    private const float LowTimeBound = .6f;
    private const float TopTimeBound = .9f;
    private const float LauncherOffsetZ = 0;
    private const float TinyCarGap = .3f;
    private BlockGenerator _blockGen;
    private RandomItem<int> _metAmountDistr, _carCountDistr, _carSizeDistr;
    private Transform _launcher;
    private PlaySettings _ss;
    private float[] _lowerBounds, _topBounds;
    private bool[] _isActual, _isMeteorTarget;
    private CarDescriptor[] _lastOnLine;
    private int _minCount, _maxCount, _count, _i;
    private CarDescriptor _carMock = new CarDescriptor(Vector2.zero, 0, 1);

    public bool HasMore => _i < _count;

    public MeteorsGenerator(PlaySettings ss, RandomItem<int> carSizeDistribution,RandomItem<int> carAmountDistribution, 
        RandomItem<int> metAmountDistribution, int minCount, int maxCount,
        Transform launcher)
    {
        if( carAmountDistribution.Items.Min()<2 
            || carAmountDistribution.Items.Max()>ss.Road.LinesCount 
            || metAmountDistribution.Items.Min()<1 
            || metAmountDistribution.Items.Max()>=carAmountDistribution.Items.Max() ) 
            throw new ArgumentOutOfRangeException();
        _blockGen = new BlockGenerator(ss,carSizeDistribution,carAmountDistribution);
        _metAmountDistr = metAmountDistribution;
        _carCountDistr = carAmountDistribution;
        _carSizeDistr = carSizeDistribution;
        _launcher = launcher;
        _ss = ss;
        _minCount = minCount;
        _maxCount = maxCount;
        _lowerBounds = new float[_ss.Road.LinesCount];
        _topBounds = new float[_ss.Road.LinesCount];
        _lastOnLine = new CarDescriptor[_ss.Road.LinesCount];
        _isActual = new bool[_ss.Road.LinesCount];
        _isMeteorTarget = new bool[_ss.Road.LinesCount];
    }
    
    public void GenerateBlock(LinkedList<IDescriptorWithID> list)
    {

        ++_i;
        
        if (_i>1 && _i<=_count)
        {
            list.Clear();
            var lines = _ss.Road.LinesCount;
            var actualCount = 0;
            for(var i=0;i<lines;++i)
                if(_isActual[i]) ++actualCount;
            var meteorCount = _metAmountDistr.Next();
            meteorCount = meteorCount < actualCount ? meteorCount : actualCount - 1;
            while (meteorCount>0)
            {
                var i = Random.Range(0, lines);
                if (!_isActual[i]) continue;
                _isActual[i] = false;
                var time = Random.Range(_ss.TimeWindow * LowTimeBound, _ss.TimeWindow * TopTimeBound);
                var to = _lastOnLine[i].Position;
                to.y += _lastOnLine[i].Velocity * time + Vehicle.LengthStatic(_lastOnLine[i].Size) / 2;
                var from = new Vector2(to.x,_launcher.position.z);
                list.AddLast(new MeteorDescriptor(from, to, time));
                --meteorCount;
            }

            float commonLowerBound = 0;
            for (var i = 0; i < lines; ++i)
            {
                var car = _lastOnLine[i];
                var bound = car.Position.y + Vehicle.LengthStatic(car.Size) + (_isActual[i] ? _ss.MinCarGap : TinyCarGap);
                _lowerBounds[i] = bound;
                if(!_isActual[i]) continue;
                
                _isActual[i] = false;
                if (bound > commonLowerBound) commonLowerBound = bound;
                for (var l = 0; l < lines; ++l)
                {
                    bound = _ss.TopBound(i, l, car.Size) + car.Position.y;
                    if (_topBounds[l] < bound) 
                        _topBounds[l] = bound;
                }
            }
            for(var i=0;i<lines;i++)
                if (_lowerBounds[i] < commonLowerBound)
                    _lowerBounds[i] = commonLowerBound;

            var carCount = 0;
            for(var i=0;i<lines;i++)
                if(_topBounds[i]>_lowerBounds[i]) ++carCount;
            if (carCount > _carCountDistr.Next()) carCount = _carCountDistr.Current;

            do
            {
                var i = Random.Range(0, lines);
                if (_lastOnLine[i].Position.y < _lowerBounds[i] &&
                    _lowerBounds[i] < _topBounds[i])
                {
                    _isActual[i] = true;
                    var pos = new Vector2(_ss.Road.LinePosXFromNum(i), 
                        Random.Range(_lowerBounds[i], _topBounds[i]));
                    _lastOnLine[i] = new CarDescriptor(pos,_ss.NormalCarSpeed,_carSizeDistr.Next());
                    list.AddLast(_lastOnLine[i]);
                    --carCount;
                }
            } while (carCount>0);

        }
        else if (_i==1)
        {
            var tmp1 = _ss.MinCarSpeed;
            var tmp2 = _ss.MaxCarSpeed;
            _ss.MinCarSpeed = _ss.NormalCarSpeed*.9f;
            _ss.MinCarSpeed = _ss.NormalCarSpeed*1.1f;
            _blockGen.GenerateBlock(list);
            _ss.MinCarSpeed = tmp1;
            _ss.MaxCarSpeed = tmp2;
            foreach (CarDescriptor e in list)
            {
                var line = _ss.Road.LineNumFromPosX(e.Position.x);
                _lastOnLine[line] = e;
                _isActual[line] = true;
            }

        }
    }

    public bool WasBlockModifable => false;
    
    public void Reset()
    {
        _blockGen.Reset();
        _i = 0;
        _count = Random.Range(_minCount, _maxCount + 1);
        for (var i = 0; i < _lastOnLine.Length; i++)
        {
            _lastOnLine[i] = _carMock;
            _isActual[i] = false;
            _lowerBounds[i] = _topBounds[i] = 0;
        }
    }

    private bool ReadinessTest(IDictionary<int, GameObject> items, LinkedList<IDescriptorWithID> list, float boundary)
    {
        var res = MultiGeneratorProvider.DefaultReadinessTest(items, list, boundary);
        if (!res) return false;
        for (var i = 0;i < _lastOnLine.Length; i++)
        {
            GameObject car;
            if(!items.TryGetValue(_lastOnLine[i].GetID(),out car)) continue;
//            _lastOnLine[i].Position.y = ((Vehicle) car).BackPosZ();
            _lastOnLine[i].Position.y = car.transform.position.z;
        }
        return true;
    }
    
    public Func<IDictionary<int, GameObject>, LinkedList<IDescriptorWithID>, float, bool> ReadinessTestHandler => ReadinessTest;
}
