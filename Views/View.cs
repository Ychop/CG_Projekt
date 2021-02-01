namespace CG_Projekt
{
    using CG_Projekt.Framework;
    using CG_Projekt.Models;
    using OpenTK;
    using OpenTK.Graphics.OpenGL;
    using GL = OpenTK.Graphics.OpenGL.GL;
    using System;

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
        private int texfont;
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
        private readonly Random random = new Random();
        public bool GameOver, GameStarted, TexturesLoaded = false;
        private int texture = 0;

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
            this.texfont = Texture.Load(Resource.LoadStream(content + "SilverFont.png"));
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

        internal void DrawObject(int tex_, Vector2 position_, float sizeX_, float sizeY_, float scale_, float offsetX_, float offsetY_)
        {
            //if no scale needed scale_ = 1
            //if no offset needed offsetXY = 0
            GL.BindTexture(TextureTarget.Texture2D, tex_);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(new Vector2(0, 0));
            GL.Vertex2(position_ + scale_ * new Vector2(-sizeX_ + offsetX_, -sizeY_ + offsetY_));
            GL.TexCoord2(new Vector2(1, 0));
            GL.Vertex2(position_ + scale_ * new Vector2(sizeX_ + offsetX_, -sizeY_ + offsetY_));
            GL.TexCoord2(new Vector2(1, 1));
            GL.Vertex2(position_ + scale_ * new Vector2(sizeX_ + offsetX_, sizeY_ + offsetY_));
            GL.TexCoord2(new Vector2(0, 1));
            GL.Vertex2(position_ + scale_ * new Vector2(-sizeX_ + offsetX_, sizeY_ + offsetY_));
            GL.End();
        }
        internal void DrawLevel(Model model)
        {
            //Draws the water Background
            DrawObject(texWater, new Vector2(0, 0), 1, 1, 1, 0, 0);
            //Draws the Gras
            foreach (LevelGrid levelGrid in model.LevelGrids)
            {
                DrawObject(this.texGrass, levelGrid.Position, levelGrid.Size, levelGrid.Size, 1, 0, 0);
            }
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
            //Draws player healthbar
            DrawObject(this.texHealthBackground, heathbarPosition, 0.3f, 0.03f, 2 * this.Camera.Scale, 0, 0);
            DrawObject(this.texHealth, heathbarPosition, 0.3f * model.Player.Hitpoints, 0.017f, 2 * this.Camera.Scale, 0, 0);
            //Draws the Score
            GL.BindTexture(TextureTarget.Texture2D, texfont);
            DrawFont($"Kills={model.Score:D}", heathbarPosition.X + 0.6f * Camera.Scale, heathbarPosition.Y - 0.03f * Camera.Scale, Camera.Scale * 0.08f);
            //Draws the selected weapon + the ammo
            switch (model.weaponSelected)
            {
                case 1:
                    texture = this.texPistol;
                    break;
                case 2:
                    texture = this.texUzi;
                    break;
                case 3:
                    texture = this.texShotgun;
                    break;
                case 4:
                    texture = this.texRPG;
                    break;
            }
            DrawObject(texture, HUDPosition, 0.1f, 0.05f, this.Camera.Scale, 0, 0);
            switch (model.weaponSelected)
            {
                case 1:
                    GL.BindTexture(TextureTarget.Texture2D, texfont);
                    DrawFont($"{model.Player.AmmoPistol:D}", HUDPosition.X - 0.35f * Camera.Scale, HUDPosition.Y - 0.012f * Camera.Scale, Camera.Scale * 0.08f);
                    break;
                case 2:
                    GL.BindTexture(TextureTarget.Texture2D, texfont);
                    DrawFont($"{model.Player.AmmoUZI:D}", HUDPosition.X - 0.35f * Camera.Scale, HUDPosition.Y - 0.012f * Camera.Scale, Camera.Scale * 0.08f);
                    break;
                case 3:
                    GL.BindTexture(TextureTarget.Texture2D, texfont);
                    DrawFont($"{model.Player.AmmoShotgun:D}", HUDPosition.X - 0.35f * Camera.Scale, HUDPosition.Y - 0.012f * Camera.Scale, Camera.Scale * 0.08f);
                    break;
                case 4:
                    GL.BindTexture(TextureTarget.Texture2D, texfont);
                    DrawFont($"{model.Player.AmmoRPG:D}", HUDPosition.X - 0.35f * Camera.Scale, HUDPosition.Y - 0.012f * Camera.Scale, Camera.Scale * 0.08f);
                    break;
            }
        }
        internal void PressAnyKeyToStart()
        {
            if (change < 0)
            {
                texture = this.texStart;
                change += 0.25f;
            }
            else
            {
                texture = this.texStartBlack;
                change = -1f;
            }
            DrawObject(texture, Camera.Center, 0.18f, 0.05f, 1, 0, 0);
        }
        internal void DrawGameOver(Model model)
        {
            GL.BindTexture(TextureTarget.Texture2D, texfont);
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
        internal void DrawPlayer(Model model)
        {
            GL.BindTexture(TextureTarget.Texture2D, this.texPlayerPistol);
            switch (model.weaponSelected)
            {
                case 1:
                    texture = this.texPlayerPistol;
                    break;
                case 2:
                    texture = this.texPlayerUzi;
                    break;
                case 3:
                    texture = this.texPlayerShotgun;
                    break;
                case 4:
                    texture = this.texPlayerShotgun;
                    break;
            }
            GL.PushMatrix();
            GL.Translate(new Vector3(model.Player.Position.X, model.Player.Position.Y, 0));
            GL.Rotate(model.Player.Angle + 180, new Vector3d(0, 0, -1));
            DrawObject(texture, new Vector2(0, 0), model.Player.RadiusDraw, model.Player.RadiusDraw, 1, 0, 0);
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
                //Draws enemy helathbar
                DrawObject(this.texHealthBackground, enemy.Position + new Vector2(0, enemy.RadiusDraw + 0.001f), enemy.RadiusDraw + 0.01f, enemy.RadiusDraw, 0.6f, 0, 0);
                DrawObject(this.texHealth, enemy.Position + new Vector2(0, enemy.RadiusDraw + 0.001f), (enemy.RadiusDraw + 0.01f) * enemy.Hitpoints, enemy.RadiusDraw, 0.6f, 0, 0);
            }
        }
        internal void DrawBullets(Model model)
        {
            foreach (Bullet bullet in model.Bullets)
            {
                Vector2 bulletOffset = new Vector2(0, -0.007f);
                if (model.weaponSelected == 4)
                {
                    texture = this.texMissile;
                }
                else
                {
                    texture = this.texBullet;
                }
                GL.PushMatrix();
                GL.Translate(new Vector3(bullet.Position.X, bullet.Position.Y, 0));
                GL.Rotate(bullet.Angle, new Vector3d(0, 0, -1));
                DrawObject(texture, bulletOffset, bullet.RadiusDraw, bullet.RadiusDraw, 1, 0, 0); ;
                GL.PopMatrix();
            }
        }
        internal void DrawParticle(Model model)
        {
            foreach (Particle particle in model.Particles)
            {
                if (particle.Id == 0)
                {
                    texture = this.texFragment;
                }
                else
                {
                    texture = this.texBlood;
                }
                DrawObject(texture, particle.Position, particle.RadiusDraw, particle.RadiusDraw, 1, 0, 0);
            }
            foreach (Particle paricleFrament in model.RPGFragments)
            {
                DrawObject(this.texFragment, paricleFrament.Position, paricleFrament.RadiusDraw, paricleFrament.RadiusDraw, 1, 0, 0);
            }
        }
        internal void DrawObstacles(Model model)
        {
            foreach (Obstacle obstacle in model.Obstacles)
            {
                DrawObject(this.texObstacle, obstacle.Position, obstacle.RadiusDraw, obstacle.RadiusDraw, 1, 0, 0);
            }
        }
        internal void DrawPickups(Model model)
        {
            foreach (PickUp pickup in model.PickUps)
            {
                switch (pickup.Type)
                {
                    case 0:
                        texture = this.texHeartCollectible;
                        break;
                    case 1:
                        texture = this.texAmmoPistol;
                        break;
                    case 2:
                        texture = this.texAmmoUzi;
                        break;
                    case 3:
                        texture = this.texAmmoShotgun;
                        break;
                    case 4:
                        texture = this.texMissile;
                        break;
                }
                GL.Disable(EnableCap.Blend);
                DrawObject(texture, pickup.Position, pickup.RadiusDraw, pickup.RadiusDraw, 1, 0, 0);
                GL.Enable(EnableCap.Blend);
            }
        }
        private void DrawFont(string text, float x, float y, float size)
        {
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
        internal static void DrawRect(IReadOnlyRectangle rectangle, IReadOnlyRectangle texCoords)
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