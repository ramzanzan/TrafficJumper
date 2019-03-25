using System;
using System.Collections.Generic;
using System.Linq;
using Code.Localization;

namespace Code.Gameplay.Tasks
{
    public class JumpOrEatTask : ITask<Vehicle>
    {
        public TaskType Type { get; }
        private readonly int _target;
        private int _current;
        private string _tag;
        private int _size;
        public string Name { get; }
        public int Cost { get; }


        public JumpOrEatTask(int cost,string name, bool jumpNotEat, int target, string tag)
        {
            Cost = cost;
            if(target<1) throw new ArgumentException();
            Type = jumpNotEat ? TaskType.Jump : TaskType.Eat;
            _tag = tag;
            _target = target;
            Name = name;
        }

        public JumpOrEatTask(string name, bool jumpNotEat, int target, int size)
        {
            if(size<1 || size>3) throw new ArgumentException();
            Type = jumpNotEat ? TaskType.Jump : TaskType.Eat;
            _size= size;
            _target = target;
            Name = name;
        }

        public bool IsCompleted { get; private set; }

        public bool Test(Vehicle item)
        {
            bool res; 
            res = _size != 0 ? 
                _size == item.Size :
                _tag == item.Behaviour.Tag;
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
            var verb = Type == TaskType.Jump ? lc["jump_on"] : lc["eat"];
            if(_size==0)
                return verb + " " + _target + " " + lc[_tag] + " " + lc["cars"];
            else
            {
                var size = _size == 1 ? lc["s_size"] : _size == 2 ? lc["m_size"] : lc["l_size"];
                return verb + " " + _target + " " + size + " " + lc["cars"];
            }
        }
    }
}