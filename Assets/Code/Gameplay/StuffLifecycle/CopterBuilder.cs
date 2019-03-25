using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class CopterBuilder : IBuilder
{
    private Queue<GameObject> _copterQueue;
    private GOPool _copterPool;
    private GOPool _missilePool;
    private StyleAssets _style;

    public CopterBuilder()
    {
        _copterPool = new GOPool();
        _missilePool = new GOPool();
        _copterQueue = new Queue<GameObject>();
        Copter.MissilePool = _missilePool;
        HomingMissile.SelfPool = _missilePool;
    }

    public void RecallAll()
    {
        _copterPool.RecallAll();
        _missilePool.RecallAll();
    }
    
    public void SetStyle(StyleAssets style)
    {
        _style = style;
        _copterPool.ResetTemplate(style.Copter);
        _missilePool.ResetTemplate(style.Missile);
    }
    
    public GameObject Build(IDescriptorWithID desc)
    {
        if(!(desc is CopterDescriptor || desc is CopterRecallDescriptor)) 
            throw new ArgumentException();
        if (desc is CopterRecallDescriptor)
        {
            _copterQueue.Dequeue().GetComponent<Copter>().Recall(this);
        }
        else
        {
            var go = _copterPool.Pop();
            _style.StylizeCopter(go);
            var copter = go.GetComponent<Copter>();
            var d = (CopterDescriptor) desc;
            copter.Init(d.ReloadTime,d.MissileVelocity,
                d.MissileRotationVelocity,d.MissileLimit,d.MiddlePosX);
            copter.TakePosition();
            _copterQueue.Enqueue(go);
            return go;
        }
        return null;
    }

    public void Disassemble(GameObject go)
    {
        if(!go.CompareTag("Copter")) throw new ArgumentException();
        foreach (Transform missile in go.transform)
        {
            Copter.MissilePool.Push(missile.gameObject);
        }
    }
}

