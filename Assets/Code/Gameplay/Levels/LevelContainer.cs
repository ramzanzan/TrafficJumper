using System.Collections;
using System.Collections.Generic;
using Code.Gameplay.Tasks;
using UnityEngine;

public class LevelContainer
{
    private static LevelContainer _instance;

    public static LevelContainer GetInstance()
    {
        return _instance ?? (_instance = new LevelContainer());
    }
    
    public IDictionary<string, Level> Levels;

    protected LevelContainer()
    {
        Levels = new Dictionary<string, Level>(10);
        PlaySettings sets;
        DifficultController dc;
        MultiGenerator gen;
        Transform pc = PursuitController.GetInstance().transform;
        string name;
        Level lvl;
        ITask[] tasks;
    
//tests______
        name = "test";
        sets = new PlaySettings();
        dc = new DifficultController(null,null);
        gen = new MultiGenerator(100, 100,
            new RandomItem<IMiniGenerator>(new IMiniGenerator[]
            {
//                new BlockGenerator(sets, new RandomItem<int>(new[] {1, 2, 3}, new[] {.5f, .3f, .2f}),
//                    new RandomItem<int>(new[] {1, 2, 3, 4})),
                new BlockGenerator(sets,new RandomItem<int>(new []{1}), new RandomItem<int>(new []{1,2,3})), 
//                new BlockGenerator(sets,new RandomItem<int>(new []{2,3}), new RandomItem<int>(new []{2,3,4})), 
            }),
            1f, 5,
            new RandomItem<IBlockModifier>(new IBlockModifier[]
            {
//                new TrailerModifier(sets,1.2f),
                new WobbleModifier(sets, 3, 6,1f), 
//                new MilitariModifier(2,4,2,60,3.4f), 
            })); 
        lvl = new Level(sets,dc,name,gen);
        Levels.Add(name,lvl);
        
//main______
        name = "main";
        sets = new PlaySettings();
        dc = new DifficultController(null,null);
        gen = new MultiGenerator(100, 100,
            new RandomItem<IMiniGenerator>(new IMiniGenerator[]
            {
                new SlidingCarGenerator(sets,4,4,25,45,.7f,1.1f), 
                new BlockGenerator(sets, new RandomItem<int>(new[] {1,2,3},new []{.5f,.3f,.2f}), new RandomItem<int>(new[] {1,2,3,4})),
                new BlockGenerator(sets, new RandomItem<int>(new[] {1,2,3},new []{.5f,.3f,.2f}), new RandomItem<int>(new[] {1,2,3,4})),
                new PoliceGenerator(sets,2,sets.MaxJumpDistance,.35f),
                new MeteorsGenerator(sets, new RandomItem<int>(new[] {1,2,3}), new RandomItem<int>(new[] {2,3,4},new[]{.5f,.3f,.2f}),
                    new RandomItem<int>(new[]{3}),8,8,pc), 
                new SoloMeteorGenerator(sets, .5f, 1, 2, new RandomItem<int>(new[]{1,2,3})),
                new WallGenerator(sets,new RandomItem<int>(new[]{1,2}),new RandomItem<int>(new[]{1,2}),
                    2.5f,2.5f,3f,3f,.5f,10,15,false), 
                //new CopterGenerator
            }),
            1f, 1, 
            new RandomItem<IBlockModifier>(new IBlockModifier[]
            {
                new TrailerModifier(sets,1.2f),
                new WobbleModifier(sets, 5, 5,.5f), 
                new MilitariModifier(2,4,2,60,3.4f), 
            }));
        gen.InitGenerator = new BlockGenerator(sets, new RandomItem<int>(new[] {1,2,3},new []{.5f,.3f,.2f}), new RandomItem<int>(new[] {1,2,3,4}));
        lvl = new Level(sets,dc,name,gen);
        Levels.Add(name,lvl);
        
//company______
        name = "l01";
        sets = new PlaySettings();
        dc = new DifficultController(null,null);
        gen = new MultiGenerator(100, 100,
            new RandomItem<IMiniGenerator>(new IMiniGenerator[]
            {
                new BlockGenerator(sets, new RandomItem<int>(new[] {1,2,3},new []{.5f,.3f,.2f}),
                    new RandomItem<int>(new[] {1,2,3,4}, new[]{.4f,.3f,.15f,.15f})),
            }),
            .25f, 1, 
            new RandomItem<IBlockModifier>(new IBlockModifier[]
            {
                new TrailerModifier(sets,1.2f),
                new WobbleModifier(sets, 5,5,.5f), 
            }));
        gen.InitGenerator = new BlockGenerator(sets, new RandomItem<int>(new[] {1,2,3},new []{.5f,.3f,.2f}), new RandomItem<int>(new[] {1,2,3,4}));
        tasks = new ITask[]
        {
            new JumpOrEatTask(10,"l01t1", true, 3, "Trailer"),
            new JumpOrEatTask(10,"l01t2", false, 3, "Wobbling"),
            new DuringDistanceTask(15,"l01t3", 30, true, 1,3 ), 
            new JumpBackTask(20,"l01t4", 3),
            new DistanceTask(30,"l01t5", 50),
        };
        lvl = new Level(sets,dc,name,gen,tasks);
        Levels.Add(name,lvl);
        
//mocks_____
        lvl = new Level(sets,dc,"l02",gen,null); 
        Levels.Add("l02",lvl);
        lvl = new Level(sets,dc,"l03",gen,null); 
        Levels.Add("l03",lvl);
    }
}
