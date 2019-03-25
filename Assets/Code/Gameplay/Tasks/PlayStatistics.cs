using System.Collections.Generic;

public class PlayStatistics
{
    private Dictionary<string, int> _stats;

    public int this[string name]
    {
        get { return _stats[name]; }
        set { _stats[name] = value; }
    }

    public PlayStatistics()
    {
        _stats = new Dictionary<string, int>
        {
            {"jumps",0},
            {"meals",0}
        };
    }

    public void Reset()
    {
        foreach (var key in _stats.Keys)
            _stats[key] = 0;
    }
}
