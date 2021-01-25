namespace CG_Projekt.Models
{
    using System;
    using OpenTK;

    internal class Enemy : GameObject
    {
        private Vector2 playerDirection;

        internal Enemy(Vector2 position_, float size_, float velocity_, float hitpoints_, int id_)
            : base(position_, size_, velocity_, hitpoints_, id_)
        {
            this.Position = position_;
            this.Radius = size_;
            this.Hitpoints = hitpoints_;
            this.Velocity = velocity_;
            this.Id = id_;
        }

        internal double AngleToPlayer { get; set; }

        internal void EnemyAI(Enemy enemy, Player player, float deltaTime)
        {
            if ((Math.Pow(enemy.Position.X - player.Position.X, 2) + Math.Pow(enemy.Position.Y - player.Position.Y, 2)) < 0.2f && (Math.Pow(enemy.Position.X - player.Position.X, 2) + Math.Pow(enemy.Position.Y - player.Position.Y, 2)) > 0.0001f)
            {
                enemy.Velocity = deltaTime * 0.01f;
                this.playerDirection = new Vector2(player.Position.X - enemy.Position.X, player.Position.Y - enemy.Position.Y);
                this.playerDirection.Normalize(); // Ohne Normalize würden sich die gegner schneller zum spieler bewegen, je weiter sie von ihm weg sind weg sind
                double angleRad = Math.Atan2(this.playerDirection.Y, this.playerDirection.X);
                this.AngleToPlayer = angleRad * (180 / Math.PI);
                enemy.Position += this.playerDirection * enemy.Velocity;
            }
        }
    }
}