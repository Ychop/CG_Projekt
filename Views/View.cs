using CG_Projekt.Models;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace CG_Projekt
{
    internal class View
    {
        public View()
        {

        }

        public Camera camera { get; } = new Camera();

        internal void Draw(Model model)
        {

            GL.Clear(ClearBufferMask.ColorBufferBit);

            DrawLevel();
            DrawLevelGrid(model);
            DrawPlayer(model);
            DrawGameObjects(model);
          
            camera.Center = model.player._position;

            camera.Draw();
            model.player.AglignPlayer(model.player);
           




        }

        internal void Resize(int width, int height)
        {
            camera.Resize(width,height);
        }

        internal void DrawLevel()
        {
            GL.Begin(PrimitiveType.Quads);
            GL.Color3(Color.White);
            GL.Vertex2(-0.9f, -0.9f);
            GL.Vertex2(0.9f, -0.9f);
            GL.Vertex2(0.9f, 0.9f);
            GL.Vertex2(-0.9f, 0.9f);
            GL.End();
        }

        internal void DrawLevelGrid(Model model)
        {
            var i = 0;
            foreach (LevelGrid levelGrid in model.levelGrids)
            {
                GL.Color3(model.levelGrids[i]._color);
                GL.Begin(PrimitiveType.Quads);
                GL.Vertex2(model.levelGrids[i]._position);
                GL.Vertex2(model.levelGrids[i]._position + new Vector2(0.017f, 0));
                GL.Vertex2(model.levelGrids[i]._position + new Vector2(0.017f, 0.017f));
                GL.Vertex2(model.levelGrids[i]._position + new Vector2(0, 0.017f));
                GL.End();
                i++;
            }
        }

        internal void DrawPlayer(Model model)
        {
            
            GL.Color3(model.player._color);
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex2(model.player._position + new Vector2(-model.player._size  , -model.player._size));
            GL.Vertex2(model.player._position + new Vector2(model.player._size , -model.player._size));
            GL.Vertex2(model.player._position + new Vector2(model.player._size , model.player._size ));
            GL.Vertex2(model.player._position + new Vector2(-model.player._size, model.player._size ));
            GL.End();
            
        }

        internal void DrawGameObjects(Model model)
        {
            var i = 0;
            foreach (Enemy enemy in model.enemies)
            {
                GL.Color3(Color.Red);
                GL.Begin(PrimitiveType.Quads);
                GL.Vertex2(model.enemies[i]._position + new Vector2(-model.enemies[i]._size, -model.enemies[i]._size));
                GL.Vertex2(model.enemies[i]._position + new Vector2(model.enemies[i]._size, -model.enemies[i]._size));
                GL.Vertex2(model.enemies[i]._position + new Vector2(model.enemies[i]._size, model.enemies[i]._size));
                GL.Vertex2(model.enemies[i]._position + new Vector2(-model.enemies[i]._size, model.enemies[i]._size));
                GL.End();
                i++;
            }
            i = 0;
            foreach (Obstacle obstacle in model.obstacles)
            {
                GL.Color3(Color.Brown);
                GL.Begin(PrimitiveType.Quads);
                GL.Vertex2(model.obstacles[i]._position + new Vector2(-model.obstacles[i]._size, -model.obstacles[i]._size));
                GL.Vertex2(model.obstacles[i]._position + new Vector2(model.obstacles[i]._size, -model.obstacles[i]._size));
                GL.Vertex2(model.obstacles[i]._position + new Vector2(model.obstacles[i]._size, model.obstacles[i]._size));
                GL.Vertex2(model.obstacles[i]._position + new Vector2(-model.obstacles[i]._size, model.obstacles[i]._size));
                GL.End();
                i++;
            }
            i = 0;
            foreach (PickUp pickup in model.pickUps)
            {
                GL.Color3(Color.Yellow);
                GL.Begin(PrimitiveType.Quads);
                GL.Vertex2(model.pickUps[i]._position + new Vector2(-model.pickUps[i]._size, -model.pickUps[i]._size));
                GL.Vertex2(model.pickUps[i]._position + new Vector2(model.pickUps[i]._size, -model.pickUps[i]._size));
                GL.Vertex2(model.pickUps[i]._position + new Vector2(model.pickUps[i]._size, model.pickUps[i]._size));
                GL.Vertex2(model.pickUps[i]._position + new Vector2(-model.pickUps[i]._size, model.pickUps[i]._size));
                GL.End();
                i++;
            }

        }
    }
}