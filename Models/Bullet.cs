namespace CG_Projekt.Models
{
    using System;
    using OpenTK;

    internal class Bullet : GameObject
    {
        internal Bullet(Vector2 position_, float size_, float velocity_, float hitpoints_, int id_, Vector2 direction_)
            : base(position_, size_, velocity_, hitpoints_, id_)
        {
            this.Position = position_;
            this.Size = size_;
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
    }
}
