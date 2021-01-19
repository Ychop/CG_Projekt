using CG_Projekt.Models;
using System.Collections.Generic;
using OpenTK;
using System;
using System.Drawing;

namespace CG_Projekt
{
    internal class Model
    {
        internal List<LevelGrid> levelGrids { get; } = new List<LevelGrid>();
        internal List<Enemy> enemies = new List<Enemy>();
        internal List<Obstacle> obstacles = new List<Obstacle>();
        internal List<PickUp> pickUps = new List<PickUp>();
        internal List<Bullet> bullets = new List<Bullet>();
        internal List<GameObject> gameObjects = new List<GameObject>();
        internal List<Weapon> weapons = new List<Weapon>();

        internal Player player { get; set; }


        internal Intersection intersection = new Intersection();
        private Random random = new Random();
        private int ObjectsLimit = 50; // sets the Limit for all Gameobjects each.
        private float ranX, ranY, ranS;
        public Model()
        {
            GenerateLevelGrid();
            GenerateGameObjects();

        }
        internal bool IntersectsAny(GameObject obj_)
        {
            foreach (GameObject obj in gameObjects)
            {
                if (intersection.IsIntersecting(obj_, obj) && obj != obj_ && (Math.Pow(obj_.Position.X - obj.Position.X, 2) + Math.Pow(obj_.Position.Y - obj.Position.Y, 2) <= (obj.Size + obj_.Size)))
                {
                    return true;
                }
            }
            return false;
        }
        internal void GenerateLevelGrid()
        {
            float y = -0.9f;
            float x = -0.9f;
            for (int i = 0; i < 100; i++)
            {
                x = -0.9f;
                for (int j = 0; j < 100; j++)
                {
                    levelGrids.Add(new LevelGrid(new Vector2(x, y)));
                    x += 0.018f;
                }
                y += 0.018f;
            }
        }
        internal void GenerateGameObjects()
        {
            //Generate Weapons
            GenerateWeapons();
            //Generate Player
            GeneratePlayer();
            //Generate Obstacles
            GenerateObstacles();
            //Generate Pickups
            GeneratePickUps();
            //Generate Enemies
            GenerateEnemies();

            Console.WriteLine("Obstacles insgesamt: " + obstacles.Count + " \n" + "Enemies insgesamt: " + enemies.Count + "\n" + "Pickups insgesamt: " + pickUps.Count);
        }
        internal void GeneratePlayer()
        {
            player = new Player(new Vector2((float)random.NextDouble() * 1.8f - 0.9f, (float)random.NextDouble() * 1.8f - 0.9f), 0.01f, 0f, 1f, -1);
        }
        internal void GenerateObstacles()
        {
            for (int i = 0; i < ObjectsLimit; i++)
            {
                ranX = (float)random.NextDouble() * 1.8f - 0.9f;
                ranY = (float)random.NextDouble() * 1.8f - 0.9f;
                ranS = (float)random.NextDouble() * 0.09f + 0.01f;
                obstacles.Add(new Obstacle(new Vector2(ranX, ranY), ranS, 0f, 1000f, i));
                gameObjects.Add(obstacles[i]);
                while (Math.Pow(obstacles[i].Position.X - player.Position.X, 2) + Math.Pow(obstacles[i].Position.Y - player.Position.Y, 2) < 0.09f)
                {
                    obstacles[i].Position = new Vector2((float)random.NextDouble() * 1.8f - 0.9f,(float)random.NextDouble() * 1.8f - 0.9f);
                }
                Console.WriteLine("Obstacle " + gameObjects.Count + ". erzeugt.");

            }
        }
        internal void GenerateEnemies()
        {
            for (int i = 0; i < ObjectsLimit; i++)
            {
                ranX = (float)random.NextDouble() * 1.8f - 0.9f;
                ranY = (float)random.NextDouble() * 1.8f - 0.9f;
                enemies.Add(new Enemy(new Vector2(ranX, ranY), 0.01f, 0.007f, 1f, gameObjects.Count));
                gameObjects.Add(enemies[i]);
                for (int j = 0; j < gameObjects.Count; j++)
                {
                    while (IntersectsAny(enemies[i]))
                    {
                        enemies[i].Position = new Vector2((float)random.NextDouble() * 1.8f - 0.9f, (float)random.NextDouble() * 1.8f - 0.9f);
                    }
                }
                Console.WriteLine("Enemy " + gameObjects.Count + ". erzeugt.");
            }
        }
        internal void GeneratePickUps()
        {
            for (int i = 0; i < ObjectsLimit; i++)
            {
                ranX = (float)random.NextDouble() * 1.8f - 0.9f;
                ranY = (float)random.NextDouble() * 1.8f - 0.9f;
                pickUps.Add(new PickUp(new Vector2(ranX, ranY), 0.01f, 0f, 1f, gameObjects.Count + 1, random.Next(5)));
                gameObjects.Add(pickUps[i]);
                for (int j = 0; j < gameObjects.Count - 1; j++)
                {
                    while (IntersectsAny(pickUps[i]))
                    {
                        pickUps[i].Position = new Vector2((float)random.NextDouble() * 1.8f - 0.9f, (float)random.NextDouble() * 1.8f - 0.9f);
                    }
                }
                Console.WriteLine("Pickup " + gameObjects.Count + ". erzeugt.");
            }
        }
        internal void GenerateWeapons()
        {
            for (int i = 1; i <= 4; i++)
            {
                weapons.Add(new Weapon(i));
            }
        }
    }
}

