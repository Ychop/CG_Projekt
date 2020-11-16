using CG_Projekt.Models;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
namespace CG_Projekt
{
    class Intersection
    {
        Random random = new Random();
        float playerRechteKante, playerLinkeKante, playerObereKante, playerUntereKante, obstacleRechteKante, obstacleLinkeKante, obstacleObereKante, obstacleUntereKante;

        public void CheckPlayerBorderCollision(Player player)
        {
            playerRechteKante = player._position.X + player._size;
            playerLinkeKante = player._position.X - player._size;
            playerObereKante = player._position.Y + player._size;
            playerUntereKante = player._position.Y - player._size;
            if (playerObereKante > 0.9f) //Obere Levelgrenze
            {
                player._position = new OpenTK.Vector2(player._position.X, 0.89f);
            }
            if (playerUntereKante < -0.9f) //Untere Levelgrenze
            {
                player._position = new OpenTK.Vector2(player._position.X, -0.89f);
            }
            if (playerRechteKante > 0.9f) //Rechte Levelgrenze
            {
                player._position = new OpenTK.Vector2(0.89f, player._position.Y);
            }
            if (playerLinkeKante < -0.9f) //Linke Levelgrenze
            {
                player._position = new OpenTK.Vector2(-0.89f, player._position.Y);
            }
        }

        public void CheckPlayerCollisionWithGameobject(Player player, List<Enemy> enemies, List<Obstacle> obstacles, List<PickUp> pickUps)
        {
            // Man könnte evlt. die Ganzen Gameobjects in eine Liste Packen und dann so die Collisions abfragen, wäre vlt schöner und performanter.
            var i = 0;
            var j = 0;


            playerRechteKante = player._position.X + player._size;
            playerLinkeKante = player._position.X - player._size;
            playerObereKante = player._position.Y + player._size;
            playerUntereKante = player._position.Y - player._size;

            foreach (Enemy enemy in enemies) // Checkt Collision mit Spieler und Enemy
            {
                bool xCollision = playerRechteKante >= enemies[i]._position.X - enemies[i]._size && enemies[i]._position.X + enemies[i]._size >= playerLinkeKante;
                bool yCollision = playerObereKante >= enemies[i]._position.Y - enemies[i]._size && enemies[i]._position.Y + enemies[i]._size >= playerUntereKante;
                if (xCollision && yCollision)
                {
                    Console.WriteLine("Player Collision mit:" + i + "Gegner.");
                    //TODO: Hier könnte man noch eine Funktion hinzufügen was dann passiert wenn der spieler den Gegner berührt. (Gamefrezze oder gameover Bild) 

                }
                if ((enemies[i]._position.Y + enemies[i]._size) > 0.9f) //Obere Levelgrenze
                {
                    enemies[i]._position = new OpenTK.Vector2(enemies[i]._position.X, 0.89f);
                }
                if ((enemies[i]._position.Y - enemies[i]._size) < -0.9f) //Untere Levelgrenze
                {
                    enemies[i]._position = new OpenTK.Vector2(enemies[i]._position.X, -0.89f);
                }
                if ((enemies[i]._position.X + enemies[i]._size) > 0.9f) //Rechte Levelgrenze
                {
                    enemies[i]._position = new OpenTK.Vector2(0.89f, enemies[i]._position.Y);
                }
                if ((enemies[i]._position.X - enemies[i]._size) < -0.9f) //Linke Levelgrenze
                {
                    enemies[i]._position = new OpenTK.Vector2(-0.89f, enemies[i]._position.Y);
                }
                i++;
            }
            i = 0;
            foreach (Obstacle obstacle in obstacles) // Checkt Collision mit Spieler und Obstacle
            {
                obstacleRechteKante = obstacles[i]._position.X + obstacles[i]._size;
                obstacleLinkeKante = obstacles[i]._position.X - obstacles[i]._size;
                obstacleObereKante = obstacles[i]._position.Y + obstacles[i]._size;
                obstacleUntereKante = obstacles[i]._position.Y - obstacles[i]._size;

                bool xCollision = playerRechteKante >= obstacleLinkeKante && playerLinkeKante <= obstacleRechteKante;
                bool yCollision = playerObereKante >= obstacleUntereKante && playerUntereKante <= obstacleObereKante;


                if (xCollision && yCollision)
                {
                    if (playerRechteKante >= obstacleLinkeKante && playerLinkeKante < obstacleLinkeKante && playerLinkeKante < (obstacles[i]._position.X - (obstacles[i]._size + (1.5f * player._size))))
                    {
                        player._position = new Vector2(obstacles[i]._position.X - (player._size + obstacles[i]._size), player._position.Y);
                    }
                    else if (playerObereKante >= obstacleUntereKante && playerUntereKante < obstacleUntereKante && playerUntereKante < (obstacles[i]._position.Y - (obstacles[i]._size + (1.5f * player._size))))
                    {
                        player._position = new Vector2(player._position.X, obstacles[i]._position.Y - (player._size + obstacles[i]._size));
                    }
                    else if (playerUntereKante <= obstacleObereKante && playerObereKante > obstacleObereKante && playerObereKante > (obstacles[i]._position.Y + (obstacles[i]._size + (1.5f * player._size))))
                    {
                        player._position = new Vector2(player._position.X, obstacles[i]._position.Y + (player._size + obstacles[i]._size));
                    }
                    else if (playerLinkeKante <= obstacleRechteKante && playerRechteKante > obstacleRechteKante)
                    {
                        player._position = new Vector2(obstacles[i]._position.X + (player._size + obstacles[i]._size), player._position.Y);
                    }

                }
                i++;
            }
            i = 0;
            foreach (PickUp pickup in pickUps) // Checkt Collision mit Spieler und Pickup
            {
                bool xCollision = playerRechteKante >= pickUps[i]._position.X - pickUps[i]._size && pickUps[i]._position.X + pickUps[i]._size >= playerLinkeKante;
                bool yCollision = playerObereKante >= pickUps[i]._position.Y - pickUps[i]._size && pickUps[i]._position.Y + pickUps[i]._size >= playerUntereKante;
                if (xCollision && yCollision)
                {
                    Console.WriteLine("Player Collision mit:" + i + "Pickup.");
                    float ranX = (float)random.NextDouble() * 1.8f - 0.9f;
                    float ranY = (float)random.NextDouble() * 1.8f - 0.9f;
                    while (CheckPickUpCollision(enemies, obstacles, pickUps, ranX, ranY))
                    {
                        ranX = (float)random.NextDouble() * 1.8f - 0.9f;
                        ranY = (float)random.NextDouble() * 1.8f - 0.9f;
                    }
                    pickUps[i]._position = new Vector2(ranX, ranY);
                }
                i++;
            }
        }
        public bool CheckPickUpCollision(List<Enemy> enemies, List<Obstacle> obstacles, List<PickUp> pickups, float ranX, float ranY) // Platziert die Pickups so, dass sie nicht in einem anderen Objekt Spawnen (Player Ausgenommen)
        {
            int i = 0;
            foreach (Enemy enemy in enemies)
            {
                bool xCollision = ranX + 0.01f >= enemies[i]._position.X - enemies[i]._size && ranX - 0.01f <= enemies[i]._position.X + enemies[i]._size;
                bool yCollision = ranY + 0.01f >= enemies[i]._position.Y - enemies[i]._size && ranY - 0.01f <= enemies[i]._position.Y + enemies[i]._size;
                if (xCollision && yCollision)
                {
                    return true;
                }
                i++;
            }
            i = 0;
            foreach (Obstacle obstacle in obstacles)
            {
                bool xCollision = ranX + 0.01f >= obstacles[i]._position.X - obstacles[i]._size && ranX - 0.01f <= obstacles[i]._position.X + obstacles[i]._size;
                bool yCollision = ranY + 0.01f >= obstacles[i]._position.Y - obstacles[i]._size && ranY - 0.01f <= obstacles[i]._position.Y + obstacles[i]._size;
                if (xCollision && yCollision)
                {
                    return true;
                }
                i++;
            }
            i = 0;
            foreach (PickUp pickUp in pickups)
            {
                bool xCollision = ranX + 0.01f >= pickups[i]._position.X - pickups[i]._size && ranX - 0.01f <= pickups[i]._position.X + pickups[i]._size;
                bool yCollision = ranY + 0.01f >= pickups[i]._position.Y - pickups[i]._size && ranY - 0.01f <= pickups[i]._position.Y + pickups[i]._size;
                if (xCollision && yCollision && ranX != pickups[i]._position.X && ranY != pickups[i]._position.Y)
                {
                    return true;
                }
                i++;
            }
            return false;
        }

        public bool CheckObstacleCollision(Obstacle newObstacle, List<Obstacle> obstacles, float ranX, float ranY) // Setzt die Obstacles so, dass sie nicht in einen Anderen Objekt landen (Player Ausgenommen)
        {
            int i = 0;
            foreach (Obstacle obstacle in obstacles)
            {
                bool xCollision = ranX + newObstacle._size >= obstacles[i]._position.X - obstacles[i]._size && ranX - newObstacle._size <= obstacles[i]._position.X + obstacles[i]._size;
                bool yCollision = ranY + newObstacle._size >= obstacles[i]._position.Y - obstacles[i]._size && ranY - newObstacle._size <= obstacles[i]._position.Y + obstacles[i]._size;
                if (xCollision && yCollision)
                {
                    return true;
                }
                i++;
            }
            return false;
        }

        public bool CheckEnemyCollision(List<Obstacle> obstacles, Enemy enemy, float ranX, float ranY) // Schaut das Enemyies nicht in Obstacle landen
        {
            int i = 0;
            foreach (Obstacle obstacle in obstacles)
            {

                bool xCollision = ranX + enemy._size >= obstacles[i]._position.X - obstacles[i]._size && ranX - enemy._size <= obstacles[i]._position.X + obstacles[i]._size;
                bool yCollision = ranY + enemy._size >= obstacles[i]._position.Y - obstacles[i]._size && ranY - enemy._size <= obstacles[i]._position.Y + obstacles[i]._size;
                if (xCollision && yCollision)
                {
                    return true;
                }
                i++;
            }
            return false;
        }

        public void CheckEnemyObstacleCollision(List<Enemy> enemies, List<Obstacle> obstacles)
        {
            var j = 0;
            foreach (Enemy enemy in enemies)
            {
                for (int i = 0; i < 50; i++)
                {
                    obstacleRechteKante = obstacles[i]._position.X + obstacles[i]._size;
                    obstacleLinkeKante = obstacles[i]._position.X - obstacles[i]._size;
                    obstacleObereKante = obstacles[i]._position.Y + obstacles[i]._size;
                    obstacleUntereKante = obstacles[i]._position.Y - obstacles[i]._size;
                    // Hier wird noch als Size die vom player angegeben, da beide NOCH gleich groß sind
                    bool enemyXCollision = (enemies[j]._position.X + 0.01f) >= obstacleLinkeKante && (enemies[j]._position.X - 0.01f) <= obstacleRechteKante;
                    bool enemyYCollision = (enemies[j]._position.Y + 0.01f) >= obstacleUntereKante && (enemies[j]._position.Y - 0.01f) <= obstacleObereKante;

                    if (enemyXCollision && enemyYCollision)
                    {
                        if ((enemies[j]._position.X + 0.01f) >= obstacleLinkeKante && (enemies[j]._position.X - 0.01f) < obstacleLinkeKante && (enemies[j]._position.X - enemies[j]._size) < (obstacles[i]._position.X - (obstacles[i]._size + (1.5f * 0.01f))))
                        {
                            enemies[j]._position = new Vector2(obstacles[i]._position.X - (0.01f + obstacles[i]._size), enemies[j]._position.Y);
                        }
                        else if ((enemies[j]._position.Y + 0.01f) >= obstacleUntereKante && (enemies[j]._position.Y - 0.01f) < obstacleUntereKante && (enemies[j]._position.Y - enemies[j]._size) < (obstacles[i]._position.Y - (obstacles[i]._size + (1.5f * 0.01f))))
                        {
                            enemies[j]._position = new Vector2(enemies[j]._position.X, obstacles[i]._position.Y - (0.01f + obstacles[i]._size));
                        }
                        else if ((enemies[j]._position.Y - 0.01f) <= obstacleObereKante && (enemies[j]._position.Y + 0.01f) > obstacleObereKante && (enemies[j]._position.Y + enemies[j]._size) > (obstacles[i]._position.Y + (obstacles[i]._size + (1.5f * 0.01f))))
                        {
                            enemies[j]._position = new Vector2(enemies[j]._position.X, obstacles[i]._position.Y + (0.01f + obstacles[i]._size));
                        }
                        else if ((enemies[j]._position.X - 0.01f) <= obstacleRechteKante && (enemies[j]._position.X + 0.01f) > obstacleRechteKante)
                        {
                            enemies[j]._position = new Vector2(obstacles[i]._position.X + (0.01f + obstacles[i]._size), enemies[j]._position.Y);
                        }
                    }

                }
                j++;
            }
        }
    }
}


