using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace CG_Projekt.Model
{
    class Obstacle
    {
        public Vector2 _position { get; set; }
        public Color _color { get; set; }
        public float _size { get; }

        public Obstacle(Vector2 position)
        {
            _position = position;
            _color = Color.Black;
            _size = 0.1f;


        }

        public void DrawObstacle(Vector2 position)
        {
            GL.Color3(_color);
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex2(position + new Vector2(-0.2f / 2, -0.2f / 2));
            GL.Vertex2(position + new Vector2(0.2f / 2, -0.2f / 2));
            GL.Vertex2(position + new Vector2(0.2f / 2, 0.2f / 2));
            GL.Vertex2(position + new Vector2(-0.2f / 2, 0.2f / 2));
            GL.End();
        }
    }
}
