using CG_Projekt.Models;
using OpenTK;
namespace CG_Projekt
{
    internal class Program
    {


        static void Main()
        {
      
            var window = new GameWindow();
            var camera = new Camera();
            var model = new Model();
            var view = new View(camera);
            var controller = new Controller(view, model, window);

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
            window.Run();

        }
    }
}

/*
 * Anmerkungen: 
 * Die Gameobjects Überlappen sich noch.
 * Zugriffsklassen sind noch nicht optimal gesetzt.
 * Heathbar für Spieler und Gegner.
 * Highscore.
 * Bugs Fixen (Spawn der Gameobjects , Enemy KI, Shogun, Durchsichtige Texturen).
 * Enemy/Bullet Aglinment.
 * HauptMenü/GameoverScreen.
 * Sprites hinzufügen
 */