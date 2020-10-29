using CG_Projekt.Models;
using System.Collections.Generic;
using OpenTK;
using System;

namespace CG_Projekt
{
    internal class Model
    {
        internal List<LevelGrid> LevelGrids { get; } = new List<LevelGrid>();
        internal List<Enemy> Enemies = new List<Enemy>();
        internal List<Obstacle> Obstacles = new List<Obstacle>();
        internal List<PickUp> PickUps = new List<PickUp>();
        internal Player player { get; } = new Player();
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
                    LevelGrids.Add(new LevelGrid(new Vector2(x, y)));
                    x += 0.018f;
                }
                y += 0.018f;
            }
        }

        internal void GenerateGameObjects()
        {
            for (int i = 0; i < 10; i++) // Limit gibt die Anzahl der Gegner an, kann später durch einen Dynamischen Wert ersetzt werden (höherer Schwiergkeitsgrad => mehr gegner || Random).
            {
                Enemies.Add(new Enemy(new Vector2((float)random.NextDouble() * 1.8f - 0.9f, (float)random.NextDouble() * 1.8f - 0.9f)));
            }
            for (int i = 0; i < 10; i++)
            {
                Obstacles.Add(new Obstacle(new Vector2((float)random.NextDouble() * 1.8f - 0.9f, (float)random.NextDouble() * 1.8f - 0.9f)));
            }
            for (int i = 0; i < 10; i++)
            {
                PickUps.Add(new PickUp(new Vector2((float)random.NextDouble() * 1.8f - 0.9f, (float)random.NextDouble() * 1.8f - 0.9f)));
            }
        }
    }
}
