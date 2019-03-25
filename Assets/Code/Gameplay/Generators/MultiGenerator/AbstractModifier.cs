using System;
using System.Collections.Generic;

public abstract class AbstractModifier : IBlockModifier
{
    private int _count;
    private Random _random;
    protected CarDescriptor[] TargetsBuffer;

    protected AbstractModifier(int count)
    {
        if(count<1 || count>5) throw new ArgumentException("Bad count");
        _count = count;
        TargetsBuffer = new CarDescriptor[5];
        _random = new Random();
    }

    public abstract void Modify(LinkedList<IDescriptorWithID> list);

    protected int PrepareTargetsBuffer(LinkedList<IDescriptorWithID> list, bool onlySizeOne)
    {
        int cnt = 0;
        foreach (var v in list)
        {
            if (v is CarDescriptor && (v as CarDescriptor).IsModifable 
                && ( !onlySizeOne || (v as CarDescriptor).Size==1 ))
            {
                TargetsBuffer[cnt] = (CarDescriptor) v;
                cnt++;
            }
        }
        _random.Shuffle(TargetsBuffer,cnt);
        return cnt <= _count ? cnt : _count;
    }

    protected void CleanTargetsBuffer()
    {
        Array.Clear(TargetsBuffer,0,TargetsBuffer.Length);
    }
}
