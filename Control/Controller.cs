using CG_Projekt.Models;
using OpenTK;
using OpenTK.Input;
using System;

namespace CG_Projekt
{
    class Controller
    {
        View view;
        Model model;
        public Intersection intersection { get; } = new Intersection();
        public Camera camera { get; } = new Camera();
        internal int oldScrollValue = 0;
        internal float axisZoom = 0f;

        internal Controller(View view_, Model model_)
        {
            view = view_;
            model = model_;
        }

        internal void Update(float deltaTime)
        {
            //Zoom mit dem Mausrad
            ScrollControl(deltaTime);
            //Bewegt den Spieler
            UpdatePlayerPosition(deltaTime);
            //Checkt die Collisons
            UpdateCheckCollision();
            //Rotiert die Camera
            UpdateRotateCamera(deltaTime);
            //Position der Mouse
            UpdateMousePosition();
        }

        internal void UpdateRotateCamera(float deltaTime)
        {
            var keyboard = Keyboard.GetState();
            float rotateQE = keyboard.IsKeyDown(Key.Q) ? 1f : keyboard.IsKeyDown(Key.E) ? -1f : 0f;
            view.camera.rotate += deltaTime * rotateQE; 
            if (view.camera.rotate <= -6.3 || view.camera.rotate >= 6.3)
            {
                view.camera.rotate = 0;
            }

        }

        internal void UpdatePlayerPosition(float deltaTime)
        {
          
                var keyboard = Keyboard.GetState(); // Holt den Zustand des Keyboards
                float moveLR = (keyboard.IsKeyDown(Key.Left) || keyboard.IsKeyDown(Key.A)) ? -0.15f : keyboard.IsKeyDown(Key.Right) || keyboard.IsKeyDown(Key.D) ? 0.15f : 0.0f; // 0.2f und - 0.2f Gibt an wie schnell sich der spieler in die entsprechende Richtung bewegen kann.
                float moveUD = keyboard.IsKeyDown(Key.Down) || keyboard.IsKeyDown(Key.S) ? -0.2f : keyboard.IsKeyDown(Key.Up) || keyboard.IsKeyDown(Key.W) ? 0.2f : 0.0f;
                model.player._position += deltaTime * new Vector2(moveLR, moveUD);


            
      
        }
        internal void ScrollControl(float deltaTime)
        {
            var mouse = Mouse.GetState();
            var scrollValue = mouse.ScrollWheelValue;

            if (scrollValue < oldScrollValue)
            {
                axisZoom = 2f;
                oldScrollValue = scrollValue;
            }
            else if (scrollValue > oldScrollValue)
            {
                axisZoom = -2f;
                oldScrollValue = scrollValue;
            }
            else if (scrollValue == oldScrollValue)
            {
                axisZoom = 0f;
            }
            var zoom = view.camera.Scale * (1 + deltaTime * axisZoom);
            // zoom = MathHelper.Clamp(zoom, 0.9f, 1f); //setzt Zoom Grenze, also bis wie weit man rein/raus zoomen kann
            view.camera.Scale = zoom;
        }

        internal void UpdateMousePosition()
        {
            var mouse = Mouse.GetState();
          //  Console.WriteLine("MouseX: " + mouse.X + "MouseY: " + mouse.Y);
            if(mouse.X < 0)
            {
               
            }
        }


        internal void UpdateCheckCollision()
        {
          
            intersection.CheckPlayerBorderCollision(model.player);
            intersection.CheckPlayerCollisionWithGameobject(model.player, model.enemies, model.obstacles, model.pickUps);
        }

    }
}