using OpenTK;
using System.Drawing;

namespace CG_Projekt.Models
{
    internal class Player
    {
        public Vector2 _position { get; set; }
        public float _size { get; }

        public Color _color { get; set; }
        public Player()
        {
            _position = new Vector2(0, 0.89f);
            _size = 0.01f;
            _color = Color.Green;
        }

        
    }
}