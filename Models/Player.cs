
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
        public int AmmoPistol { get; set; }
        public int AmmoUZI { get; set; }
        public int AmmoShotgun { get; set; }
        public int AmmoRPG { get; set; }

        public float Rpm { get; set; }

        public Vector2 Direction;
        public Player(Vector2 position_, float size_, float velocity_, float hitpoints_, int id_) : base(position_, size_, velocity_, hitpoints_, id_)
        {
            this.Position = position_;
            this.Size = size_;
            this.Hitpoints = hitpoints_;
            this.Velocity = velocity_;
            this.Id = id_;
            this.AmmoPistol = 100;
            this.AmmoUZI = 0;
            this.AmmoShotgun = 3;
            this.AmmoRPG = 0;
            this.Rpm = 0.4f; //could change with diffrent Weapons, also the Damage
        }
        public void MovePlayer(Player player, float deltaTime)
        {
            //TODO: Teweak the movement a bit by using Velocity/acceleration.
            var keyboard = Keyboard.GetState(); // Holt den Zustand des Keyboards
            float moveLR = (keyboard.IsKeyDown(Key.Left) || keyboard.IsKeyDown(Key.A)) ? -0.15f : keyboard.IsKeyDown(Key.Right) || keyboard.IsKeyDown(Key.D) ? 0.15f : 0.0f; // 0.2f und - 0.2f Gibt an wie schnell sich der spieler in die entsprechende Richtung bewegen kann.
            float moveUD = keyboard.IsKeyDown(Key.Down) || keyboard.IsKeyDown(Key.S) ? -0.1f : keyboard.IsKeyDown(Key.Up) || keyboard.IsKeyDown(Key.W) ? 0.1f : 0.0f;
            player.Position += deltaTime * new Vector2(moveLR, moveUD);
        }
        public void AglignPlayer(Vector2 mousePosition)
        {
            Direction = new Vector2(mousePosition.X - Position.X, mousePosition.Y - Position.Y);
            Direction.Normalize();
            double angleRad = Math.Atan2(Direction.Y, -Direction.X);
            Angle = angleRad * (180 / Math.PI);
        }
        public void Shoot(List<Bullet> bullets, float deltaTime, Weapon weapon_)
        {
            var mouse = Mouse.GetState();
            Rpm -= deltaTime;
            if (mouse.IsButtonDown(MouseButton.Left) && Rpm < 0)
            {
                switch (weapon_.Type)
                {
                    case 1:
                        if (AmmoPistol > 0)
                        {
                            bullets.Add(new Bullet(this.Position, weapon_.Size, deltaTime * weapon_.Velocity, 5f, bullets.Count + 1, this.Direction));
                            AmmoPistol--;
                            Rpm = weapon_.RPM;
                        }
                        break;
                    case 2:
                        if (AmmoUZI > 0)
                        {
                            bullets.Add(new Bullet(this.Position, weapon_.Size, deltaTime * weapon_.Velocity, 5f, bullets.Count + 1, this.Direction));
                            AmmoUZI--;
                            Rpm = weapon_.RPM;
                        }
                        break;
                    case 3:
                        if (AmmoShotgun > 0)
                        {
                            bullets.Add(new Bullet(this.Position, weapon_.Size, deltaTime * weapon_.Velocity, 5f, bullets.Count + 1, this.Direction + Direction.PerpendicularLeft * 0.05f));
                            bullets.Add(new Bullet(this.Position, weapon_.Size, deltaTime * weapon_.Velocity, 5f, bullets.Count + 1, this.Direction + Direction.PerpendicularLeft * 0.1f));
                            bullets.Add(new Bullet(this.Position, weapon_.Size, deltaTime * weapon_.Velocity, 5f, bullets.Count + 1, this.Direction));
                            bullets.Add(new Bullet(this.Position, weapon_.Size, deltaTime * weapon_.Velocity, 5f, bullets.Count + 1, this.Direction + (Direction.PerpendicularRight*0.05f)));
                            bullets.Add(new Bullet(this.Position, weapon_.Size, deltaTime * weapon_.Velocity, 5f, bullets.Count + 1, this.Direction + (Direction.PerpendicularRight * 0.1f)));
                            AmmoShotgun--;
                            Rpm = weapon_.RPM;
                        }
                        break;
                    case 4:
                        if (AmmoRPG > 0)
                        {
                            bullets.Add(new Bullet(this.Position, weapon_.Size, deltaTime * weapon_.Velocity, 5f, bullets.Count + 1, this.Direction));
                            AmmoRPG--;
                            Rpm = weapon_.RPM;
                        }
                        break;
                    default:
                        break;
                }
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