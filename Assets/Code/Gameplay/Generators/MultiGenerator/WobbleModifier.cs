using System;
using System.Collections.Generic;
using Code.Gameplay.Descriptors;

public class WobbleModifier : AbstractModifier
{
    private float _pointerNotAvatarContactProb;
    private float _halfPeriodLenMin, _halfPeriodLenMax;
    private PlaySettings _ps;
    
    public WobbleModifier(PlaySettings ps, float halfPeriodLenMin, float halfPeriodLenMax, 
        float pointerNotAvatarContactProb, int count=1) : base(count)
    {
        if(pointerNotAvatarContactProb<0 || pointerNotAvatarContactProb>1)
            throw new ArgumentException("Bad probability");
        _halfPeriodLenMin = halfPeriodLenMin;
        _halfPeriodLenMax = halfPeriodLenMax;
        _pointerNotAvatarContactProb = pointerNotAvatarContactProb;
        _ps = ps;

    }

    public override void Modify(LinkedList<IDescriptorWithID> list)
    {
        int cnt = PrepareTargetsBuffer(list,true);
        for (int i = 0; i < cnt; i++)
        {
            var car = TargetsBuffer[i];
            var onPtr = _pointerNotAvatarContactProb - UnityEngine.Random.Range(0, 1) > 0;
            var wcar = new WobblingCarDescriptor(car.Position, car.Velocity, 
                car.Size, UnityEngine.Random.Range(_halfPeriodLenMin,_halfPeriodLenMax), _ps.MinCarSpeed, onPtr);
            list.Remove(car);
            list.AddLast(wcar);
        }
        CleanTargetsBuffer();
    }
}
