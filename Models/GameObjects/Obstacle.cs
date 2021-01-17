using OpenTK;
using System.Drawing;

namespace CG_Projekt.Models
{
    class Obstacle : GameObject
    {
        public Obstacle(Color color_, Vector2 position_, float size_, float velocity_, float hitpoints_, int id_) : base(color_, position_, size_, velocity_, hitpoints_, id_)
        {
            this.Color = color_;
            this.Position = position_;
            this.Size = size_;
            this.Hitpoints = hitpoints_;
            this.Velocity = velocity_;
            this.Id = id_;
        }
    }
}