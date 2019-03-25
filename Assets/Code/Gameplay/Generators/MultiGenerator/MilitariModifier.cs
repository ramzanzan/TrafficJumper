using System.Collections.Generic;

public class MilitariModifier : AbstractModifier
{
    private float _reloadTime;
    private float _rotSpeed;
    private float _shellSpeed;
    private float _radius;
    
    public MilitariModifier(int count, float radius, float reloadTime, 
        float rotSpeed, float shellSpeed) : base(count)
    {
        _reloadTime = reloadTime;
        _rotSpeed = rotSpeed;
        _shellSpeed = shellSpeed;
        _radius = radius;
    }

    public override void Modify(LinkedList<IDescriptorWithID> list)
    {
        int cnt = PrepareTargetsBuffer(list,true);
        for (int i = 0; i < cnt; i++)
        {
            var car = TargetsBuffer[i];
            var mcar = new MilitaryDescriptor(car.Position, car.Velocity,_radius,_reloadTime,_rotSpeed,_shellSpeed);
            list.Remove(car);
            list.AddLast(mcar);
        }
        CleanTargetsBuffer();
    }
}
