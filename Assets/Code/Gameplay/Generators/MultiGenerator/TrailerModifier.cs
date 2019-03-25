using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class TrailerModifier : AbstractModifier
{
    private const float Gap = 0.1f;
    private readonly float _trailerLen;
    private PlaySettings _ps;
    
    public TrailerModifier(PlaySettings ps,float trailerLen, int count=1) : base(count)
    {
        _ps = ps;
        _trailerLen = trailerLen;
    }
    
    public override void Modify(LinkedList<IDescriptorWithID> list)
    {
        if(_trailerLen>_ps.MinCarGap) return;
        int count = PrepareTargetsBuffer(list,true);
        for (int i = 0; i < count; i++)
        {
            Vector2 pos = TargetsBuffer[i].Position;
            TargetsBuffer[i].Position = pos + new Vector2(0, _trailerLen/2 + Gap);
            pos.y -= _trailerLen / 2;
            list.AddLast(new TrailerDescriptor(pos, TargetsBuffer[i].Velocity));
        }
        CleanTargetsBuffer();
    }
}
