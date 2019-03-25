using Code.Localization;

namespace Code.Gameplay.Tasks
{
    public class DistanceTask : ITask<int>
    {
        private readonly int _target;
        public string Name { get; }
        public int Cost { get; }

        public DistanceTask(int cost, string name,int target)
        {
            _target = target;
            Name = name;
            Cost = cost;
        }
        
        public TaskType Type => TaskType.Distance;
        public bool IsCompleted { get; private set; }
        public bool Test(int item)
        {
            IsCompleted = item >= _target;
            return IsCompleted;
        }

        public void Reset()
        {
            IsCompleted = false;
        }

        public override string ToString()
        {
            var lc = LocalizationController.Instance;
            return lc["distance"] + " " + lc["during"] + " " + _target + " " + lc["meters"];
        }
    }
}