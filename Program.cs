﻿using CG_Projekt.Models;
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
            window.Run();

        }
    }
}

/*
 * Anmerkungen: 
 * Particels bei Schuss.
 * Zugriffsklassen sind noch nicht optimal gesetzt.
 * Heathbar für Spieler und Gegner.
 * Highscore.
 * Bugs Fixen (Enemy KI, Durchsichtige Texturen).
 * Bullet Aglinment.
 * HauptMenü/GameoverScreen OPTINAL.
 * Sprites hinzufügen
 */