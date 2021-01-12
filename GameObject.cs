using OpenTK;
using System.Drawing;
using Rect = CG_Projekt.Framework.Rect;

namespace CG_Projekt
{
    public class GameObject : Rect
    {
        public Color Color { get; set; }
        public Vector2 Position { get; set; }
        public float Size { get; set; }
        public float Velocity { get; set; }
        public float Hitpoints { get; set; }
        public int Id { get; set; }

        public GameObject(Color color_, Vector2 position_, float minX, float minY, float sizeX, float sizeY, float velocity_, float hitpoints_, int id_) : base(minX,minY,sizeX,sizeY)
        {
            this.Color = color_;
            this.Velocity = velocity_;
            this.Hitpoints = hitpoints_;
            this.Id = id_;
        }

        public virtual void Update(float frameTime)
        {
        }

    }
}
