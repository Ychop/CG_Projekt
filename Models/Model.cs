namespace CG_Projekt
{
    using System;
    using System.Collections.Generic;
    using CG_Projekt.Models;
    using OpenTK;
    internal class Model
    {
        private readonly Random rng = new Random();
        private readonly int objectsLimit = 50; // sets the Limit for all Gameobjects each.
        internal int weaponSelected = 1;
        private float ranX;
        private float ranY;
        public Model()
        {
            this.GenerateLevelGrid();
            this.GenerateGameObjects();
        }
        internal Intersection Intersection { get; set; } = new Intersection();
        internal List<LevelGrid> LevelGrids { get; } = new List<LevelGrid>();
        internal List<Enemy> Enemies { get; set; } = new List<Enemy>();
        internal List<Obstacle> Obstacles { get; set; } = new List<Obstacle>();
        internal List<PickUp> PickUps { get; set; } = new List<PickUp>();
        internal List<Bullet> Bullets { get; set; } = new List<Bullet>();
        internal List<GameObject> GameObjects { get; set; } = new List<GameObject>();
        internal List<Weapon> Weapons { get; set; } = new List<Weapon>();
        internal List<Particle> Particles { get; set; } = new List<Particle>();
        internal List<Particle> RPGFragments { get; set; } = new List<Particle>();
        internal Player Player { get; set; }
        internal int Score { get; set; } = 0;
        internal float Time { get; private set; } = 0;
        internal void GenerateLevelGrid()
        {
            float y = -0.588f;
            float x;
            for (int i = 0; i < 99; i++)
            {
                x = -0.588f;
                for (int j = 0; j < 99; j++)
                {
                    this.LevelGrids.Add(new LevelGrid(new Vector2(x, y), 0.012f));
                    x += 0.012f;
                }
                y += 0.012f;
            }
        }
        internal void GenerateGameObjects()
        {
            // Generate Weapons
            this.GenerateWeapons();
            // Generate Player
            this.GeneratePlayer();
            // Generate Enemies
            this.GenerateEnemies();
            // Generate Obstacles
            this.GenerateObstacles();
            Console.WriteLine("Obstacles insgesamt: " + this.Obstacles.Count + " \n" + "Enemies insgesamt: " + this.Enemies.Count);
        }

        internal void GeneratePlayer()
        {
            float playerSizeDraw = 0.015f;
            float playerSizeColl = 0.01f;
            float playerHitpoints = 1f;
            float playerVelocity = 0.8f;
            int playerId = -1;
            Vector2 playerPosition = new Vector2(((float)this.rng.NextDouble() * 1.1f) - 0.60f, ((float)this.rng.NextDouble() * 1.1f) - 0.60f);
            this.Player = new Player(playerPosition, playerSizeDraw, playerSizeColl, playerVelocity, playerHitpoints, playerId);
            GameObjects.Add(this.Player);
        }

        internal void GenerateObstacles()
        {
            float obstacleVelocity = 0f;
            float maxSize = 0.06f;
            float obstacleHitpoints = 1000f;
            var collideRadDiff = 0.005f;
            this.ranX = ((float)this.rng.NextDouble() * 1.2f) - 0.6f;
            this.ranY = ((float)this.rng.NextDouble() * 1.2f) - 0.6f;
            for (int i = 0; i < this.objectsLimit; i++)
            {
                float obstacleSize = 0.02f;
                this.Obstacles.Add(new Obstacle(new Vector2(this.ranX, this.ranY), obstacleSize, obstacleSize - collideRadDiff, obstacleVelocity, obstacleHitpoints, GameObjects.Count - 1));
                while (Intersection.IntersectsAny(this.GameObjects, Obstacles[i]))
                {
                    this.ranX = ((float)this.rng.NextDouble() * 1.2f) - 0.6f;
                    this.ranY = ((float)this.rng.NextDouble() * 1.2f) - 0.6f;
                    Obstacles[i].Position = new Vector2(ranX, ranY);
                }
                while (!Intersection.IntersectsAny(this.GameObjects, Obstacles[i]) && obstacleSize < maxSize)
                {
                    obstacleSize += 0.01f;
                    Obstacles[i].RadiusDraw = obstacleSize;
                    Obstacles[i].RadiusCollision = obstacleSize - collideRadDiff;
                }
                this.GameObjects.Add(this.Obstacles[i]);
                Console.WriteLine("Obstacle " + (this.GameObjects.Count - 1) + ". erzeugt.");
            }
            //covers the level border edges
          
            this.Obstacles.Add(new Obstacle(new Vector2(-0.6f, -0.6f), 0.03f, 0.027f, 0f, 1000, GameObjects.Count - 1));
            this.Obstacles.Add(new Obstacle(new Vector2(0.6f, -0.6f), 0.03f, 0.027f, 0f, 1000, GameObjects.Count - 1));
            this.Obstacles.Add(new Obstacle(new Vector2(0.6f, 0.6f), 0.03f, 0.027f, 0f, 1000, GameObjects.Count - 1));
            this.Obstacles.Add(new Obstacle(new Vector2(-0.6f, 0.6f), 0.03f, 0.027f, 0f, 1000, GameObjects.Count - 1));
            
        }

        internal void GenerateEnemies()
        {
            float enemySizeDraw = 0.015f;
            float enemySizeColl = 0.011f;
            float enemyHitpoints = 1f;
            float enemyVelocity = 0.05f;
            var minDistanceEnemyPlayer = 0.3f;
            for (int i = 0; i < 50; i++)
            {
                this.ranX = ((float)this.rng.NextDouble() * 1.2f) - 0.6f;
                this.ranY = ((float)this.rng.NextDouble() * 1.2f) - 0.6f;
                this.Enemies.Add(new Enemy(new Vector2(this.ranX, this.ranY), enemySizeDraw, enemySizeColl, enemyVelocity, enemyHitpoints, (this.GameObjects.Count - 1), animationLength: 1.5f));
                this.GameObjects.Add(this.Enemies[i]);
                for (int j = 0; j < this.GameObjects.Count; j++)
                {
                    double distanceEnemyPlayer = Math.Pow(this.Enemies[i].Position.X - this.Player.Position.X, 2) + Math.Pow(this.Enemies[i].Position.Y - this.Player.Position.Y, 2);
                    while (Intersection.IntersectsAny(this.GameObjects, this.Enemies[i]) && distanceEnemyPlayer < minDistanceEnemyPlayer)
                    {
                        this.ranX = ((float)this.rng.NextDouble() * 1.2f) - 0.6f;
                        this.ranY = ((float)this.rng.NextDouble() * 1.2f) - 0.6f;
                        this.Enemies[i].Position = new Vector2(ranX, ranY);
                    }
                }
                Console.WriteLine("Enemy " + (this.GameObjects.Count - 1) + ". erzeugt.");
            }
        }

        internal void GeneratePickUp(Vector2 position_)
        {
            float pickupSizeDraw = 0.005f;
            float pickupSizeColl = pickupSizeDraw;
            float pickupVelocity = 0.8f;
            float pickupHitpoints = 100f;

            if ((Score % 2) == 0)
            {
                Vector2 ranDir = new Vector2((float)rng.NextDouble() * 0.02f - 0.01f, (float)rng.NextDouble() * 0.02f - 0.01f);
                PickUps.Add(new PickUp(position_ + ranDir, pickupSizeDraw, pickupSizeDraw, pickupVelocity, pickupHitpoints, GameObjects.Count, 0));
            }
            if ((Score % 3) == 0)
            {
                Vector2 ranDir = new Vector2((float)rng.NextDouble() * 0.02f - 0.01f, (float)rng.NextDouble() * 0.02f - 0.01f);
                PickUps.Add(new PickUp(position_ + ranDir, pickupSizeDraw, pickupSizeDraw, pickupVelocity, pickupHitpoints, GameObjects.Count, 1));
            }
            if ((Score % 5) == 0)
            {
                Vector2 ranDir = new Vector2((float)rng.NextDouble() * 0.02f - 0.01f, (float)rng.NextDouble() * 0.02f - 0.01f);
                PickUps.Add(new PickUp(position_ + ranDir, pickupSizeDraw, pickupSizeDraw, pickupVelocity, pickupHitpoints, GameObjects.Count, 2));
            }
            if ((Score % 20) == 0)
            {
                Vector2 ranDir = new Vector2((float)rng.NextDouble() * 0.02f - 0.01f, (float)rng.NextDouble() * 0.02f - 0.01f);
                PickUps.Add(new PickUp(position_ + ranDir, pickupSizeDraw, pickupSizeDraw, pickupVelocity, pickupHitpoints, GameObjects.Count, 3));
            }
            if ((Score % 50) == 0)
            {
                Vector2 ranDir = new Vector2((float)rng.NextDouble() * 0.02f - 0.01f, (float)rng.NextDouble() * 0.02f - 0.01f);
                PickUps.Add(new PickUp(position_ + ranDir, pickupSizeDraw, pickupSizeDraw, pickupVelocity, pickupHitpoints, GameObjects.Count, 4));
            }
            Console.WriteLine("Pickup " + this.GameObjects.Count + ". erzeugt.");
        }

        internal void GenerateWeapons()
        {
            for (int i = 1; i <= 4; i++)
            {
                this.Weapons.Add(new Weapon(i));
            }
        }
    }
}