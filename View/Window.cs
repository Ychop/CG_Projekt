using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;


namespace CG_Projekt
{
    internal class Window : GameWindow
    {
        private KeyboardState _keyboard;
        private Player player = new Player();
        private List<Enemy> Enemies = new List<Enemy>();
        private Random _random = new Random();
        public Window()
        {
            GL.ClearColor(Color.Beige);
            _keyboard = new KeyboardState(this);
            for (int i = 0; i < 5; i++)
            {
                Enemies.Add(new Enemy(new Vector2((float)_random.NextDouble()*2 -1, (float)_random.NextDouble() * 2 - 1)));
            }
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            Initialize();
            SwapBuffers();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Width, Height);

        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            if (_keyboard.IsKeyPressed(Key.W))
            {
                player._position = player._position + new Vector2(0, 0.01f);
                player.DrawPlayer(player._position);
            }
            if (_keyboard.IsKeyPressed(Key.A))
            {
                player._position = player._position - new Vector2(0.005f, 0);
                player.DrawPlayer(player._position);
            }
            if (_keyboard.IsKeyPressed(Key.S))
            {
                player._position = player._position - new Vector2(0, 0.01f);
                player.DrawPlayer(player._position);
            }
            if (_keyboard.IsKeyPressed(Key.D))
            {
                player._position = player._position + new Vector2(0.005f, 0);
                player.DrawPlayer(player._position);
            }
        }

        private void Initialize()
        {
            player.DrawPlayer(player._position);

         
            foreach(Enemy enemy in Enemies)
            {
                enemy.DrawEnemy(enemy._position);
            }
        }
    }

}

