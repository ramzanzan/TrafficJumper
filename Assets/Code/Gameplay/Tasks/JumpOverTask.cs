using System.Collections.Generic;
using Code.Localization;
using UnityEngine;

namespace Code.Gameplay.Tasks
{
    public class JumpOverTask : ITask<GameObject>
    {
        private readonly string _tag;
        private readonly int _target;
        private int _current;
        public string Name { get; }
        public int Cost { get; }

        public JumpOverTask(int cost, string name, int target, string tag)
        {
            Cost = cost;
            _target = target;
            _tag = tag;
            Name = name;
        }
        
        public TaskType Type => TaskType.JumpOver;
        
        public bool IsCompleted { get; private set; }
        
        public bool Test(GameObject item)
        {
            bool res;
            if (item.CompareTag("Car"))
                res = item.GetComponent<Vehicle>().Behaviour.Tag==_tag;
            else 
                res = item.CompareTag(_tag);
            if (res) ++_current;
            IsCompleted = _current >= _target;
            return res;
        }

        public void Reset()
        {
            IsCompleted = false;
            _current = 0;
        }

        public override string ToString()
        {
            var lc = LocalizationController.Instance;
            return lc["jump_over"] + " " + _target + " " + lc[_tag] + " " + lc["cars"];
        }
    }
}