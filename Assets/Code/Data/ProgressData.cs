using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class ProgressData
{
    public enum DataType
    {
        Level,
        Task,
        Skin
    }
    private static ProgressData _instance;

    public static ProgressData GetInstance()
    {
        //todo проверка, первая ли загрузка игры, и нужно ли загружать или создавать прогресс
        return _instance ?? (_instance = new ProgressData());
    }

    public event Action<DataType, string, bool> DataChanged;
    private Dictionary<DataType, Dictionary<string, bool>> Data;
    private Dictionary<string, bool> Levels;
    private Dictionary<string, bool> Tasks;
    private Dictionary<string, bool> Skins;
    private Dictionary<string, bool> Styles;

    protected ProgressData()
    {
        Levels = new Dictionary<string, bool>();
        Tasks = new Dictionary<string, bool>();
        foreach (var p in LevelContainer.GetInstance().Levels)
        {
            Levels.Add(p.Key, false);
            if(p.Value.Tasks==null) continue;
            foreach(var t in p.Value.Tasks) 
                Tasks.Add(t.Name,false);
        }
        Skins = new Dictionary<string, bool>();
        foreach (var skin in SkinsContainer.GetInstance().Skins)
        {
            Skins.Add(skin.Skin.name,false);
        }
        //todo
        Data = new Dictionary<DataType, Dictionary<string, bool>>
        {
            {DataType.Level,Levels},
            {DataType.Task,Tasks},
            {DataType.Skin,Skins},
        };
        
        
        //special
        Levels["main"] = true;
    }
    
    public bool this[DataType type, string name]
    {
        get { return Data[type][name]; }
        set
        {
            Data[type][name] = value;
            DataChanged?.Invoke(type, name, value);

            if (type == DataType.Task && value)
            {
                var res = true;
                var tsks = LevelContainer.GetInstance().Levels[name.Substring(0, 3)].Tasks;
                foreach (var t in tsks)
                    res &= this[DataType.Task, t.Name];
                if (res) this[DataType.Level, name.Substring(0, 3)] = true;
            }
        }
    }


}