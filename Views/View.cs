using System;
using OpenTK.Graphics;

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
        private readonly int texfontScore;
        private readonly int texfontAmmo;
        private readonly int texHeartCollectible;
        private readonly int texAmmoPistol;
        private readonly int texAmmoUzi;
        private readonly int texAmmoShotgun;
        private readonly int texAmmoMissile;
        private readonly int texPistol;
        private readonly int texUzi;
        private readonly int texShotgun;
        private readonly int texRPG;
        private Random random = new Random();

        internal View(Camera camera)
        {
            this.Camera = camera;
            var content = $"{nameof(CG_Projekt)}.Content.";
            this.texPlayer = Texture.Load(Resource.LoadStream(content + "playerNew.png"));
            this.texEnemy = Texture.Load(Resource.LoadStream(content + "enemyNew.png"));
            this.texObstacle = Texture.Load(Resource.LoadStream(content + "rocks.png"));
            this.texCollectible = Texture.Load(Resource.LoadStream(content + "collectible.png"));
            this.texHeartCollectible = Texture.Load(Resource.LoadStream(content + "heart.png"));
            this.texBullet = Texture.Load(Resource.LoadStream(content + "bullet.png"));
            this.texFloor = Texture.Load(Resource.LoadStream(content + "grass.png"));
            this.texHealth = Texture.Load(Resource.LoadStream(content + "healthbar.png"));
            this.texAmmoPistol = Texture.Load(Resource.LoadStream(content + "ammoPistol.png"));
            this.texAmmoUzi = Texture.Load(Resource.LoadStream(content + "ammoUZI.png"));
            this.texAmmoShotgun = Texture.Load(Resource.LoadStream(content + "ammoShotgun.png"));
            this.texAmmoMissile = Texture.Load(Resource.LoadStream(content + "ammoMissile.png"));
            this.texPistol = Texture.Load(Resource.LoadStream(content + "Pistol.png"));
            this.texUzi = Texture.Load(Resource.LoadStream(content + "Uzi.png"));
            this.texShotgun = Texture.Load(Resource.LoadStream(content + "Shotgun.png"));
            this.texRPG = Texture.Load(Resource.LoadStream(content + "Rocketlauncher.png"));
            this.texfontScore = Texture.Load(Resource.LoadStream(content + "Blood_Bath.png"));
            this.texfontAmmo = Texture.Load(Resource.LoadStream(content + "Blood_Bath.png"));
            GL.Enable(EnableCap.AlphaTest);
            GL.AlphaFunc(AlphaFunction.Greater, 0.2f);
            GL.Enable(EnableCap.Texture2D);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.Blend);

        }

        internal Camera Camera { get; }

        internal void Draw(Model model)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            this.Camera.Center = model.Player.Position;
            this.DrawLevelGrid(model);
            this.DrawGameObjects(model);
            this.DrawPlayer(model);
            this.DrawBullets(model);
            this.DrawParticle(model);
            this.DrawHUD(model);
            this.Camera.Draw();
        }

        internal void DrawHUD(Model model)
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
            GL.BindTexture(TextureTarget.Texture2D, texfontScore);
            DrawFont($"Score={model.Score:D}", heathbarPosition.X + 0.1f, heathbarPosition.Y - 0.003f, 0.008f);
            Vector2 HUDPosition = this.Camera.Center + (this.Camera.Scale * new Vector2(-0.5f, -0.92f));
            switch (model.weaponSelected)
            {
                case 1:
                    GL.BindTexture(TextureTarget.Texture2D, texPistol);
                    break;
                case 2:
                    GL.BindTexture(TextureTarget.Texture2D, texUzi);
                    break;
                case 3:
                    GL.BindTexture(TextureTarget.Texture2D, texShotgun);
                    break;
                case 4:
                    GL.BindTexture(TextureTarget.Texture2D, texRPG);
                    break;
            }
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(new Vector2(0, 0));
            GL.Vertex2(HUDPosition + (this.Camera.Scale * new Vector2(-0.1f, -0.05f)));
            GL.TexCoord2(new Vector2(1, 0));
            GL.Vertex2(HUDPosition + (this.Camera.Scale * new Vector2(0.1f, -0.05f)));
            GL.TexCoord2(new Vector2(1, 1));
            GL.Vertex2(HUDPosition + (this.Camera.Scale * new Vector2(0.1f, 0.05f)));
            GL.TexCoord2(new Vector2(0, 1));
            GL.Vertex2(HUDPosition + (this.Camera.Scale * new Vector2(-0.1f, 0.05f)));
            GL.End();
            // TODO: Ammo count
            switch (model.weaponSelected)
            {
                case 1:
                    GL.BindTexture(TextureTarget.Texture2D, texfontAmmo);
                    DrawFont($"{model.Player.AmmoPistol:D}", HUDPosition.X - 0.019f, HUDPosition.Y - 0.012f, 0.008f);
                    break;
                case 2:
                    GL.BindTexture(TextureTarget.Texture2D, texfontAmmo);
                    DrawFont($"{model.Player.AmmoUZI:D}", HUDPosition.X - 0.019f, HUDPosition.Y - 0.012f, 0.008f);
                    break;
                case 3:
                    GL.BindTexture(TextureTarget.Texture2D, texfontAmmo);
                    DrawFont($"{model.Player.AmmoShotgun:D}", HUDPosition.X - 0.019f, HUDPosition.Y - 0.012f, 0.008f);
                    break;
                case 4:
                    GL.BindTexture(TextureTarget.Texture2D, texfontAmmo);
                    DrawFont($"{model.Player.AmmoRPG:D}", HUDPosition.X - 0.019f, HUDPosition.Y - 0.012f, 0.008f);
                    break;
            }


        }
        internal void EnemyHealth(Enemy enemy)
        {
            GL.BindTexture(TextureTarget.Texture2D, this.texHealth);
            GL.LineWidth(5f);
            GL.Begin(PrimitiveType.Lines);
            GL.TexCoord2(new Vector2(0, 1));
            GL.Vertex2(enemy.Position + new Vector2(-enemy.Radius, enemy.Radius + 0.002f));
            GL.TexCoord2(new Vector2(1, 1));
            GL.Vertex2(enemy.Position + new Vector2(enemy.Radius, enemy.Radius + 0.002f));
            GL.TexCoord2(new Vector2(1, 0));
            GL.Vertex2(enemy.Position + new Vector2(enemy.Radius, -enemy.Radius + 0.002f));
            GL.TexCoord2(new Vector2(0, 0));
            GL.Vertex2(enemy.Position + new Vector2(-enemy.Radius, -enemy.Radius + 0.002f));
            GL.End();



            GL.LineWidth(4f);
            GL.Begin(PrimitiveType.Lines);
            GL.TexCoord2(new Vector2(0, 0));
            GL.Vertex2(enemy.Position + new Vector2(-enemy.Radius * enemy.Hitpoints, enemy.Radius + 0.002f));
            GL.TexCoord2(new Vector2(1, 1));
            GL.Vertex2(enemy.Position + new Vector2(enemy.Radius * enemy.Hitpoints, enemy.Radius + 0.002f));
            GL.TexCoord2(new Vector2(1, 0));
            GL.Vertex2(enemy.Position + new Vector2(enemy.Radius * enemy.Hitpoints, -enemy.Radius + 0.002f));
            GL.TexCoord2(new Vector2(0, 0));
            GL.Vertex2(enemy.Position + new Vector2(-enemy.Radius * enemy.Hitpoints, -enemy.Radius + 0.002f));
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
            GL.Vertex2(new Vector2(-model.Player.Radius, -model.Player.Radius));
            GL.TexCoord2(new Vector2(0, 1));
            GL.Vertex2(new Vector2(model.Player.Radius, -model.Player.Radius));
            GL.TexCoord2(new Vector2(0, 0));
            GL.Vertex2(new Vector2(model.Player.Radius, model.Player.Radius));
            GL.TexCoord2(new Vector2(1, 0));
            GL.Vertex2(new Vector2(-model.Player.Radius, model.Player.Radius));
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
                GL.Vertex2(new Vector2(-bullet.Radius, -bullet.Radius));
                GL.TexCoord2(new Vector2(0, 1));
                GL.Vertex2(new Vector2(bullet.Radius, -bullet.Radius));
                GL.TexCoord2(new Vector2(1, 1));
                GL.Vertex2(new Vector2(bullet.Radius, bullet.Radius));
                GL.TexCoord2(new Vector2(1, 0));
                GL.Vertex2(new Vector2(-bullet.Radius, bullet.Radius));
                GL.End();
                GL.PopMatrix();
            }
        }
        internal void DrawParticle(Model model)
        {
         
        }
        internal void DrawGameObjects(Model model)
        {
            float y = ((float)this.random.NextDouble() * 0.1f + 0.9f);
            foreach (Enemy enemy in model.Enemies)
            {
                GL.BindTexture(TextureTarget.Texture2D, this.texEnemy);
                GL.Disable(EnableCap.Blend);
                GL.PushMatrix();
                GL.Translate(new Vector3(enemy.Position.X, enemy.Position.Y, 0));
                GL.Rotate(enemy.AngleToPlayer, new Vector3d(0, 0, 1));
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(new Vector2(0, 0));
                GL.Vertex2(new Vector2((float)(-enemy.Radius * Math.Sin(y)), (float) (-enemy.Radius * Math.Sin(y))));
                GL.TexCoord2(new Vector2(1, 0));
                GL.Vertex2(new Vector2((float)(enemy.Radius * Math.Sin(y)), (float)(-enemy.Radius * Math.Sin(y))));
                GL.TexCoord2(new Vector2(1, 1));
                GL.Vertex2(new Vector2((float)(enemy.Radius * Math.Sin(y)), (float)(enemy.Radius * Math.Sin(y))));
                GL.TexCoord2(new Vector2(0, 1));
                GL.Vertex2(new Vector2((float)(-enemy.Radius * Math.Sin(y)), (float)(enemy.Radius * Math.Sin(y))));
                GL.End();
                GL.PopMatrix();
                GL.Enable(EnableCap.Blend);
                this.EnemyHealth(enemy);
            }

            foreach (Obstacle obstacle in model.Obstacles)
            {
                GL.BindTexture(TextureTarget.Texture2D, this.texObstacle);
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(new Vector2(0, 0));
                GL.Vertex2(obstacle.Position + new Vector2(-obstacle.Radius, -obstacle.Radius));
                GL.TexCoord2(new Vector2(1, 0));
                GL.Vertex2(obstacle.Position + new Vector2(obstacle.Radius, -obstacle.Radius));
                GL.TexCoord2(new Vector2(1, 1));
                GL.Vertex2(obstacle.Position + new Vector2(obstacle.Radius, obstacle.Radius));
                GL.TexCoord2(new Vector2(0, 1));
                GL.Vertex2(obstacle.Position + new Vector2(-obstacle.Radius, obstacle.Radius));
                GL.End();
            }

            foreach (PickUp pickup in model.PickUps)
            {
                switch (pickup.Type)
                {
                    case 0:
                        GL.BindTexture(TextureTarget.Texture2D, this.texHeartCollectible);
                        break;
                    case 1:
                        GL.BindTexture(TextureTarget.Texture2D, this.texAmmoPistol);
                        break;
                    case 2:
                        GL.BindTexture(TextureTarget.Texture2D, this.texAmmoUzi);
                        break;
                    case 3:
                        GL.BindTexture(TextureTarget.Texture2D, this.texAmmoShotgun);
                        break;
                    case 4:
                        GL.BindTexture(TextureTarget.Texture2D, this.texAmmoMissile);
                        break;
                }
                GL.Disable(EnableCap.Blend);
                //GL.AlphaFunc(AlphaFunction.Greater, 0.05f);
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(new Vector2(0, 0));
                GL.Vertex2(pickup.Position + new Vector2(-pickup.Radius, -pickup.Radius));
                GL.TexCoord2(new Vector2(1, 0));
                GL.Vertex2(pickup.Position + new Vector2(pickup.Radius, -pickup.Radius));
                GL.TexCoord2(new Vector2(1, 1));
                GL.Vertex2(pickup.Position + new Vector2(pickup.Radius, pickup.Radius));
                GL.TexCoord2(new Vector2(0, 1));
                GL.Vertex2(pickup.Position + new Vector2(-pickup.Radius, pickup.Radius));
                GL.End();
                GL.Enable(EnableCap.Blend);
            }
        }

        private void DrawFont(string text, float x, float y, float size)
        {
            GL.Color4(Color4.White);
            const uint firstCharacter = 32; // the ASCII code of the first character stored in the bitmap font
            const uint charactersPerColumn = 10; // how many characters are in each column
            const uint charactersPerRow = 10; // how many characters are in each row
            var rect = new Rect(x, y, size, size); // rectangle of the first character
            foreach (var spriteId in SpriteSheetTools.StringToSpriteIds(text, firstCharacter))
            {
                //TODO: Calculate the texture coordinates of the characters letter from the bitmap font texture
                var texCoords = SpriteSheetTools.CalcTexCoords(spriteId, charactersPerRow, charactersPerColumn);
                //TODO: Draw a rectangle at the characters relative position
                DrawRect(rect, texCoords);
                rect.MinX += rect.SizeX;
            }

        }

        private static void DrawRect(IReadOnlyRectangle rectangle, IReadOnlyRectangle texCoords)
        {
            //TODO: draw a rectangle with texture coordinates
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