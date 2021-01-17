using CG_Projekt.Models;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;

namespace CG_Projekt
{
    class Controller
    {
        public Intersection intersection { get; } = new Intersection();
        public Camera Camera { get; }

        internal View view;
        internal Model model;
        internal GameWindow window;
        internal int oldScrollValue = 0;
        internal Weapon weapon;
        internal float axisZoom = 0f;
        internal bool GameOver;
        internal Player player;
        private Random rng = new Random();
        internal Vector2 mousePosition;



        internal Controller(View view_, Model model_, GameWindow window_)
        {
            view = view_;
            model = model_;
            window = window_;
            player = model.player;
            Camera = view.Camera;
            weapon = model.weapons[0];
        }
        internal void Update(float deltaTime)
        {

            if (GameOver)
            {
                return;
            }
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
            // Check WeaponSelection
           
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
        internal void WepaonSelection(char key_)
        {         
            switch (key_)
            {
                case '1':
                    weapon = model.weapons[0];
                    Console.WriteLine("Pistole ausgewählt.");
                    break;
                case '2':
                    weapon = model.weapons[1];
                    Console.WriteLine("UZI ausgewählt.");
                    break;
                case '3':
                    weapon = model.weapons[2];
                    Console.WriteLine("Shotgun ausgewählt.");
                    break;
                case '4':
                    weapon = model.weapons[3];
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
                    float ranX = (float)rng.NextDouble() * 1.8f - 0.9f;
                    float ranY = (float)rng.NextDouble() * 1.8f - 0.9f;
                    for (int j = 0; j < model.gameObjects.Count; j++)
                    {
                        if (model.enemies[i].Id == model.gameObjects[j].Id)
                        {
                            j++;
                        }
                        if (intersection.IsIntersecting(model.enemies[i], model.gameObjects[j]) && Math.Pow(enemies[i].Position.X - player.Position.X, 2) + Math.Pow(enemies[i].Position.Y - player.Position.Y, 2) > 0.2f)
                        {
                            ranX = (float)rng.NextDouble() * 1.8f - 0.9f;
                            ranY = (float)rng.NextDouble() * 1.8f - 0.9f;
                        }
                        else
                        {
                            model.enemies[i].Position = new Vector2(ranX, ranY);
                            model.enemies[i].Hitpoints = 1f;
                        }
                    }                  
                }
            }
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].EnemyAI(enemies[i], player, deltaTime);
            }
        }
        internal void UpdatePlayer(float deltaTime)
        {
            player.MovePlayer(model.player, deltaTime);
            player.AglignPlayer(mousePosition);
            player.Shoot(model.bullets, deltaTime, this.weapon);
            if(player.Hitpoints < 0)
            {
                GameOver = true;
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
            var zoom = view.Camera.Scale * (1 + deltaTime * axisZoom);
            if (zoom > 0.5f)
            {
                zoom = 0.5f;
            }
            if (zoom < 0.1f)
            {
                zoom = 0.1f;
            }
            // zoom = MathHelper.Clamp(zoom, 0.9f, 1f); //setzt Zoom Grenze, also bis wie weit man rein/raus zoomen kann
            view.Camera.Scale = zoom;
        }

        internal void UpdateCheckCollision()
        {
            //Checkt Enemy/Player with LeverBorder Collision
            intersection.ObjectCollidingWithLeverBorder(player);
            for (int i = 0; i < model.enemies.Count; i++)
            {
                intersection.ObjectCollidingWithLeverBorder(model.enemies[i]);
            }

            //Check Enemy and Player collision
            for (int i = 0; i < model.enemies.Count; i++)
            {
                if (intersection.IsIntersecting(model.player, model.enemies[i]))
                {
                    Console.WriteLine("Player Collision mit Enemy: " + model.enemies[i].Id);
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
                if (intersection.IsIntersecting(player, model.obstacles[i]))
                {
                    Console.WriteLine("Player Collision mit Obstacle: " + model.obstacles[i].Id);
                    intersection.ResetGameObjectPosition(player, model.obstacles[i]);
                }
            }

            //Check Pickup with Player collision
            for (int i = 0; i < model.pickUps.Count; i++)
            {
                if (intersection.IsIntersecting(model.player, model.pickUps[i]))
                {
                    Console.WriteLine("Player Collision mit Pickup: " + model.pickUps[i].Id);
                    float ranX = (float)rng.NextDouble() * 1.8f - 0.9f;
                    float ranY = (float)rng.NextDouble() * 1.8f - 0.9f;
                    for (int j = 0; j < model.gameObjects.Count; j++)
                    {
                        if (model.gameObjects[j].Id > 100)
                        {
                            continue;
                        }
                        if (intersection.IsIntersecting(model.pickUps[i], model.gameObjects[j]) && Math.Pow(model.pickUps[i].Position.X - model.gameObjects[j].Position.X, 2) + Math.Pow(model.pickUps[i].Position.Y - model.gameObjects[j].Position.Y, 2) < model.pickUps[i].Size + model.gameObjects[j].Size)
                        {
                            ranX = (float)rng.NextDouble() * 1.8f - 0.9f;
                            ranY = (float)rng.NextDouble() * 1.8f - 0.9f;
                        }
                        else
                        {
                            model.pickUps[i].Position = new Vector2(ranX, ranY);
                        }
                    }              
                    if (model.pickUps[i].Type == 1)
                    {
                        player.Ammo += 100;
                        Console.WriteLine("Ammo: +100");
                    }
                    if (model.pickUps[i].Type == 0 && player.Hitpoints < 1)
                    {
                        player.Hitpoints += 0.1f;
                        Console.WriteLine("Leben: +100");
                    }
                }
            }
            //Check Enemy with Obstacle Collision
            for (int i = 0; i < model.enemies.Count; i++)
            {
                for (int j = 0; j < model.obstacles.Count; j++)
                {
                    if (intersection.IsIntersecting(model.enemies[i], model.obstacles[j]))
                    {
                        intersection.ResetGameObjectPosition(model.enemies[i], model.obstacles[j]);
                    }
                }

            }

            //Check Bullet collision with GameObjects
            for (int i = 0; i < model.gameObjects.Count; i++)
            {
                for (int j = 0; j < model.bullets.Count; j++)
                {
                    if (intersection.IsIntersecting(model.bullets[j], model.gameObjects[i]) && model.gameObjects[i].Id < 101)
                    {
                        model.bullets.RemoveAt(j);
                       model.gameObjects[i].Hitpoints -= weapon.Damage;
                    }

                }
            }

            //TODO: EnemyWithEnemyCollision
        }

        internal void TranslateMouseCoordinates(int x, int y)
        {
            var fromViewportToWorld = Transformation.Combine(Camera.InvViewportMatrix, Camera.CameraMatrix.Inverted());
            var mouseVector = new Vector2(x, y);
            mousePosition = mouseVector.Transform(fromViewportToWorld);
            //  Console.WriteLine("Mouse X " + test.X + "\n" + "Mouse Y " + test.Y);
        }
    }

}