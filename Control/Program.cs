using OpenTK;
namespace CG_Projekt
{
    internal class Program
    {
        static void Main()
        {
            var window = new GameWindow();
            var model = new Model();
            var view = new View();
            var controller = new Controller(view, model);

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
 * - Intersections Fehlen noch.
 * - Zugriffsklassen sind noch nicht optimal gesetzt.
 */