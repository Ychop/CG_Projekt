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
        internal List<Player> players= new List<Player>();
        internal List<GameObject> gameObjects = new List<GameObject>();

        //internal Player player { get; set; }


        internal Intersection intersection = new Intersection();
        private Random random = new Random();
        private int ObjectsLimit = 50; // sets the Limit for all Gameobjects each.

        public Model()
        {
            GenerateLevelGrid();
            GenerateGameObjects();
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
                    levelGrids.Add(new LevelGrid(new Vector2(x, y), Color.Blue));
                    x += 0.018f;
                }
                y += 0.018f;
            }
        }

        internal void GenerateGameObjects()
        {

            //TODO: Overlapping still exists

            float ranX, ranY, ranS;
            //Generate Obstacles
            for (int i = 0; i < ObjectsLimit; i++)
            {
                ranX = (float)random.NextDouble() * 1.8f - 0.9f;
                ranY = (float)random.NextDouble() * 1.8f - 0.9f;
                ranS = (float)random.NextDouble() * 0.09f + 0.01f;
                obstacles.Add(new Obstacle(Color.Gray,new Vector2(ranX,ranY), ranX, ranY, ranS, 0f, 1000f, i));
                gameObjects.Add(obstacles[i]);
                Console.WriteLine("Obstacle " + i + ". erzeugt.");
            }
            //Generate Enemies
            for (int i = 0; i < ObjectsLimit; i++)
            {
                ranX = (float)random.NextDouble() * 1.8f - 0.9f;
                ranY = (float)random.NextDouble() * 1.8f - 0.9f;
                enemies.Add(new Enemy(Color.Red, new Vector2(ranX, ranY), ranX, ranY, 0.02f,0, 1f, gameObjects.Count + i));
                gameObjects.Add(enemies[i]);
                for(int j = 0; j < gameObjects.Count; j++)
                {
                    if (intersection.IsIntersecting(enemies[i], gameObjects[j]))
                    {
                        enemies[i].Position = new Vector2((float)random.NextDouble() * 1.8f - 0.9f, (float)random.NextDouble() * 1.8f - 0.9f);
                    }
                }                                
                Console.WriteLine("Enemy "  + gameObjects.Count + ". erzeugt.");
            }

            //Generate Pickups
            for (int i = 0; i < ObjectsLimit; i++)
            {
                ranX = (float)random.NextDouble() * 1.8f - 0.9f;
                ranY = (float)random.NextDouble() * 1.8f - 0.9f;
                pickUps.Add(new PickUp(Color.Yellow, new Vector2(ranX,ranY), ranX, ranY, 0.02f, 0f, 1f, gameObjects.Count + i, random.Next(2)));
                gameObjects.Add(pickUps[i]);
                for (int j = 0; j < gameObjects.Count - 1; j++)
                {
                    if (intersection.IsIntersecting(pickUps[i], gameObjects[j]))
                    {
                        pickUps[i].Position = new Vector2((float)random.NextDouble() * 1.8f - 0.9f, (float)random.NextDouble() * 1.8f - 0.9f);
                        j = 0;
                    }
                }
                Console.WriteLine("Pickup " + gameObjects.Count +  ". erzeugt.");
            }

            //Generate Player
            players.Add( new Player(Color.Green, new Vector2((float)random.NextDouble() * 1.8f - 0.9f, (float)random.NextDouble() * 1.8f - 0.9f), (float)random.NextDouble() * 1.8f - 0.9f, (float)random.NextDouble() * 1.8f - 0.9f, 0.02f, 0f, 1f, 1));
            for (int j = 0; j < gameObjects.Count - 1; j++)
            {
                while (intersection.IsIntersecting(players[0], gameObjects[j]))
                {
                    players[0].Position = new Vector2((float)random.NextDouble() * 1.8f - 0.9f, (float)random.NextDouble() * 1.8f - 0.9f);
                }
            }
            //gameObjects.Add(players[0]);

        }

    }
}
