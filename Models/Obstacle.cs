namespace CG_Projekt.Models
{
    using OpenTK;

    internal class Obstacle : GameObject
    {
        internal Obstacle(Vector2 position_, float size_, float velocity_, float hitpoints_, int id_)
            : base(position_, size_, velocity_, hitpoints_, id_)
        {
            this.Position = position_;
            this.Radius = size_;
            this.Hitpoints = hitpoints_;
            this.Velocity = velocity_;
            this.Id = id_;
        }
    }
}