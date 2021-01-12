
using OpenTK;
using System.Drawing;
using OpenTK.Input;
using System;


namespace CG_Projekt.Models
{
    internal class Player : GameObject
    {
        public double Angle { get; set; }
        public int Ammo { get; set; }
        public float ShotCoolDown { get; set; }

        public Vector2 Direction;
        public Player(Color color_, Vector2 position_, float size_, float velocity_, float hitpoints_,int id_) : base (color_,position_,size_,velocity_,hitpoints_,id_)
        {
           
            this.Color = color_;
            this.Position = position_;
            this.Size = size_;
            this.Hitpoints = hitpoints_;
            this.Velocity = velocity_;
            this.Id = id_;
            Ammo = 100;
            ShotCoolDown = 1f;
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
            MouseState mouseState = Mouse.GetState();
            Direction = new Vector2(mouseState.X - Position.X, mouseState.Y - Position.Y);
            double _anglerad = Math.Atan2(Direction.Y, Direction.X);
            Angle = _anglerad * (180 / Math.PI);
            if (Angle > 180 || Angle < -180)
            {
                Angle = 0;
            }
            // Console.WriteLine("Winkel: " + Angle);
        }
        public bool Shoot()
        {
            var mouse = Mouse.GetState();
            //Console.WriteLine("Cooldown:" + shotCoolDown);
            if (mouse.IsButtonDown(MouseButton.Left) && Ammo > 0 && ShotCoolDown < 0)
            {
                // Console.WriteLine("es Wird scharf geschossen!");
                this.ShotCoolDown = 0.125f;
                return true;
            }
            return false;
        }
    }
}