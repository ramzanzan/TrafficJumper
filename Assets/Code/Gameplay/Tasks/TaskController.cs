using System;
using System.Collections;
using System.Collections.Generic;
using Code.Gameplay.Tasks;
using UnityEngine;
using static Code.Gameplay.Tasks.TaskType;
using static ProgressData.DataType;

public class TaskController : MonoBehaviour
{
    private static TaskController _instance;

    public static TaskController GetInstance()
    {
        if (_instance == null) throw new Exception("Сlasses initialization order have violated");
        return _instance;
    }

    private PlayStatistics _stats;
    [NonSerialized]
    private ProgressData _globalProgress;
    private Level _level;
    private Dictionary<TaskType, ITask> _tasks;
    private PlayerAvatar _avatar;
    public PlayerAvatar Avatar
    {
        get { return _avatar; }
        set
        {
            _avatar = value;
            _avatar.OnLanding += LandingHandler;
            _avatar.OnSuccsefulEating += EatingHandler;
        }
    }
    public event Action<ITask> TaskCompleted;

    private void Start()
    {
        if (_instance != null) throw new Exception("Second singleton");
        _instance = this;
    }

    public void Initialize()
    {
        _globalProgress = ProgressData.GetInstance();
        _tasks = new Dictionary<TaskType, ITask>();
    }

    public void SetLevel(Level lvl)
    {
        _level = lvl;
        _tasks.Clear();
        foreach (var t in lvl.Tasks)
            if (!_globalProgress[Task,t.Name])
                _tasks.Add(t.Type, t);
    }

    public void StartLevel()
    {
        foreach (var p in _tasks)
            p.Value.Reset();
    }

    private void LandingHandler()
    {
        ITask task;
        if (_tasks.TryGetValue(Jump, out task)
            && (task as ITask<Vehicle>).Test(_avatar.Vehicle)
            && task.IsCompleted)
        {
            _globalProgress[Task,task.Name] = true;
            _tasks.Remove(Jump);
            TaskCompleted(task);
        }

        if (_tasks.TryGetValue(JumpBack, out task)
            && (task as ITask<Vehicle>).Test(_avatar.Vehicle)
            && task.IsCompleted)
        {
            _globalProgress[Task,task.Name] = true;
            _tasks.Remove(JumpBack);
            TaskCompleted(task);
        }

        if (_tasks.TryGetValue(JumpLines, out task)
            && (task as ITask<Vehicle>).Test(_avatar.Vehicle)
            && task.IsCompleted)
        {
            _globalProgress[Task,task.Name] = true;
            _tasks.Remove(JumpLines);
            TaskCompleted(task);
        }

        //заодно и дистанцию
        if (_tasks.TryGetValue(Distance, out task)
            && (task as ITask<int>).Test((int) GameplayController.GetInstance().TraveledDistance)
            && task.IsCompleted)
        {
            _globalProgress[Task,task.Name] = true;
            _tasks.Remove(Distance);
            TaskCompleted(task);
        }
    }

    private void EatingHandler()
    {
        ITask task;
        if (_tasks.TryGetValue(Eat, out task)
            && (task as ITask<Vehicle>).Test(_avatar.Vehicle)
            && task.IsCompleted)
        {
            _globalProgress[Task,task.Name] = true;
            _tasks.Remove(Eat);
            TaskCompleted(task);
        }
    }

    private void JumpOverHandler(GameObject obj)
    {
        //todo
    }

    
    

    
}
