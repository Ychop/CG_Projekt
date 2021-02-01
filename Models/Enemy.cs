namespace CG_Projekt.Models
{
    using OpenTK;
    using System;
    internal class Enemy : GameObject
    {
        public Vector2 playerDirection;

        internal Enemy(Vector2 position_, float radiusDraw_, float radiusColl_, float velocity_, float hitpoints_, int id_, float animationLength)
            : base(position_, radiusDraw_, radiusColl_, velocity_, hitpoints_, id_)
        {
            this.Position = position_;
            this.RadiusDraw = radiusDraw_;
            this.RadiusCollision = radiusColl_;
            this.Hitpoints = hitpoints_;
            this.Velocity = velocity_;
            this.Id = id_;
            AnimationLength = animationLength;
        }
        internal double AngleToPlayer { get; set; }
        internal float SpeedUp { get; set; } = 0.03f;
        internal float Damage { get; set; } = 0.025f;
        internal float NormalizedAnimationTime { get; set; } = 0f;
        public float AnimationLength { get; }
        public void AnimationUpdate(float deltaTime)
        {
            base.Update(deltaTime);
            NormalizedAnimationTime += deltaTime / AnimationLength;
            NormalizedAnimationTime %= 1f;
        }

        internal void EnemyAI(Enemy enemy, Player player, float deltaTime)
        {
            double distanceEnemyPlayer = (Math.Pow(enemy.Position.X - player.Position.X, 2) + Math.Pow(enemy.Position.Y - player.Position.Y, 2));
            double minDistanceEnemyPlayer = 0.05f;
            double maxDistanceEnemyPlayer = 0.0001f;
            if (distanceEnemyPlayer < minDistanceEnemyPlayer && distanceEnemyPlayer > maxDistanceEnemyPlayer)
            {
                enemy.Velocity = deltaTime * SpeedUp;
                this.playerDirection = new Vector2(player.Position.X - enemy.Position.X, player.Position.Y - enemy.Position.Y);
                // Ohne Normalize würden sich die gegner schneller zum spieler bewegen, je weiter sie von ihm weg sind weg sind
                this.playerDirection.Normalize();
                double angleRad = Math.Atan2(this.playerDirection.Y, this.playerDirection.X);
                this.AngleToPlayer = angleRad * (180 / Math.PI);
                enemy.Position += this.playerDirection * enemy.Velocity;
            }
        }
    }
}