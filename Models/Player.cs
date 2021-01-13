
using OpenTK;
using System.Drawing;
using OpenTK.Input;
using System;
using System.Collections.Generic;

namespace CG_Projekt.Models
{
    internal class Player : GameObject
    {
        public double Angle { get; set; }
        public int Ammo { get; set; }

        public float rpm { get; set; }

        public Vector2 Direction;
        //TODO: Vector2 position_ vielleicht entfernen weil redundant
        public Player(Color color_, Vector2 position_, float posX, float posY, float width, float velocity_, float hitpoints_, int id_) : base(color_, position_, posX, posY, width, width, velocity_, hitpoints_, id_)
        {
            this.Position = position_;
            this.Color = color_;
            this.Size = width;
            this.Hitpoints = hitpoints_;
            this.Velocity = velocity_;
            this.Id = id_;
            Ammo = 100;
            rpm = 0.4f; //could change with diffrent Weapons, also the Damage
        }
        public void MovePlayer(float deltaTime)
        {
            //TODO: Teweak the movement a bit by using Velocity/acceleration.
            var keyboard = Keyboard.GetState(); // Holt den Zustand des Keyboards
            float moveLR = (keyboard.IsKeyDown(Key.Left) || keyboard.IsKeyDown(Key.A)) ? -0.15f : keyboard.IsKeyDown(Key.Right) || keyboard.IsKeyDown(Key.D) ? 0.15f : 0.0f; // 0.2f und - 0.2f Gibt an wie schnell sich der spieler in die entsprechende Richtung bewegen kann.
            float moveUD = keyboard.IsKeyDown(Key.Down) || keyboard.IsKeyDown(Key.S) ? -0.15f : keyboard.IsKeyDown(Key.Up) || keyboard.IsKeyDown(Key.W) ? 0.15f : 0.0f;
            this.Position += deltaTime * new Vector2(moveLR, moveUD);

        }
        public void AglignPlayer(Vector2 mousePosition)
        {
            Direction = new Vector2(mousePosition.X - Position.X, mousePosition.Y - Position.Y);
            Direction.Normalize();
            double angleRad = Math.Atan2(Direction.Y, -Direction.X);
            Angle = angleRad * (180 / Math.PI);

        }
        public void Shoot(List<Bullet> bullets, float deltaTime)
        {
            var mouse = Mouse.GetState();
            rpm -= deltaTime;
            if (mouse.IsButtonDown(MouseButton.Left) && Ammo > 0 && rpm < 0)
            {
                bullets.Add(new Bullet(Color.Black, this.Position, this.Position.X, this.Position.Y, 0.001f, 0.001f, deltaTime * 0.5f, 5f, bullets.Count + 1, this.Direction));
                Ammo--;
                rpm = 0.4f;
            }
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Position += bullets[i].Direction * bullets[i].Velocity;
                bullets[i].Hitpoints -= deltaTime;
                if (bullets[i].Hitpoints < 0)
                {
                    bullets.RemoveAt(i);
                }
            }
        }
    }
}