using OpenTK;
using System.Drawing;


namespace CG_Projekt.Models
{
    class Weapon
    {
        public enum Wepaons { Pistol = 1, Uzi, Shotgun, RPG }

        internal int Type;
        internal float RPM;
        internal float Damage;
        internal float Size;
        internal float Velocity;
        public Weapon(int type_)
        {
            this.Type = type_;
            switch (Type)
            {
                case 1:
                    this.RPM = 0.4f;
                    this.Damage = 0.1f;
                    this.Size = 0.001f;
                    this.Velocity = 0.5f;
                    break;
                case 2:
                    this.RPM = 0.1f;
                    this.Damage = 0.025f;
                    this.Size = 0.0008f;
                    this.Velocity = 0.6f;
                    break;
                case 3:
                    this.RPM = 0.5f;
                    this.Damage = 0.18f;
                    this.Size = 0.0012f;
                    this.Velocity = 0.3f;
                    break;
                case 4:
                    this.RPM = 0.8f;
                    this.Damage = 0.5f;
                    this.Size = 0.003f;
                    this.Velocity = 0.1f;
                    break;
                default:
                    break;
            }

        }



    }
}
