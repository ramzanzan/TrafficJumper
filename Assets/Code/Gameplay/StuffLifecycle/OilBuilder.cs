using System;
using System.Collections.Generic;
using UnityEngine;

public class OilBuilder : IBuilder
{
    private const float DefaultPosY = .1f;
    private GOPool _firstPool;
    private GOPool _transitPool;
    private GOPool _endPool;
    private float _endPosY;
    private StyleAssets _style;
	public Dictionary<int, GameObject> ForDisassemble;
    
    public OilBuilder(CommonAssets commonData)
    {
        _firstPool = new GOPool();
        _transitPool = new GOPool();
        _endPosY = commonData.OilEnd.transform.position.y;
        _endPool = new GOPool(commonData.OilEnd);
    }

    public void RecallAll()
    {
        _firstPool.RecallAll();
        _transitPool.RecallAll();
        _endPool.RecallAll();
    }
    
    public void SetStyle(StyleAssets style)
    {
        _style = style;
        _firstPool.ResetTemplate(style.FirstPuddle);
        _transitPool.ResetTemplate(style.TransitPuddle);
        _endPool.RecallAll();
        
    }
    
    public GameObject Build(IDescriptorWithID desc)
    {
        GameObject puddle = null;
        if(!(desc is OilPuddleDescriptor || desc is OilEndDescriptor)) throw new ArgumentException();
        if (desc is OilPuddleDescriptor)
        {
            var d = (OilPuddleDescriptor) desc;
            switch (d.State)
            {
                case OilPuddleDescriptor.PosState.First: 
                    puddle = _firstPool.Pop();
                    _style.StylizeFirstPuddle(puddle);
                    break;
                case OilPuddleDescriptor.PosState.Last: 
                    puddle = _transitPool.Pop();
                    _style.StylizeLastPuddle(puddle);
                    break;
                case OilPuddleDescriptor.PosState.Transitional:
                    puddle = _transitPool.Pop();
                    _style.StylizeTransitPuddle(puddle);
                    break;
                default: throw new ArgumentOutOfRangeException();
            }
            puddle.transform.position = d.Position.MyVec2ToVec3(DefaultPosY);
            var scale = new Vector3(d.Width,d.Length,1);
            puddle.transform.localScale = scale;
        }
        else
        {
            var end = _endPool.Pop();
            end.transform.position = ((OilEndDescriptor) desc).Position.MyVec2ToVec3(_endPosY);
            ForDisassemble.Add(end.GetHashCode(),end);
        }

        return puddle;
    }

    public void Disassemble(GameObject go)
    {
        if (go.CompareTag("OilPuddle"))
        {
            if(go.GetComponent(typeof(BoxCollider))==null)
                _transitPool.Push(go);
            else
                _firstPool.Push(go);
        }
        else
            _endPool.Push(go);
    }
}
