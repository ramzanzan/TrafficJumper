using System.Collections.Generic;
using Code.Gameplay.Tasks;

public class Level
{
    public readonly string Name;
    public readonly ITask[] Tasks;
    public readonly PlaySettings PlaySettings;
    public readonly DifficultController DifficultCtrl;
    public readonly MultiGenerator MultiGenerator;

    public Level(PlaySettings ps, DifficultController dc, string name, MultiGenerator gen,  params ITask[] tasks)
    {
        Name = name;
        PlaySettings = ps;
        DifficultCtrl = dc;
        Tasks = tasks;
        MultiGenerator = gen;
    }

    public void Reset()
    {
        PlaySettings.Reset();
        DifficultCtrl.Reset();
    }
}
