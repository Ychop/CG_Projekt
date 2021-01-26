

using CG_Projekt.Framework;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace CG_Projekt.Views
{
    public class MainMenu
    {
        private Model model;
        private Vector2 ButtonSize;
        private Vector2 PositionStartButton = new Vector2(0, 0.3f);
        private Vector2 PositionOptionsButton = new Vector2(0, -0.2f);
        private Vector2 PositionExitButton = new Vector2(0, -0.7f);
        private readonly int texStartButton;
        private readonly int texExitButton;
        private readonly int texOptionsButton;
        public Models.Button StartButton;
        public Models.Button OptionsButton;
        public Models.Button ExitButton;

        internal MainMenu(Model model_)
        {
            model = model_;
            var content = $"{nameof(CG_Projekt)}.Content.";
            texStartButton = Texture.Load(Resource.LoadStream(content + "StartButton.png"));
            texExitButton = Texture.Load(Resource.LoadStream(content + "ExitButton.png"));
            texOptionsButton = Texture.Load(Resource.LoadStream(content + "OptionsButton.png"));
            ButtonSize = new Vector2(0.4f, 0.2f);
            StartButton = new Models.Button(PositionStartButton, -ButtonSize.X, ButtonSize.X, -ButtonSize.Y, ButtonSize.Y);
            OptionsButton = new Models.Button(PositionStartButton, -ButtonSize.X, ButtonSize.X, -ButtonSize.Y, ButtonSize.Y);
            ExitButton = new Models.Button(PositionStartButton, -ButtonSize.X, ButtonSize.X, -ButtonSize.Y, ButtonSize.Y);
        }

        internal void DrawMainMenu()
        {
            DrawStartButton();
            DrawOptionsButton();
            DrawExitButton();
        }

        internal void DrawStartButton()
        {
            GL.BindTexture(TextureTarget.Texture2D, this.texStartButton);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(new Vector2(0, 0));
            GL.Vertex2(PositionStartButton + new Vector2(StartButton.MinX, StartButton.MinY));
            GL.TexCoord2(new Vector2(1, 0));
            GL.Vertex2(PositionStartButton + new Vector2(StartButton.MaxX, StartButton.MinY));
            GL.TexCoord2(new Vector2(1, 1));
            GL.Vertex2(PositionStartButton + new Vector2(StartButton.MaxX, StartButton.MaxY));
            GL.TexCoord2(new Vector2(0, 1));
            GL.Vertex2(PositionStartButton + new Vector2(StartButton.MinX, StartButton.MaxY));
            GL.End();
        }
        internal void DrawOptionsButton()
        {
            GL.BindTexture(TextureTarget.Texture2D, this.texOptionsButton);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(new Vector2(0, 0));
            GL.Vertex2(PositionOptionsButton + new Vector2(OptionsButton.MinX, OptionsButton.MinY));
            GL.TexCoord2(new Vector2(1, 0));
            GL.Vertex2(PositionOptionsButton + new Vector2(OptionsButton.MaxX, OptionsButton.MinY));
            GL.TexCoord2(new Vector2(1, 1));
            GL.Vertex2(PositionOptionsButton + new Vector2(OptionsButton.MaxX, OptionsButton.MaxY));
            GL.TexCoord2(new Vector2(0, 1));
            GL.Vertex2(PositionOptionsButton + new Vector2(OptionsButton.MinX, OptionsButton.MaxY));
            GL.End();
        }
        internal void DrawExitButton()
        {
            GL.BindTexture(TextureTarget.Texture2D, this.texExitButton);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(new Vector2(0, 0));
            GL.Vertex2(PositionExitButton + new Vector2(ExitButton.MinX, ExitButton.MinY));
            GL.TexCoord2(new Vector2(1, 0));
            GL.Vertex2(PositionExitButton + new Vector2(ExitButton.MaxX, ExitButton.MinY));
            GL.TexCoord2(new Vector2(1, 1));
            GL.Vertex2(PositionExitButton + new Vector2(ExitButton.MaxX, ExitButton.MaxY));
            GL.TexCoord2(new Vector2(0, 1));
            GL.Vertex2(PositionExitButton + new Vector2(ExitButton.MinX, ExitButton.MaxY));
            GL.End();
        }
    }
}
