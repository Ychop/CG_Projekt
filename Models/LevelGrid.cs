namespace CG_Projekt
{
    using OpenTK;
    internal class LevelGrid
    {
        internal LevelGrid(Vector2 position,float size_)
        {
            this.Position = position;
            this.Size = size_;
        }
        internal float Size { get; set; }
        internal Vector2 Position { get; set; }
    }
}