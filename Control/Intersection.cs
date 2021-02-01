namespace CG_Projekt
{
    using OpenTK;
    using System;
    using System.Collections.Generic;
    internal class Intersection
    {
        internal bool IntersectsAny(List<GameObject> gameObjects, GameObject obj_)
        {
            foreach (GameObject obj in gameObjects)
            {
                double distanceObjects = Math.Pow(obj_.Position.X - obj.Position.X, 2) + Math.Pow(obj_.Position.Y - obj.Position.Y, 2);
                if (IsIntersectingCircle(obj_, obj) && obj != obj_ && distanceObjects < (obj.RadiusDraw + obj_.RadiusDraw))
                {
                    return true;
                }
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
        internal bool ObjectCollidingWithLeverBorder(GameObject obj)
        {
            float objRadXRight = obj.Position.X + obj.RadiusCollision;
            float objRadXLeft = obj.Position.X - obj.RadiusCollision;
            float objRadYTop = obj.Position.Y + obj.RadiusCollision;
            float objRadYBottom = obj.Position.Y - obj.RadiusCollision;
            float levelBorder = 0.6f;
            // Obere Levelgrenze
            if (objRadYTop > levelBorder)
            {
                obj.Position = new Vector2(obj.Position.X, levelBorder - obj.RadiusCollision);
                return true;
            }
            // Untere Levelgrenze
            if (objRadYBottom < -levelBorder)
            {
                obj.Position = new Vector2(obj.Position.X, -levelBorder + obj.RadiusCollision);
                return true;
            }
            // Rechte Levelgrenze
            if (objRadXRight > levelBorder)
            {
                obj.Position = new Vector2(levelBorder - obj.RadiusCollision, obj.Position.Y);
                return true;
            }
            // Linke Levelgrenze
            if (objRadXLeft < -levelBorder)
            {
                obj.Position = new Vector2(-levelBorder + obj.RadiusCollision, obj.Position.Y);
                return true;
            }
            return false;
        }
    }
}