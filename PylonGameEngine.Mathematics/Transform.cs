using System.Text;

namespace PylonGameEngine.Mathematics
{
    public class Transform
    {
        public Transform()
        {
            PositionChange += () => { };
            ScaleChange += () => { };
            RotationChange += () => { };
        }

        public Transform(Transform t, Transform newParent = null)
        {
            PositionChange += () => { };
            ScaleChange += () => { };
            RotationChange += () => { };

            _Position = (t.Position);
            _Rotation = (t.Rotation);
            _Scale = (t.Scale);
            if (newParent != null)
                SetParent(newParent);
            else
                SetParent(t.Parent);
        }

        public Transform(Vector3 Position, Quaternion Rotation, Vector3 Scale, Transform newParent = null)
        {
            PositionChange += () => { };
            ScaleChange += () => { };
            RotationChange += () => { };

            _Position = Position;
            _Rotation = Rotation;
            _Scale = Scale;

            if (newParent != null)
                SetParent(newParent);
        }

        public Transform ToGlobalTransform()
        {
            return new Transform(GlobalPosition, GlobalRotation, GlobalScale);
        }

        public delegate void OnPositionChange();
        public event OnPositionChange PositionChange;
        private Vector3 _Position = new Vector3(0, 0, 0);
        public Vector3 Position
        {
            get
            {
                return _Position;
            }
            set
            {
                _Position = value;
                PositionChange();
            }
        }
        public Vector3 GlobalPosition
        {
            get
            {
                return GlobalMatrix.TranslationVector;
            }
        }
        public void UnsafeSetPosition(Vector3 v)
        {
            _Position = v;
        }

        public delegate void OnScaleChange();
        public event OnPositionChange ScaleChange;
        public Vector3 _Scale = new Vector3(1, 1, 1);
        public Vector3 Scale
        {
            get
            {
                return _Scale;
            }
            set
            {
                _Scale = value;
                ScaleChange();
            }
        }
        public Vector3 GlobalScale
        {
            get
            {
                return GlobalMatrix.ScaleVector;
            }
        }
        public void UnsafeSetScale(Vector3 v)
        {
            _Scale = v;
        }

        public delegate void OnRotationChange();
        public event OnPositionChange RotationChange;
        public Quaternion _Rotation = new Quaternion(0, 0, 0, 1);
        public Quaternion Rotation
        {
            get
            {
                return _Rotation;
            }
            set
            {
                _Rotation = value;
                RotationChange();
            }
        }
        public Quaternion GlobalRotation
        {
            get
            {
                return Quaternion.FromRotationMatrix(GlobalMatrix);
            }
        }
        public void UnsafeSetRotation(Quaternion q)
        {
            _Rotation = q;
        }

        public Transform Parent { get; private set; }

        public void SetParent(Transform parenttransform)
        {
            Parent = parenttransform;
        }

        public Vector3 Forward
        {
            get
            {
                return Rotation * Vector3.Forward;
            }
        }

        public Vector3 Backward
        {
            get
            {
                return Rotation * Vector3.Backward;
            }
        }

        public Vector3 Left
        {
            get
            {
                return Rotation * Vector3.Left;
            }
        }

        public Vector3 Right
        {
            get
            {
                return Rotation * Vector3.Right;
            }
        }

        public Vector3 Up
        {
            get
            {
                return Rotation * Vector3.Up;
            }
        }

        public Vector3 Down
        {
            get
            {
                return Rotation * Vector3.Down;
            }
        }


        public Matrix4x4 ObjectMatrix
        {
            get
            {
                Matrix4x4 t = Matrix4x4.Translation(Position);
                Matrix4x4 r = Matrix4x4.RotationQuaternion(Rotation);
                Matrix4x4 s = Matrix4x4.Scaling(Scale);
                return s * r * t;
            }
        }

        public Matrix4x4 GlobalMatrix
        {
            get
            {

                if (Parent == null)
                {

                    return ObjectMatrix * Matrix4x4.Identity;
                }
                else
                {

                    return ObjectMatrix * Parent.GlobalMatrix;
                }
            }
        }

        public Vector3 WorldPosition
        {
            get
            {
                return GlobalMatrix.TranslationVector;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Position: " + Position.ToString());
            sb.AppendLine("Scale: " + Scale.ToString());
            sb.Append("Rotation: " + Rotation.ToString());

            return sb.ToString();
        }
    }
}
