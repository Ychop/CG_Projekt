using OpenTK;

namespace CG_Projekt.Models
{
    internal class Player
    {
        public Vector2 _position { get; set; }
        public Player()
        {
            _position = new Vector2(0, 0.89f);
        }
    }
}