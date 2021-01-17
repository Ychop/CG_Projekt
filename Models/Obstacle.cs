using OpenTK;
using System.Drawing;

namespace CG_Projekt.Models
{
    class Obstacle : GameObject
    {
        public Obstacle( Vector2 position_, float size_, float velocity_, float hitpoints_, int id_) : base(position_, size_, velocity_, hitpoints_, id_)
        {
            this.Position = position_;
            this.Size = size_;
            this.Hitpoints = hitpoints_;
            this.Velocity = velocity_;
            this.Id = id_;
        }
    }
}