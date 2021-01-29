namespace CG_Projekt.Models
{
    using System;
    using System.Collections.Generic;
    using CG_Projekt.Framework;
    using OpenTK;
    using OpenTK.Input;

    internal class Player : GameObject
    {

        internal float moveLR;
        internal float moveUD;
        public Vector2 direction;
        internal SoundManager manager;


        public Player(Vector2 position_, float radiusDraw_, float radiusColl_, float velocity_, float hitpoints_, int id_)
            : base(position_, radiusDraw_, radiusColl_, velocity_, hitpoints_, id_)
        {
            this.Position = position_;
            this.RadiusDraw = radiusDraw_;
            this.RadiusCollision = radiusColl_;
            this.Hitpoints = hitpoints_;
            this.Velocity = velocity_;
            this.Id = id_;
            this.AmmoPistol = 100;
            this.AmmoUZI = 10;
            this.AmmoShotgun = 3;
            this.AmmoRPG = 100;
            this.Rpm = 0.4f; // could change with diffrent Weapons, also the Damage.
            this.manager = new SoundManager();
        }

        internal double Angle { get; set; }
        internal int SelectedWeapon { get; set; }

        internal float Stamina { get; set; } = 1f;

        internal int AmmoPistol { get; set; }

        internal int AmmoUZI { get; set; }

        internal int AmmoShotgun { get; set; }

        internal int AmmoRPG { get; set; }

        internal float Rpm { get; set; }
        internal float Spm { get; set; }

        internal void MovePlayer(Player player, float deltaTime)
        {
            var keyboard = Keyboard.GetState();
            var walkingSound = new CachedSound("../../Content/PlayerWalk.mp3");
            Spm = 0.4f;
            this.Spm -= deltaTime;
            if (keyboard.IsKeyDown(Key.W) || keyboard.IsKeyDown(Key.A) || keyboard.IsKeyDown(Key.S) || keyboard.IsKeyDown(Key.D) && this.Spm < 0)
            {
                this.manager.PlaySound(walkingSound);
                this.Spm = 0.4f;
            }

            if ((keyboard.IsKeyDown(Key.A) && keyboard.IsKeyDown(Key.D)) || keyboard.IsKeyDown(Key.S) && keyboard.IsKeyDown(Key.W))
            {
                this.moveLR = 0;
                this.moveUD = 0;
            }
            else
            {
                this.moveLR = keyboard.IsKeyDown(Key.A) ? -0.15f : keyboard.IsKeyDown(Key.D) ? 0.15f : 0.0f; // 0.2f und - 0.2f Gibt an wie schnell sich der spieler in die entsprechende Richtung bewegen kann.
                this.moveUD = keyboard.IsKeyDown(Key.S) ? -0.15f : keyboard.IsKeyDown(Key.W) ? 0.15f : 0.0f;
            }
            if (keyboard.IsKeyDown(Key.ShiftLeft) && this.Stamina >= 0)
            {
                this.Velocity = 1.6f;
                this.Stamina = 0f;
            }
            this.Stamina *= (deltaTime* 0.05f);
            this.Velocity = 0.8f;
            player.Position += deltaTime * new Vector2(moveLR, moveUD) * this.Velocity;
        }

        internal void AglignPlayer(Vector2 mousePosition)
        {
            this.direction = new Vector2(mousePosition.X - this.Position.X, mousePosition.Y - this.Position.Y);
            this.direction.Normalize();
            double angleRad = Math.Atan2(this.direction.Y, -this.direction.X);
            this.Angle = angleRad * (180 / Math.PI);
        }

        internal void Shoot(List<Bullet> bullets, List<Explosion> explosion, float deltaTime, Weapon weapon_)
        {
            var mouse = Mouse.GetState();
            this.Rpm -= deltaTime;
            Vector2 bulletOffset = new Vector2(this.RadiusDraw, this.RadiusDraw);
            double angleRadOffset = Math.Atan2(bulletOffset.Y, -bulletOffset.X);
            if (mouse.IsButtonDown(MouseButton.Left) && this.Rpm < 0)
            {
                switch (weapon_.Type)
                {
                    case 1:
                        if (this.AmmoPistol > 0)
                        {
                            var AmmoSound = new CachedSound("../../Content/PistolSound.mp3");
                            this.manager.PlaySound(AmmoSound);
                            bullets.Add(new Bullet(this.Position, weapon_.Size, weapon_.Size, deltaTime * weapon_.Velocity, 5f, bullets.Count + 1, this.direction));
                            this.AmmoPistol--;
                            this.Rpm = weapon_.RPM;
                            SelectedWeapon = weapon_.Type;
                        }

                        break;
                    case 2:
                        if (this.AmmoUZI > 0)
                        {
                            var AmmoSound = new CachedSound("../../Content/UZISound.mp3");
                            this.manager.PlaySound(AmmoSound);
                            bullets.Add(new Bullet(this.Position, weapon_.Size, weapon_.Size, deltaTime * weapon_.Velocity, 5f, bullets.Count + 1, this.direction));
                            this.AmmoUZI--;
                            this.Rpm = weapon_.RPM;
                            SelectedWeapon = weapon_.Type;
                        }

                        break;
                    case 3:
                        if (this.AmmoShotgun > 0)
                        {
                            var AmmoSound = new CachedSound("../../Content/ShotgunSound.mp3");
                            this.manager.PlaySound(AmmoSound);
                            bullets.Add(new Bullet(this.Position, weapon_.Size, weapon_.Size, deltaTime * weapon_.Velocity, 5f, bullets.Count + 1, this.direction + (this.direction.PerpendicularLeft * 0.15f)));
                            bullets.Add(new Bullet(this.Position, weapon_.Size, weapon_.Size, deltaTime * weapon_.Velocity, 5f, bullets.Count + 1, this.direction + (this.direction.PerpendicularLeft * 0.3f)));
                            bullets.Add(new Bullet(this.Position, weapon_.Size, weapon_.Size, deltaTime * weapon_.Velocity, 5f, bullets.Count + 1, this.direction));
                            bullets.Add(new Bullet(this.Position, weapon_.Size, weapon_.Size, deltaTime * weapon_.Velocity, 5f, bullets.Count + 1, this.direction + (this.direction.PerpendicularRight * 0.15f)));
                            bullets.Add(new Bullet(this.Position, weapon_.Size, weapon_.Size, deltaTime * weapon_.Velocity, 5f, bullets.Count + 1, this.direction + (this.direction.PerpendicularRight * 0.3f)));
                            this.AmmoShotgun--;
                            this.Rpm = weapon_.RPM;
                            SelectedWeapon = weapon_.Type;
                        }

                        break;
                    case 4:
                        if (this.AmmoRPG > 0)
                        {
                            var AmmoSound = new CachedSound("../../Content/RPGSound.mp3");
                            this.manager.PlaySound(AmmoSound);
                            bullets.Add(new Bullet(this.Position, weapon_.Size, weapon_.Size, deltaTime * weapon_.Velocity, 5f, bullets.Count + 1, this.direction));
                            explosion.Add(new Explosion(this.Position, weapon_.Size, weapon_.Size, deltaTime * weapon_.Velocity, 5f, bullets.Count + 1, this.direction));
                            explosion[0].NormalizedAnimationTime += deltaTime / explosion[0].AnimationLength;
                            explosion[0].NormalizedAnimationTime %= 1f;

                            this.AmmoRPG--;
                            this.Rpm = weapon_.RPM;
                            SelectedWeapon = weapon_.Type;
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