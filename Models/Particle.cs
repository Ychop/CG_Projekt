using OpenTK;
namespace CG_Projekt.Models
{
    class Particle : GameObject
    {
        public Vector2 RanDir { get; set; }
        public GameObject OriginObj { get; set; }
        internal Particle(Vector2 position_, float radiusDraw_, float radiusColl_, float velocity_, float hitpoints_, int id_, Vector2 ranDir_, GameObject originObj_) : base(position_, radiusDraw_, radiusColl_, velocity_, hitpoints_, id_)
        {
            Position = position_;
            RadiusDraw = radiusDraw_;
            RadiusCollision = radiusColl_;
            Velocity = velocity_;
            Hitpoints = hitpoints_;
            Id = id_;
            RanDir = ranDir_;
            OriginObj = originObj_;
            RanDir.Normalize();
        }
    }
}
