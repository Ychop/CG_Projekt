namespace CG_Projekt
{
    using CG_Projekt.Models;
    using OpenTK;

    internal class Intersection
    {
        private float objARechteKante;
        private float objALinkeKante;
        private float objAObereKante;
        private float objAUntereKante;
        private float obstacleRechteKante;
        private float obstacleLinkeKante;
        private float obstacleObereKante;
        private float obstacleUntereKante;


        internal bool IsIntersectingCircle(GameObject objA, GameObject objB)
        {
            float radiusSum = (objA.Radius + objB.Radius) * 0.95f;
            Vector2 diff = objA.Position - objB.Position;
            bool isIntersecting = radiusSum * radiusSum > diff.LengthSquared;
            if (isIntersecting)
            {
                objA.Position += diff * (radiusSum*radiusSum);
                return true;

            }
            return false;
        }

        internal bool IsIntersecting(GameObject objA, GameObject objB)
        {
            float objALeftSide, objARightSide, objATopSide, objALowerSide, objBLeftSide, objBRightSide, objBTopSide, objBLowerSide;
            objALeftSide = objA.Position.X - objA.Radius;
            objARightSide = objA.Position.X + objA.Radius;
            objATopSide = objA.Position.Y + objA.Radius;
            objALowerSide = objA.Position.Y - objA.Radius;
            objBLeftSide = objB.Position.X - objB.Radius;
            objBRightSide = objB.Position.X + objB.Radius;
            objBTopSide = objB.Position.Y + objB.Radius;
            objBLowerSide = objB.Position.Y - objB.Radius;
       

            bool xCollision = objARightSide >= objBLeftSide && objBRightSide >= objALeftSide;
            bool yCollision = objATopSide >= objBLowerSide && objBTopSide >= objALowerSide;
            if (xCollision && yCollision)
            {
                return true;
            }

            return false;
        }

        internal void ObjectCollidingWithLeverBorder(GameObject obj)
        {
            this.objARechteKante = obj.Position.X + obj.Radius;
            this.objALinkeKante = obj.Position.X - obj.Radius;
            this.objAObereKante = obj.Position.Y + obj.Radius;
            this.objAUntereKante = obj.Position.Y - obj.Radius;

            // Obere Levelgrenze
            if (this.objAObereKante > 0.9f)
            {
                obj.Position = new Vector2(obj.Position.X, 0.89f);
            }

            // Untere Levelgrenze
            if (this.objAUntereKante < -0.9f)
            {
                obj.Position = new Vector2(obj.Position.X, -0.89f);
            }

            // Rechte Levelgrenze
            if (this.objARechteKante > 0.9f)
            {
                obj.Position = new Vector2(0.89f, obj.Position.Y);
            }

            // Linke Levelgrenze
            if (this.objALinkeKante < -0.9f)
            {
                obj.Position = new Vector2(-0.89f, obj.Position.Y);
            }
        }

        internal void ResetGameObjectPosition(GameObject objA, Obstacle obstacle)
        {
            this.objARechteKante = objA.Position.X + objA.Radius;
            this.objALinkeKante = objA.Position.X - objA.Radius;
            this.objAObereKante = objA.Position.Y + objA.Radius;
            this.objAUntereKante = objA.Position.Y - objA.Radius;
            this.obstacleRechteKante = obstacle.Position.X + obstacle.Radius;
            this.obstacleLinkeKante = obstacle.Position.X - obstacle.Radius;
            this.obstacleObereKante = obstacle.Position.Y + obstacle.Radius;
            this.obstacleUntereKante = obstacle.Position.Y - obstacle.Radius;

            if (this.objARechteKante >= this.obstacleLinkeKante && this.objALinkeKante < this.obstacleLinkeKante && this.objALinkeKante < (obstacle.Position.X - (obstacle.Radius + (1.5f * objA.Radius))))
            {
                objA.Position = new Vector2(obstacle.Position.X - (objA.Radius + obstacle.Radius), objA.Position.Y);
            }
            else if (this.objAObereKante >= this.obstacleUntereKante && this.objAUntereKante < this.obstacleUntereKante && this.objAUntereKante < (obstacle.Position.Y - (obstacle.Radius + (1.5f * objA.Radius))))
            {
                objA.Position = new Vector2(objA.Position.X, obstacle.Position.Y - (objA.Radius + obstacle.Radius));
            }
            else if (this.objAUntereKante <= this.obstacleObereKante && this.objAObereKante > this.obstacleObereKante && this.objAObereKante > (obstacle.Position.Y + (obstacle.Radius + (1.5f * objA.Radius))))
            {
                objA.Position = new Vector2(objA.Position.X, obstacle.Position.Y + (objA.Radius + obstacle.Radius));
            }
            else if (this.objALinkeKante <= this.obstacleRechteKante && this.objARechteKante > this.obstacleRechteKante)
            {
                objA.Position = new Vector2(obstacle.Position.X + (objA.Radius + obstacle.Radius), objA.Position.Y);
            }
        }
    }
}