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
        float objRechteKante, objLinkeKante, objObereKante, objUntereKante, obstacleRechteKante, obstacleLinkeKante, obstacleObereKante, obstacleUntereKante;
       public bool gameOver = false;
        Random random = new Random();

        public bool IsIntersecting(GameObject objA, GameObject objB)
        {
            float objALeftSide, objARightSide, objATopSide, objALowerSide, objBLeftSide, objBRightSide, objBTopSide, objBLowerSide;

            objALeftSide = objA.Position.X - objA.Size;
            objARightSide = objA.Position.X + objA.Size;
            objATopSide = objA.Position.Y + objA.Size;
            objALowerSide = objA.Position.Y - objA.Size;
            objBLeftSide = objB.Position.X - objB.Size;
            objBRightSide = objB.Position.X + objB.Size;
            objBTopSide = objB.Position.Y + objB.Size;
            objBLowerSide = objB.Position.Y - objB.Size;           

            bool xCollision = objARightSide >= objBLeftSide && objBRightSide >= objALeftSide;
            bool yCollision = objATopSide >=objBLowerSide && objBTopSide >= objALowerSide;
            if (xCollision && yCollision)
            {
                return true;
            }
            return false;
        }

        public void ObjCollidingWithLeverBorder(GameObject obj)
        {
            objRechteKante = obj.Position.X + obj.Size;
            objLinkeKante = obj.Position.X - obj.Size;
            objObereKante = obj.Position.Y + obj.Size;
            objUntereKante = obj.Position.Y - obj.Size;
            if (objObereKante > 0.9f) //Obere Levelgrenze
            {
                obj.Position = new Vector2(obj.Position.X, 0.89f);
            }
            if (objUntereKante < -0.9f) //Untere Levelgrenze
            {
                obj.Position = new Vector2(obj.Position.X, -0.89f);
            }
            if (objRechteKante > 0.9f) //Rechte Levelgrenze
            {
                obj.Position = new Vector2(0.89f, obj.Position.Y);
            }
            if (objLinkeKante < -0.9f) //Linke Levelgrenze
            {
                obj.Position = new Vector2(-0.89f, obj.Position.Y);
            }
        }

        public void CheckPlayerCollisionWithObstacle(Player player,List<Obstacle> obstacles)
        {
            objRechteKante = player.Position.X + player.Size;
            objLinkeKante = player.Position.X - player.Size;
            objObereKante = player.Position.Y + player.Size;
            objUntereKante = player.Position.Y - player.Size;
            int i = 0;
            foreach (Obstacle obstacle in obstacles) // Checkt Collision mit Spieler und Obstacle
            {
                obstacleRechteKante = obstacles[i].Position.X + obstacles[i].Size;
                obstacleLinkeKante = obstacles[i].Position.X - obstacles[i].Size;
                obstacleObereKante = obstacles[i].Position.Y + obstacles[i].Size;
                obstacleUntereKante = obstacles[i].Position.Y - obstacles[i].Size;

                bool xCollision = objRechteKante >= obstacleLinkeKante && objLinkeKante <= obstacleRechteKante;
                bool yCollision = objObereKante >= obstacleUntereKante && objUntereKante <= obstacleObereKante;


                if (xCollision && yCollision)
                {
                    if (objRechteKante >= obstacleLinkeKante && objLinkeKante < obstacleLinkeKante && objLinkeKante < (obstacles[i].Position.X - (obstacles[i].Size + (1.5f * player.Size))))
                    {
                        player.Position = new Vector2(obstacles[i].Position.X - (player.Size + obstacles[i].Size), player.Position.Y);
                    }
                    else if (objObereKante >= obstacleUntereKante && objUntereKante < obstacleUntereKante && objUntereKante < (obstacles[i].Position.Y - (obstacles[i].Size + (1.5f * player.Size))))
                    {
                        player.Position = new Vector2(player.Position.X, obstacles[i].Position.Y - (player.Size + obstacles[i].Size));
                    }
                    else if (objUntereKante <= obstacleObereKante && objObereKante > obstacleObereKante && objObereKante > (obstacles[i].Position.Y + (obstacles[i].Size + (1.5f * player.Size))))
                    {
                        player.Position = new Vector2(player.Position.X, obstacles[i].Position.Y + (player.Size + obstacles[i].Size));
                    }
                    else if (objLinkeKante <= obstacleRechteKante && objRechteKante > obstacleRechteKante)
                    {
                        player.Position = new Vector2(obstacles[i].Position.X + (player.Size + obstacles[i].Size), player.Position.Y);
                    }

                }
                i++;
            }
            i = 0;
         
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
                        enemies[i].Hitpoints -= 0.01f;
                        return true;
                    }
                }
                j++;
            }


            return false;
        }

        internal void EnemyWithEnemyCollision(List<Enemy> enemies)
        {
          
            for (int j = 0; j < enemies.Count; j++)
            {
                Enemy enemyToCheck = enemies[j];


                for (int i = 0; i < enemies.Count; i++)
                {
                    if(enemies[i] == enemyToCheck)
                    {
                        continue;
                    }
                    float enemyRechteKante = enemies[i].Position.X + enemies[i].Size;
                    float enemyLinkeKante = enemies[i].Position.X - enemies[i].Size;
                    float enemyObereKante = enemies[i].Position.Y + enemies[i].Size;
                    float enemyUntereKante = enemies[i].Position.Y - enemies[i].Size;


                    bool enemyXCollision = (enemyToCheck.Position.X >= enemyLinkeKante && enemyToCheck.Position.X  <= enemyRechteKante);
                    bool enemyYCollision = (enemyToCheck.Position.Y  >= enemyUntereKante && enemyToCheck.Position.Y <= enemyObereKante);
                   
                    if (enemyXCollision && enemyYCollision)
                    {
                        enemyToCheck.Velocity = 0;
                        Console.WriteLine("Collision with Enemy!");
                    }
                   
                }

            }

        }
    }
}


