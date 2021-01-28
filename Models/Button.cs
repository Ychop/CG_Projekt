using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG_Projekt.Models
{
    public class Button
    {
        public Vector2 Position { get; set; }
        public float MinX { get; set; }
        public float MaxX { get; set; }
        public float MinY { get; set; }
        public float MaxY { get; set; }
        public Button(Vector2 position_, float minX_, float maxX_, float minY_, float maxY_)
        {
            Position = position_;
            MinX = minX_;
            MaxX = maxX_;
            MinY = minY_;
            MaxY = maxY_;
        }
    }
}
