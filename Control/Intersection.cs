using CG_Projekt.Models;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
namespace CG_Projekt
{
    class Intersection
    {
        float objARechteKante, objALinkeKante, objAObereKante, objAUntereKante, obstacleRechteKante, obstacleLinkeKante, obstacleObereKante, obstacleUntereKante;

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
            bool yCollision = objATopSide >= objBLowerSide && objBTopSide >= objALowerSide;
            if (xCollision && yCollision)
            {
                return true;
            }
            return false;
        }
        public void ObjectCollidingWithLeverBorder(GameObject obj)
        {
            objARechteKante = obj.Position.X + obj.Size;
            objALinkeKante = obj.Position.X - obj.Size;
            objAObereKante = obj.Position.Y + obj.Size;
            objAUntereKante = obj.Position.Y - obj.Size;
            if (objAObereKante > 0.9f) //Obere Levelgrenze
            {
                obj.Position = new Vector2(obj.Position.X, 0.89f);
            }
            if (objAUntereKante < -0.9f) //Untere Levelgrenze
            {
                obj.Position = new Vector2(obj.Position.X, -0.89f);
            }
            if (objARechteKante > 0.9f) //Rechte Levelgrenze
            {
                obj.Position = new Vector2(0.89f, obj.Position.Y);
            }
            if (objALinkeKante < -0.9f) //Linke Levelgrenze
            {
                obj.Position = new Vector2(-0.89f, obj.Position.Y);
            }
        }
        public void ResetGameObjectPosition(GameObject objA, Obstacle obstacle)
        {
            objARechteKante = objA.Position.X + objA.Size;
            objALinkeKante = objA.Position.X - objA.Size;
            objAObereKante = objA.Position.Y + objA.Size;
            objAUntereKante = objA.Position.Y - objA.Size;
            obstacleRechteKante = obstacle.Position.X + obstacle.Size;
            obstacleLinkeKante = obstacle.Position.X - obstacle.Size;
            obstacleObereKante = obstacle.Position.Y + obstacle.Size;
            obstacleUntereKante = obstacle.Position.Y - obstacle.Size;

            if (objARechteKante >= obstacleLinkeKante && objALinkeKante < obstacleLinkeKante && objALinkeKante < (obstacle.Position.X - (obstacle.Size + (1.5f * objA.Size))))
            {
                objA.Position = new Vector2(obstacle.Position.X - (objA.Size + obstacle.Size), objA.Position.Y);
            }
            else if (objAObereKante >= obstacleUntereKante && objAUntereKante < obstacleUntereKante && objAUntereKante < (obstacle.Position.Y - (obstacle.Size + (1.5f * objA.Size))))
            {
                objA.Position = new Vector2(objA.Position.X, obstacle.Position.Y - (objA.Size + obstacle.Size));
            }
            else if (objAUntereKante <= obstacleObereKante && objAObereKante > obstacleObereKante && objAObereKante > (obstacle.Position.Y + (obstacle.Size + (1.5f * objA.Size))))
            {
                objA.Position = new Vector2(objA.Position.X, obstacle.Position.Y + (objA.Size + obstacle.Size));
            }
            else if (objALinkeKante <= obstacleRechteKante && objARechteKante > obstacleRechteKante)
            {
                objA.Position = new Vector2(obstacle.Position.X + (objA.Size + obstacle.Size), objA.Position.Y);
            }
        }

    }
}


