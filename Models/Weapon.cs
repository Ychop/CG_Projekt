namespace CG_Projekt.Models
{
    internal class Weapon
    {
        internal Weapon(int type_)
        {
            this.Type = type_;
            switch (this.Type)
            {
                case 1:
                    this.RPM = 0.4f;
                    this.Damage = 0.1f;
                    this.Size = 0.002f;
                    this.Velocity = 0.5f;
                    break;
                case 2:
                    this.RPM = 0.1f;
                    this.Damage = 0.025f;
                    this.Size = 0.0016f;
                    this.Velocity = 0.6f;
                    break;
                case 3:
                    this.RPM = 0.6f;
                    this.Damage = 0.12f;
                    this.Size = 0.0015f;
                    this.Velocity = 0.3f;
                    break;
                case 4:
                    this.RPM = 0.8f;
                    this.Damage = 0.5f;
                    this.Size = 0.006f;
                    this.Velocity = 0.1f;
                    break;
                default:
                    break;
            }
        }

        internal enum Wepaons
        {
            Pistol = 1,
            Uzi,
            Shotgun,
            RPG,
        }

        internal int Type { get; }

        internal float RPM { get; }

        internal float Damage { get; }

        internal float Size { get; }

        internal float Velocity { get; }
    }
}
