
using OpenTK;
using System;
using System.Drawing;

namespace CG_Projekt.Models
{
    public class Bullet : GameObject
    {
        public Vector2 Direction { get; set; }

        public Bullet(Color color_, Vector2 position_, float posX, float posY, float width, float velocity_, float hitpoints_, int id_) : base(color_,position_, posX, posY, width, width, velocity_, hitpoints_, id_)
        {
            this.Position = position_;
            this.Color = color_;
            this.Hitpoints = hitpoints_;
            this.Velocity = velocity_;
            this.Id = id_;
        }

        public void CreateBullet()
        {

        }
        public void MoveBullet(Bullet bullet, Vector2 direction_)
        {
            //Bullet bewegen sich noch seltsam          
            bullet.Direction = direction_;
            bullet.Direction.Normalized();
            bullet.Position += bullet.Direction * Velocity;
        }
    }
}
