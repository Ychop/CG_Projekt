namespace CG_Projekt.Models
{
    using System;
    using System.Collections.Generic;
    using OpenTK;

    internal class Bullet : GameObject
    {
        internal Bullet(Vector2 position_, float radiusDraw_, float radiusColl_, float velocity_, float hitpoints_, int id_, Vector2 direction_)
            : base(position_, radiusDraw_, radiusColl_, velocity_, hitpoints_, id_)
        {
            this.Position = position_;
            this.RadiusDraw = radiusDraw_;
            this.RadiusCollision = radiusColl_;
            this.Hitpoints = hitpoints_;
            this.Velocity = velocity_;
            this.Id = id_;
            this.Direction = direction_;
            this.Direction.Normalize();
            double angleRad = Math.Atan2(-this.Direction.Y, this.Direction.X);
            this.Angle = angleRad * (180 / Math.PI);
        }

        internal Vector2 Direction { get; set; }

        internal double Angle { get; }


        public void MoveBullet(Bullet bullet)
        {
            bullet.Position += bullet.Direction * bullet.Velocity;
        }
    }
}
