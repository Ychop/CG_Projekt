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
        private readonly int texBullet;
        private readonly int texFloor;
        private readonly int texHealth;

        public View(Camera camera)
        {
            Camera = camera;
            var content = $"{nameof(CG_Projekt)}.Content.";
            texPlayer = Texture.Load(Resource.LoadStream(content + "playerNew.png"));
            texEnemy = Texture.Load(Resource.LoadStream(content + "enemyNew.png"));
            texObstacle = Texture.Load(Resource.LoadStream(content + "rocks.png"));
            texCollectible = Texture.Load(Resource.LoadStream(content + "collectible.png"));
            texBullet = Texture.Load(Resource.LoadStream(content + "bullet.png"));
            texFloor = Texture.Load(Resource.LoadStream(content + "grass.png"));
            texHealth = Texture.Load(Resource.LoadStream(content + "healthbar.png"));
            GL.Enable(EnableCap.Texture2D);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha); 
            GL.Enable(EnableCap.Blend);
        }

        public Camera Camera { get; } 

        internal void Draw(Model model)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            Camera.Center = model.player.Position;
            DrawLevel();
            DrawLevelGrid(model);
            DrawGameObjects(model);
            DrawPlayer(model);
            DrawBullets(model);
            DrawUI(model);
            Camera.Draw();





        }


        internal void DrawUI(Model model)
        {

            Vector2 HeathbarPosition = (model.player.Position + (Camera.Scale * new Vector2(0, -0.92f)));
            //Helathbar
            GL.BindTexture(TextureTarget.Texture2D, texHealth);
            GL.Begin(PrimitiveType.Quads);
            GL.Color3(Color.White);
            GL.TexCoord2(new Vector2(0, 0));
            GL.Vertex2(HeathbarPosition + Camera.Scale * new Vector2(-0.3f, -0.03f));
            GL.TexCoord2(new Vector2(1, 0));
            GL.Vertex2(HeathbarPosition + Camera.Scale * new Vector2(0.3f, -0.03f));
            GL.TexCoord2(new Vector2(1, 1));
            GL.Vertex2(HeathbarPosition + Camera.Scale * new Vector2(0.3f, 0.03f));
            GL.TexCoord2(new Vector2(0, 1));
            GL.Vertex2(HeathbarPosition + Camera.Scale * new Vector2(-0.3f, 0.03f));
            GL.End();

            GL.Begin(PrimitiveType.Quads);
            GL.Color3(Color.Green);
            GL.TexCoord2(new Vector2(0, 0));
            GL.Vertex2(HeathbarPosition + Camera.Scale * new Vector2(-0.3f * model.player.Hitpoints, -0.017f));
            GL.TexCoord2(new Vector2(1, 0));
            GL.Vertex2(HeathbarPosition + Camera.Scale * new Vector2(0.3f * model.player.Hitpoints, -0.017f));
            GL.TexCoord2(new Vector2(1, 1));
            GL.Vertex2(HeathbarPosition + Camera.Scale * new Vector2(0.3f * model.player.Hitpoints, 0.017f));
            GL.TexCoord2(new Vector2(0, 1));
            GL.Vertex2(HeathbarPosition + Camera.Scale * new Vector2(-0.3f * model.player.Hitpoints, 0.017f));
            GL.End();
            //TODO: Position der Helthbar ist noch nicht richtig


            //TODO: Highscore
            //TODO: Ammo count
        }

        public void EnemyHelath(Enemy enemy)
        {
            GL.BindTexture(TextureTarget.Texture2D, texHealth);
            GL.LineWidth(5f);
            GL.Begin(PrimitiveType.Lines);
            //GL.Color3(Color.White);
            GL.TexCoord2(new Vector2(0, 0));
            GL.Vertex2(enemy.Position + new Vector2(-enemy.Size, enemy.Size + 0.002f));
            GL.TexCoord2(new Vector2(1, 1));
            GL.Vertex2(enemy.Position + new Vector2(enemy.Size, enemy.Size + 0.002f));
            GL.End();

            GL.LineWidth(4f);
            GL.Begin(PrimitiveType.Lines);
            // GL.Color3(Color.Green);
            GL.TexCoord2(new Vector2(0, 0));
            GL.Vertex2(enemy.Position + new Vector2(-enemy.Size * enemy.Hitpoints, enemy.Size + 0.002f));
            GL.TexCoord2(new Vector2(1, 1));
            GL.Vertex2(enemy.Position + new Vector2(enemy.Size * enemy.Hitpoints, enemy.Size + 0.002f));
            GL.End();
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

                GL.BindTexture(TextureTarget.Texture2D, texFloor);
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(new Vector2(0, 0));
                GL.Vertex2(model.levelGrids[i]._position);
                GL.TexCoord2(new Vector2(1, 0));
                GL.Vertex2(model.levelGrids[i]._position + new Vector2(0.018f, 0));
                GL.TexCoord2(new Vector2(1, 1));
                GL.Vertex2(model.levelGrids[i]._position + new Vector2(0.018f, 0.018f));
                GL.TexCoord2(new Vector2(0, 1));
                GL.Vertex2(model.levelGrids[i]._position + new Vector2(0, 0.018f));
                GL.End();

                i++;
            }
        }

        internal void DrawPlayer(Model model)
        {
            GL.BindTexture(TextureTarget.Texture2D, texPlayer);
            GL.PushMatrix();
            GL.Translate(new Vector3(model.player.Position.X, model.player.Position.Y, 0));
            GL.Rotate(model.player.Angle, new Vector3d(0, 0, -1));
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(new Vector2(1, 1));
            GL.Vertex2(new Vector2(-model.player.Size, -model.player.Size));
            GL.TexCoord2(new Vector2(0, 1));
            GL.Vertex2(new Vector2(model.player.Size, -model.player.Size));
            GL.TexCoord2(new Vector2(0, 0));
            GL.Vertex2(new Vector2(model.player.Size, model.player.Size));
            GL.TexCoord2(new Vector2(1, 0));
            GL.Vertex2(new Vector2(-model.player.Size, model.player.Size));
            GL.End();

            GL.PopMatrix();
        }
        internal void DrawBullets(Model model)
        {
            foreach (Bullet bullet in model.bullets)
            {
                GL.BindTexture(TextureTarget.Texture2D, texBullet);
                GL.Color3(Color.Black);
                GL.Begin(PrimitiveType.Quads);

                GL.TexCoord2(new Vector2(0, 0));
                GL.Vertex2(bullet.Position + new Vector2(-bullet.Size, -bullet.Size));
                GL.TexCoord2(new Vector2(0, 1));
                GL.Vertex2(bullet.Position + new Vector2(bullet.Size, -bullet.Size));
                GL.TexCoord2(new Vector2(1, 1));
                GL.Vertex2(bullet.Position + new Vector2(bullet.Size, bullet.Size));
                GL.TexCoord2(new Vector2(1, 0));
                GL.Vertex2(bullet.Position + new Vector2(-bullet.Size, bullet.Size));
                GL.End();
            }
        }


        internal void DrawGameObjects(Model model)
        {
            var i = 0;
            foreach (Enemy enemy in model.enemies)
            {

                GL.BindTexture(TextureTarget.Texture2D, texEnemy);

                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(new Vector2(0, 0));
                GL.Vertex2(model.enemies[i].Position + new Vector2(-model.enemies[i].Size, -model.enemies[i].Size));
                GL.TexCoord2(new Vector2(1, 0));
                GL.Vertex2(model.enemies[i].Position + new Vector2(model.enemies[i].Size, -model.enemies[i].Size));
                GL.TexCoord2(new Vector2(1, 1));
                GL.Vertex2(model.enemies[i].Position + new Vector2(model.enemies[i].Size, model.enemies[i].Size));
                GL.TexCoord2(new Vector2(0, 1));
                GL.Vertex2(model.enemies[i].Position + new Vector2(-model.enemies[i].Size, model.enemies[i].Size));

                GL.End();
                EnemyHelath(model.enemies[i]);
                i++;
            }
            i = 0;

            foreach (Obstacle obstacle in model.obstacles)
            {
                // GL.Color3(obstacle.Color);
                GL.BindTexture(TextureTarget.Texture2D, texObstacle);
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(new Vector2(0, 0));
                GL.Vertex2(model.obstacles[i].Position + new Vector2(-model.obstacles[i].Size, -model.obstacles[i].Size));
                GL.TexCoord2(new Vector2(1, 0));
                GL.Vertex2(model.obstacles[i].Position + new Vector2(model.obstacles[i].Size, -model.obstacles[i].Size));
                GL.TexCoord2(new Vector2(1, 1));
                GL.Vertex2(model.obstacles[i].Position + new Vector2(model.obstacles[i].Size, model.obstacles[i].Size));
                GL.TexCoord2(new Vector2(0, 1));
                GL.Vertex2(model.obstacles[i].Position + new Vector2(-model.obstacles[i].Size, model.obstacles[i].Size));
                GL.End();
                i++;
            }
            i = 0;
            foreach (PickUp pickup in model.pickUps)
            {
                GL.BindTexture(TextureTarget.Texture2D, texCollectible);
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(new Vector2(0, 0));
                GL.Vertex2(model.pickUps[i].Position + new Vector2(-model.pickUps[i].Size, -model.pickUps[i].Size));
                GL.TexCoord2(new Vector2(1, 0));
                GL.Vertex2(model.pickUps[i].Position + new Vector2(model.pickUps[i].Size, -model.pickUps[i].Size));
                GL.TexCoord2(new Vector2(1, 1));
                GL.Vertex2(model.pickUps[i].Position + new Vector2(model.pickUps[i].Size, model.pickUps[i].Size));
                GL.TexCoord2(new Vector2(0, 1));
                GL.Vertex2(model.pickUps[i].Position + new Vector2(-model.pickUps[i].Size, model.pickUps[i].Size));
                GL.End();
                i++;
            }
        }
    }
}