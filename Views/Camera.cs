﻿using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace CG_Projekt
{
    class Camera
    {

        public Matrix4 CameraMatrix => cameraMatrix;
        public Matrix4 InvViewportMatrix { get; private set; }
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

        public void Resize(int width_, int height_)
        {
            GL.Viewport(0, 0, width_, height_); // tell OpenGL to use the whole window for drawing
            _invWindowAspectRatio = height_ / (float)width_;
            InvViewportMatrix = Transformation.Combine(Transformation.Scale(2f / width_, 2f / height_), Transformation.Translate(-Vector2.One));
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