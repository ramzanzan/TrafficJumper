using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using static GenUtils;
using Random = UnityEngine.Random;

public class PoliceGenerator : IMiniGenerator
{
    private float _sandvichProb;
    private bool _firstNotSecondBlock, _third;
    private bool _sandvich;
    private readonly PlaySettings SS;
    private BlockGenerator _blockGen;
    private float _sideSpeed;
    private float _radius;
    
    public bool HasMore { get; private set; }

    public PoliceGenerator(PlaySettings ss, float sideSpeed, float radius, float sandvichProb = 0)
    {
        SS=ss;
        _sandvichProb = sandvichProb;
        _blockGen=new BlockGenerator(SS,new RandomItem<int>(new[]{1}),null);
        _sideSpeed = sideSpeed;
        _radius = radius;

    }

    public void GenerateBlock(LinkedList<IDescriptorWithID> list)
    {
        if (_firstNotSecondBlock)
        {
            if (_sandvichProb-Random.Range(0,1f)<=0)
            {
                _blockGen.GenerateBlock(list, 0, Random.Range(1, 4));
            }
            else
            {
                _sandvich = true;
                _blockGen.GenerateBlock(list, 0xAF, Random.Range(1,3));
            }
            _firstNotSecondBlock = false;
        }
        else if(!_third)
        {
            if (!_sandvich)
            {
                byte mask = 0;
                foreach (var car in list)
                    mask |= MaskLine(SS.Road.LineNumFromPosX(((CarDescriptor) car).Position.x));
                _blockGen.GenerateBlock(list,mask,Random.Range(2,4));
                var maxVel = list.Max<IDescriptorWithID>((e) => (e as CarDescriptor).Velocity);
                var polNum = Random.Range(0,list.Count);
                var i = list.Count-1;
                CarDescriptor mem = null;
                foreach (CarDescriptor car in list)
                {
                    if (i == polNum) 
                        mem = car;
                    car.Velocity = maxVel;
                    i--;
                }
                list.Remove(mem);
                list.AddLast(new PoliceDescriptor(mem.Position, maxVel,_sideSpeed,_radius));
            }
            else
            {
                _blockGen.GenerateBlock(list,0xDF,1);
                CarDescriptor c = (CarDescriptor) list.First();
                Vector2 pos = c.Position;
                pos.x -= SS.Road.LineWidth * 2;
                list.AddLast(new PoliceDescriptor(pos, c.Velocity,_sideSpeed,_radius));
                pos.x += SS.Road.LineWidth * 4;
                list.AddLast(new PoliceDescriptor(pos, c.Velocity,_sideSpeed,_radius));
            }

            _third = true;
        }
        else
        {
            var node = list.First;
            while (node!=null)
            {
                var desc = node.Value;
                node = node.Next;
                if (desc is PoliceDescriptor)
                    list.Remove(desc);
            }
            _blockGen.GenerateBlock(list,0,Random.Range(1,4));
            HasMore = false;
        }
    }

    public bool WasBlockModifable => !HasMore;

    public void Reset()
    {
        HasMore = true;
        _sandvich = false;
        _firstNotSecondBlock = true;
        _third = false;
        _blockGen.Reset();
    }

    public Func<IDictionary<int, GameObject>, LinkedList<IDescriptorWithID>, float, bool> ReadinessTestHandler => null;
}