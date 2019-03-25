using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.Gameplay.Tasks
{
    public interface ITask
    {
        TaskType Type { get; }
        bool IsCompleted { get; } 
        void Reset();
        string Name { get; }
        int Cost { get; }
    }
    
    public interface ITask<T> : ITask
    {
        bool Test(T item);
    }
}
