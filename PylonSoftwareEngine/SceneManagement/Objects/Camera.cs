/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

using PylonSoftwareEngine.Mathematics;
using PylonSoftwareEngine.Render11;
using System.Runtime.InteropServices;
using Vortice.Direct3D11;

namespace PylonSoftwareEngine.SceneManagement.Objects
{
    public class Camera : SoftwareObject3D
    {
        public float FoV = 90f;
        public float OrthographicFoV = 1f;
        public float Near = 0.1f;
        public float Far = 10000.0f;
        public bool Enabled = true;

        public RenderTexture RenderTarget;

        internal ID3D11Buffer CameraMatrixBuffer3D;
        internal ID3D11Buffer CameraMatrixBuffer2D;
        internal ID3D11Buffer CameraPositionBuffer;

        public Camera(RenderTexture renderTarget)
        {
            RenderTarget = renderTarget;
        }

        public override void OnDestroy()
        {
            SceneContext.Cameras.Remove(this);
        }

        public Matrix4x4 ViewMatrix3D
        {
            get
            {
                Matrix4x4 rotationMatrix = Transform.GlobalMatrix.RotationMatrix();

                Vector3 lookAt = Vector3.TransformCoordinate(new Vector3(0, 0, 1), rotationMatrix);

                Vector3 up = Vector3.TransformCoordinate(Vector3.Up, rotationMatrix);

                lookAt = Transform.GlobalMatrix.TranslationVector + lookAt;

                return Matrix4x4.LookAtLH(Transform.GlobalMatrix.TranslationVector, lookAt, up);

            }
        }

        public Matrix4x4 ViewMatrix2D
        {
            get
            {
                Matrix4x4 rotationMatrix = Matrix4x4.RotationYawPitchRoll(0f, 0f, 0f);

                Vector3 lookAt = Vector3.TransformCoordinate(new Vector3(0, 0, 1), rotationMatrix);

                Vector3 up = Vector3.TransformCoordinate(Vector3.Up, rotationMatrix);

                return Matrix4x4.LookAtLH(new Vector3(0, 0, 0), lookAt, up);
            }
        }

        public Vector3 WorldToScreenPoint(Vector3 vector)
        {
            Vector3 v = new Vector3();
            float x = 0;
            float y = 0;
            float width = (int)RenderTarget.Size.X;
            float height = (int)RenderTarget.Size.Y;
            float minZ = Near;
            float maxZ = Far;
            Matrix4x4 worldViewProjection = ViewMatrix3D * ProjectionMatrix;

            v = Vector3.TransformCoordinate(vector, worldViewProjection);

            return new Vector3(((1.0f + v.X) * 0.5f * width) + x, ((1.0f - v.Y) * 0.5f * height) + y, (v.Z * (maxZ - minZ)) + minZ);
        }

        public Vector2 WorldToScreenPoint2(Vector3 vector)
        {
            Vector3 v = new Vector3();
            float x = 0;
            float y = 0;
            float width = (int)RenderTarget.Size.X;
            float height = (int)RenderTarget.Size.Y;
            Matrix4x4 worldViewProjection = ViewMatrix3D * ProjectionMatrix;

            v = Vector3.TransformCoordinate(vector, worldViewProjection);

            return new Vector2(((1.0f + v.X) * 0.5f * width) + x, ((1.0f - v.Y) * 0.5f * height) + y);
        }

        public bool PointInView(Vector3 vector)
        {
            Vector3 v = new Vector3();
            float x = 0;
            float y = 0;
            float width = (int)RenderTarget.Size.X;
            float height = (int)RenderTarget.Size.Y;
            float minZ = Near;
            float maxZ = Far;
            Matrix4x4 worldViewProjection = ViewMatrix3D * ProjectionMatrix;

            var q = Vector3.TransformCoordinateQuaternion(vector, worldViewProjection);

            v = new Vector3(((1.0f + v.X) * 0.5f * width) + x, ((1.0f - v.Y) * 0.5f * height) + y, (v.Z * (maxZ - minZ)) + minZ);
            if (q.X < -1f || q.X > 1f)
                return false;
            if (q.Y < -1f || q.Y > 1f)
                return false;
            if (q.Z < -1f || q.Z > 1f)
                return false;

            return true;
        }

        public Vector2[] WorldToScreenPoint2(Vector3[] vectors)
        {
            var output = new Vector2[vectors.Length];
            for (int i = 0; i < vectors.Length; i++)
            {
                output[i] = WorldToScreenPoint2(vectors[i]);
            }
            return output;
        }

        public Vector3 ScreenToWorldPoint(Vector3 vector)
        {
            Quaternion v = new Quaternion();
            float x = 0;
            float y = 0;
            float width = (int)RenderTarget.Size.X;
            float height = (int)RenderTarget.Size.Y;
            float minZ = Near;
            float maxZ = Far;
            Matrix4x4 worldViewProjection = ViewMatrix3D * ProjectionMatrix;

            Matrix4x4.Invert(ref worldViewProjection, out var matrix);

            v.X = (((vector.X - x) / width) * 2.0f) - 1.0f;
            v.Y = -((((vector.Y - y) / height) * 2.0f) - 1.0f);
            v.Z = (vector.Z - minZ) / (maxZ - minZ);

            return Vector3.TransformCoordinate(new Vector3(v), matrix);
        }

        public Matrix4x4 ProjectionMatrix
        {
            get
            {
                return Matrix4x4.PerspectiveFovLH(FoV * (float)System.Math.PI / 180f, (RenderTarget.Size.X / (float)RenderTarget.Size.Y), Near, Far);
            }
        }

        public Matrix4x4 OrthographicMatrix
        {
            get
            {
                //return Matrix4x4.OrthoLH(GlobalManager.MainWindow.Width / OrthographicFoV, GlobalManager.MainWindow.Height / OrthographicFoV, 0, 1);;
                return Matrix4x4.OrthoOffCenterLH((-RenderTarget.Size.X) / 2f * OrthographicFoV, (RenderTarget.Size.X) / 2f, (-RenderTarget.Size.Y) / 2f * OrthographicFoV, (RenderTarget.Size.Y) / 2f, 0, 1);
            }
        }

        internal void UpdateBuffers()
        {
            if(CameraMatrixBuffer3D != null && CameraMatrixBuffer2D != null && CameraPositionBuffer != null)
            {
                CameraMatrixBuffer3D.Release();
                CameraMatrixBuffer2D.Release();
                CameraPositionBuffer.Release();
            }

            CameraMatrixBuffer3D = CreateCameraMatrixBuffer(this, true);
            CameraMatrixBuffer2D = CreateCameraMatrixBuffer(this, false);

            CameraPositionBuffer = D3D11GraphicsDevice.CreateStructBuffer(new CameraPositionBufferStructure() { CameraPosition = this.Transform.GlobalPosition });
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct MatrixBufferStructure
        {
            public Mathematics.Matrix4x4 ViewMatrix;
            public Mathematics.Matrix4x4 ProjectionMatrix;
        }


        [StructLayout(LayoutKind.Sequential)]
        internal struct CameraPositionBufferStructure
        {
            public Mathematics.Vector3 CameraPosition;
            public float padding;
        }

        private static ID3D11Buffer CreateCameraMatrixBuffer(Camera Camera, bool RenderMode3D)
        {
            MatrixBufferStructure Matrix = new MatrixBufferStructure();

            if (RenderMode3D)
            {
                var Viewmatrix = Camera.ViewMatrix3D;
                var ProjectionMatrix = Camera.ProjectionMatrix;
                Viewmatrix.Transpose();
                ProjectionMatrix.Transpose();
                Matrix.ViewMatrix = Viewmatrix;
                Matrix.ProjectionMatrix = ProjectionMatrix;
            }
            else
            {
                var Viewmatrix = Camera.ViewMatrix2D;
                var ProjectionMatrix = Camera.OrthographicMatrix;
                Viewmatrix.Transpose();
                ProjectionMatrix.Transpose();
                Matrix.ViewMatrix = Viewmatrix;
                Matrix.ProjectionMatrix = ProjectionMatrix;
            }

            return D3D11GraphicsDevice.CreateStructBuffer(Matrix);
        }
    }
}
