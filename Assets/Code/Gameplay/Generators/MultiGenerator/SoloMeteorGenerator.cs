
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoloMeteorGenerator : IMiniGenerator
{
    private bool _firstNotSecondBlock;
    private RandomItem<int> _carAmountDistr;
    private PlaySettings SS;
    private float _minDelay;
    private float _maxDelay;
    private float _addVelocity;
    private BlockGenerator _blockGen;
    private float _time;
    private float _distance;

    public SoloMeteorGenerator(PlaySettings ss, float minDelay, float maxDelay, float addVelocity, 
        RandomItem<int> secondBlockCarAmntDistr)
    {
        SS = ss;
        _carAmountDistr = secondBlockCarAmntDistr;
        _blockGen = new BlockGenerator(SS,new RandomItem<int>(new[]{1,2}),_carAmountDistr);
        _minDelay = minDelay;
        _maxDelay = maxDelay;
        _addVelocity = addVelocity;

    }
    
    public bool HasMore { get; private set; }
    
    public void GenerateBlock(LinkedList<IDescriptorWithID> list)
    {
        if (_firstNotSecondBlock)
        {
            _blockGen.GenerateBlock(list, 0, 1);
            var car = (CarDescriptor)list.First.Value;
            list.RemoveFirst();
            _time = SS.PowerTime + Random.Range(_minDelay, _maxDelay);
            _distance = _time * _addVelocity;
            var amcar = new AutoMeteorCarDescriptor(car.Position,car.Velocity,_addVelocity,
                _distance+car.Velocity*_time,_time );
            list.AddLast(amcar);
            _firstNotSecondBlock = false;
            HasMore = true;
        }
        else
        {
            var tmp = SS.MinJumpDistance;
            //todo костыльчик
            SS.MinJumpDistance = SS.MaxJumpDistance * .9f;
            var car = (CarDescriptor) list.First.Value;
            Vector2 pos = car.Position;
            pos.y += _distance;
            car.Position = pos;
            _blockGen.GenerateBlock(list);
            SS.MinJumpDistance = tmp;
            foreach (CarDescriptor c in list)
                c.Velocity = car.Velocity;
            HasMore = false;
        }
    }

    public bool WasBlockModifable => false;
    
    public void Reset()
    {
        _firstNotSecondBlock = true;
        HasMore = true;
        _blockGen.Reset();
    }

    public Func<IDictionary<int, GameObject>, LinkedList<IDescriptorWithID>, float, bool> ReadinessTestHandler => null;
}
