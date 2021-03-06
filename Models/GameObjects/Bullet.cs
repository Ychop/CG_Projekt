﻿
using OpenTK;
using System;
using System.Drawing;

namespace CG_Projekt.Models
{
    public class Bullet : GameObject
    {
        public Vector2 Direction { get; set; }

        public Bullet(Vector2 position_, float size_, float velocity_, float hitpoints_, int id_, Vector2 direction_) : base( position_, size_, velocity_, hitpoints_, id_)
        {
            this.Position = position_;
            this.Size = size_;
            this.Hitpoints = hitpoints_;
            this.Velocity = velocity_;
            this.Id = id_;
            this.Direction = direction_;
            this.Direction.Normalize();
        }
    }
}
