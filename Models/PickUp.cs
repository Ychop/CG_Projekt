using OpenTK;
using System.Drawing;

namespace CG_Projekt.Models
{
    class PickUp : GameObject
    {

        public int Type { get; set; }

        public PickUp(Vector2 position_, float size_, float velocity_, float hitpoints_, int id_, int type_) : base(position_, size_, velocity_, hitpoints_, id_)
        {
            this.Position = position_;
            this.Size = size_;
            this.Hitpoints = hitpoints_;
            this.Velocity = velocity_;
            this.Id = id_;
            this.Type = type_;
        }
    }
}
