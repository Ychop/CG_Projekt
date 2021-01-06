
using OpenTK;
using System.Drawing;
using OpenTK.Input;
using System;
using OpenTK.Graphics.OpenGL;


namespace CG_Projekt.Models
{
    internal class Player
    {
        public double Angle { get; set; }
        public Vector2 Position { get; set; }
        public float Size { get; }
        public float Velocity { get; set; }
        public int Ammo { get; set; }
        public float Health { get; set; }

      

        public Color Color { get; set; }
        public Vector2 Direction;
        public Player()
        {
            Position = new Vector2(0, 0.89f);
            Size = 0.01f;
            Color = Color.Green;
            Ammo = 100;
            Health = 1f;
        }

        public void MovePlayer(Player player, float deltaTime)
        {
            var keyboard = Keyboard.GetState(); // Holt den Zustand des Keyboards
            float moveLR = (keyboard.IsKeyDown(Key.Left) || keyboard.IsKeyDown(Key.A)) ? -0.15f : keyboard.IsKeyDown(Key.Right) || keyboard.IsKeyDown(Key.D) ? 0.15f : 0.0f; // 0.2f und - 0.2f Gibt an wie schnell sich der spieler in die entsprechende Richtung bewegen kann.
            float moveUD = keyboard.IsKeyDown(Key.Down) || keyboard.IsKeyDown(Key.S) ? -0.2f : keyboard.IsKeyDown(Key.Up) || keyboard.IsKeyDown(Key.W) ? 0.2f : 0.0f;
            player.Position += deltaTime * new Vector2(moveLR, moveUD);
            Velocity = deltaTime;
        }
        public void AglignPlayer()
        {
            Vector2 yAxis = new Vector2(0, 1);
            MouseState mouseState = Mouse.GetState();
            Direction = new Vector2(mouseState.X - Position.X, mouseState.Y - Position.Y);
           
            double _anglerad = Math.Atan2(Direction.Y, Direction.X);
            Angle = _anglerad * (180 / Math.PI);
            if (Angle > 180 || Angle < -180)
            {
                Angle = 0;
            }
            Console.WriteLine("Winkel: " + Angle);
        }
        public bool Shoot()
        {
            var mouse = Mouse.GetState();

            if (mouse.IsButtonDown(MouseButton.Left) && Ammo > 0)
            {
                Console.WriteLine("es Wird scharf geschossen!");

                return true;
            }
            return false;


        }
    }
}