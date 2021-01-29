namespace CG_Projekt
{
    using CG_Projekt.Framework;
    using CG_Projekt.Views;
    using OpenTK;

    internal class Program
    {

        private static void Main()
        {
            var soundManager = new SoundManager();
            var window = new GameWindow();
            var camera = new Camera();
            var model = new Model();
            MainMenu MainMenu = new MainMenu(model);
            var view = new View(camera, MainMenu);
            var controller = new Controller(view, model, window);            
            window.Title = "Topdown-Shooter";
            window.MouseMove += (_, args) => controller.TranslateMouseCoordinates(args.X, window.Height - 1 - args.Y);
            window.KeyPress += (_, args) => controller.WepaonSelection(args.KeyChar);
            window.UpdateFrame += (_, __) =>
            {
                controller.Update((float)__.Time);
            };
            window.WindowState = WindowState.Maximized;
            window.Resize += (_, __) => view.Resize(window.Width, window.Height);
            window.RenderFrame += (_, __) => view.Draw(model);
            window.RenderFrame += (_, __) => window.SwapBuffers();
            var backgroundmusic = new CachedSound("../../Content/14 - Indo Silver Club.mp3");
            soundManager.PlaySound(backgroundmusic);
            window.Run();
        }
    }
}

/*
 * Anmerkungen:
 * Particels bei Schuss. MARCUS
 * Circle Collider. MARCUS
 * Sprites hinzufügen
 * Level Hintergrund ?
 */