using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace CG_Projekt.Model
{
    class Pickup
    {
        public Vector2 _position { get; set; }
        public Color _color { get; set; }
        public float _size { get; }

        public Pickup(Vector2 position)
        {
            _position = position;
            _color = Color.Yellow;
            _size = 0.05f;


        }

        public void DrawPickup(Vector2 position)
        {
            GL.Color3(_color);
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex2(position + new Vector2(-0.05f / 2, -0.05f / 2));
            GL.Vertex2(position + new Vector2(0.05f / 2, -0.05f / 2));
            GL.Vertex2(position + new Vector2(0.05f / 2, 0.05f / 2));
            GL.Vertex2(position + new Vector2(-0.05f / 2, 0.05f / 2));
            GL.End();
        }
    }
}

