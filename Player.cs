using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;


namespace CG_Projekt
{
    class Player
    {
        public Vector2 _position { get; set; }
        public Color _color { get; set; }
        public float _size { get; }

        public Player()
        {
            _position = new Vector2(0, -0.99f);
            _color = Color.Blue;
            _size = 0.1f;

            
        }

        public void DrawPlayer(Vector2 position)
        {
            GL.Color3(_color);
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex2(position + new Vector2(-0.1f / 2, -0.1f / 2));
            GL.Vertex2(position + new Vector2(0.1f / 2, -0.1f / 2));
            GL.Vertex2(position + new Vector2(0.1f / 2, 0.1f / 2));
            GL.Vertex2(position + new Vector2(-0.1f / 2, 0.1f / 2));
            GL.End();
        }

    }
}
