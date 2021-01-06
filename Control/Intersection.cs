using CG_Projekt.Models;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
namespace CG_Projekt
{
    class Intersection
    {

        public Intersection()
        {

        }
        internal bool gameOver = false;
        Random random = new Random();
        float playerRechteKante, playerLinkeKante, playerObereKante, playerUntereKante, obstacleRechteKante, obstacleLinkeKante, obstacleObereKante, obstacleUntereKante;

        public void CheckPlayerBorderCollision(Player player)
        {
            playerRechteKante = player.Position.X + player.Size;
            playerLinkeKante = player.Position.X - player.Size;
            playerObereKante = player.Position.Y + player.Size;
            playerUntereKante = player.Position.Y - player.Size;
            if (playerObereKante > 0.9f) //Obere Levelgrenze
            {
                player.Position = new OpenTK.Vector2(player.Position.X, 0.89f);
            }
            if (playerUntereKante < -0.9f) //Untere Levelgrenze
            {
                player.Position = new OpenTK.Vector2(player.Position.X, -0.89f);
            }
            if (playerRechteKante > 0.9f) //Rechte Levelgrenze
            {
                player.Position = new OpenTK.Vector2(0.89f, player.Position.Y);
            }
            if (playerLinkeKante < -0.9f) //Linke Levelgrenze
            {
                player.Position = new OpenTK.Vector2(-0.89f, player.Position.Y);
            }
        }

        public void CheckPlayerCollisionWithGameobject(Player player, List<Enemy> enemies, List<Obstacle> obstacles, List<PickUp> pickUps)
        {
            // Man könnte evlt. die Ganzen Gameobjects in eine Liste Packen und dann so die Collisions abfragen, wäre vlt schöner und performanter.
            var i = 0;



            playerRechteKante = player.Position.X + player.Size;
            playerLinkeKante = player.Position.X - player.Size;
            playerObereKante = player.Position.Y + player.Size;
            playerUntereKante = player.Position.Y - player.Size;

            foreach (Enemy enemy in enemies) // Checkt Collision mit Spieler und Enemy
            {
                bool xCollision = playerRechteKante >= enemies[i].Position.X - enemies[i].Size && enemies[i].Position.X + enemies[i].Size >= playerLinkeKante;
                bool yCollision = playerObereKante >= enemies[i].Position.Y - enemies[i].Size && enemies[i].Position.Y + enemies[i].Size >= playerUntereKante;
                if (xCollision && yCollision)
                {
                    Console.WriteLine("Player Collision mit:" + i + "Gegner.");
                    //TODO: Hier könnte man noch eine Funktion hinzufügen was dann passiert wenn der spieler den Gegner berührt. (Gamefrezze oder gameover Bild) 
                    player.Health -= 0.001f;
                    if(player.Health < 0)
                    {
                        gameOver = true;
                    }
                 


                }
                if ((enemies[i].Position.Y + enemies[i].Size) > 0.9f) //Obere Levelgrenze
                {
                    enemies[i].Position = new OpenTK.Vector2(enemies[i].Position.X, 0.89f);
                }
                if ((enemies[i].Position.Y - enemies[i].Size) < -0.9f) //Untere Levelgrenze
                {
                    enemies[i].Position = new OpenTK.Vector2(enemies[i].Position.X, -0.89f);
                }
                if ((enemies[i].Position.X + enemies[i].Size) > 0.9f) //Rechte Levelgrenze
                {
                    enemies[i].Position = new OpenTK.Vector2(0.89f, enemies[i].Position.Y);
                }
                if ((enemies[i].Position.X - enemies[i].Size) < -0.9f) //Linke Levelgrenze
                {
                    enemies[i].Position = new OpenTK.Vector2(-0.89f, enemies[i].Position.Y);
                }
                i++;
            }
            i = 0;
            foreach (Obstacle obstacle in obstacles) // Checkt Collision mit Spieler und Obstacle
            {
                obstacleRechteKante = obstacles[i].Position.X + obstacles[i].Size;
                obstacleLinkeKante = obstacles[i].Position.X - obstacles[i].Size;
                obstacleObereKante = obstacles[i].Position.Y + obstacles[i].Size;
                obstacleUntereKante = obstacles[i].Position.Y - obstacles[i].Size;

                bool xCollision = playerRechteKante >= obstacleLinkeKante && playerLinkeKante <= obstacleRechteKante;
                bool yCollision = playerObereKante >= obstacleUntereKante && playerUntereKante <= obstacleObereKante;


                if (xCollision && yCollision)
                {
                    if (playerRechteKante >= obstacleLinkeKante && playerLinkeKante < obstacleLinkeKante && playerLinkeKante < (obstacles[i].Position.X - (obstacles[i].Size + (1.5f * player.Size))))
                    {
                        player.Position = new Vector2(obstacles[i].Position.X - (player.Size + obstacles[i].Size), player.Position.Y);
                    }
                    else if (playerObereKante >= obstacleUntereKante && playerUntereKante < obstacleUntereKante && playerUntereKante < (obstacles[i].Position.Y - (obstacles[i].Size + (1.5f * player.Size))))
                    {
                        player.Position = new Vector2(player.Position.X, obstacles[i].Position.Y - (player.Size + obstacles[i].Size));
                    }
                    else if (playerUntereKante <= obstacleObereKante && playerObereKante > obstacleObereKante && playerObereKante > (obstacles[i].Position.Y + (obstacles[i].Size + (1.5f * player.Size))))
                    {
                        player.Position = new Vector2(player.Position.X, obstacles[i].Position.Y + (player.Size + obstacles[i].Size));
                    }
                    else if (playerLinkeKante <= obstacleRechteKante && playerRechteKante > obstacleRechteKante)
                    {
                        player.Position = new Vector2(obstacles[i].Position.X + (player.Size + obstacles[i].Size), player.Position.Y);
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

                    // Hier wird die Art des Pickups bestimmt
                    if (pickUps[i].Type == 1)
                    {
                        player.Ammo += 100;
                    }
                    if (pickUps[i].Type == 0)
                    {
                        player.Health += 0.1f;
                    }
                }
                i++;
            }
        }
        public bool CheckPickUpCollision(List<Enemy> enemies, List<Obstacle> obstacles, List<PickUp> pickups, float ranX, float ranY) // Platziert die Pickups so, dass sie nicht in einem anderen Objekt Spawnen (Player Ausgenommen)
        {
            int i = 0;
            foreach (Enemy enemy in enemies)
            {
                bool xCollision = ranX + 0.01f >= enemies[i].Position.X - enemies[i].Size && ranX - 0.01f <= enemies[i].Position.X + enemies[i].Size;
                bool yCollision = ranY + 0.01f >= enemies[i].Position.Y - enemies[i].Size && ranY - 0.01f <= enemies[i].Position.Y + enemies[i].Size;
                if (xCollision && yCollision)
                {
                    return true;
                }
                i++;
            }
            i = 0;
            foreach (Obstacle obstacle in obstacles)
            {
                bool xCollision = ranX + 0.01f >= obstacles[i].Position.X - obstacles[i].Size && ranX - 0.01f <= obstacles[i].Position.X + obstacles[i].Size;
                bool yCollision = ranY + 0.01f >= obstacles[i].Position.Y - obstacles[i].Size && ranY - 0.01f <= obstacles[i].Position.Y + obstacles[i].Size;
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
                bool xCollision = ranX + newObstacle.Size >= obstacles[i].Position.X - obstacles[i].Size && ranX - newObstacle.Size <= obstacles[i].Position.X + obstacles[i].Size;
                bool yCollision = ranY + newObstacle.Size >= obstacles[i].Position.Y - obstacles[i].Size && ranY - newObstacle.Size <= obstacles[i].Position.Y + obstacles[i].Size;
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

                bool xCollision = ranX + enemy.Size >= obstacles[i].Position.X - obstacles[i].Size && ranX - enemy.Size <= obstacles[i].Position.X + obstacles[i].Size;
                bool yCollision = ranY + enemy.Size >= obstacles[i].Position.Y - obstacles[i].Size && ranY - enemy.Size <= obstacles[i].Position.Y + obstacles[i].Size;
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
                    obstacleRechteKante = obstacles[i].Position.X + obstacles[i].Size;
                    obstacleLinkeKante = obstacles[i].Position.X - obstacles[i].Size;
                    obstacleObereKante = obstacles[i].Position.Y + obstacles[i].Size;
                    obstacleUntereKante = obstacles[i].Position.Y - obstacles[i].Size;
                    // Hier wird noch als Size die vom player angegeben, da beide NOCH gleich groß sind
                    bool enemyXCollision = (enemies[j].Position.X + 0.01f) >= obstacleLinkeKante && (enemies[j].Position.X - 0.01f) <= obstacleRechteKante;
                    bool enemyYCollision = (enemies[j].Position.Y + 0.01f) >= obstacleUntereKante && (enemies[j].Position.Y - 0.01f) <= obstacleObereKante;

                    if (enemyXCollision && enemyYCollision)
                    {
                        if ((enemies[j].Position.X + 0.01f) >= obstacleLinkeKante && (enemies[j].Position.X - 0.01f) < obstacleLinkeKante && (enemies[j].Position.X - enemies[j].Size) < (obstacles[i].Position.X - (obstacles[i].Size + (1.5f * 0.01f))))
                        {
                            enemies[j].Position = new Vector2(obstacles[i].Position.X - (0.01f + obstacles[i].Size), enemies[j].Position.Y);
                        }
                        else if ((enemies[j].Position.Y + 0.01f) >= obstacleUntereKante && (enemies[j].Position.Y - 0.01f) < obstacleUntereKante && (enemies[j].Position.Y - enemies[j].Size) < (obstacles[i].Position.Y - (obstacles[i].Size + (1.5f * 0.01f))))
                        {
                            enemies[j].Position = new Vector2(enemies[j].Position.X, obstacles[i].Position.Y - (0.01f + obstacles[i].Size));
                        }
                        else if ((enemies[j].Position.Y - 0.01f) <= obstacleObereKante && (enemies[j].Position.Y + 0.01f) > obstacleObereKante && (enemies[j].Position.Y + enemies[j].Size) > (obstacles[i].Position.Y + (obstacles[i].Size + (1.5f * 0.01f))))
                        {
                            enemies[j].Position = new Vector2(enemies[j].Position.X, obstacles[i].Position.Y + (0.01f + obstacles[i].Size));
                        }
                        else if ((enemies[j].Position.X - 0.01f) <= obstacleRechteKante && (enemies[j].Position.X + 0.01f) > obstacleRechteKante)
                        {
                            enemies[j].Position = new Vector2(obstacles[i].Position.X + (0.01f + obstacles[i].Size), enemies[j].Position.Y);
                        }
                    }

                }
                j++;
            }

        }
        public bool BulletCollision(List<Bullet> bullets, List<Enemy> enemies, List<Obstacle> obstacles)
        {
            //Bullet mit Obstacle
            var j = 0;
            foreach (Bullet bullet in bullets)
            {
                for (int i = 0; i < 50; i++)
                {
                    obstacleRechteKante = obstacles[i].Position.X + obstacles[i].Size;
                    obstacleLinkeKante = obstacles[i].Position.X - obstacles[i].Size;
                    obstacleObereKante = obstacles[i].Position.Y + obstacles[i].Size;
                    obstacleUntereKante = obstacles[i].Position.Y - obstacles[i].Size;
                    bool bulletXCollision = (bullets[j].Position.X + 0.01f) >= obstacleLinkeKante && (bullets[j].Position.X - 0.01f) <= obstacleRechteKante;
                    bool bulletYCollision = (bullets[j].Position.Y + 0.01f) >= obstacleUntereKante && (bullets[j].Position.Y - 0.01f) <= obstacleObereKante;
                    if (bulletXCollision && bulletYCollision)
                    {
                        return true;
                    }
                }
                j++;
            }
            //Collsion mit Level Wand
            j = 0;
            foreach (Bullet bullet in bullets)
            {
                if (bullets[j].Position.X > 0.9f || bullets[j].Position.X < -0.9f || bullets[j].Position.Y > 0.9f || (bullets[j].Position.Y < -0.9f)) //Obere Levelgrenze
                {
                    return true;
                }
                j++;
            }

            //Collision mit Enemies
            j = 0;
            foreach (Bullet bullet in bullets)
            {
                for (int i = 0; i < enemies.Count; i++)
                {
                    float enemyRechteKante = enemies[i].Position.X + enemies[i].Size;
                    float enemyLinkeKante = enemies[i].Position.X - enemies[i].Size;
                    float enemyObereKante = enemies[i].Position.Y + enemies[i].Size;
                    float enemyUntereKante = enemies[i].Position.Y - enemies[i].Size;
                    bool bulletXCollision = (bullets[j].Position.X + 0.01f) >= enemyLinkeKante && (bullets[j].Position.X - 0.01f) <= enemyRechteKante;
                    bool bulletYCollision = (bullets[j].Position.Y + 0.01f) >= enemyUntereKante && (bullets[j].Position.Y - 0.01f) <= enemyObereKante;
                    if (bulletXCollision && bulletYCollision)
                    {
                        enemies[i].Health -= 0.01f;  
                        return true;
                    }
                }
                j++;
            }


            return false;
        }

    }
}


