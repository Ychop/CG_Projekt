using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Collections.Generic;

namespace CG_Projekt.Models
{
    class Enemy : GameObject
    {
        internal Enemy(Color color_, Vector2 position_, float size_, float velocity_, float hitpoints_, int id_) : base(color_, position_, size_, velocity_, hitpoints_, id_)
        {
            this.Color = color_;
            this.Position = position_;
            this.Size = size_;
            this.Hitpoints = hitpoints_;
            this.Velocity = velocity_;
            this.Id = id_;
        }

        public void EnemyAI(Enemy enemy, Player player, float deltaTime)
        {
            this.Velocity = deltaTime * 0.007f;
            Vector2 playerDirection = new Vector2(player.Position.X - enemy.Position.X, player.Position.Y - enemy.Position.Y);
            playerDirection.Normalize(); // Ohne Normalize würden sich die gegner schneller zum spieler bewegen, je weiter sie von ihm weg sind weg sind
            enemy.Position += playerDirection * Velocity;
        }


        public void EnemyHelath(Enemy enemy)
        {
            GL.LineWidth(5f);
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(Color.White);
            GL.Vertex2(enemy.Position + new Vector2(-Size, Size + 0.002f));
            GL.Vertex2(enemy.Position + new Vector2(Size, Size + 0.002f));
            GL.End();

            GL.LineWidth(4f);
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(Color.Green);
            GL.Vertex2(enemy.Position + new Vector2(-Size * enemy.Hitpoints, Size + 0.002f));
            GL.Vertex2(enemy.Position + new Vector2(Size * enemy.Hitpoints, Size + 0.002f));
            GL.End();
        }

       

    }
}