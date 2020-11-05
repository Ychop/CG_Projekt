using OpenTK;

namespace CG_Projekt.Models
{
    class Obstacle
    {
        public Vector2 _position { get;}
        public float _size { get; }


        public Obstacle(Vector2 position, float size)
        {
            _position = position;
            _size = size;
        }
    }
}