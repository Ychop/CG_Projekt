﻿using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System;

namespace CG_Projekt.Models
{
    class Enemy : GameObject
    {
        internal Vector2 PlayerDirection;
        internal double AngleToPlayer;
        internal Enemy(Vector2 position_, float size_, float velocity_, float hitpoints_, int id_) : base(position_, size_, velocity_, hitpoints_, id_)
        {
            this.Position = position_;
            this.Size = size_;
            this.Hitpoints = hitpoints_;
            this.Velocity = velocity_;
            this.Id = id_;
        }

        public void EnemyAI(Enemy enemy, Player player, float deltaTime)
        {
            if((Math.Pow(enemy.Position.X - player.Position.X, 2) + Math.Pow(enemy.Position.Y - player.Position.Y, 2)) < 0.2f)
            {
                enemy.Velocity = deltaTime * 0.007f;
                PlayerDirection = new Vector2(player.Position.X - enemy.Position.X, player.Position.Y - enemy.Position.Y);
                PlayerDirection.Normalize(); // Ohne Normalize würden sich die gegner schneller zum spieler bewegen, je weiter sie von ihm weg sind weg sind
                double angleRad = Math.Atan2(PlayerDirection.Y, PlayerDirection.X);
                AngleToPlayer = angleRad * (180 / Math.PI);
                enemy.Position += PlayerDirection * enemy.Velocity;
            }
        }
    }
}