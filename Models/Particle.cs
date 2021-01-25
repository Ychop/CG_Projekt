using OpenTK;
using System;
namespace CG_Projekt.Models
{
    class Particle : GameObject
    {
        private readonly Random rng = new Random();
        public Vector2 direction;

        internal Particle(Vector2 position_, float radius_, float velocity_, float hitpoints_, int id_) : base(position_, radius_, velocity_, hitpoints_, id_)
        {
            Position = position_;
            Radius = radius_;
            Velocity = velocity_;
            Hitpoints = hitpoints_;
            Id = id_;
            direction = new Vector2(rng.Next(-1, 1), rng.Next(-1, 1));
            direction.Normalize();
        }
    }
}
