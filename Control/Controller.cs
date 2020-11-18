using CG_Projekt.Models;
using OpenTK.Input;
using System.Collections.Generic;


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
            //Updatet den Spieler
            UpdatePlayer(deltaTime);
            //Checkt die Collisons
            UpdateCheckCollision();
            //Rotiert die Camera
            UpdateRotateCamera(deltaTime);
            // Updatet die Enemies
            UpdateEnemy(model.enemies, deltaTime);
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

        internal void UpdateEnemy(List<Enemy> enemies, float deltaTime) // Enemies bewegen sich richtung sdasdwsadSpieler
        {
            foreach (Enemy enemy in enemies)
            {
                enemy.EnemyAI(enemy, deltaTime, model.player);
            }
        }
        internal void UpdatePlayer(float deltaTime)
        {
            model.player.MovePlayer(model.player, deltaTime);

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
      
        internal void UpdateCheckCollision()
        {
            intersection.CheckPlayerBorderCollision(model.player);
            intersection.CheckPlayerCollisionWithGameobject(model.player, model.enemies, model.obstacles, model.pickUps);
            intersection.CheckEnemyObstacleCollision(model.enemies, model.obstacles);


        }

    }
}