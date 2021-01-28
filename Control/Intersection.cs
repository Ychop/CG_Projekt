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
        internal bool MouseIntersection(Vector2 mousePos_, Button button_)
        {
            float buttonLeftSide, buttonRightSide, buttonTopSide, buttonLowerSide;
            buttonLeftSide = button_.MinX;
            buttonRightSide = button_.MaxX;
            buttonTopSide = button_.MaxY;
            buttonLowerSide = button_.MinY;         
            bool xCollision = mousePos_.X >= buttonLeftSide && mousePos_.X <= buttonRightSide;
            bool yCollision = mousePos_.Y >= buttonLowerSide && mousePos_.Y <= buttonTopSide;
            if (xCollision && yCollision)
            {
                return true;
            }
            return false;
        }
        internal bool IsIntersectingCircle(GameObject objA, GameObject objB)
        {
            float radiusSum = (objA.RadiusCollision + objB.RadiusCollision);
            Vector2 diff = objA.Position - objB.Position;
            bool isIntersecting = radiusSum * radiusSum > diff.LengthSquared;
            if (isIntersecting)
            {
                return true;
            }
            return false;
        }
        internal bool IsIntersecting(GameObject objA, GameObject objB)
        {
            float objALeftSide, objARightSide, objATopSide, objALowerSide, objBLeftSide, objBRightSide, objBTopSide, objBLowerSide;
            objALeftSide = objA.Position.X - objA.RadiusCollision;
            objARightSide = objA.Position.X + objA.RadiusCollision;
            objATopSide = objA.Position.Y + objA.RadiusCollision;
            objALowerSide = objA.Position.Y - objA.RadiusCollision;
            objBLeftSide = objB.Position.X - objB.RadiusCollision;
            objBRightSide = objB.Position.X + objB.RadiusCollision;
            objBTopSide = objB.Position.Y + objB.RadiusCollision;
            objBLowerSide = objB.Position.Y - objB.RadiusCollision;
            bool xCollision = objARightSide >= objBLeftSide && objBRightSide >= objALeftSide;
            bool yCollision = objATopSide >= objBLowerSide && objBTopSide >= objALowerSide;
            if (xCollision && yCollision)
            {
                return true;
            }
            return false;
        }
        internal bool ObjectCollidingWithLeverBorder(GameObject obj)
        {
            this.objARechteKante = obj.Position.X + obj.RadiusCollision;
            this.objALinkeKante = obj.Position.X - obj.RadiusCollision;
            this.objAObereKante = obj.Position.Y + obj.RadiusCollision;
            this.objAUntereKante = obj.Position.Y - obj.RadiusCollision;
            // Obere Levelgrenze
            if (this.objAObereKante > 0.6f)
            {
                obj.Position = new Vector2(obj.Position.X, 0.6f - obj.RadiusCollision);
                return true;
            }

            // Untere Levelgrenze
            if (this.objAUntereKante < -0.6f)
            {
                obj.Position = new Vector2(obj.Position.X, -0.6f + obj.RadiusCollision);
                return true;
            }

            // Rechte Levelgrenze
            if (this.objARechteKante > 0.6f)
            {
                obj.Position = new Vector2(0.6f - obj.RadiusCollision, obj.Position.Y);
                return true;
            }

            // Linke Levelgrenze
            if (this.objALinkeKante < -0.6f)
            {
                obj.Position = new Vector2(-0.6f + obj.RadiusCollision, obj.Position.Y);
                return true;
            }
            return false;
        }
    }
}