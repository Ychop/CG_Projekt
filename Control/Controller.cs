namespace CG_Projekt
{
    using System;
    using System.Collections.Generic;
    using CG_Projekt.Framework;
    using CG_Projekt.Models;
    using OpenTK;
    using OpenTK.Input;

    internal class Controller
    {
        private readonly View view;
        private readonly Model model;
        private readonly Player player;
        private readonly Random rng = new Random();
        private float axisZoom = 0f;
        private float oldScrollValue = 0;
        private Weapon weapon;
        private Vector2 mousePosition;
        public bool GameStarted = false;
        private readonly SoundManager sManager;

        internal Controller(View view_, Model model_)
        {
            this.view = view_;
            this.model = model_;
            this.player = this.model.Player;
            this.Camera = this.view.Camera;
            this.weapon = this.model.Weapons[0];
            sManager = new SoundManager();
        }
        internal Intersection Intersection { get; } = new Intersection();

        internal Camera Camera { get; }

        internal void Update(float deltaTime)
        {
            UpdateMainMenu();
            if (!view.GameOver && GameStarted && view.TexturesLoaded)
            {
                // Zoom mit dem Mausrad
                this.ScrollControl(deltaTime);

                //Updatet die GameObjects.
                this.UpdatedGameObjects(deltaTime);

                // Checkt die Collisons
                this.CheckCollisions();
            }
        }
        internal void UpdatedGameObjects(float deltaTime)
        {
            // Updatet den Spieler
            this.UpdatePlayer(deltaTime);
            // Updatet die Enemies
            this.UpdateEnemy(deltaTime);
            //Updated die Partikel
            UpdateParticle(deltaTime);
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
        internal bool IsDead(GameObject obj_)
        {
            if (obj_.Hitpoints < 0)
            {
                return true;
            }
            else
            {
                return false;
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
        internal void UpdateEnemy(float deltaTime)
        {
            foreach (Enemy enemy in model.Enemies)
            {
                if (IsDead(enemy))
                {
                    model.Score++;
                    model.GeneratePickUp(enemy.Position);
                    this.PlaceNewEnemy(enemy);
                    enemy.Hitpoints = 1f;
                }
                if (enemy.SpeedUp > 0.8f)
                {
                    enemy.SpeedUp = 0.8f;
                }
                if (enemy.Hitpoints > 3f)
                {
                    enemy.Hitpoints = 3f;
                }
                enemy.EnemyAI(enemy, this.player, deltaTime);
                enemy.AnimationUpdate(deltaTime);
            }
        }
        internal void UpdatePlayer(float deltaTime)
        {
            this.player.MovePlayer(this.model.Player, deltaTime);
            this.player.AglignPlayer(this.mousePosition);
            this.player.Shoot(this.model.Bullets, deltaTime, this.weapon);
            if (IsDead(this.player))
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
            var maxZoomIn = 0.1f;
            var maxZoomOut = 0.2f;
            float zoomChange = 2f;
            if (scrollValue < this.oldScrollValue)
            {
                this.axisZoom = zoomChange;
                this.oldScrollValue = scrollValue;
            }
            else if (scrollValue > this.oldScrollValue)
            {
                this.axisZoom = -zoomChange;
                this.oldScrollValue = scrollValue;
            }
            else if (scrollValue == this.oldScrollValue)
            {
                this.axisZoom = 0f;
            }
            var zoom = this.view.Camera.Scale * (1 + (deltaTime * this.axisZoom));
            if (zoom > maxZoomOut)
            {
                zoom = maxZoomOut;
            }
            if (zoom < maxZoomIn)
            {
                zoom = maxZoomIn;
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
            foreach (Enemy enemy in model.Enemies)
            {
                this.Intersection.ObjectCollidingWithLeverBorder(enemy);
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
            foreach (Enemy enemy in model.Enemies)
            {
                if (this.Intersection.IsIntersectingCircle(this.model.Player, enemy))
                {
                    var minSplatter = 10;
                    var maxSplatter = 20;
                    var particleSize = 0.0015f;
                    var particleId = 1;
                    var particleHitpoints = 5f;
                    float particleSpeed;
                    Vector2 particleDirection;
                    var powerOfschubs = 0.001f;
                    Console.WriteLine("Player Collision mit Enemy: " + enemy.Id);
                    this.model.Player.Hitpoints -= enemy.Damage;
                    this.model.Player.Position += enemy.playerDirection.Normalized() * powerOfschubs;
                    for (int j = 0; j < rng.Next(minSplatter, maxSplatter); j++)
                    {
                        particleSpeed = (float)rng.NextDouble() - 0.2f;
                        particleDirection = new Vector2((float)rng.NextDouble() * 2 - 1, (float)rng.NextDouble() * 2 - 1);
                        this.model.Particles.Add(new Particle(player.Position, particleSize, particleSize, particleSpeed, particleHitpoints, particleId, particleDirection));
                    }
                    var Grunt = new CachedSound("../../Content/Sounds/Grunt.mp3");
                    sManager.PlaySound(Grunt);
                }
            }

            // Check Obstacle with Player collision
            foreach (Obstacle obstacle in model.Obstacles)
            {
                if (this.Intersection.IsIntersectingCircle(this.player, obstacle))
                {
                    float radiusSum = (player.RadiusCollision + obstacle.RadiusCollision);
                    Vector2 diff = player.Position - obstacle.Position;
                    diff /= diff.Length;
                    diff *= (radiusSum);
                    this.player.Position = obstacle.Position + diff;
                    Console.WriteLine("Player Collision mit Obstacle: " + obstacle.Id);
                }
            }
            // Check Pickup with Player collision
            for (int i = 0; i < this.model.PickUps.Count; i++)
            {
                if (this.Intersection.IsIntersectingCircle(this.model.Player, this.model.PickUps[i]))
                {
                    Console.WriteLine("Player Collision mit Pickup: " + this.model.PickUps[i].Id);
                    if (this.model.PickUps[i].Type > 0)
                    {
                        var ReloadSound = new CachedSound("../../Content/Sounds/Reload.mp3");
                        sManager.PlaySound(ReloadSound);
                    }
                    else
                    {
                        var Lifeup = new CachedSound("../../Content/Sounds/Lifeup.mp3");
                        sManager.PlaySound(Lifeup);
                    }
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
            int isBlood;//0 = false 1 = true;
            var fragmentSize = 0.002f;
            var fragmentSpeed = 0.5f;
            var fragmentHitpoints = 2f;
            var fragmentID = 0;
            var weaponIsRocket = 4;
            Vector2 fragmentDirection;
            foreach (GameObject gameObject in model.GameObjects)
            {
                for (int j = 0; j < this.model.Bullets.Count; j++)
                {
                    if (this.Intersection.IsIntersectingCircle(this.model.Bullets[j], gameObject) && gameObject.Id != -1)
                    {
                        if (gameObject.Id > 50)
                        {
                            isBlood = 0; ;
                        }
                        else
                        {
                            isBlood = 1;
                            var Bloodsound = new CachedSound("../../Content/Sounds/Bloodsound.mp3");
                            sManager.PlaySound(Bloodsound);
                        }
                        if (this.player.SelectedWeapon == weaponIsRocket)
                        {
                            for (int i = 0; i < rng.Next(10, 20); i++)
                            {
                                fragmentDirection = new Vector2((float)rng.NextDouble() * 2 - 1, (float)rng.NextDouble() * 2 - 1);
                                this.model.RPGFragments.Add(new Particle(gameObject.Position, fragmentSize, fragmentSize, fragmentSpeed, fragmentHitpoints, fragmentID, fragmentDirection));
                            }
                        }
                        for (int i = 0; i < rng.Next(10, 20); i++)
                        {
                            fragmentDirection = new Vector2((float)rng.NextDouble() * 2 - 1, (float)rng.NextDouble() * 2 - 1);
                            fragmentSize = 0.0015f;
                            fragmentSpeed = (float)rng.NextDouble() - 0.2f;
                            fragmentHitpoints = 5f;
                            this.model.Particles.Add(new Particle(gameObject.Position + (this.model.Bullets[j].Position - gameObject.Position), fragmentSize, fragmentSize, fragmentSpeed, fragmentHitpoints, isBlood, fragmentDirection));
                        }
                        this.model.Bullets.RemoveAt(j);
                        gameObject.Hitpoints -= this.weapon.Damage;
                    }
                }
            }
        }
        internal void CheckDebrisCollison()
        {
            var playerID = -1;
            foreach (GameObject gameObject in model.GameObjects)
            {
                foreach (Particle fragment in model.RPGFragments)
                {
                    if (this.Intersection.IsIntersectingCircle(fragment, gameObject) && gameObject.Id != playerID)
                    {
                        gameObject.Hitpoints -= this.weapon.Damage;
                    }
                }
            }
        }
        internal void CheckEnemyCollisions()
        {
            var enemyInOtherSphere = 0.0005f;
            var enemySlowdown = -0.05f;
            // EnemyWithEnemyCollision
            foreach (Enemy enemy in this.model.Enemies)
            {
                foreach (Enemy enemy1 in this.model.Enemies)
                {
                    double distanceEnemies = Math.Pow(enemy.Position.X - enemy1.Position.X, 2) + Math.Pow(enemy.Position.Y - enemy1.Position.Y, 2);
                    if (distanceEnemies <= enemyInOtherSphere)
                    {
                        enemy.Position += new Vector2(enemy1.Position.X - enemy.Position.X, enemy1.Position.Y - enemy.Position.Y) * enemySlowdown;
                    }
                }
            }
            // Check Enemy with Obstacle Collision
           foreach(Enemy enemy in model.Enemies)
            {
               foreach(Obstacle obstacle in model.Obstacles)
                {
                    if (this.Intersection.IsIntersectingCircle(enemy, obstacle))
                    {
                        float radiusSum = (enemy.RadiusCollision + obstacle.RadiusCollision);
                        Vector2 diff = enemy.Position - obstacle.Position;
                        diff /= diff.Length;
                        diff *= (radiusSum);
                        enemy.Position = obstacle.Position + diff;
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
                if (this.Intersection.IntersectsAny(model.GameObjects,obj))
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
