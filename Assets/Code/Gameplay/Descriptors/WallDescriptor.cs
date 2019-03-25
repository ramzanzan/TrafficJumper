using UnityEngine;

public class WallDescriptor : IDescriptorWithID
    {
        private const string Tag = "Wall";
        public string GetTag()
        {
            return Tag;
        }
        
        private int _id;
        public Vector2 Position;
        public float Length;

        public WallDescriptor(Vector2 pos, float len)
        {
            Position = pos;
            Length = len;
        }
        
        public int GetID()
        {
            return _id;
        }

        public void SetID(int id)
        {
            _id = id;
        }
    }
