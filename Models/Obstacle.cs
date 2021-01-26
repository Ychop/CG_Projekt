namespace CG_Projekt.Models
{
    using OpenTK;

    internal class Obstacle : GameObject
    {
        internal Obstacle(Vector2 position_, float radiusDraw_,float radiusColl_, float velocity_, float hitpoints_, int id_)
            : base(position_, radiusDraw_, radiusColl_, velocity_, hitpoints_, id_)
        {
            this.Position = position_;
            this.RadiusDraw = radiusDraw_;
            this.RadiusCollision = radiusColl_;
            this.Hitpoints = hitpoints_;
            this.Velocity = velocity_;
            this.Id = id_;

        }
    }
}