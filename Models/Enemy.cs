using OpenTK;

namespace CG_Projekt.Models
{
    class Enemy
    {
        public Vector2 _position { get; set; }

        internal Enemy(Vector2 position)
        {
            _position = position;
        }
    }
}