using System.Text;

namespace PylonSoftwareEngine.Mathematics
{
    public class Transform2D
    {
        public Transform2D()
        {
            PositionChange += () => { };
            SizeChange += () => { };
            RotationChange += () => { };
        }

        public Transform2D(Vector2 Position, float Rotation, Vector2 Size, Transform2D parent = null)
        {
            PositionChange += () => { };
            SizeChange += () => { };
            RotationChange += () => { };

            _Position = Position;
            _Rotation = Rotation;
            _Size = Size;

            if (parent != null)
                SetParent(parent);
        }


        #region Position
        public delegate void OnPositionChange();
        public event OnPositionChange PositionChange;

        private Vector2 _Position = new Vector2(0, 0);
        public Vector2 Position
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

        public void UnsafeSetPosition(Vector2 v)
        {
            _Position = v;
        }
        #endregion Position

        #region Size
        public delegate void OnSizeChange();
        public event OnPositionChange SizeChange;
        public Vector2 _Size = new Vector2(1, 1);
        public Vector2 Size
        {
            get
            {
                return _Size;
            }
            set
            {
                _Size = value;
                SizeChange();
            }
        }

        public void UnsafeSetSize(Vector2 v)
        {
            _Size = v;
        }
        #endregion Size

        #region Rotation
        public delegate void OnRotationChange();
        public event OnPositionChange RotationChange;
        public float _Rotation = 0f;
        public float Rotation
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

        public void UnsafeSetRotation(float q)
        {
            _Rotation = q;
        }

        #endregion Rotation

        public Transform2D Parent { get; private set; }

        public void SetParent(Transform2D parentTransform2D)
        {
            Parent = parentTransform2D;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Position: " + Position.ToString());
            sb.AppendLine("Size: " + Size.ToString());
            sb.Append("Rotation: " + Rotation.ToString());

            return sb.ToString();
        }
    }
}
