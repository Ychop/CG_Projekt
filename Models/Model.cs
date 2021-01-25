namespace CG_Projekt
{
    using System;
    using System.Collections.Generic;
    using CG_Projekt.Models;
    using OpenTK;

    internal class Model
    {
        private Random random = new Random();
        private int objectsLimit = 50; // sets the Limit for all Gameobjects each.
        private float ranX;
        private float ranY;
        private float ranS;

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

        internal Player Player { get; set; }
        internal int Score { get; set; } = 0;
        internal int weaponSelected = 1;
        internal bool IntersectsAny(GameObject obj_)
        {
            foreach (GameObject obj in this.GameObjects)
            {
                if (this.Intersection.IsIntersecting(obj_, obj) && obj != obj_ && (Math.Pow(obj_.Position.X - obj.Position.X, 2) + Math.Pow(obj_.Position.Y - obj.Position.Y, 2) <= (obj.Radius + obj_.Radius)))
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
                    this.LevelGrids.Add(new LevelGrid(new Vector2(x, y)));
                    x += 0.018f;
                }

                y += 0.018f;
            }
        }

        internal void GenerateGameObjects()
        {
            // Generate Weapons
            this.GenerateWeapons();

            // Generate Player
            this.GeneratePlayer();

            // Generate Obstacles
            this.GenerateObstacles();

            // Generate Pickups
            this.GeneratePickUps();

            // Generate Enemies
            this.GenerateEnemies();

            Console.WriteLine("Obstacles insgesamt: " + this.Obstacles.Count + " \n" + "Enemies insgesamt: " + this.Enemies.Count + "\n" + "Pickups insgesamt: " + this.PickUps.Count);
        }

        internal void GeneratePlayer()
        {
            this.Player = new Player(new Vector2(((float)this.random.NextDouble() * 1.8f) - 0.9f, ((float)this.random.NextDouble() * 1.8f) - 0.9f), 0.01f, 0.8f, 1f, -1);
        }

        internal void GenerateObstacles()
        {
            for (int i = 0; i < this.objectsLimit; i++)
            {
                this.ranX = ((float)this.random.NextDouble() * 1.8f) - 0.9f;
                this.ranY = ((float)this.random.NextDouble() * 1.8f) - 0.9f;
                this.ranS = ((float)this.random.NextDouble() * 0.09f) + 0.01f;
                this.Obstacles.Add(new Obstacle(new Vector2(this.ranX, this.ranY), this.ranS, 0f, 1000f, i));
                this.GameObjects.Add(this.Obstacles[i]);
                while (Math.Pow(this.Obstacles[i].Position.X - this.Player.Position.X, 2) + Math.Pow(this.Obstacles[i].Position.Y - this.Player.Position.Y, 2) < 0.09f)
                {
                    this.Obstacles[i].Position = new Vector2(((float)this.random.NextDouble() * 1.8f) - 0.9f, ((float)this.random.NextDouble() * 1.8f) - 0.9f);
                }

                Console.WriteLine("Obstacle " + this.GameObjects.Count + ". erzeugt.");
            }
        }

        internal void GenerateEnemies()
        {
            for (int i = 0; i < this.objectsLimit; i++)
            {
                this.ranX = ((float)this.random.NextDouble() * 1.8f) - 0.9f;
                this.ranY = ((float)this.random.NextDouble() * 1.8f) - 0.9f;
                this.Enemies.Add(new Enemy(new Vector2(this.ranX, this.ranY), 0.01f, 0.007f, 1f, this.GameObjects.Count));
                this.GameObjects.Add(this.Enemies[i]);
                for (int j = 0; j < this.GameObjects.Count; j++)
                {
                    while (this.IntersectsAny(this.Enemies[i]))
                    {
                        this.Enemies[i].Position = new Vector2(((float)this.random.NextDouble() * 1.8f) - 0.9f, ((float)this.random.NextDouble() * 1.8f) - 0.9f);
                    }
                }

                Console.WriteLine("Enemy " + this.GameObjects.Count + ". erzeugt.");
            }
        }

        internal void GeneratePickUps()
        {
            for (int i = 0; i < this.objectsLimit; i++)
            {
                this.ranX = ((float)this.random.NextDouble() * 1.8f) - 0.9f;
                this.ranY = ((float)this.random.NextDouble() * 1.8f) - 0.9f;
                this.PickUps.Add(new PickUp(new Vector2(this.ranX, this.ranY), 0.005f, 0f, 1f, this.GameObjects.Count + 1, this.random.Next(5)));
                this.GameObjects.Add(this.PickUps[i]);
                for (int j = 0; j < this.GameObjects.Count - 1; j++)
                {
                    while (this.IntersectsAny(this.PickUps[i]))
                    {
                        this.PickUps[i].Position = new Vector2(((float)this.random.NextDouble() * 1.8f) - 0.9f, ((float)this.random.NextDouble() * 1.8f) - 0.9f);
                    }
                }

                Console.WriteLine("Pickup " + this.GameObjects.Count + ". erzeugt.");
            }
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