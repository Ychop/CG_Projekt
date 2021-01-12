using OpenTK;
using System.Drawing;

namespace CG_Projekt.Models
{
    class Obstacle : GameObject
    {
        public Obstacle(Color color_, Vector2 position_, float posX, float posY, float width, float velocity_, float hitpoints_, int id_) : base(color_, position_, posX, posY, width, width, velocity_, hitpoints_, id_)
        {
            this.Position = position_;
            this.Size = width;
            this.Color = color_;
            this.Hitpoints = hitpoints_;
            this.Velocity = velocity_;
            this.Id = id_;
        }
    }
}