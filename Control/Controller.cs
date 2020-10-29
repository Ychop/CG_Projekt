using OpenTK;
using OpenTK.Input;

namespace CG_Projekt
{
    class Controller
    {
        View view;
        Model model;
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
        }

        internal void UpdatePlayerPosition(float deltaTime)
        {
            var keyboard = Keyboard.GetState(); // Holt den Zustand des Keyboards
            float moveLR = keyboard.IsKeyDown(Key.Left) ? -0.2f : keyboard.IsKeyDown(Key.Right) ? 0.2f : 0.0f; // 0.2f und - 0.2f Gibt an wie schnell sich der spieler in die entsprechende Richtung bewegen kann.
            float moveUD = keyboard.IsKeyDown(Key.Down) ? -0.2f : keyboard.IsKeyDown(Key.Up) ? 0.2f : 0.0f;
            model.player._position += deltaTime * new Vector2(moveLR, moveUD);
        }
        internal void ScrollControl(float deltaTime)
        {
            var mouse = Mouse.GetState();
            var scrollValue = mouse.ScrollWheelValue;

            if (scrollValue < oldScrollValue)
            {
                axisZoom = -2f;
                oldScrollValue = scrollValue;
            }
            else if (scrollValue > oldScrollValue)
            {
                axisZoom = 2f;
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
    }
}