using System.Collections.Generic;
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
        private readonly int texEnemy;
        private readonly int texObstacle;
        private readonly int texCollectible;

        public View()
        {
            var content = $"{nameof(CG_Projekt)}.Content.";
            texPlayer = Texture.Load(Resource.LoadStream(content + "player.jpg"));
            texEnemy = Texture.Load(Resource.LoadStream(content + "monster.jpg"));
            texObstacle = Texture.Load(Resource.LoadStream(content + "rocks.png"));
            texCollectible = Texture.Load(Resource.LoadStream(content + "collectible.jpg"));
            GL.Enable(EnableCap.Texture2D);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.Blend);
            
        }

        public Camera camera { get; } = new Camera();

        internal void Draw(Model model)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            camera.Center = model.players[0].Position;
            DrawLevel();
            DrawLevelGrid(model);
            DrawPlayerNew(model.players);
            DrawBullets(model);
            DrawUI(model);
            DrawEnemy(model.enemies);
            DrawObstacles(model.obstacles);
            DrawPickUps(model.pickUps);
            camera.Draw();
        }

        internal void DrawUI(Model model)
        {

            Vector2 HeathbarPosition = (model.players[0].Position + (camera.Scale * new Vector2(0, -0.92f)));
            //Helathbar
            GL.Begin(PrimitiveType.Quads);
            GL.Color3(Color.White);
            GL.Vertex2(HeathbarPosition + Camera.Scale * new Vector2(-0.2f, -0.01f));
            GL.Vertex2(HeathbarPosition + Camera.Scale * new Vector2(0.2f, -0.01f));
            GL.Vertex2(HeathbarPosition + Camera.Scale * new Vector2(0.2f, 0.01f));
            GL.Vertex2(HeathbarPosition + Camera.Scale * new Vector2(-0.2f, 0.01f));
            GL.End();

            GL.Begin(PrimitiveType.Quads);
            GL.Color3(Color.Green);
            GL.Vertex2(HeathbarPosition + camera.Scale * new Vector2(-0.199f * model.players[0].Hitpoints, -0.009f));
            GL.Vertex2(HeathbarPosition + camera.Scale * new Vector2(0.199f * model.players[0].Hitpoints, -0.009f));
            GL.Vertex2(HeathbarPosition + camera.Scale * new Vector2(0.199f * model.players[0].Hitpoints, 0.009f));
            GL.Vertex2(HeathbarPosition + camera.Scale * new Vector2(-0.199f * model.players[0].Hitpoints, 0.009f));
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
            Camera.Resize(width, height);
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
        private void DrawEnemy(IEnumerable<Enemy> enemies)
        {
            // bind the texture
            GL.BindTexture(TextureTarget.Texture2D, texEnemy);
            foreach (var enemy in enemies)
            {
                Draw(enemy, new Rect(0f, 0f, 1f, 1f));
            }

        }
        private void DrawObstacles(IEnumerable<Obstacle> obstacles)
        {
            // bind the texture
            GL.BindTexture(TextureTarget.Texture2D, texObstacle);
            foreach (var obstacle in obstacles)
            {
                Draw(obstacle, new Rect(0f, 0f, 1f, 1f));
            }

        }
        private void DrawPickUps(IEnumerable<PickUp> pickups)
        {
            // bind the texture
            GL.BindTexture(TextureTarget.Texture2D, texCollectible);
            foreach (var pickup in pickups)
            {
                Draw(pickup, new Rect(0f, 0f, 1f, 1f));
            }

        }
        private void DrawPlayerNew(IEnumerable<Player> players)
        {
            // bind the texture
            GL.BindTexture(TextureTarget.Texture2D, texPlayer);
            foreach (var player in players)
            {
                //GL.PushMatrix();
                //GL.Translate(new Vector3(player.Position.X, player.Position.Y, 0));
                //GL.Rotate(player.Angle, new Vector3d(0, 0, -1));

                Draw(player, new Rect(0f, 0f, 1f, 1f));
                //GL.PopMatrix();
            }

        }

        internal void DrawPlayer(Model model)
        {
            //GL.BindTexture(TextureTarget.Texture2D, texPlayer);
            GL.PushMatrix();
            GL.Translate(new Vector3(model.players[0].Position.X, model.players[0].Position.Y, 0));
            GL.Rotate(model.players[0].Angle, new Vector3d(0, 0, -1));
            GL.Color3(model.players[0].Color);
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex2(new Vector2(-model.players[0].Size, -model.players[0].Size));
            GL.Vertex2(new Vector2(model.players[0].Size, -model.players[0].Size));
            GL.Vertex2(new Vector2(model.players[0].Size, model.players[0].Size));
            GL.Vertex2(new Vector2(-model.players[0].Size, model.players[0].Size));
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

        private static void Draw(IReadOnlyRectangle rectangle, IReadOnlyRectangle texCoords)
        {
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(texCoords.MinX, texCoords.MinY);
            GL.Vertex2(rectangle.MinX, rectangle.MinY);
            GL.TexCoord2(texCoords.MaxX, texCoords.MinY);
            GL.Vertex2(rectangle.MaxX, rectangle.MinY);
            GL.TexCoord2(texCoords.MaxX, texCoords.MaxY);
            GL.Vertex2(rectangle.MaxX, rectangle.MaxY);
            GL.TexCoord2(texCoords.MinX, texCoords.MaxY);
            GL.Vertex2(rectangle.MinX, rectangle.MaxY);
            GL.End();
        }

    }
}