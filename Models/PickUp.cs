using OpenTK;
using System.Drawing;

namespace CG_Projekt.Models
{
    class PickUp : GameObject
    {

        public int Type { get; set; }

        public PickUp(Color color_, Vector2 position_, float posX, float posY, float width, float velocity_, float hitpoints_, int id_, int type_) : base(color_,position_, posX, posY, width, width, velocity_, hitpoints_, id_)
        {
            this.Position = position_;
            this.Color = color_;
            this.Size = width;
            this.Hitpoints = hitpoints_;
            this.Velocity = velocity_;
            this.Id = id_;
            this.Type = type_;
        }
    }
}
