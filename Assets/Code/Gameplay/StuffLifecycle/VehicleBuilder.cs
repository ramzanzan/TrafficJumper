using System;
using Code.Gameplay.Descriptors;
using UnityEngine;

public class VehicleBuilder : IBuilder
{
    private GOPool _s1Pool, _s2Pool, _s3Pool, _pPool, _mPool, _tPool;
    private GOPool _projectilePool;
    private StyleAssets _style;
    private CommonAssets _commonAssets;
    
    public VehicleBuilder(CommonAssets commonAssets)
    {
        _commonAssets = commonAssets;
        _s1Pool = new GOPool();
        _s2Pool = new GOPool();
        _s3Pool = new GOPool();
        _pPool = new GOPool();
        _mPool = new GOPool();
        _tPool = new GOPool();
        _projectilePool = new GOPool();
        ProjectileGun.ProjectilePool = _projectilePool;
        Projectile.SelfPool = _projectilePool;
    }

    public void RecallAll()
    {
        _s1Pool.RecallAll();
        _s2Pool.RecallAll();
        _s3Pool.RecallAll();
        _pPool.RecallAll();
        _mPool.RecallAll();
        _tPool.RecallAll();
        _projectilePool.RecallAll();
    }

    public void SetStyle(StyleAssets style)
    {
        _style = style;
        _s1Pool.ResetTemplate(_style.Car1);
        _s2Pool.ResetTemplate(_style.Car2);
        _s3Pool.ResetTemplate(_style.Car3);
        _pPool.ResetTemplate(_style.Police);
        _mPool.ResetTemplate(_style.Military);
        _tPool.ResetTemplate(_style.Trailer);
        _projectilePool.ResetTemplate(_style.Projectile);

    }

    public GameObject Build(IDescriptorWithID desc)
    {
        if(!(desc is VehicleDescriptor)) throw new ArgumentException();

        GameObject go = null;
        Vehicle.VehicleBehaviour behaviour = null;
        Vehicle vehicle;
        var bt = Vehicle.BlockType.S1;
        var size = 0;
        
        if (desc is MilitaryDescriptor)
        {
            go = _mPool.Pop();
            _style.StylizeMilitary(go);
            behaviour = new Vehicle.MilitaryCar((MilitaryDescriptor) desc);
        }
        else if(desc is PoliceDescriptor)
        {
            go = _pPool.Pop();
            _style.StylizePolice(go);
            behaviour = new Vehicle.PoliceCar((PoliceDescriptor)desc);
            
        }else if(desc is CarDescriptor)
        {
            switch (((CarDescriptor)desc).Size)
            {
                case 1:
                    go = _s1Pool.Pop();
                    _style.StylizeCar1(go);
                    break;
                case 2:
                    go = _s2Pool.Pop();
                    _style.StylizeCar2(go);
                    bt = Vehicle.BlockType.S2;
                    size = 2;
                    break;
                case 3:
                    go = _s3Pool.Pop();
                    _style.StylizeCar3(go);
                    bt = Vehicle.BlockType.S3;
                    size = 3;
                    break;
            }
            
            if(desc is WobblingCarDescriptor)
                behaviour = new Vehicle.WobblingCar((WobblingCarDescriptor)desc);
            else if(desc is SlidingCarDescriptor)
                behaviour = new Vehicle.SlidingCar((SlidingCarDescriptor)desc);
            else if(desc is AutoMeteorCarDescriptor)
                behaviour = new Vehicle.AutoMeteorCar((AutoMeteorCarDescriptor)desc);
            else
                behaviour = new Vehicle.SimpleCar((CarDescriptor)desc);
        }
        else if(desc is TrailerDescriptor)
        {
            go = _tPool.Pop();
            _style.StylizeTrailer(go);
            behaviour = new Vehicle.Trailer((TrailerDescriptor)desc);
        }
        
        go.transform.position = ((VehicleDescriptor) desc).Position.MyVec2ToVec3();
        go.transform.rotation = Quaternion.identity;
        vehicle = go.GetComponent<Vehicle>();
        vehicle.Rezet();
        if(size==0) vehicle.InitSize();
        else vehicle.InitSize(bt,size);
        vehicle.InitBehaviour(behaviour);

        return go;
    }

    public void Disassemble(GameObject go)
    {
        if(!go.CompareTag("Car")) throw new ArgumentException();
        
        var veh = go.GetComponent<Vehicle>();
        veh.Rezet();

        if (veh.Behaviour is Vehicle.MilitaryCar)
            _mPool.Push(go);
        else if(veh.Behaviour is Vehicle.PoliceCar)
            _pPool.Push(go);
        else if(veh.Behaviour is Vehicle.Trailer)
            _tPool.Push(go);
        else if (veh.Behaviour is Vehicle.CarBehaviour)
        {
            switch (veh.Size)
            {
                case 1:
                    _s1Pool.Push(go);
                    break;
                case 2:
                    _s2Pool.Push(go);
                    break;
                case 3:
                    _s3Pool.Push(go);
                    break;
                default:
                    throw new ArgumentException();
            }
        }
        
        veh.Behaviour.Destruct();
    }
}
