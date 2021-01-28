namespace CG_Projekt
{
    using System;
    using OpenTK;
    using OpenTK.Graphics.OpenGL;

    internal class Camera
    {
        private Matrix4 cameraMatrix = Matrix4.Identity;
        private float scale = 0.1f; // Setzt den Start Zoom auf den spieler
        private float invWindowAspectRatio = 1f;
        private Vector2 center;

        internal Matrix4 CameraMatrix => this.cameraMatrix;

        internal Matrix4 InvViewportMatrix { get; private set; }

        internal Vector2 Center // Zentrum der Camera
        {
            get => this.center;
            set
            {
                this.center = value;
                this.UpdateMatrix();
            }
        }

        internal float Scale // ist der Zoom für die camera
        {
            get => this.scale;
            set
            {
                this.scale = Math.Max(0.001f, value); // avoid division by 0 and negative
                this.UpdateMatrix();
            }
        }

        public void Resize(int width_, int height_)
        {
            GL.Viewport(0, 0, width_, height_); // tell OpenGL to use the whole window for drawing
            this.invWindowAspectRatio = height_ / (float)width_;
            this.InvViewportMatrix = Transformation.Combine(Transformation.Scale(2f / width_, 2f / height_), Transformation.Translate(-Vector2.One));
            this.UpdateMatrix();
        }

        public void Draw()
        {
            GL.LoadMatrix(ref this.cameraMatrix);
        }

        private void UpdateMatrix()
        {
            var translate = Transformation.Translate(-this.Center);
            var scale = Transformation.Scale(1f / this.Scale);
            var aspect = Transformation.Scale(this.invWindowAspectRatio, 1f);
            this.cameraMatrix = Transformation.Combine(translate, scale, aspect);
        }
    }
}