using CG_Projekt.Models;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace CG_Projekt
{
    class Controller
    {
        View view;
        Model model;
        GameWindow window;
        public Intersection intersection { get; } = new Intersection();
        public Camera camera { get; } = new Camera();
        internal int oldScrollValue = 0;
        internal float axisZoom = 0f;
        internal bool GameOver;
        internal Player player;

        private Random rng = new Random();
        internal Controller(View view_, Model model_, GameWindow window_)
        {
            view = view_;
            model = model_;
            window = window_;
            player = model.player;

        }

        internal void Update(float deltaTime)
        {
            //Menu
            MenuUpdate();
            //Zoom mit dem Mausrad
            ScrollControl(deltaTime);
            //Updatet den Spieler
            UpdatePlayer(deltaTime);
            //Checkt die Collisons
             UpdateCheckCollision();
            // Updatet die Enemies
            UpdateEnemy(model.enemies, deltaTime);
        }

        internal void MenuUpdate()
        {
            KeyboardState keyboard = Keyboard.GetState();

            if (GameOver)
            {
                return;
                //wenn spieler keine Health mehr hat => Frezze
            }

            if (keyboard.IsKeyDown(Key.Escape))
            {
                window.Exit();
            }



        }
        internal void UpdateEnemy(List<Enemy> enemies, float deltaTime) // Enemies bewegen sich richtung sdasdwsadSpieler
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].Hitpoints < 0)
                {
                    enemies.RemoveAt(i);
                }
            }
            foreach (Enemy enemy in enemies)
            {
                enemy.EnemyAI(enemy, player);
            }
        }
        internal void UpdatePlayer(float deltaTime)
        {
            MouseState mouseState = Mouse.GetState();

           // Console.WriteLine("Mouse X" + mouseState.X + "\n" + "Mouse Y " + mouseState.Y);
            model.player.ShotCoolDown -= deltaTime;

            if (model.player.Hitpoints > 1)
            {
                model.player.Hitpoints = 1f;
            }
            model.player.MovePlayer(model.player, deltaTime);
            model.player.AglignPlayer();

            if (model.player.Shoot())
            {
                model.bullets.Add(new Bullet(Color.Black, model.player.Position, 0.001f, 0f, 2f,0));
                model.player.Ammo--;
            }
            int i = 0;
            foreach (Bullet bullet in model.bullets)
            {
                model.bullets[i].Velocity = deltaTime * 0.0005f;
                model.bullets[i].MoveBullet(model.bullets[i], model.player.Direction);
                model.bullets[i].Hitpoints -= deltaTime;
                //Console.WriteLine("Lifetime:" + model.bullets[i].Lifetime); i++;
            }
        }

        internal void ScrollControl(float deltaTime)
        {
            var mouse = Mouse.GetState();
            var scrollValue = mouse.ScrollWheelValue;

            if (scrollValue < oldScrollValue)
            {
                axisZoom = 2f;
                oldScrollValue = scrollValue;
            }
            else if (scrollValue > oldScrollValue)
            {
                axisZoom = -2f;
                oldScrollValue = scrollValue;
            }
            else if (scrollValue == oldScrollValue)
            {
                axisZoom = 0f;
            }
            var zoom = view.camera.Scale * (1 + deltaTime * axisZoom);
            if (zoom > 0.5f)
            {
                zoom = 0.5f;
            }
            if (zoom < 0.1f)
            {
                zoom = 0.1f;
            }
            // zoom = MathHelper.Clamp(zoom, 0.9f, 1f); //setzt Zoom Grenze, also bis wie weit man rein/raus zoomen kann
            view.camera.Scale = zoom;
        }

        internal void UpdateCheckCollision()
        {
            //Checkt Enemy/Player with LeverBorder Collision
            intersection.ObjCollidingWithLeverBorder(player);
            for (int i = 0; i < model.enemies.Count; i++)
            {
                intersection.ObjCollidingWithLeverBorder(model.enemies[i]);
            }

            //Check Enemy and Player collision
            for (int i = 0; i < model.enemies.Count; i++)
            {
                if (intersection.IsIntersecting(model.player, model.enemies[i]))
                {
                    Console.WriteLine("Player Collision mit:" + i + "Gegner.");
                    model.player.Hitpoints -= 0.001f;
                    if (model.player.Hitpoints < 0)
                    {
                        GameOver = true;
                    }
                }
            }

            //Check Obstacle with Player collision
            for (int i = 0; i < model.obstacles.Count; i++)
            {             
                    Console.WriteLine("Player Collision mit:" + i + "Obstacle.");
                intersection.CheckPlayerCollisionWithObstacle(player, model.obstacles);                
            }

            //Check Pickup with Player collision
            for (int i = 0; i < model.pickUps.Count; i++)
            {
                if (intersection.IsIntersecting(model.player, model.pickUps[i]))
                {
                    Console.WriteLine("Player Collision mit:" + i + "Pickup.");
                    float ranX = (float)rng.NextDouble() * 1.8f - 0.9f;
                    float ranY = (float)rng.NextDouble() * 1.8f - 0.9f;
                    for(int j = 0; j < model.gameObjects.Count; j++)
                    {
                        if(model.pickUps[i].Id == model.gameObjects[j].Id)
                        {
                            j++;
                        }
                        while (intersection.IsIntersecting(model.pickUps[i], model.gameObjects[j]))
                        {
                            ranX = (float)rng.NextDouble() * 1.8f - 0.9f;
                            ranY = (float)rng.NextDouble() * 1.8f - 0.9f;
                        }
                    }                 
                    model.pickUps[i].Position = new Vector2(ranX, ranY);

                    if (model.pickUps[i].Type == 1)
                    {
                        player.Ammo += 100;
                        Console.WriteLine("Ammo: +100");
                    }
                    if (model.pickUps[i].Type == 0)
                    {
                        player.Hitpoints += 0.1f;
                        Console.WriteLine("Leben: +100");
                    }
                }
            }
            //Check Enemy with Obstacle Collision
            intersection.CheckEnemyObstacleCollision(model.enemies, model.obstacles);

            //   intersection.EnemyWithEnemyCollision(model.enemies);



            //TODO: Bullet collison refacor
            for (int x = 0; x < model.bullets.Count; x++)
            {
                if (intersection.BulletCollision(model.bullets, model.enemies, model.obstacles) || model.bullets[x].Hitpoints < 0)
                {
                    model.bullets.RemoveAt(x);
                }
            }
        }

    }
}