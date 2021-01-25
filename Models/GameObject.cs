namespace CG_Projekt
{
    using OpenTK;

    internal class GameObject
    {
        public GameObject(Vector2 position_, float radius_, float velocity_, float hitpoints_, int id_)
        {
            this.Position = position_;
            this.Radius = radius_;
            this.Velocity = velocity_;
            this.Hitpoints = hitpoints_;
            this.Id = id_;
        }

        internal Vector2 Position { get; set; }

        internal float Radius { get; set; }

        internal float Velocity { get; set; }

        internal float Hitpoints { get; set; }

        internal int Id { get; set; }
    }
}
