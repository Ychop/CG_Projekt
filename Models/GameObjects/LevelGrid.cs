using OpenTK;
using System.Drawing;

namespace CG_Projekt
{
    class LevelGrid
    {
        internal Vector2 _position;
        internal Color _color;
        internal LevelGrid(Vector2 position, Color color)
        {
            _position = position;
            _color = color;
        }
    }
}