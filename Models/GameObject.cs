namespace CG_Projekt
{
    using OpenTK;

    internal class GameObject
    {
        public GameObject(Vector2 position_, float radiusDraw_, float radiusColl_, float velocity_, float hitpoints_, int id_)
        {
            this.Position = position_;
            this.RadiusDraw = radiusDraw_;
            this.RadiusCollision = radiusColl_;
            this.Velocity = velocity_;
            this.Hitpoints = hitpoints_;
            this.Id = id_;
        }
        internal float RadiusCollision { get; set; }

        internal Vector2 Position { get; set; }

        internal float RadiusDraw { get; set; }

        internal float Velocity { get; set; }

        internal float Hitpoints { get; set; }

        internal int Id { get; set; }
        public virtual void Update(float frameTime)
        {
        }

    }
}
