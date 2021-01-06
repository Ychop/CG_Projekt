using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Collections.Generic;

namespace CG_Projekt.Models
{
    class Enemy
    {
        public Vector2 Position { get; set; }
        public float Size { get; }
        public float Velocity { get; set; }
        public float Health { get; set; }

        internal Enemy(Vector2 position_, float size_)
        {

            Position = position_;
            Size = size_;
            Health = 1f;
        }

        public void EnemyHelath(Enemy enemy)
        {
            GL.LineWidth(5f);
            GL.Begin(PrimitiveType.Lines);          
            GL.Color3(Color.White);
            GL.Vertex2(enemy.Position + new Vector2(-Size ,Size + 0.002f));
            GL.Vertex2(enemy.Position + new Vector2(Size, Size + 0.002f));
            GL.End();

            GL.LineWidth(4f);
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(Color.Green);
            GL.Vertex2(enemy.Position + new Vector2(-Size * enemy.Health, Size + 0.002f));
            GL.Vertex2(enemy.Position + new Vector2(Size * enemy.Health, Size + 0.002f));
            GL.End();
        }

        public void EnemyAI(Enemy enemy,float deltaTime, Player player)
        {
            Velocity = deltaTime * 0.01f;// man kann die gewschwindigkeit auch randommachen, vlt an die größe koppeln ?
            Vector2 playerDirection = new Vector2(player.Position.X - enemy.Position.X, player.Position.Y - enemy.Position.Y);
            playerDirection.Normalize(); // Ohne Normalize würden sich die gegner schneller zum spieler bewegen, je weiter sie von ihm weg sind weg sind
            enemy.Position += playerDirection * Velocity;
        }

        public void EnemyCollision(List<Enemy> enemies)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                Enemy enemy = enemies[i];
                for (int j = 0; j < enemies.Count; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }
                    bool enemyXCollision = (enemy.Position.X + 0.01f) >= (enemies[j].Position.X - enemies[j].Size) && (enemy.Position.X - 0.01f) <= (enemies[j].Position.X + enemies[j].Size);
                    bool enemyYCollision = (enemy.Position.Y + 0.01f) >= (enemies[j].Position.Y - enemies[j].Size) && (enemies[j].Position.Y - 0.01f) <= (enemies[j].Position.Y + enemies[j].Size);
                    if (enemyXCollision && enemyYCollision)
                    {
                        enemy.Velocity = 0;
                    }
                }
            }
        }
    }
}