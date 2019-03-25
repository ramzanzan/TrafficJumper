using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CopterGenerator : IMiniGenerator
{

    private PlaySettings _ps;
    private float _reloadTime, _missileVel, _missileRotVel, _missileLimit;
    private int _minCount, _maxCount, _count;
    private bool _first;
    private BlockGenerator _blockGen;
    
    public CopterGenerator(PlaySettings ps, float reloadTime, float missileVel, float missileRotVel,
        float missileLimit, int minCount, int maxCount, 
        RandomItem<int> carSizeDistr, RandomItem<int> carCountDistr)
    {
        _ps = ps;
        _reloadTime = reloadTime;
        _missileVel = missileVel;
        _missileRotVel = missileRotVel;
        _missileLimit = missileLimit;
        _minCount = minCount;
        _maxCount = maxCount;
        _blockGen = new BlockGenerator(ps,carSizeDistr,carCountDistr);
    }

    public bool HasMore => _count > 0;
    public void GenerateBlock(LinkedList<IDescriptorWithID> list)
    {
        _blockGen.GenerateBlock(list);
        --_count;
        if (_count == 0)
        {
            list.AddLast(new CopterRecallDescriptor());
        }
        else if (_first)
        {
            list.AddLast(new CopterDescriptor(_reloadTime, _missileVel, 
                _missileRotVel, _missileLimit, _ps.Road.MiddlePosition));
            _first = false;
        }
        
    }

    public bool WasBlockModifable => true;
    public Func<IDictionary<int, GameObject>, LinkedList<IDescriptorWithID>, float, bool> ReadinessTestHandler => null;
    public void Reset()
    {
        _count = Random.Range(_minCount, _maxCount + 1);
        _first = true;
        _blockGen.Reset();
    }
}
