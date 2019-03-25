using Code.Localization;

namespace Code.Gameplay.Tasks
{
    public class JumpBackTask : ITask<Vehicle>
    {
        private Vehicle _prevVeh;
        private readonly int _target;
        private int _current;
        public string Name { get; }
        public int Cost { get; }

        public JumpBackTask(int cost,string name,int target)
        {
            Cost = cost;
            _target = target;
            Name = name;
        }

        public TaskType Type => TaskType.JumpBack;
        public bool IsCompleted { get; private set; }
        
        public bool Test(Vehicle item)
        {
            if (_prevVeh == null)
            {
                _prevVeh = item;
                return false;
            }
            var res = item.transform.position.z < _prevVeh.transform.position.z;
            _prevVeh = item;
            if (res) ++_current;
            IsCompleted = _current >= _target;
            return res;
        }

        public void Reset()
        {
            _current = 0;
            IsCompleted = false;
        }

        public override string ToString()
        {
            var lc = LocalizationController.Instance;
            return lc["jump_back1"] + " " + _target + " " + lc["jump_back2"];
        }
    }
}