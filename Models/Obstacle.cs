using OpenTK;

namespace CG_Projekt.Models
{
    class Obstacle
    {
        public Vector2 _position { get; set; }

        public Obstacle(Vector2 position)
        {
            _position = position;
        }
    }
}