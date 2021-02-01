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