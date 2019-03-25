using System;

public class DifficultController
{
    private float[] _milestones;
    private Action<PlaySettings,int> _rule;
    private int _i;
    
    public DifficultController(float[] milestones, Action<PlaySettings,int> rule)
    {
        if(milestones!=null)
            for (int i = 1; i < milestones.Length; i++)
                if(milestones[i-1]>=milestones[i]) throw new ArgumentException();
        _milestones = milestones;
        _rule = rule;
    }

    public bool TestReadiness(float distance)
    {
        return _milestones!=null && distance >= _milestones[_i];
    }
    
    public void IncDifficult(PlaySettings ps)
    {
        if (_rule == null) return;
        _rule.Invoke(ps, _i);
        ++_i;
    }

    public void Reset()
    {
        _i = 0;
    }
}
