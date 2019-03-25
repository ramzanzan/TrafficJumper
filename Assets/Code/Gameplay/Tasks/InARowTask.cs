using System;
using Code.Localization;
using static Code.Gameplay.Tasks.TaskType;

namespace Code.Gameplay.Tasks
{
    public class InARowTask<T> : ITask<T>
    {
        private readonly ITask<T> _task;
        public string Name => _task.Name;
        public int Cost => _task.Cost;

        public InARowTask(ITask<T> task)
        {
            _task = task;
            if(Type!=Jump || Type!= JumpBack || Type!=JumpOver) throw new ArgumentException();
        }

        public TaskType Type => _task.Type;
        public bool IsCompleted { get; private set; } 
        public bool Test(T item)
        {
            var res = _task.Test(item);
            if (res) IsCompleted=_task.IsCompleted;
            else _task.Reset();
            return res;
        }

        public void Reset()
        {
            _task.Reset();
        }

        public override string ToString()
        {
            var lc = LocalizationController.Instance;
            return _task + " " + lc["row"];
        }
    }
}