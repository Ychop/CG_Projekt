using CG_Projekt.Models;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Drawing;
using CG_Projekt.Framework;
using GL = OpenTK.Graphics.OpenGL.GL;

namespace CG_Projekt
{
    internal class View
    {

        private readonly int texPlayer;

        public View()
        {
            var content = $"{nameof(CG_Projekt)}.Content.";
            texPlayer = Texture.Load(Resource.LoadStream(content + "player.jpg"));
            GL.Enable(EnableCap.Texture2D);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.Blend);

        }

        public Camera camera { get; } = new Camera();

        internal void Draw(Model model)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            camera.Center = model.player.Position;
            DrawLevel();
            DrawLevelGrid(model);
            DrawGameObjects(model);
            DrawPlayer(model);
            DrawBullets(model);
            DrawUI(model);
            camera.Draw();





        }


        internal void DrawUI(Model model)
        {

            Vector2 HeathbarPosition = (model.player.Position + (camera.Scale * new Vector2(0, -0.92f)));
            //Helathbar
            GL.Begin(PrimitiveType.Quads);
            GL.Color3(Color.White);
            GL.Vertex2(HeathbarPosition + camera.Scale * new Vector2(-0.2f, -0.01f));
            GL.Vertex2(HeathbarPosition + camera.Scale * new Vector2(0.2f, -0.01f));
            GL.Vertex2(HeathbarPosition + camera.Scale * new Vector2(0.2f, 0.01f));
            GL.Vertex2(HeathbarPosition + camera.Scale * new Vector2(-0.2f, 0.01f));
            GL.End();

            GL.Begin(PrimitiveType.Quads);
            GL.Color3(Color.Green);
            GL.Vertex2(HeathbarPosition + camera.Scale * new Vector2(-0.199f * model.player.Hitpoints, -0.009f));
            GL.Vertex2(HeathbarPosition + camera.Scale * new Vector2(0.199f * model.player.Hitpoints, -0.009f));
            GL.Vertex2(HeathbarPosition + camera.Scale * new Vector2(0.199f * model.player.Hitpoints, 0.009f));
            GL.Vertex2(HeathbarPosition + camera.Scale * new Vector2(-0.199f * model.player.Hitpoints, 0.009f));
            GL.End();
            //TODO: Position der Helthbar ist noch nicht richtig


            //TODO: Highscore
            //TODO: Ammo count
        }
        internal void DrawGameOver()
        {
            //TODO: Draw Gameover Screen
        }

        internal void Resize(int width, int height)
        {
            camera.Resize(width, height);
        }

        internal void DrawLevel()
        {
            GL.Begin(PrimitiveType.Quads);
            GL.Color3(Color.White);
            GL.TexCoord2(-0.9f, -0.9f);
            GL.Vertex2(-0.9f, -0.9f);
            GL.TexCoord2(0.9f, -0.9f);
            GL.Vertex2(0.9f, -0.9f);
            GL.TexCoord2(0.9f, 0.9f);
            GL.Vertex2(0.9f, 0.9f);
            GL.TexCoord2(-0.9f, 0.9f);
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
                GL.TexCoord2(model.levelGrids[i]._position);
                GL.Vertex2(model.levelGrids[i]._position);
                GL.TexCoord2(model.levelGrids[i]._position + new Vector2(0.017f, 0));
                GL.Vertex2(model.levelGrids[i]._position + new Vector2(0.017f, 0));
                GL.TexCoord2(model.levelGrids[i]._position + new Vector2(0.017f, 0.017f));
                GL.Vertex2(model.levelGrids[i]._position + new Vector2(0.017f, 0.017f));
                GL.TexCoord2(model.levelGrids[i]._position + new Vector2(0, 0.017f));
                GL.Vertex2(model.levelGrids[i]._position + new Vector2(0, 0.017f));
                GL.End();

                i++;
            }
        }

        internal void DrawPlayer(Model model)
        {
            GL.PushMatrix();
            GL.Translate(new Vector3(model.player.Position.X, model.player.Position.Y, 0));
            GL.Rotate(model.player.Angle, new Vector3d(0, 0, -1));
            GL.Color3(model.player.Color);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(new Vector2(-model.player.Size, -model.player.Size));
            GL.Vertex2(new Vector2(-model.player.Size, -model.player.Size));
            GL.TexCoord2(new Vector2(model.player.Size, -model.player.Size));
            GL.Vertex2(new Vector2(model.player.Size, -model.player.Size));
            GL.TexCoord2(new Vector2(model.player.Size, model.player.Size));
            GL.Vertex2(new Vector2(model.player.Size, model.player.Size));
            GL.TexCoord2(new Vector2(-model.player.Size, model.player.Size));
            GL.Vertex2(new Vector2(-model.player.Size, model.player.Size));
            GL.End();
            GL.PopMatrix();
        }
        internal void DrawBullets(Model model)
        {
            foreach (Bullet bullet in model.bullets)
            {
                GL.Color3(Color.Black);
                GL.Begin(PrimitiveType.Quads);
                GL.Vertex2(bullet.Position + new Vector2(-bullet.Size, -bullet.Size));
                GL.Vertex2(bullet.Position + new Vector2(bullet.Size, -bullet.Size));
                GL.Vertex2(bullet.Position + new Vector2(bullet.Size, bullet.Size));
                GL.Vertex2(bullet.Position + new Vector2(-bullet.Size, bullet.Size));
                GL.End();
            }
        }
        internal void DrawGameObjects(Model model)
        {
            var i = 0;
            foreach (Enemy enemy in model.enemies)
            {

                GL.Color3(enemy.Color);
                GL.Begin(PrimitiveType.Quads);
                GL.Vertex2(model.enemies[i].Position + new Vector2(-model.enemies[i].Size, -model.enemies[i].Size));
                GL.Vertex2(model.enemies[i].Position + new Vector2(model.enemies[i].Size, -model.enemies[i].Size));
                GL.Vertex2(model.enemies[i].Position + new Vector2(model.enemies[i].Size, model.enemies[i].Size));
                GL.Vertex2(model.enemies[i].Position + new Vector2(-model.enemies[i].Size, model.enemies[i].Size));

                GL.End();
                model.enemies[i].EnemyHelath(model.enemies[i]);
                i++;
            }
            i = 0;
            foreach (Obstacle obstacle in model.obstacles)
            {
                GL.Color3(obstacle.Color);
                GL.Begin(PrimitiveType.Quads);
                GL.Vertex2(model.obstacles[i].Position + new Vector2(-model.obstacles[i].Size, -model.obstacles[i].Size));
                GL.Vertex2(model.obstacles[i].Position + new Vector2(model.obstacles[i].Size, -model.obstacles[i].Size));
                GL.Vertex2(model.obstacles[i].Position + new Vector2(model.obstacles[i].Size, model.obstacles[i].Size));
                GL.Vertex2(model.obstacles[i].Position + new Vector2(-model.obstacles[i].Size, model.obstacles[i].Size));
                GL.End();
                i++;
            }
            i = 0;
            foreach (PickUp pickup in model.pickUps)
            {
                GL.Color3(pickup.Color);
                GL.Begin(PrimitiveType.Quads);
                GL.Vertex2(model.pickUps[i].Position + new Vector2(-model.pickUps[i].Size, -model.pickUps[i].Size));
                GL.Vertex2(model.pickUps[i].Position + new Vector2(model.pickUps[i].Size, -model.pickUps[i].Size));
                GL.Vertex2(model.pickUps[i].Position + new Vector2(model.pickUps[i].Size, model.pickUps[i].Size));
                GL.Vertex2(model.pickUps[i].Position + new Vector2(-model.pickUps[i].Size, model.pickUps[i].Size));
                GL.End();
                i++;
            }
        }
    }
}