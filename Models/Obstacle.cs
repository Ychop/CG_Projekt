using OpenTK;

namespace CG_Projekt.Models
{
    class Obstacle
    {
        public Vector2 Position { get;}
        public float Size { get; }


        public Obstacle(Vector2 position_, float size_)
        {
            Position = position_;
            Size = size_;
        }
    }
}