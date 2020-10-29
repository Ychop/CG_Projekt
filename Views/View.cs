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
            camera.Draw();
            camera.Center = model.player._position;
        }

        internal void Resize(int width, int height)
        {
            GL.Viewport(0, 0, width, height);
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
            foreach (LevelGrid levelGrid in model.LevelGrids)
            {
                GL.Color3(Color.Blue);
                GL.Begin(PrimitiveType.Quads);
                GL.Vertex2(model.LevelGrids[i]._Position);
                GL.Vertex2(model.LevelGrids[i]._Position + new Vector2(0.017f, 0));
                GL.Vertex2(model.LevelGrids[i]._Position + new Vector2(0.017f, 0.017f));
                GL.Vertex2(model.LevelGrids[i]._Position + new Vector2(0, 0.017f));
                GL.End();
                i++;
            }
        }

        internal void DrawPlayer(Model model)
        {
            GL.Color3(Color.Green);
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex2(model.player._position + new Vector2(-0.01f, -0.01f));
            GL.Vertex2(model.player._position + new Vector2(0.01f, -0.01f));
            GL.Vertex2(model.player._position + new Vector2(0.01f, 0.01f));
            GL.Vertex2(model.player._position + new Vector2(-0.01f, 0.01f));
            GL.End();
        }

        internal void DrawGameObjects(Model model)
        {
            var i = 0;
            foreach (Enemy enemy in model.Enemies)
            {
                GL.Color3(Color.Red);
                GL.Begin(PrimitiveType.Quads);
                GL.Vertex2(model.Enemies[i]._position + new Vector2(-0.01f, -0.01f));
                GL.Vertex2(model.Enemies[i]._position + new Vector2(0.01f, -0.01f));
                GL.Vertex2(model.Enemies[i]._position + new Vector2(0.01f, 0.01f));
                GL.Vertex2(model.Enemies[i]._position + new Vector2(-0.01f, 0.01f));
                GL.End();
                i++;
            }
            i = 0;
            foreach (Obstacle obstacle in model.Obstacles)
            {
                GL.Color3(Color.Brown);
                GL.Begin(PrimitiveType.Quads);
                GL.Vertex2(model.Obstacles[i]._position + new Vector2(-0.01f, -0.01f));
                GL.Vertex2(model.Obstacles[i]._position + new Vector2(0.01f, -0.01f));
                GL.Vertex2(model.Obstacles[i]._position + new Vector2(0.01f, 0.01f));
                GL.Vertex2(model.Obstacles[i]._position + new Vector2(-0.01f, 0.01f));
                GL.End();
                i++;
            }
            i = 0;
            foreach (PickUp pickup in model.PickUps)
            {
                GL.Color3(Color.Yellow);
                GL.Begin(PrimitiveType.Quads);
                GL.Vertex2(model.PickUps[i]._position + new Vector2(-0.01f, -0.01f));
                GL.Vertex2(model.PickUps[i]._position + new Vector2(0.01f, -0.01f));
                GL.Vertex2(model.PickUps[i]._position + new Vector2(0.01f, 0.01f));
                GL.Vertex2(model.PickUps[i]._position + new Vector2(-0.01f, 0.01f));
                GL.End();
                i++;
            }

        }
    }
}