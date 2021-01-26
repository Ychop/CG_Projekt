namespace CG_Projekt
{
    using System;
    using System.Collections.Generic;
    using CG_Projekt.Models;
    using OpenTK;
    using OpenTK.Input;

    internal class Controller
    {
        private readonly View view;
        private readonly Model model;
        private readonly GameWindow window;
        private readonly Player player;
        private readonly Random rng = new Random();
        private float axisZoom = 0f;
        private float oldScrollValue = 0;
        private Weapon weapon;
        private Vector2 mousePosition;
        public bool GameStarted = false;

        internal Controller(View view_, Model model_, GameWindow window_)
        {
            this.view = view_;
            this.model = model_;
            this.window = window_;
            this.player = this.model.Player;
            this.Camera = this.view.Camera;
            this.weapon = this.model.Weapons[0];
        }
        internal Intersection Intersection { get; } = new Intersection();

        internal Camera Camera { get; }

        internal void Update(float deltaTime)
        {
            UpdateMainMenu();
            if (!view.GameOver && GameStarted)
            {
                // Zoom mit dem Mausrad
                this.ScrollControl(deltaTime);

                // Updatet den Spieler
                this.UpdatePlayer(deltaTime);

                // Checkt die Collisons
                this.CheckCollisions();

                // Updatet die Enemies
                this.UpdateEnemy(this.model.Enemies, deltaTime);

                //Updated die Partikel
                UpdateParticle(deltaTime);

            }
        }

        internal void UpdateMainMenu()
        {
            var keyboard = Keyboard.GetState();
            if (keyboard.IsAnyKeyDown)
            {
                GameStarted = true;
                view.GameStarted = true;
            }
        }

        internal void WepaonSelection(char key_)
        {
            switch (key_)
            {
                case '1':
                    this.weapon = this.model.Weapons[0];
                    model.weaponSelected = 1;
                    Console.WriteLine("Pistole ausgewählt.");
                    break;
                case '2':
                    this.weapon = this.model.Weapons[1];
                    Console.WriteLine("UZI ausgewählt.");
                    model.weaponSelected = 2;
                    break;
                case '3':
                    this.weapon = this.model.Weapons[2];
                    Console.WriteLine("Shotgun ausgewählt.");
                    model.weaponSelected = 3;
                    break;
                case '4':
                    this.weapon = this.model.Weapons[3];
                    Console.WriteLine("RPG ausgewählt.");
                    model.weaponSelected = 4;
                    break;
                default:
                    break;
            }
        }
        internal void UpdateParticle(float deltaTime)
        {
            for (int i = 0; i < this.model.Particles.Count; i++)
            {
                this.model.Particles[i].Hitpoints -= 0.01f;
                this.model.Particles[i].Position += (this.model.Particles[i].RanDir * this.model.Particles[i].Velocity) * deltaTime;
                this.model.Particles[i].Velocity -= 0.1f;
                if (this.model.Particles[i].Velocity < 0)
                {
                    this.model.Particles[i].Velocity = 0;
                }
                if (this.model.Particles[i].Hitpoints < 0)
                {
                    this.model.Particles.RemoveAt(i);
                }
            }
            for (int i = 0; i < this.model.RPGFragments.Count; i++)
            {
                this.model.RPGFragments[i].Hitpoints -= 0.01f;
                this.model.RPGFragments[i].Position += (this.model.RPGFragments[i].RanDir * this.model.RPGFragments[i].Velocity) * deltaTime;
                this.model.RPGFragments[i].Velocity -= 0.01f;
                if (this.model.RPGFragments[i].Velocity < 0)
                {
                    this.model.RPGFragments[i].Velocity = 0;
                }
                if (this.model.RPGFragments[i].Hitpoints < 0)
                {
                    this.model.RPGFragments.RemoveAt(i);
                }
            }
        }
        internal void UpdateEnemy(List<Enemy> enemies, float deltaTime) // Enemies bewegen sich richtung Spieler
        {
            foreach (Enemy enemy in model.Enemies)
            {
                if (enemy.Hitpoints < 0)
                {
                    model.Score++;
                    model.GeneratePickUp(enemy.Position);
                    this.PlaceNewEnemy(enemy);
                    enemy.Hitpoints = 1f;
                }
                if(enemy.SpeedUp > 0.8f)
                {
                    enemy.SpeedUp = 0.8f;
                }
                if(enemy.Hitpoints > 5f)
                {
                    enemy.Hitpoints = 5f;
                }
                enemy.EnemyAI(enemy, this.player, deltaTime);
            }
        }

        internal void UpdatePlayer(float deltaTime)
        {
            this.player.MovePlayer(this.model.Player, deltaTime);
            this.player.AglignPlayer(this.mousePosition);
            this.player.Shoot(this.model.Bullets, deltaTime, this.weapon);
            if (this.player.Hitpoints < 0)
            {
                view.GameOver = true;
            }
            if (this.player.Hitpoints > 1)
            {
                this.player.Hitpoints = 1;
            }
        }

        internal void ScrollControl(float deltaTime)
        {
            var mouse = Mouse.GetState();
            var scrollValue = mouse.ScrollWheelValue;

            if (scrollValue < this.oldScrollValue)
            {
                this.axisZoom = 2f;
                this.oldScrollValue = scrollValue;
            }
            else if (scrollValue > this.oldScrollValue)
            {
                this.axisZoom = -2f;
                this.oldScrollValue = scrollValue;
            }
            else if (scrollValue == this.oldScrollValue)
            {
                this.axisZoom = 0f;
            }

            var zoom = this.view.Camera.Scale * (1 + (deltaTime * this.axisZoom));
            if (zoom > 0.2f)
            {
                zoom = 0.2f;
            }

            if (zoom < 0.1f)
            {
                zoom = 0.1f;
            }

            this.view.Camera.Scale = zoom;
        }
        internal void CheckCollisions()
        {
            CheckLevelBorderCollision();
            CheckPlayerCollisions();
            CheckBulletCollision();
            CheckEnemyCollisions();
            CheckDebrisCollison();
        }
        internal void CheckLevelBorderCollision()
        {
            // Checkt Enemy/Player with LeverBorder Collision
            this.Intersection.ObjectCollidingWithLeverBorder(this.player);
            for (int i = 0; i < this.model.Enemies.Count; i++)
            {
                this.Intersection.ObjectCollidingWithLeverBorder(this.model.Enemies[i]);
            }
            for (int i = 0; i < this.model.Bullets.Count; i++)
            {
                if (this.Intersection.ObjectCollidingWithLeverBorder(this.model.Bullets[i]))
                {
                    this.model.Bullets.RemoveAt(i);
                }
            }
        }
        internal void CheckPlayerCollisions()
        {
            // Check Enemy and Player collision
            for (int i = 0; i < this.model.Enemies.Count; i++)
            {
                if (this.Intersection.IsIntersectingCircle(this.model.Player, this.model.Enemies[i]))
                {
                    float powerOfschubs = 0.001f;
                    Console.WriteLine("Player Collision mit Enemy: " + this.model.Enemies[i].Id);
                    this.model.Player.Hitpoints -= model.Enemies[i].Damage;
                    this.model.Player.Position += this.model.Enemies[i].playerDirection.Normalized()* powerOfschubs;
                    for (int j = 0; j < rng.Next(10, 20); j++)
                    {
                        this.model.Particles.Add(new Particle(player.Position, 0.0015f, 0.0015f, (float)rng.NextDouble() - 0.2f, 5f, 1, new Vector2((float)rng.NextDouble() * 2 - 1, (float)rng.NextDouble() * 2 - 1)));
                    }
                }
            }

            // Check Obstacle with Player collision
            for (int i = 0; i < this.model.Obstacles.Count; i++)
            {
                if (this.Intersection.IsIntersectingCircle(this.player, this.model.Obstacles[i]))
                {
                    float radiusSum = (player.RadiusCollision + model.Obstacles[i].RadiusCollision);
                    Vector2 diff = player.Position - model.Obstacles[i].Position;
                    diff /= diff.Length;
                    diff *= (radiusSum);
                    this.player.Position = model.Obstacles[i].Position + diff;
                    Console.WriteLine("Player Collision mit Obstacle: " + this.model.Obstacles[i].Id);
                }
            }
            // Check Pickup with Player collision
            for (int i = 0; i < this.model.PickUps.Count; i++)
            {
                if (this.Intersection.IsIntersectingCircle(this.model.Player, this.model.PickUps[i]))
                {
                    Console.WriteLine("Player Collision mit Pickup: " + this.model.PickUps[i].Id);
                    switch (this.model.PickUps[i].Type)
                    {
                        case 0:
                            this.player.Hitpoints += 0.1f;
                            Console.WriteLine("Leben: +100");
                            this.model.PickUps.RemoveAt(i);
                            break;
                        case 1:
                            this.player.AmmoPistol += 25;
                            Console.WriteLine("Pistol Ammo: +50");
                            this.model.PickUps.RemoveAt(i);
                            break;
                        case 2:
                            this.player.AmmoUZI += 100;
                            Console.WriteLine("UZI Ammo: +100");
                            this.model.PickUps.RemoveAt(i);
                            break;
                        case 3:
                            this.player.AmmoShotgun += 10;
                            Console.WriteLine("Shotgun Ammo: +25");
                            this.model.PickUps.RemoveAt(i);
                            break;
                        case 4:
                            this.player.AmmoRPG += 3;
                            Console.WriteLine("RPG Ammo: +5");
                            this.model.PickUps.RemoveAt(i);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        internal void CheckBulletCollision()
        {
            // Check Bullet collision with GameObjects
            int Id;
            foreach (GameObject gameObject in model.GameObjects)
            {
                for (int j = 0; j < this.model.Bullets.Count; j++)
                {
                    if (this.Intersection.IsIntersectingCircle(this.model.Bullets[j], gameObject) && gameObject.Id != -1)
                    {
                        if (gameObject.Id > 50)
                        {
                            Id = 0;
                        }
                        else
                        {
                            Id = 1;
                        }
                        if (this.player.SelectedWeapon == 4)
                        {
                            for (int i = 0; i < rng.Next(10, 20); i++)
                            {
                                this.model.RPGFragments.Add(new Particle(gameObject.Position, 0.002f, 0.002f, 0.5f, 2f, 0, new Vector2((float)rng.NextDouble() * 2 - 1, (float)rng.NextDouble() * 2 - 1)));                             
                            }
                        }
                        for (int i = 0; i < rng.Next(10, 20); i++)
                        {
                           
                            this.model.Particles.Add(new Particle(gameObject.Position+(this.model.Bullets[j].Position-gameObject.Position), 0.0015f, 0.0015f,(float)rng.NextDouble()-0.2f, 5f, Id, new Vector2((float)rng.NextDouble() * 2 - 1, (float)rng.NextDouble() * 2 - 1)));
                        }
                       
                        this.model.Bullets.RemoveAt(j);
                        gameObject.Hitpoints -= this.weapon.Damage;
                    }
                }             
            }
        }
        internal void CheckDebrisCollison()
        {
            foreach(GameObject gameObject in model.GameObjects)
            {
                foreach(Particle fragment in model.RPGFragments)
                {
                    if (this.Intersection.IsIntersectingCircle(fragment, gameObject))
                    {
                        gameObject.Hitpoints -= this.weapon.Damage;
                    }
                }
              
            }
        }
        internal void CheckEnemyCollisions()
        {
            // EnemyWithEnemyCollision
            foreach (Enemy enemy in this.model.Enemies)
            {
                foreach (Enemy enemy1 in this.model.Enemies)
                {
                    if (Math.Pow(enemy.Position.X - enemy1.Position.X, 2) + Math.Pow(enemy.Position.Y - enemy1.Position.Y, 2) <= 0.0003f)
                    {
                        enemy.Position += new Vector2(enemy1.Position.X - enemy.Position.X, enemy1.Position.Y - enemy.Position.Y) * -0.005f;
                    }
                }
            }
            // Check Enemy with Obstacle Collision
            for (int i = 0; i < this.model.Enemies.Count; i++)
            {
                for (int j = 0; j < this.model.Obstacles.Count; j++)
                {
                    if (this.Intersection.IsIntersectingCircle(this.model.Enemies[i], this.model.Obstacles[j]))
                    {
                        float radiusSum = (this.model.Enemies[i].RadiusCollision + model.Obstacles[j].RadiusCollision);
                        Vector2 diff = this.model.Enemies[i].Position - model.Obstacles[j].Position;
                        diff /= diff.Length;
                        diff *= (radiusSum);
                        this.model.Enemies[i].Position = model.Obstacles[j].Position + diff;
                    }
                }
            }
        }
        internal void PlaceNewEnemy(Enemy obj)
        {
            float ranX = ((float)this.rng.NextDouble() * 1.2f) - 0.6f;
            float ranY = ((float)this.rng.NextDouble() * 1.2f) - 0.6f;
            obj.SpeedUp += 0.0025f;
            obj.Hitpoints += 0.0025f;
            obj.Damage += 0.0025f;
            for (int i = 0; i < this.model.GameObjects.Count; i++)
            {
                obj.Position = new Vector2(ranX, ranY);
                if (this.model.IntersectsAny(obj) && (Math.Pow(obj.Position.X - player.Position.X, 2) + Math.Pow(obj.Position.Y - player.Position.Y, 2)) < 0.05f)
                {
                    ranX = ((float)this.rng.NextDouble() * 1.2f) - 0.6f;
                    ranY = ((float)this.rng.NextDouble() * 1.2f) - 0.6f;
                    i = 0;
                }
                else
                {
                    obj.Position = new Vector2(ranX, ranY);
                }
            }
        }
        internal void TranslateMouseCoordinates(int x, int y)
        {
            var fromViewportToWorld = Transformation.Combine(this.Camera.InvViewportMatrix, this.Camera.CameraMatrix.Inverted());
            var mouseVector = new Vector2(x, y);
            this.mousePosition = mouseVector.Transform(fromViewportToWorld);
        }
    }
}
