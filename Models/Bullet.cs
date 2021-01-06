
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Drawing;

namespace CG_Projekt.Models
{
    public class Bullet
    {
        public Vector2 Direction { get; set; }
        public float Velocity { get; set; }
        public Vector2 Position { get; set; }

        public Bullet(Vector2 position_)
        {
            Position = position_ ;
        }
        public void MoveBullet(float deltaTime, Bullet bullet)
        {
            //Bullet bewegen sich noch seltsam
            MouseState mouseState = Mouse.GetState();
            Velocity = deltaTime * 0.005f;
            bullet.Direction = new Vector2(mouseState.X - bullet.Position.X,mouseState.Y - bullet.Position.Y);
            bullet.Direction.Normalized();
            bullet.Position += bullet.Direction * Velocity;
        }
    }
}
