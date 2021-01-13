using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG_Projekt.Framework
{
    public class Rect : IReadOnlyRectangle
    {
        public float MinX { get; set; }
        public float MinY { get; set; }
        public float SizeX { get; set; }
        public float SizeY { get; set; }

        public float MaxX => MinX + SizeX;
        public float MaxY => MinY + SizeY;

        public Rect(float minX, float minY, float sizeX, float sizeY)
        {
            MinX = minX;
            MinY = minY;
            SizeX = sizeX;
            SizeY = sizeY;
        }
    }
}
