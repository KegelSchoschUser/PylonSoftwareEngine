using PylonGameEngine.Mathematics;
using PylonGameEngine.Render11;
using System;

namespace PylonGameEngine.GameWorld
{
    public class CameraObject : GameObject3D
    {
        public float FoV = 90f;
        public float OrthographicFoV = 1f;
        public float Near = 0.1f;
        public float Far = 10000.0f;
        public CameraRender CameraRender;
        public bool Enabled = true;


        public CameraObject(ref RenderTexture renderTarget, bool HasUI = true)
        {
            CameraRender = new CameraRender(this, ref renderTarget, HasUI);

            WorldManager.Add(this);
        }

        ~CameraObject()
        {
            WorldManager.Remove(this);
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
            float width = MyGame.MainWindow.Width;
            float height = MyGame.MainWindow.Height;
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
            float width = MyGame.MainWindow.Width;
            float height = MyGame.MainWindow.Height;
            Matrix4x4 worldViewProjection = ViewMatrix3D * ProjectionMatrix;

            v = Vector3.TransformCoordinate(vector, worldViewProjection);

            return new Vector2(((1.0f + v.X) * 0.5f * width) + x, ((1.0f - v.Y) * 0.5f * height) + y);
        }

        public bool PointInView(Vector3 vector)
        {
            Vector3 v = new Vector3();
            float x = 0;
            float y = 0;
            float width = MyGame.MainWindow.Width;
            float height = MyGame.MainWindow.Height;
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
            float width = MyGame.MainWindow.Width;
            float height = MyGame.MainWindow.Height;
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
                return Matrix4x4.PerspectiveFovLH(FoV * (float)System.Math.PI / 180f, (MyGame.MainWindow.Width / (float)MyGame.MainWindow.Height), Near, Far);
            }
        }


        public Matrix4x4 OrthographicMatrix
        {
            get
            {
                //return Matrix4x4.OrthoLH(GlobalManager.MainWindow.Width / OrthographicFoV, GlobalManager.MainWindow.Height / OrthographicFoV, 0, 1);;
                return Matrix4x4.OrthoOffCenterLH((-MyGame.MainWindow.Width) / 2f * OrthographicFoV, (MyGame.MainWindow.Width) / 2f, (-MyGame.MainWindow.Height) / 2f * OrthographicFoV, (MyGame.MainWindow.Height) / 2f, 0, 1);
            }
        }

        public void Activate()
        {
            MyGameWorld.ActiveCamera = this;
        }

        public void Deactivate()
        {
            if (MyGameWorld.ActiveCamera == this)
            {
                MyGameWorld.ActiveCamera = null;
            }
        }
    }
}
