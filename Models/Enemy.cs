using OpenTK;
using System.Collections.Generic;

namespace CG_Projekt.Models
{
    class Enemy
    {
        public Vector2 _position { get; set; }
        public float _size { get; }


        internal Enemy(Vector2 position, float size)
        {

            _position = position;
            _size = size;
        }

        public void EnemyAI(Enemy enemy, float deltaTime, Player player)
        {
            Vector2 playerDirection = new Vector2(player._position.X - enemy._position.X, player._position.Y - enemy._position.Y);
            playerDirection.Normalize(); // Ohne Normalize würden sich die gegner schneller zum spieler bewegen, je weiter sie von ihm weg sind weg sind
            enemy._position += playerDirection * deltaTime * 0.01f;
        }
    }
}