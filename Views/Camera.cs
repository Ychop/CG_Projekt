using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace CG_Projekt
{
    class Camera
    {
        public Matrix4 CameraMatrix => cameraMatrix;
        private Matrix4 cameraMatrix = Matrix4.Identity;
        private float _scale = 0.2f; // Setzt den Start Zoom auf den spieler
        private float _invWindowAspectRatio = 1f;
        private Vector2 _center;
        internal Vector2 Center // Zentrum der Camera
        {
            get => _center;
            set
            {
                _center = value;
                UpdateMatrix();
            }
        }

        internal float Scale //ist der Zoom für die camera
        {
            get => _scale;
            set
            {
                _scale = Math.Max(0.001f, value); // avoid division by 0 and negative
                UpdateMatrix();
            }
        }
        public Matrix4 InvViewportMatrix { get; private set; } //ist fürs Clicken

        public Camera()
        {

        }
        public void Resize(int width, int height)
        {
            GL.Viewport(0, 0, width, height); // tell OpenGL to use the whole window for drawing
            _invWindowAspectRatio = height / (float)width;
            InvViewportMatrix = Transformation.Combine(Transformation.Scale(2f / width, 2f / height), Transformation.Translate(-Vector2.One));
            UpdateMatrix();
        }

        public void Draw()
        {
            GL.LoadMatrix(ref cameraMatrix);
        }

        private void UpdateMatrix()
        {
            var translate = Transformation.Translate(-Center);
            var scale = Transformation.Scale(1f / Scale);
            var aspect = Transformation.Scale(_invWindowAspectRatio, 1f);
            cameraMatrix = Transformation.Combine(translate, scale, aspect);
        }

    }
}