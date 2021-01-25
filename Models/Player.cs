namespace CG_Projekt.Models
{
    using System;
    using System.Collections.Generic;
    using OpenTK;
    using OpenTK.Input;

    internal class Player : GameObject
    {

        internal float moveLR;
        internal float moveUD;
        private Vector2 direction;


        public Player(Vector2 position_, float size_, float velocity_, float hitpoints_, int id_)
            : base(position_, size_, velocity_, hitpoints_, id_)
        {
            this.Position = position_;
            this.Radius = size_;
            this.Hitpoints = hitpoints_;
            this.Velocity = velocity_;
            this.Id = id_;
            this.AmmoPistol = 100;
            this.AmmoUZI = 0;
            this.AmmoShotgun = 3;
            this.AmmoRPG = 0;
            this.Rpm = 0.4f; // could change with diffrent Weapons, also the Damage.
        }

        internal double Angle { get; set; }

        internal int AmmoPistol { get; set; }

        internal int AmmoUZI { get; set; }

        internal int AmmoShotgun { get; set; }

        internal int AmmoRPG { get; set; }

        internal float Rpm { get; set; }

        internal void MovePlayer(Player player, float deltaTime)
        {
            // TODO: Teweak the movement a bit by using Velocity/acceleration.
            var keyboard = Keyboard.GetState();
            this.moveLR = keyboard.IsKeyDown(Key.A) ? -0.15f : keyboard.IsKeyDown(Key.D) ? 0.15f : 0.0f; // 0.2f und - 0.2f Gibt an wie schnell sich der spieler in die entsprechende Richtung bewegen kann.
            this.moveUD = keyboard.IsKeyDown(Key.S) ? -0.15f : keyboard.IsKeyDown(Key.W) ? 0.15f : 0.0f;
            player.Position += deltaTime * new Vector2(moveLR, moveUD) * this.Velocity;
        }

        internal void AglignPlayer(Vector2 mousePosition)
        {
            this.direction = new Vector2(mousePosition.X - this.Position.X, mousePosition.Y - this.Position.Y);
            this.direction.Normalize();
            double angleRad = Math.Atan2(this.direction.Y, -this.direction.X);
            this.Angle = angleRad * (180 / Math.PI);
        }

        internal void Shoot(List<Bullet> bullets, float deltaTime, Weapon weapon_)
        {
            var mouse = Mouse.GetState();
            this.Rpm -= deltaTime;
            if (mouse.IsButtonDown(MouseButton.Left) && this.Rpm < 0)
            {
                switch (weapon_.Type)
                {
                    case 1:
                        if (this.AmmoPistol > 0)
                        {
                            bullets.Add(new Bullet(this.Position, weapon_.Size, deltaTime * weapon_.Velocity, 5f, bullets.Count + 1, this.direction));
                            this.AmmoPistol--;
                            this.Rpm = weapon_.RPM;
                        }

                        break;
                    case 2:
                        if (this.AmmoUZI > 0)
                        {
                            bullets.Add(new Bullet(this.Position, weapon_.Size, deltaTime * weapon_.Velocity, 5f, bullets.Count + 1, this.direction));
                            this.AmmoUZI--;
                            this.Rpm = weapon_.RPM;
                        }

                        break;
                    case 3:
                        if (this.AmmoShotgun > 0)
                        {
                            bullets.Add(new Bullet(this.Position, weapon_.Size, deltaTime * weapon_.Velocity, 5f, bullets.Count + 1, this.direction + (this.direction.PerpendicularLeft * 0.05f)));
                            bullets.Add(new Bullet(this.Position, weapon_.Size, deltaTime * weapon_.Velocity, 5f, bullets.Count + 1, this.direction + (this.direction.PerpendicularLeft * 0.1f)));
                            bullets.Add(new Bullet(this.Position, weapon_.Size, deltaTime * weapon_.Velocity, 5f, bullets.Count + 1, this.direction));
                            bullets.Add(new Bullet(this.Position, weapon_.Size, deltaTime * weapon_.Velocity, 5f, bullets.Count + 1, this.direction + (this.direction.PerpendicularRight * 0.05f)));
                            bullets.Add(new Bullet(this.Position, weapon_.Size, deltaTime * weapon_.Velocity, 5f, bullets.Count + 1, this.direction + (this.direction.PerpendicularRight * 0.1f)));
                            this.AmmoShotgun--;
                            this.Rpm = weapon_.RPM;
                        }

                        break;
                    case 4:
                        if (this.AmmoRPG > 0)
                        {
                            bullets.Add(new Bullet(this.Position, weapon_.Size, deltaTime * weapon_.Velocity, 5f, bullets.Count + 1, this.direction));
                            this.AmmoRPG--;
                            this.Rpm = weapon_.RPM;
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