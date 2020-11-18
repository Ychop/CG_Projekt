using OpenTK;
using System.Drawing;
using OpenTK.Input;
using System;
using OpenTK.Graphics.OpenGL;
namespace CG_Projekt.Models
{
    internal class Player
    {
        public Matrix4 playerRot = new Matrix4();
        public Vector2 _position { get; set; }
        public float _size { get; }

        public Color _color { get; set; }
        public Player()
        {
            _position = new Vector2(0, 0.89f);
            _size = 0.01f;
            _color = Color.Green;
        }

        public void MovePlayer(Player player, float deltaTime)
        {
            var keyboard = Keyboard.GetState(); // Holt den Zustand des Keyboards
            float moveLR = (keyboard.IsKeyDown(Key.Left) || keyboard.IsKeyDown(Key.A)) ? -0.15f : keyboard.IsKeyDown(Key.Right) || keyboard.IsKeyDown(Key.D) ? 0.15f : 0.0f; // 0.2f und - 0.2f Gibt an wie schnell sich der spieler in die entsprechende Richtung bewegen kann.
            float moveUD = keyboard.IsKeyDown(Key.Down) || keyboard.IsKeyDown(Key.S) ? -0.2f : keyboard.IsKeyDown(Key.Up) || keyboard.IsKeyDown(Key.W) ? 0.2f : 0.0f;
            player._position += deltaTime * new Vector2(moveLR, moveUD);
        }
        public void AglignPlayer(Player player)
        {         
            var mouse = Mouse.GetState();
            Vector2 mouseV = new Vector2(mouse.X, mouse.Y);
            Vector2 playerDirection = new Vector2(mouse.X - player._position.X, mouse.Y - player._position.Y);
            playerDirection.Normalized();
            float angle = (float)Math.Atan2(mouse.X, mouse.Y);      
            var trans = Transformation.Translate(player._position.X, player._position.Y);
            var rot = Transformation.Rotation(angle);
            playerRot = Transformation.Combine(trans, rot);
            //GL.LoadMatrix(ref playerRot);
           
        }


    }
}