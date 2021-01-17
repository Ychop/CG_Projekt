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
 * - Man könnte ein Interface für die GameObjects(Player,Enemy,Obstacle,Pickup) machen, da sie alle eine Position haben.
 * - Die Gameobjects Überlappen sich noch.
 * - ist die Klasse Intersections im richtigen Ordner? oder ist sie teil der Models?
 * - die LevelGrids sind noch etwas zuklein. Das dient nur dazu das man sie sieht.
 * - Intersections nur Teilweiße implementiert(Spieler kann noch durch Obstacles laufen; GameObject Spawnen evlt. noch auf Spieler;Bei Intersection mit Gegner Passiert noch nichts;Intersection zwischen GameObjects während des Loop noch nicht implementiert).
 * - Zugriffsklassen sind noch nicht optimal gesetzt.
 * - Lösung für die Intersections ist ziemlich ugly. Eine Mögliche lösung dafür wären Eneties.
 * - Velocity für spieler Fehlt noch
 * 
 * Schießen Implementieren.
 * Heathbar für Spieler und Gegner.
 * Highscore.
 * Bugs Fixen (Spawn der Gameobjects , Enemy KI).
 * Pickups Machen also Leben bekkommen / Munition.
 * 
 */