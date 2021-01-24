using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG_Projekt.Framework
{
    internal interface IReadOnlyRectangle
    {
        float MaxX { get; }
        float MaxY { get; }
        float MinX { get; }
        float MinY { get; }
        float SizeX { get; }
        float SizeY { get; }
    }
}