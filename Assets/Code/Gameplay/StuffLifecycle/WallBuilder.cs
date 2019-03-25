using System;
using UnityEngine;

public class WallBuilder : IBuilder
{
    private GOPool _wallPool;
    private StyleAssets _style;
    
    public WallBuilder()
    {
        _wallPool = new GOPool();
    }

    public void RecallAll()
    {
        _wallPool.RecallAll();
    }
    
    public void SetStyle(StyleAssets style)
    {
        _style = style;
        _wallPool.ResetTemplate(style.Wall);
    }
    
    public GameObject Build(IDescriptorWithID desc)
    {
        if(!(desc is WallDescriptor)) throw new ArgumentException();
        var wall = _wallPool.Pop();
        _style.StylizeWall(wall);
        var d = (WallDescriptor) desc;
        wall.transform.position = d.Position.MyVec2ToVec3();
        var scale = wall.transform.localScale;
        scale.z = d.Length;
        wall.transform.localScale = scale;
        return wall;
    }

    public void Disassemble(GameObject go)
    {
        _wallPool.Push(go);
    }
}
