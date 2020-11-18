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



        internal Player player { get; } = new Player();
        internal Intersection intersection = new Intersection();
        private Random random = new Random();

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
            float ranX, ranY, ranS;

            for (int i = 0; i < 50; i++)
            {
                ranX = (float)random.NextDouble() * 1.8f - 0.9f;
                ranY = (float)random.NextDouble() * 1.8f - 0.9f;
                ranS = (float)random.NextDouble() * 0.09f + 0.01f;
                while (i > 0 && intersection.CheckObstacleCollision(obstacles[i - 1], obstacles, ranX, ranY) && (ranS + ranX > 0.9 || ranS + ranY > 0.9 || -ranS - ranX < -0.9 || -ranS - ranY < -0.9))
                {
                    ranX = (float)random.NextDouble() * 1.8f - 0.9f;
                    ranY = (float)random.NextDouble() * 1.8f - 0.9f;
                    ranS = (float)random.NextDouble() * 0.09f + 0.01f;
                }
                obstacles.Add(new Obstacle(new Vector2(ranX, ranY), ranS));
            }
            for (int i = 0; i < 50; i++) // Limit gibt die Anzahl der Gegner an, kann später durch einen Dynamischen Wert ersetzt werden (höherer Schwiergkeitsgrad => mehr gegner || Random).
            {
                ranX = (float)random.NextDouble() * 1.8f - 0.9f;
                ranY = (float)random.NextDouble() * 1.8f - 0.9f;
              
                while (i > 0 && intersection.CheckEnemyCollision(obstacles,enemies[i-1],ranX,ranY))
                {

                    ranX = (float)random.NextDouble() * 1.8f - 0.9f;
                    ranY = (float)random.NextDouble() * 1.8f - 0.9f;
                }
                enemies.Add(new Enemy(new Vector2(ranX, ranY), 0.01f));

            }
            for (int i = 0; i < 50; i++)
            {
                ranX = (float)random.NextDouble() * 1.8f - 0.9f;
                ranY = (float)random.NextDouble() * 1.8f - 0.9f;
                while (intersection.CheckPickUpCollision(enemies, obstacles, pickUps, ranX, ranY))
                {
                    ranX = (float)random.NextDouble() * 1.8f - 0.9f;
                    ranY = (float)random.NextDouble() * 1.8f - 0.9f;
                }
                pickUps.Add(new PickUp(new Vector2(ranX, ranY), 0.01f));
            }
        }
    }
}
