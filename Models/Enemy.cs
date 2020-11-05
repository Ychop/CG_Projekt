using OpenTK;

namespace CG_Projekt.Models
{
    class Enemy
    {
        public Vector2 _position { get; set; }
        public float _size { get; }

        internal Enemy(Vector2 position,float size)
        {

            _position = position;
            _size = size;
        }

    }
}