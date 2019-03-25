using System;
using static ProgressData.DataType;

public class LocalStore
{
    public static bool Buy(ProgressData.DataType type, string name, int cost)
    {
        if(type!=Skin && type!=Task) throw new ArgumentException();
        var gs = GlobalStatistics.GetInstance();
        if (gs.Score < cost) return false;
        gs.Score -= cost;
        ProgressData.GetInstance()[type, name] = true;
        return true;
    }
}
