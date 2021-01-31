namespace CG_Projekt
{
    using OpenTK;
    internal class LevelGrid
    {
        internal LevelGrid(Vector2 position)
        {
            this.Position = position;
        }
        internal Vector2 Position { get; set; }
    }
}