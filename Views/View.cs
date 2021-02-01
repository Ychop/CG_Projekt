using OpenTK.Input;

namespace CG_Projekt
{
    using CG_Projekt.Framework;
    using CG_Projekt.Models;
    using OpenTK;
    using OpenTK.Graphics.OpenGL;
    using GL = OpenTK.Graphics.OpenGL.GL;
    using System.Linq;
    using System;
    using OpenTK.Graphics;

    internal class View
    {
        private int texPlayerPistol;
        private int texPlayerShotgun;
        private int texPlayerUzi;
        private int texEnemyWalk;
        private int texObstacle;
        private int texStart;
        private int texStartBlack;
        private int texGrass;
        private int texWater;
        private int texHealth;
        private int texBlood;
        private int texFragment;
        private int texfontScore;
        private int texfontAmmo;
        private int texHeartCollectible;
        private int texAmmoPistol;
        private int texAmmoUzi;
        private int texAmmoShotgun;
        private int texMissile;
        private int texPistol;
        private int texUzi;
        private int texShotgun;
        private int texRPG;
        private int texBullet;
        private int texHealthBackground;
        private Random random = new Random();
        public bool GameOver = false;
        public bool GameStarted = false;
        public bool TexturesLoaded = false;

        internal View(Camera camera)
        {
            this.Camera = camera;
            this.Loadtextures();

        }
        internal void Loadtextures()
        {
            var content = $"{nameof(CG_Projekt)}.Content.Textures.";
            this.texPlayerPistol = Texture.Load(Resource.LoadStream(content + "PlayerPistol.png"));
            this.texPlayerShotgun = Texture.Load(Resource.LoadStream(content + "PlayerShotgun.png"));
            this.texPlayerUzi = Texture.Load(Resource.LoadStream(content + "PlayerUzi.png"));
            this.texEnemyWalk = Texture.Load(Resource.LoadStream(content + "enemywalkanimation.png"));
            this.texObstacle = Texture.Load(Resource.LoadStream(content + "rocks.png"));
            this.texHeartCollectible = Texture.Load(Resource.LoadStream(content + "heart.png"));
            this.texStart = Texture.Load(Resource.LoadStream(content + "Press.png"));
            this.texStartBlack = Texture.Load(Resource.LoadStream(content + "PressBlack.png"));
            this.texGrass = Texture.Load(Resource.LoadStream(content + "grass.png"));
            this.texFragment = Texture.Load(Resource.LoadStream(content + "debris.png"));
            this.texBlood = Texture.Load(Resource.LoadStream(content + "BloodNew.png"));
            this.texWater = Texture.Load(Resource.LoadStream(content + "water.jpg"));
            this.texHealth = Texture.Load(Resource.LoadStream(content + "healthbar.png"));
            this.texHealthBackground = Texture.Load(Resource.LoadStream(content + "healthbar -Background.png"));
            this.texAmmoPistol = Texture.Load(Resource.LoadStream(content + "ammoPistol.png"));
            this.texAmmoUzi = Texture.Load(Resource.LoadStream(content + "ammoUZI.png"));
            this.texAmmoShotgun = Texture.Load(Resource.LoadStream(content + "ammoShotgun.png"));
            this.texMissile = Texture.Load(Resource.LoadStream(content + "ammoMissile.png"));
            this.texBullet = Texture.Load(Resource.LoadStream(content + "bullet.png"));
            this.texPistol = Texture.Load(Resource.LoadStream(content + "Pistol.png"));
            this.texUzi = Texture.Load(Resource.LoadStream(content + "Uzi.png"));
            this.texShotgun = Texture.Load(Resource.LoadStream(content + "Shotgun.png"));
            this.texRPG = Texture.Load(Resource.LoadStream(content + "Rocketlauncher.png"));
            this.texfontScore = Texture.Load(Resource.LoadStream(content + "Blood_Bath.png"));
            this.texfontAmmo = Texture.Load(Resource.LoadStream(content + "SilverFont.png"));
            GL.Enable(EnableCap.AlphaTest);
            GL.AlphaFunc(AlphaFunction.Greater, 0.2f);
            GL.Enable(EnableCap.Texture2D);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.Blend);
            TexturesLoaded = true;
        }

        internal float change = -1f;
        internal Camera Camera { get; }
        internal void Draw(Model model)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            this.Camera.Center = model.Player.Position;
            this.DrawLevel(model);
            this.DrawGameObjects(model);
            this.DrawHUD(model);
            if (!GameStarted)
            {
                PressAnyKeyToStart();
            }
            if (GameOver)
            {
                this.DrawGameOver(model);
            }
        }
        internal void DrawLevel(Model model)
        {
            this.DrawSea(model);
            this.DrawLevelGrid(model);
            this.Camera.Draw();
        }
        internal void DrawGameObjects(Model model)
        {
            this.DrawBullets(model);
            this.DrawParticle(model);
            this.DrawObstacles(model);
            this.DrawEnemyWalk(model);
            this.DrawPlayer(model);
            this.DrawPickups(model);

        }
        internal void DrawHUD(Model model)
        {
            Vector2 healthbarScaleOffset = new Vector2(0, -0.92f);
            Vector2 hudScaleOffset = new Vector2(-0.8f, -0.92f);
            Vector2 heathbarPosition = this.Camera.Center + (this.Camera.Scale * healthbarScaleOffset);
            Vector2 HUDPosition = this.Camera.Center + (this.Camera.Scale * hudScaleOffset);
            DrawPlayerHealth(model, heathbarPosition);
            DrawScore(model, heathbarPosition);
            DrawWeaponSelected(model, HUDPosition);
        }
        internal void PressAnyKeyToStart()
        {
            if (change < 0)
            {
                GL.BindTexture(TextureTarget.Texture2D, this.texStart);
                change += 0.25f;
            }
            else
            {
                GL.BindTexture(TextureTarget.Texture2D, this.texStartBlack);
                change = -1f;
            }

            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(new Vector2(0, 0));
            GL.Vertex2(Camera.Center + new Vector2(-0.18f, -0.05f));
            GL.TexCoord2(new Vector2(1, 0));
            GL.Vertex2(Camera.Center + new Vector2(0.18f, -0.05f));
            GL.TexCoord2(new Vector2(1, 1));
            GL.Vertex2(Camera.Center + new Vector2(0.18f, 0.05f));
            GL.TexCoord2(new Vector2(0, 1));
            GL.Vertex2(Camera.Center + new Vector2(-0.18f, 0.05f));
            GL.End();
        }
        internal void DrawSea(Model model)
        {
            GL.BindTexture(TextureTarget.Texture2D, this.texWater);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(new Vector2(0, 0));
            GL.Vertex2(-1, -1);
            GL.TexCoord2(new Vector2(1, 0));
            GL.Vertex2(1, -1);
            GL.TexCoord2(new Vector2(1, 1));
            GL.Vertex2(1, 1);
            GL.TexCoord2(new Vector2(0, 1));
            GL.Vertex2(-1, 1);
            GL.End();
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
                    DrawFont($"{model.Player.AmmoPistol:D}", HUDPosition.X - 0.35f * Camera.Scale, HUDPosition.Y - 0.012f * Camera.Scale, Camera.Scale * 0.08f);
                    break;
                case 2:
                    GL.BindTexture(TextureTarget.Texture2D, texfontAmmo);
                    DrawFont($"{model.Player.AmmoUZI:D}", HUDPosition.X - 0.35f * Camera.Scale, HUDPosition.Y - 0.012f * Camera.Scale, Camera.Scale * 0.08f);
                    break;
                case 3:
                    GL.BindTexture(TextureTarget.Texture2D, texfontAmmo);
                    DrawFont($"{model.Player.AmmoShotgun:D}", HUDPosition.X - 0.35f * Camera.Scale, HUDPosition.Y - 0.012f * Camera.Scale, Camera.Scale * 0.08f);
                    break;
                case 4:
                    GL.BindTexture(TextureTarget.Texture2D, texfontAmmo);
                    DrawFont($"{model.Player.AmmoRPG:D}", HUDPosition.X - 0.35f * Camera.Scale, HUDPosition.Y - 0.012f * Camera.Scale, Camera.Scale * 0.08f);
                    break;
            }
        }
        internal void DrawGameOver(Model model)
        {
            GL.BindTexture(TextureTarget.Texture2D, texfontScore);
            GameOver = true;
            GL.Clear(ClearBufferMask.ColorBufferBit);
            DrawFont($"Du bist gestorben.", Camera.Center.X - 1.8f * Camera.Scale, Camera.Center.Y, 0.2f * Camera.Scale);
            DrawFont($"Deine Kills waren:", Camera.Center.X - 1.8f * Camera.Scale, Camera.Center.Y - 0.25f * Camera.Scale, 0.2f * Camera.Scale);
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
            GL.BindTexture(TextureTarget.Texture2D, this.texPlayerPistol);
            switch (model.weaponSelected)
            {
                case 1:
                    GL.BindTexture(TextureTarget.Texture2D, this.texPlayerPistol);
                    break;
                case 2:
                    GL.BindTexture(TextureTarget.Texture2D, this.texPlayerUzi);
                    break;
                case 3:
                    GL.BindTexture(TextureTarget.Texture2D, this.texPlayerShotgun);
                    break;
                case 4:
                    GL.BindTexture(TextureTarget.Texture2D, this.texPlayerShotgun);
                    break;
            }
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
        internal void DrawEnemyWalk(Model model)
        {
            const uint spritesPerColumn = 2;
            const uint spritesPerRow = 4;
            foreach (Enemy enemy in model.Enemies)
            {

                GL.BindTexture(TextureTarget.Texture2D, texEnemyWalk);
                //NormalizedAnimationTime ist 0 am anfang der animation und nahe bei 1 am ende
                var spriteId = (uint)Math.Round(enemy.NormalizedAnimationTime * (spritesPerRow * spritesPerColumn - 1));        //Zahl zwischen 0 und 7
                var texCoords = SpriteSheetTools.CalcTexCoords(spriteId, spritesPerRow, spritesPerColumn);
                GL.Disable(EnableCap.Blend);
                GL.PushMatrix();
                GL.Translate(new Vector3(enemy.Position.X, enemy.Position.Y, 0));
                GL.Rotate(enemy.AngleToPlayer + 90, new Vector3d(0, 0, 1));
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(texCoords.MinX, texCoords.MinY);
                GL.Vertex2(new Vector2((-enemy.RadiusDraw), (-enemy.RadiusDraw)));
                GL.TexCoord2(texCoords.MaxX, texCoords.MinY);
                GL.Vertex2(new Vector2((enemy.RadiusDraw), (-enemy.RadiusDraw)));
                GL.TexCoord2(texCoords.MaxX, texCoords.MaxY);
                GL.Vertex2(new Vector2((enemy.RadiusDraw), (enemy.RadiusDraw)));
                GL.TexCoord2(texCoords.MinX, texCoords.MaxY);
                GL.Vertex2(new Vector2((-enemy.RadiusDraw), (enemy.RadiusDraw)));
                GL.End();
                GL.PopMatrix();
                GL.Enable(EnableCap.Blend);
                EnemyHealth(enemy);
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
        internal void DrawBullets(Model model)
        {
            foreach (Bullet bullet in model.Bullets)
            {
                Vector2 bulletOffset = new Vector2(0, -0.007f);
                if (model.weaponSelected == 4)
                {
                    GL.BindTexture(TextureTarget.Texture2D, this.texMissile);
                }
                else
                {
                    GL.BindTexture(TextureTarget.Texture2D, this.texBullet);
                }
                GL.PushMatrix();
                GL.Translate(new Vector3(bullet.Position.X, bullet.Position.Y, 0));
                GL.Rotate(bullet.Angle, new Vector3d(0, 0, -1));
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(new Vector2(0, 0));
                GL.Vertex2(bulletOffset + new Vector2(-bullet.RadiusDraw, -bullet.RadiusDraw));
                GL.TexCoord2(new Vector2(0, 1));
                GL.Vertex2(bulletOffset + new Vector2(bullet.RadiusDraw, -bullet.RadiusDraw));
                GL.TexCoord2(new Vector2(1, 1));
                GL.Vertex2(bulletOffset + new Vector2(bullet.RadiusDraw, bullet.RadiusDraw));
                GL.TexCoord2(new Vector2(1, 0));
                GL.Vertex2(bulletOffset + new Vector2(-bullet.RadiusDraw, bullet.RadiusDraw));
                GL.End();
                GL.PopMatrix();
            }
        }
        internal void DrawParticle(Model model)
        {
            foreach (Particle particle in model.Particles)
            {
                if (particle.Id == 0)
                {
                    GL.BindTexture(TextureTarget.Texture2D, this.texFragment);
                }
                else
                {
                    GL.BindTexture(TextureTarget.Texture2D, this.texBlood);
                }
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
            foreach (Particle paricleFrament in model.RPGFragments)
            {
                GL.BindTexture(TextureTarget.Texture2D, this.texFragment);
                GL.Begin(PrimitiveType.Quads);
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
        internal void DrawObstacles(Model model)
        {

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
        }
        internal void DrawPickups(Model model)
        {
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
                        GL.BindTexture(TextureTarget.Texture2D, this.texMissile);
                        break;
                }
                GL.Disable(EnableCap.Blend);
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
                var texCoords = SpriteSheetTools.CalcTexCoords(spriteId, charactersPerRow, charactersPerColumn);
                DrawRect(rect, texCoords);
                rect.MinX += rect.SizeX;
            }
        }

        private static void DrawRect(IReadOnlyRectangle rectangle, IReadOnlyRectangle texCoords)
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