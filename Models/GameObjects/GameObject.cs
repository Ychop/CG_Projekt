﻿using OpenTK;
using System.Drawing;

namespace CG_Projekt
{
    public class GameObject
    {
        public Color Color { get; set; }
        public Vector2 Position { get; set; }
        public float Size { get; set; }
        public float Velocity { get; set; }
        public float Hitpoints { get; set; }
        public int Id { get; set; }

        public GameObject(Color color_, Vector2 position_, float size_, float velocity_, float hitpoints_, int id_)
        {
            this.Color = color_;
            this.Position = position_;
            this.Size = size_;
            this.Velocity = velocity_;
            this.Hitpoints = hitpoints_;
            this.Id = id_;
        }
    }
}
