namespace CG_Projekt
{
    using System.Drawing;
    using CG_Projekt.Framework;
    using CG_Projekt.Models;
    using OpenTK;
    using OpenTK.Graphics.OpenGL;
    using GL = OpenTK.Graphics.OpenGL.GL;

    internal class View
    {
        private readonly int texPlayer;
        private readonly int texEnemy;
        private readonly int texObstacle;
        private readonly int texCollectible;
        private readonly int texBullet;
        private readonly int texFloor;
        private readonly int texHealth;

        internal View(Camera camera)
        {
            this.Camera = camera;
            var content = $"{nameof(CG_Projekt)}.Content.";
            this.texPlayer = Texture.Load(Resource.LoadStream(content + "playerNew.png"));
            this.texEnemy = Texture.Load(Resource.LoadStream(content + "enemyNew.png"));
            this.texObstacle = Texture.Load(Resource.LoadStream(content + "rocks.png"));
            this.texCollectible = Texture.Load(Resource.LoadStream(content + "collectible.png"));
            this.texBullet = Texture.Load(Resource.LoadStream(content + "bullet.png"));
            this.texFloor = Texture.Load(Resource.LoadStream(content + "grass.png"));
            this.texHealth = Texture.Load(Resource.LoadStream(content + "healthbar.png"));
            GL.Enable(EnableCap.Texture2D);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.Blend);
        }

        internal Camera Camera { get; }

        internal void Draw(Model model)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            this.Camera.Center = model.Player.Position;
            this.DrawLevel();
            this.DrawLevelGrid(model);
            this.DrawGameObjects(model);
            this.DrawPlayer(model);
            this.DrawBullets(model);
            this.DrawUI(model);
            this.Camera.Draw();
        }

        internal void DrawUI(Model model)
        {
            Vector2 heathbarPosition = this.Camera.Center + (this.Camera.Scale * new Vector2(0, -0.92f));

            // Helathbar
            GL.BindTexture(TextureTarget.Texture2D, this.texHealth);
            GL.Begin(PrimitiveType.Quads);
            GL.Color3(Color.White);
            GL.TexCoord2(new Vector2(0, 0));
            GL.Vertex2(heathbarPosition + (this.Camera.Scale * new Vector2(-0.3f, -0.03f)));
            GL.TexCoord2(new Vector2(1, 0));
            GL.Vertex2(heathbarPosition + (this.Camera.Scale * new Vector2(0.3f, -0.03f)));
            GL.TexCoord2(new Vector2(1, 1));
            GL.Vertex2(heathbarPosition + (this.Camera.Scale * new Vector2(0.3f, 0.03f)));
            GL.TexCoord2(new Vector2(0, 1));
            GL.Vertex2(heathbarPosition + (this.Camera.Scale * new Vector2(-0.3f, 0.03f)));
            GL.End();

            GL.Begin(PrimitiveType.Quads);
            GL.Color3(Color.Green);
            GL.TexCoord2(new Vector2(0, 0));
            GL.Vertex2(heathbarPosition + (this.Camera.Scale * new Vector2(-0.3f * model.Player.Hitpoints, -0.017f)));
            GL.TexCoord2(new Vector2(1, 0));
            GL.Vertex2(heathbarPosition + (this.Camera.Scale * new Vector2(0.3f * model.Player.Hitpoints, -0.017f)));
            GL.TexCoord2(new Vector2(1, 1));
            GL.Vertex2(heathbarPosition + (this.Camera.Scale * new Vector2(0.3f * model.Player.Hitpoints, 0.017f)));
            GL.TexCoord2(new Vector2(0, 1));
            GL.Vertex2(heathbarPosition + (this.Camera.Scale * new Vector2(-0.3f * model.Player.Hitpoints, 0.017f)));
            GL.End();

            // TODO: Position der Helthbar ist noch nicht richtig
            // TODO: Highscore
            // TODO: Ammo count
        }

        internal void EnemyHelath(Enemy enemy)
        {
            GL.BindTexture(TextureTarget.Texture2D, this.texHealth);
            GL.LineWidth(5f);
            GL.Begin(PrimitiveType.Lines);
            GL.TexCoord2(new Vector2(0, 0));
            GL.Vertex2(enemy.Position + new Vector2(-enemy.Size, enemy.Size + 0.002f));
            GL.TexCoord2(new Vector2(1, 1));
            GL.Vertex2(enemy.Position + new Vector2(enemy.Size, enemy.Size + 0.002f));
            GL.End();

            GL.LineWidth(4f);
            GL.Begin(PrimitiveType.Lines);
            GL.TexCoord2(new Vector2(0, 0));
            GL.Vertex2(enemy.Position + new Vector2(-enemy.Size * enemy.Hitpoints, enemy.Size + 0.002f));
            GL.TexCoord2(new Vector2(1, 1));
            GL.Vertex2(enemy.Position + new Vector2(enemy.Size * enemy.Hitpoints, enemy.Size + 0.002f));
            GL.End();
        }

        internal void DrawGameOver()
        {
            // TODO: Draw Gameover Screen
        }

        internal void Resize(int width, int height)
        {
            this.Camera.Resize(width, height);
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
            foreach (LevelGrid levelGrid in model.LevelGrids)
            {
                GL.BindTexture(TextureTarget.Texture2D, this.texFloor);
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(new Vector2(0, 0));
                GL.Vertex2(levelGrid.Position);
                GL.TexCoord2(new Vector2(1, 0));
                GL.Vertex2(levelGrid.Position + new Vector2(0.018f, 0));
                GL.TexCoord2(new Vector2(1, 1));
                GL.Vertex2(levelGrid.Position + new Vector2(0.018f, 0.018f));
                GL.TexCoord2(new Vector2(0, 1));
                GL.Vertex2(levelGrid.Position + new Vector2(0, 0.018f));
                GL.End();
            }
        }

        internal void DrawPlayer(Model model)
        {
            GL.BindTexture(TextureTarget.Texture2D, this.texPlayer);
            GL.PushMatrix();
            GL.Translate(new Vector3(model.Player.Position.X, model.Player.Position.Y, 0));
            GL.Rotate(model.Player.Angle, new Vector3d(0, 0, -1));
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(new Vector2(1, 1));
            GL.Vertex2(new Vector2(-model.Player.Size, -model.Player.Size));
            GL.TexCoord2(new Vector2(0, 1));
            GL.Vertex2(new Vector2(model.Player.Size, -model.Player.Size));
            GL.TexCoord2(new Vector2(0, 0));
            GL.Vertex2(new Vector2(model.Player.Size, model.Player.Size));
            GL.TexCoord2(new Vector2(1, 0));
            GL.Vertex2(new Vector2(-model.Player.Size, model.Player.Size));
            GL.End();
            GL.PopMatrix();
        }

        internal void DrawBullets(Model model)
        {
            foreach (Bullet bullet in model.Bullets)
            {
                GL.BindTexture(TextureTarget.Texture2D, this.texBullet);
                GL.Color3(Color.Black);
                GL.PushMatrix();
                GL.Translate(new Vector3(bullet.Position.X, bullet.Position.Y, 0));
                GL.Rotate(bullet.Angle, new Vector3d(0, 0, -1));
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(new Vector2(0, 0));
                GL.Vertex2(new Vector2(-bullet.Size, -bullet.Size));
                GL.TexCoord2(new Vector2(0, 1));
                GL.Vertex2(new Vector2(bullet.Size, -bullet.Size));
                GL.TexCoord2(new Vector2(1, 1));
                GL.Vertex2(new Vector2(bullet.Size, bullet.Size));
                GL.TexCoord2(new Vector2(1, 0));
                GL.Vertex2(new Vector2(-bullet.Size, bullet.Size));
                GL.End();
                GL.PopMatrix();
            }
        }

        internal void DrawGameObjects(Model model)
        {
            foreach (Enemy enemy in model.Enemies)
            {
                GL.BindTexture(TextureTarget.Texture2D, this.texEnemy);
                GL.PushMatrix();
                GL.Translate(new Vector3(enemy.Position.X, enemy.Position.Y, 0));
                GL.Rotate(enemy.AngleToPlayer, new Vector3d(0, 0, 1));
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(new Vector2(0, 0));
                GL.Vertex2(new Vector2(-enemy.Size, -enemy.Size));
                GL.TexCoord2(new Vector2(1, 0));
                GL.Vertex2(new Vector2(enemy.Size, -enemy.Size));
                GL.TexCoord2(new Vector2(1, 1));
                GL.Vertex2(new Vector2(enemy.Size, enemy.Size));
                GL.TexCoord2(new Vector2(0, 1));
                GL.Vertex2(new Vector2(-enemy.Size, enemy.Size));
                GL.End();
                GL.PopMatrix();
                this.EnemyHelath(enemy);
            }

            foreach (Obstacle obstacle in model.Obstacles)
            {
                GL.BindTexture(TextureTarget.Texture2D, this.texObstacle);
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(new Vector2(0, 0));
                GL.Vertex2(obstacle.Position + new Vector2(-obstacle.Size, -obstacle.Size));
                GL.TexCoord2(new Vector2(1, 0));
                GL.Vertex2(obstacle.Position + new Vector2(obstacle.Size, -obstacle.Size));
                GL.TexCoord2(new Vector2(1, 1));
                GL.Vertex2(obstacle.Position + new Vector2(obstacle.Size, obstacle.Size));
                GL.TexCoord2(new Vector2(0, 1));
                GL.Vertex2(obstacle.Position + new Vector2(-obstacle.Size, obstacle.Size));
                GL.End();
            }

            foreach (PickUp pickup in model.PickUps)
            {
                GL.BindTexture(TextureTarget.Texture2D, this.texCollectible);
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(new Vector2(0, 0));
                GL.Vertex2(pickup.Position + new Vector2(-pickup.Size, -pickup.Size));
                GL.TexCoord2(new Vector2(1, 0));
                GL.Vertex2(pickup.Position + new Vector2(pickup.Size, -pickup.Size));
                GL.TexCoord2(new Vector2(1, 1));
                GL.Vertex2(pickup.Position + new Vector2(pickup.Size, pickup.Size));
                GL.TexCoord2(new Vector2(0, 1));
                GL.Vertex2(pickup.Position + new Vector2(-pickup.Size, pickup.Size));
                GL.End();
            }
        }
    }
}