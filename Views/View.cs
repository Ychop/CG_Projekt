using System;
using OpenTK.Graphics;

namespace CG_Projekt
{
    using CG_Projekt.Framework;
    using CG_Projekt.Models;
    using OpenTK;
    using OpenTK.Graphics.OpenGL;
    using System.Drawing;
    using GL = OpenTK.Graphics.OpenGL.GL;

    internal class View
    {
        private readonly int texPlayer;
        private readonly int texEnemy;
        private readonly int texObstacle;
        private readonly int texCollectible;
        private readonly int texBullet;
        private readonly int texGrass;
        private readonly int texMud;
        private readonly int texWater;
        private readonly int texHealth;
        private readonly int texBlood;
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
        private readonly int texHealthBackground;
        private Random random = new Random();
        public bool GameOver = false;

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
            this.texGrass = Texture.Load(Resource.LoadStream(content + "grass.png"));
            this.texMud = Texture.Load(Resource.LoadStream(content + "mud.jpg"));
            this.texBlood = Texture.Load(Resource.LoadStream(content + "Blood.png"));
            this.texWater = Texture.Load(Resource.LoadStream(content + "water.png"));
            this.texHealth = Texture.Load(Resource.LoadStream(content + "healthbar.png"));
            this.texHealthBackground = Texture.Load(Resource.LoadStream(content + "healthbar -Background.png"));
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
            this.DrawBullets(model);
            this.DrawParticle(model);
            this.DrawGameObjects(model);
            this.DrawPlayer(model);         
            this.Camera.Draw();
            this.DrawHUD(model);
            if (GameOver)
            {
                this.DrawGameOver(model);
            }
        }
        internal void DrawHUD(Model model)
        {
            Vector2 heathbarPosition = this.Camera.Center + (this.Camera.Scale * new Vector2(0, -0.92f));
            Vector2 HUDPosition = this.Camera.Center + (this.Camera.Scale * new Vector2(-0.8f, -0.92f));
            DrawPlayerHealth(model, heathbarPosition);
            DrawScore(model, heathbarPosition);
            DrawWeaponSelected(model, HUDPosition);
        }
        internal void DrawPlayerHealth(Model model, Vector2 heathbarPosition)
        {
            GL.BindTexture(TextureTarget.Texture2D, this.texHealthBackground);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(new Vector2(0, 0));
            GL.Vertex2(heathbarPosition + (2 * this.Camera.Scale * new Vector2(-0.3f, -0.03f)));
            GL.TexCoord2(new Vector2(1, 0));
            GL.Vertex2(heathbarPosition + (2 * this.Camera.Scale * new Vector2(0.3f, -0.03f)));
            GL.TexCoord2(new Vector2(1, 1));
            GL.Vertex2(heathbarPosition + (2 * this.Camera.Scale * new Vector2(0.3f, 0.03f)));
            GL.TexCoord2(new Vector2(0, 1));
            GL.Vertex2(heathbarPosition + (2 * this.Camera.Scale * new Vector2(-0.3f, 0.03f)));
            GL.End();
            GL.BindTexture(TextureTarget.Texture2D, this.texHealth);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(new Vector2(0, 0));
            GL.Vertex2(heathbarPosition + (2 * this.Camera.Scale * new Vector2(-0.3f * model.Player.Hitpoints, -0.017f)));
            GL.TexCoord2(new Vector2(1, 0));
            GL.Vertex2(heathbarPosition + (2 * this.Camera.Scale * new Vector2(0.3f * model.Player.Hitpoints, -0.017f)));
            GL.TexCoord2(new Vector2(1, 1));
            GL.Vertex2(heathbarPosition + (2 * this.Camera.Scale * new Vector2(0.3f * model.Player.Hitpoints, 0.017f)));
            GL.TexCoord2(new Vector2(0, 1));
            GL.Vertex2(heathbarPosition + (2 * this.Camera.Scale * new Vector2(-0.3f * model.Player.Hitpoints, 0.017f)));
            GL.End();
        }
        internal void DrawScore(Model model, Vector2 heathbarPosition)
        {
            GL.BindTexture(TextureTarget.Texture2D, texfontScore);
            DrawFont($"Kills={model.Score:D}", heathbarPosition.X + 0.6f * Camera.Scale, heathbarPosition.Y - 0.03f * Camera.Scale, Camera.Scale * 0.08f);
        }
        internal void DrawWeaponSelected(Model model, Vector2 HUDPosition)
        {
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
            switch (model.weaponSelected)
            {
                case 1:
                    GL.BindTexture(TextureTarget.Texture2D, texfontAmmo);
                    DrawFont($"{model.Player.AmmoPistol:D}", HUDPosition.X - 0.3f * Camera.Scale, HUDPosition.Y - 0.012f * Camera.Scale, Camera.Scale * 0.08f);
                    break;
                case 2:
                    GL.BindTexture(TextureTarget.Texture2D, texfontAmmo);
                    DrawFont($"{model.Player.AmmoUZI:D}", HUDPosition.X - 0.3f * Camera.Scale, HUDPosition.Y - 0.012f * Camera.Scale, Camera.Scale * 0.08f);
                    break;
                case 3:
                    GL.BindTexture(TextureTarget.Texture2D, texfontAmmo);
                    DrawFont($"{model.Player.AmmoShotgun:D}", HUDPosition.X - 0.3f * Camera.Scale, HUDPosition.Y - 0.012f * Camera.Scale, Camera.Scale * 0.08f);
                    break;
                case 4:
                    GL.BindTexture(TextureTarget.Texture2D, texfontAmmo);
                    DrawFont($"{model.Player.AmmoRPG:D}", HUDPosition.X - 0.3f * Camera.Scale, HUDPosition.Y - 0.012f * Camera.Scale, Camera.Scale * 0.08f);
                    break;
            }
        }
        internal void EnemyHealth(Enemy enemy)
        {
            GL.BindTexture(TextureTarget.Texture2D, this.texHealthBackground);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(new Vector2(0, 0));
            GL.Vertex2(enemy.Position + new Vector2(-enemy.RadiusDraw, enemy.RadiusDraw + 0.0025f));
            GL.TexCoord2(new Vector2(1, 0));
            GL.Vertex2(enemy.Position + new Vector2(enemy.RadiusDraw, enemy.RadiusDraw + 0.0025f));
            GL.TexCoord2(new Vector2(1, 1));
            GL.Vertex2(enemy.Position + new Vector2(enemy.RadiusDraw, -enemy.RadiusDraw + 0.02f));
            GL.TexCoord2(new Vector2(0, 1));
            GL.Vertex2(enemy.Position + new Vector2(-enemy.RadiusDraw, -enemy.RadiusDraw + 0.02f));
            GL.End();
            GL.BindTexture(TextureTarget.Texture2D, this.texHealth);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(new Vector2(0, 0));
            GL.Vertex2(enemy.Position + new Vector2(-enemy.RadiusDraw * enemy.Hitpoints, enemy.RadiusDraw + 0.0025f));
            GL.TexCoord2(new Vector2(1, 0));
            GL.Vertex2(enemy.Position + new Vector2(enemy.RadiusDraw * enemy.Hitpoints, enemy.RadiusDraw + 0.0025f));
            GL.TexCoord2(new Vector2(1, 1));
            GL.Vertex2(enemy.Position + new Vector2(enemy.RadiusDraw * enemy.Hitpoints, -enemy.RadiusDraw + 0.02f));
            GL.TexCoord2(new Vector2(0, 1));
            GL.Vertex2(enemy.Position + new Vector2(-enemy.RadiusDraw * enemy.Hitpoints, -enemy.RadiusDraw + 0.02f));
            GL.End();
        }
        internal void DrawGameOver(Model model)
        {
            GameOver = true;
            GL.Clear(ClearBufferMask.ColorBufferBit);
            DrawFont($"Du bist Gestorben.", Camera.Center.X - 1.8f * Camera.Scale, Camera.Center.Y, 0.2f * Camera.Scale);
            DrawFont($"Deine Kills Waren:", Camera.Center.X - 1.8f * Camera.Scale, Camera.Center.Y - 0.25f * Camera.Scale, 0.2f * Camera.Scale);
            DrawFont($"{model.Score:D}", Camera.Center.X - 0.01f, Camera.Center.Y - 0.45f * Camera.Scale, 0.2f * Camera.Scale);
        }
        internal void Resize(int width, int height)
        {
            this.Camera.Resize(width, height);
        }
        internal void DrawLevelGrid(Model model)
        {
            foreach (LevelGrid levelGrid in model.LevelGrids)
            {
                GL.BindTexture(TextureTarget.Texture2D, this.texGrass);
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(new Vector2(0, 0));
                GL.Vertex2(levelGrid.Position);
                GL.TexCoord2(new Vector2(1, 0));
                GL.Vertex2(levelGrid.Position + new Vector2(0.012f, 0));
                GL.TexCoord2(new Vector2(1, 1));
                GL.Vertex2(levelGrid.Position + new Vector2(0.012f, 0.012f));
                GL.TexCoord2(new Vector2(0, 1));
                GL.Vertex2(levelGrid.Position + new Vector2(0, 0.012f));
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
            GL.Vertex2(new Vector2(-model.Player.RadiusDraw, -model.Player.RadiusDraw));
            GL.TexCoord2(new Vector2(0, 1));
            GL.Vertex2(new Vector2(model.Player.RadiusDraw, -model.Player.RadiusDraw));
            GL.TexCoord2(new Vector2(0, 0));
            GL.Vertex2(new Vector2(model.Player.RadiusDraw, model.Player.RadiusDraw));
            GL.TexCoord2(new Vector2(1, 0));
            GL.Vertex2(new Vector2(-model.Player.RadiusDraw, model.Player.RadiusDraw));
            GL.End();
            GL.PopMatrix();
        }
        internal void DrawBullets(Model model)
        {
            foreach (Bullet bullet in model.Bullets)
            {
                GL.BindTexture(TextureTarget.Texture2D, this.texBullet);
                GL.PushMatrix();
                GL.Translate(new Vector3(bullet.Position.X, bullet.Position.Y, 0));
                GL.Rotate(bullet.Angle, new Vector3d(0, 0, -1));
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(new Vector2(0, 0));
                GL.Vertex2(new Vector2(-bullet.RadiusDraw, -bullet.RadiusDraw));
                GL.TexCoord2(new Vector2(0, 1));
                GL.Vertex2(new Vector2(bullet.RadiusDraw, -bullet.RadiusDraw));
                GL.TexCoord2(new Vector2(1, 1));
                GL.Vertex2(new Vector2(bullet.RadiusDraw, bullet.RadiusDraw));
                GL.TexCoord2(new Vector2(1, 0));
                GL.Vertex2(new Vector2(-bullet.RadiusDraw, bullet.RadiusDraw));
                GL.End();
                GL.PopMatrix();
            }
        }
        internal void DrawParticle(Model model)
        {
            foreach (Particle particle in model.Particles)
            {
                GL.BindTexture(TextureTarget.Texture2D, this.texBlood);
                GL.Begin(PrimitiveType.Quads);
                // GL.Color3(Color.Red);
                GL.TexCoord2(new Vector2(0, 0));
                GL.Vertex2(particle.Position + new Vector2(-particle.RadiusDraw, -particle.RadiusDraw));
                GL.TexCoord2(new Vector2(0, 1));
                GL.Vertex2(particle.Position + new Vector2(particle.RadiusDraw, -particle.RadiusDraw));
                GL.TexCoord2(new Vector2(1, 1));
                GL.Vertex2(particle.Position + new Vector2(particle.RadiusDraw, particle.RadiusDraw));
                GL.TexCoord2(new Vector2(1, 0));
                GL.Vertex2(particle.Position + new Vector2(-particle.RadiusDraw, particle.RadiusDraw));
                GL.End();
            }
            foreach(Particle paricleFrament in model.RPGFragments)
            {
                GL.BindTexture(TextureTarget.Texture2D, this.texBlood);
                GL.Begin(PrimitiveType.Quads);
                // GL.Color3(Color.Red);
                GL.TexCoord2(new Vector2(0, 0));
                GL.Vertex2(paricleFrament.Position + new Vector2(-paricleFrament.RadiusDraw, -paricleFrament.RadiusDraw));
                GL.TexCoord2(new Vector2(0, 1));
                GL.Vertex2(paricleFrament.Position + new Vector2(paricleFrament.RadiusDraw, -paricleFrament.RadiusDraw));
                GL.TexCoord2(new Vector2(1, 1));
                GL.Vertex2(paricleFrament.Position + new Vector2(paricleFrament.RadiusDraw, paricleFrament.RadiusDraw));
                GL.TexCoord2(new Vector2(1, 0));
                GL.Vertex2(paricleFrament.Position + new Vector2(-paricleFrament.RadiusDraw, paricleFrament.RadiusDraw));
                GL.End();
            }
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
                GL.Vertex2(new Vector2((float)(-enemy.RadiusDraw * Math.Sin(y)), (float)(-enemy.RadiusDraw * Math.Sin(y))));
                GL.TexCoord2(new Vector2(1, 0));
                GL.Vertex2(new Vector2((float)(enemy.RadiusDraw * Math.Sin(y)), (float)(-enemy.RadiusDraw * Math.Sin(y))));
                GL.TexCoord2(new Vector2(1, 1));
                GL.Vertex2(new Vector2((float)(enemy.RadiusDraw * Math.Sin(y)), (float)(enemy.RadiusDraw * Math.Sin(y))));
                GL.TexCoord2(new Vector2(0, 1));
                GL.Vertex2(new Vector2((float)(-enemy.RadiusDraw * Math.Sin(y)), (float)(enemy.RadiusDraw * Math.Sin(y))));
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
                GL.Vertex2(obstacle.Position + new Vector2(-obstacle.RadiusDraw, -obstacle.RadiusDraw));
                GL.TexCoord2(new Vector2(1, 0));
                GL.Vertex2(obstacle.Position + new Vector2(obstacle.RadiusDraw, -obstacle.RadiusDraw));
                GL.TexCoord2(new Vector2(1, 1));
                GL.Vertex2(obstacle.Position + new Vector2(obstacle.RadiusDraw, obstacle.RadiusDraw));
                GL.TexCoord2(new Vector2(0, 1));
                GL.Vertex2(obstacle.Position + new Vector2(-obstacle.RadiusDraw, obstacle.RadiusDraw));
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
                GL.Vertex2(pickup.Position + new Vector2(-pickup.RadiusDraw, -pickup.RadiusDraw));
                GL.TexCoord2(new Vector2(1, 0));
                GL.Vertex2(pickup.Position + new Vector2(pickup.RadiusDraw, -pickup.RadiusDraw));
                GL.TexCoord2(new Vector2(1, 1));
                GL.Vertex2(pickup.Position + new Vector2(pickup.RadiusDraw, pickup.RadiusDraw));
                GL.TexCoord2(new Vector2(0, 1));
                GL.Vertex2(pickup.Position + new Vector2(-pickup.RadiusDraw, pickup.RadiusDraw));
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