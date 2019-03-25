using System;
using System.Collections.Generic;
using UnityEngine;

public class MultiGeneratorProvider : IDescriptorProvider
{
    private MultiGenerator _generator;
    private IDictionary<int,GameObject> _items;
    private IEnumerator<IDescriptorWithID> _iter;
    private Func<IDictionary<int, GameObject>, LinkedList<IDescriptorWithID>, float, bool> _readinessTestHandler;
    private PlaySettings _ps;
    private PursuitController _pursuitCtrl;
    private IDGenerator _idGen;
    
    public MultiGeneratorProvider(IDictionary<int,GameObject> items, PursuitController pursuitCtrl, IDGenerator idGen)
    {
        _items = items;
        _pursuitCtrl = pursuitCtrl;
        _idGen = idGen;
    }

    public void SetGenerator(MultiGenerator generator)
    {
        _generator = generator;
        _generator.IDGen = _idGen;
        _ps = generator.PS;
    }
    
    //todo front pos
    public void Reset(CarDescriptor enter)
    {
        _generator.UseInitGen = true;
        _generator.Reset();
        _readinessTestHandler = _generator.ReadinessTestHandler ?? DefaultReadinessTest;
        _generator.DescriptorList.Clear();
        _generator.DescriptorList.AddLast(enter);
    }

    public static bool DefaultReadinessTest(IDictionary<int, GameObject> items, LinkedList<IDescriptorWithID> list, float boundary)
    {
        var ready = false;
        foreach (var e in list)
        {
            if (!(e is VehicleDescriptor)) continue;
            GameObject car;
            if(!items.TryGetValue(e.GetID(),out car)) continue;
            //todo redo на гм сс + boundary + max
            //todo 1.2 - standard car gap
            ready |= car.transform.position.z + 1.2f < boundary;
        }
        return ready;
    }
    
    public bool IsReadyForMore()
    {
        var ready = 
            _readinessTestHandler(_items, _generator.DescriptorList, _pursuitCtrl.ScreenTop());
        if (ready)
        {
            ResetDescriptors();
            _generator.Generate();
            _iter = _generator.DescriptorList.GetEnumerator();
        }
        return ready;
    }

    private void ResetDescriptors()
    {
        foreach (var e in _generator.DescriptorList)
        {
            if (!(e is VehicleDescriptor)) continue;
            GameObject car;
            var desc = (VehicleDescriptor)e;
            if(!_items.TryGetValue(e.GetID(),out car)) continue;
            desc.Position.x = car.transform.position.x;
            desc.Position.y = car.transform.position.z;
        }
    }

    public bool HasNext()
    {
        return _iter.MoveNext();
    }

    public IDescriptorWithID Next()
    {
        return _iter.Current;
    }

    public bool HasMore()
    {
        if (!_generator.HasMore()) return false;
        _readinessTestHandler = _generator.ReadinessTestHandler ?? DefaultReadinessTest;
        return true;
    }
    
    //todo ?
    public void SetIDGenerator(IDGenerator gen)
    {
        _idGen = gen;
    }
}
