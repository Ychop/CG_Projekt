using OpenTK;
using System;

namespace CG_Projekt.Models
{
    class PickUp
    {
        public Vector2 _position { get; set; }
        public float _size { get; }
        public int Type { get; set; }

        public PickUp(Vector2 position, float size, int type_)
        {
            _position = position;
            _size = size;
            Type = type_; // zahl 0 gibt ammo, zahl 1 gibt Leben
        }
    }
}
