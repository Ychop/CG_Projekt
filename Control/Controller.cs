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
        private bool gameOver;
        private Vector2 mousePosition;

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
            if (this.gameOver)
            {
                return;
            }

            // Menu
            this.MenuUpdate();

            // Zoom mit dem Mausrad
            this.ScrollControl(deltaTime);

            // Updatet den Spieler
            this.UpdatePlayer(deltaTime);

            // Checkt die Collisons
            this.CheckCollisions();

            // Updatet die Enemies
            this.UpdateEnemy(this.model.Enemies, deltaTime);

            //Updated die Partikel
        }

        internal void MenuUpdate()
        {
            KeyboardState keyboard = Keyboard.GetState();
            if (this.gameOver)
            {
                return;

                // Wenn Spieler keine Health mehr hat => Frezze
            }

            if (keyboard.IsKeyDown(Key.Escape))
            {
                this.window.Exit();
            }
        }

        internal void WepaonSelection(char key_)
        {
            switch (key_)
            {
                case '1':
                    this.weapon = this.model.Weapons[0];
                    Console.WriteLine("Pistole ausgewählt.");
                    break;
                case '2':
                    this.weapon = this.model.Weapons[1];
                    Console.WriteLine("UZI ausgewählt.");
                    break;
                case '3':
                    this.weapon = this.model.Weapons[2];
                    Console.WriteLine("Shotgun ausgewählt.");
                    break;
                case '4':
                    this.weapon = this.model.Weapons[3];
                    Console.WriteLine("RPG ausgewählt.");
                    break;
                default:
                    break;
            }
        }    

        internal void UpdateEnemy(List<Enemy> enemies, float deltaTime) // Enemies bewegen sich richtung sdasdwsadSpieler
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].Hitpoints < 0)
                {
                    model.Score++;
                    this.PlaceNewObj(enemies[i]);
                    enemies[i].Hitpoints = 1f;
                }
            }

            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].EnemyAI(enemies[i], this.player, deltaTime);
            }
        }

        internal void UpdatePlayer(float deltaTime)
        {
            this.player.MovePlayer(this.model.Player, deltaTime);
            this.player.AglignPlayer(this.mousePosition);
            this.player.Shoot(this.model.Bullets, deltaTime, this.weapon);
            if (this.player.Hitpoints < 0)
            {
                this.gameOver = true;
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
            if (zoom > 0.5f)
            {
                zoom = 0.5f;
            }

            if (zoom < 0.1f)
            {
                zoom = 0.1f;
            }

            // zoom = MathHelper.Clamp(zoom, 0.9f, 1f); //setzt Zoom Grenze, also bis wie weit man rein/raus zoomen kann
            this.view.Camera.Scale = zoom;
        }

        internal void CheckCollisions()
        {
            var keyboard = Keyboard.GetState();
            // Checkt Enemy/Player with LeverBorder Collision
            this.Intersection.ObjectCollidingWithLeverBorder(this.player);
            for (int i = 0; i < this.model.Enemies.Count; i++)
            {
                this.Intersection.ObjectCollidingWithLeverBorder(this.model.Enemies[i]);
            }

            // Check Enemy and Player collision
            for (int i = 0; i < this.model.Enemies.Count; i++)
            {
                if (this.Intersection.IsIntersecting(this.model.Player, this.model.Enemies[i]))
                {
                    Console.WriteLine("Player Collision mit Enemy: " + this.model.Enemies[i].Id);
                    this.model.Player.Hitpoints -= 0.001f;
                    this.model.Player.Velocity = 0.08f; 
                    if (this.model.Player.Hitpoints < 0)
                    {
                        this.gameOver = true;
                    }
                }
            }           

            // Check Obstacle with Player collision
            for (int i = 0; i < this.model.Obstacles.Count; i++)
            {
                if (this.Intersection.IsIntersectingCircle(this.player, this.model.Obstacles[i]))
                {
                    this.Intersection.ResetGameObjectPosition(this.player, this.model.Obstacles[i]);
                    Console.WriteLine("Player Collision mit Obstacle: " + this.model.Obstacles[i].Id);
                }



            }
            // Check Pickup with Player collision
            for (int i = 0; i < this.model.PickUps.Count; i++)
            {
                if (this.Intersection.IsIntersecting(this.model.Player, this.model.PickUps[i]))
                {
                    Console.WriteLine("Player Collision mit Pickup: " + this.model.PickUps[i].Id);
                    this.PlaceNewObj(this.model.PickUps[i]);
                    switch (this.model.PickUps[i].Type)
                    {
                        case 0:
                            this.player.Hitpoints += 0.1f;
                            Console.WriteLine("Leben: +100");
                            break;
                        case 1:
                            this.player.AmmoPistol += 50;
                            Console.WriteLine("Pistol Ammo: +50");
                            break;
                        case 2:
                            this.player.AmmoUZI += 100;
                            Console.WriteLine("UZI Ammo: +100");
                            break;
                        case 3:
                            this.player.AmmoShotgun += 25;
                            Console.WriteLine("Shotgun Ammo: +25");
                            break;
                        case 4:
                            this.player.AmmoRPG += 5;
                            Console.WriteLine("RPG Ammo: +5");
                            break;
                        default:
                            break;
                    }
                }
            }

            // Check Enemy with Obstacle/Enemy Collision
            for (int i = 0; i < this.model.Enemies.Count; i++)
            {
                for (int j = 0; j < this.model.Obstacles.Count; j++)
                {
                    if (this.Intersection.IsIntersecting(this.model.Enemies[i], this.model.Obstacles[j]))
                    {
                        this.Intersection.ResetGameObjectPosition(this.model.Enemies[i], this.model.Obstacles[j]);
                    }
                }
            }

            // Check Bullet collision with GameObjects
            foreach (GameObject gameObject in model.GameObjects)
            {
                for (int j = 0; j < this.model.Bullets.Count; j++)
                {
                    if (this.Intersection.IsIntersectingCircle(this.model.Bullets[j], gameObject))
                    {
                        this.model.Bullets.RemoveAt(j);
                        gameObject.Hitpoints -= this.weapon.Damage;
                    }
                }
            }

            // EnemyWithEnemyCollision
            foreach (Enemy enemy in this.model.Enemies)
            {
                foreach (Enemy enemy1 in this.model.Enemies)
                {
                    if (Math.Pow(enemy.Position.X - enemy1.Position.X, 2) + Math.Pow(enemy.Position.Y - enemy1.Position.Y, 2) <= 0.0003f)
                    {
                        enemy.Position += new Vector2(enemy1.Position.X - enemy.Position.X, enemy1.Position.Y - enemy.Position.Y) * -0.005f;
                    }

                    if (this.Intersection.IsIntersecting(enemy, enemy1) && enemy1 != enemy)
                    {
                        // Make Bigger Enemy ?
                    }
                }
            }
        }
  
        internal void PlaceNewObj(GameObject obj)
        {
            float ranX = ((float)this.rng.NextDouble() * 1.8f) - 0.9f;
            float ranY = ((float)this.rng.NextDouble() * 1.8f) - 0.9f;

            for (int i = 0; i < this.model.GameObjects.Count; i++)
            {
                obj.Position = new Vector2(ranX, ranY);
                if (this.model.IntersectsAny(obj))
                {
                    ranX = ((float)this.rng.NextDouble() * 1.8f) - 0.9f;
                    ranY = ((float)this.rng.NextDouble() * 1.8f) - 0.9f;
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
