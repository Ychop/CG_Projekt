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

        internal bool IsIntersecting(GameObject objA, GameObject objB)
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

        internal void ObjectCollidingWithLeverBorder(GameObject obj)
        {
            this.objARechteKante = obj.Position.X + obj.Size;
            this.objALinkeKante = obj.Position.X - obj.Size;
            this.objAObereKante = obj.Position.Y + obj.Size;
            this.objAUntereKante = obj.Position.Y - obj.Size;

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
            this.objARechteKante = objA.Position.X + objA.Size;
            this.objALinkeKante = objA.Position.X - objA.Size;
            this.objAObereKante = objA.Position.Y + objA.Size;
            this.objAUntereKante = objA.Position.Y - objA.Size;
            this.obstacleRechteKante = obstacle.Position.X + obstacle.Size;
            this.obstacleLinkeKante = obstacle.Position.X - obstacle.Size;
            this.obstacleObereKante = obstacle.Position.Y + obstacle.Size;
            this.obstacleUntereKante = obstacle.Position.Y - obstacle.Size;

            if (this.objARechteKante >= this.obstacleLinkeKante && this.objALinkeKante < this.obstacleLinkeKante && this.objALinkeKante < (obstacle.Position.X - (obstacle.Size + (1.5f * objA.Size))))
            {
                objA.Position = new Vector2(obstacle.Position.X - (objA.Size + obstacle.Size), objA.Position.Y);
            }
            else if (this.objAObereKante >= this.obstacleUntereKante && this.objAUntereKante < this.obstacleUntereKante && this.objAUntereKante < (obstacle.Position.Y - (obstacle.Size + (1.5f * objA.Size))))
            {
                objA.Position = new Vector2(objA.Position.X, obstacle.Position.Y - (objA.Size + obstacle.Size));
            }
            else if (this.objAUntereKante <= this.obstacleObereKante && this.objAObereKante > this.obstacleObereKante && this.objAObereKante > (obstacle.Position.Y + (obstacle.Size + (1.5f * objA.Size))))
            {
                objA.Position = new Vector2(objA.Position.X, obstacle.Position.Y + (objA.Size + obstacle.Size));
            }
            else if (this.objALinkeKante <= this.obstacleRechteKante && this.objARechteKante > this.obstacleRechteKante)
            {
                objA.Position = new Vector2(obstacle.Position.X + (objA.Size + obstacle.Size), objA.Position.Y);
            }
        }
    }
}