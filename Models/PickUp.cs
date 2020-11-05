using OpenTK;

namespace CG_Projekt.Models
{
    class PickUp
    {
        public Vector2 _position { get; set; }
        public float _size { get; }


        public PickUp(Vector2 position, float size)
        {
            _position = position;
            _size = size;
        }
    }
}
