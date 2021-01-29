using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace CG_Projekt.Models
{
    class Explosion : GameObject
    {
        internal Explosion(Vector2 position_, float radiusDraw_, float radiusColl_, float velocity_, float hitpoints_, int id_, Vector2 direction_)
            : base(position_, radiusDraw_, radiusColl_, velocity_, hitpoints_, id_)
        {
            this.Position = position_;
            this.RadiusDraw = radiusDraw_;
            this.RadiusCollision = radiusColl_;
            this.Hitpoints = hitpoints_;
            this.Velocity = velocity_;
            this.Id = id_;
        }

        internal float NormalizedAnimationTime { get; set; } = 0f;
        public float AnimationLength { get; }
    }
}
