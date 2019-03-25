
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildController
{
    private static BuildController _instance;
    public static BuildController GetInstance()
    {
        if(_instance==null) 
            _instance = new BuildController();
        return _instance;
    }

    private Dictionary<string, IBuilder> _builders;
    private Dictionary<int,GameObject> _forDiassemble;

    private BuildController()
    {
       _builders = new Dictionary<string, IBuilder>
        {
            ["Car"] = new VehicleBuilder(TrafficJumperController.GetInstance().CommonAssets),
            ["Meteor"] = new MeteorController(PursuitController.GetInstance().transform),
            ["OilPuddle"] = new OilBuilder(TrafficJumperController.GetInstance().CommonAssets),
            ["Wall"] = new WallBuilder(),
            ["Copter"] = new CopterBuilder()
        };
        _builders["OilEnd"] = _builders["OilPuddle"];
        _forDiassemble = new Dictionary<int, GameObject>(50);
        (_builders["Meteor"] as MeteorController).ForDisassemble = _forDiassemble;
        (_builders["OilPuddle"] as OilBuilder).ForDisassemble = _forDiassemble;
    }

    public GameObject Build(IDescriptorWithID desc)
    {
        var go = _builders[desc.GetTag()].Build(desc);
        if(go!=null)
            _forDiassemble.Add(go.GetHashCode(),go);
        return go;
    }

    public void Disassemble(GameObject go)
    {
        try
        {
            _builders[go.tag].Disassemble(go);
        }
        catch 
        {
            Debug.LogWarning(go.tag + go);
        }
    }

    public void DisassembleAllBelow(float zCoord)
    {
        var keys = new List<int>(_forDiassemble.Count);
        foreach (var e in _forDiassemble)
            if (e.Value.transform.position.z < zCoord)
            {
                keys.Add(e.Key);
                Disassemble(e.Value);
            }

        foreach (var key in keys)
            _forDiassemble.Remove(key);

    }

    public void RecallAll()
    {
        foreach (var b in _builders.Values)
            b.RecallAll();
        _forDiassemble.Clear();
    }

    public void SetStyle(StyleAssets style)
    {
        foreach (var b in _builders.Values)
            b.SetStyle(style);
    }

    public void SetMeteorController()
    {
		 Vehicle.AutoMeteorCar.MeteorCtrl = (MeteorController)_builders["Meteor"];
    }
}
