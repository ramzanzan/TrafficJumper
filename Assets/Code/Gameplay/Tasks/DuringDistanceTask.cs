using System;
using System.Linq;
using System.Text;
using Code.Localization;
using UnityEngine;

namespace Code.Gameplay.Tasks
{
    public class DuringDistanceTask: ITask<Vehicle>
    {
        public static Transform Avatar;
        private readonly float _distance;
        private readonly string _tag;
        private readonly int[] _nums;
        private float _start;
        public string Name { get; }
        public int Cost { get; }

        private DuringDistanceTask(int cost, string name, float distance)
        {
            _distance = distance;
            Cost = cost;
            Name = name;
        }

        public DuringDistanceTask(int cost, string name, float distance, string tag) : this(cost, name, distance)
        {
            _tag = tag;
            Type = TaskType.Jump;
        }

        public DuringDistanceTask(int cost, string name, float distance, bool linesNotSize, params int[] nums) : 
            this(cost,name,distance)
        {
            if (linesNotSize)
            {
                if(nums.Min()<0 || nums.Max()>4) throw new ArgumentException();
                Type = TaskType.JumpLines;
            }
            else
            {
                if(nums.Min()<1 || nums.Max()>3 || nums.Length!=1) throw new ArgumentException();
                Type = TaskType.Jump;
            }

            _nums = nums;
        }

        public TaskType Type { get; }
        public bool IsCompleted { get; private set; }
        public bool Test(Vehicle item)
        {
            bool res;
            if (Type == TaskType.Jump)
                res = item.Behaviour.Tag == _tag;
            else
                res = _nums.Contains(Road.Instance.LineNumFromPosX(Avatar.position.x));
            if (res)
            {
                if (_start == 0)
                    _start = Avatar.position.z;
                else
                    IsCompleted = Avatar.position.z - _start >= _distance;
            }
            else
                _start = 0;
            return IsCompleted;
        }

        public void Reset()
        {
            _start = 0;
            IsCompleted = false;
        }

        public override string ToString()
        {
            var lc = LocalizationController.Instance;
            var sb = new StringBuilder();
            if (Type == TaskType.JumpLines)
            {
                sb.Append(lc["jump_only_lines"]).Append(" ");
                for (int i = 0; i < _nums.Length; ++i)
                {
                    if (i == 0)
                        sb.Append(_nums[0]);
                    else if (i != _nums.Length - 1)
                        sb.Append(", ").Append(_nums[i]);
                    else
                        sb.Append(" ").Append(lc["and"]).Append(" ").Append(_nums[i]);
                }
            }
            else
            {
                sb.Append(lc["jump_only_cars"]).Append(" ").Append(lc[_tag]).Append(" ").Append(lc["cars"]);
            }
            sb.Append(" ").Append(lc["during"]).Append(" ").Append(_distance).Append(" ").Append(lc["meters"]);
            return sb.ToString();
        }
    }
}