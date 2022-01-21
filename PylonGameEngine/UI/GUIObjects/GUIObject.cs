using PylonGameEngine.General;
using PylonGameEngine.Input;
using PylonGameEngine.Mathematics;
using PylonGameEngine.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace PylonGameEngine.GameWorld
{
    public enum PositionLayout
    {
        Pixel,
        UnitInterval,

        TopLeft, TopMiddle, TopRight,
        MiddleLeft, Center, MiddleRight,
        BottomLeft, BottomMiddle, BottomRight
    }

    public enum RotationLayout
    {
        TopLeft, TopMiddle, TopRight,
        MiddleLeft, Center, MiddleRight,
        BottomLeft, BottomMiddle, BottomRight
    }

    public class GUIObject : UniqueNameInterface, IGameObject
    {
        public List<GameScript> scripts;
        public Transform2D Transform = new Transform2D();
        public PylonGameEngine.UI.Drawing.Graphics Graphics;

        public PositionLayout PositionLayout = PositionLayout.Pixel;
        public RotationLayout RotationLayout = RotationLayout.TopLeft;

        private GUIObject _Parent;
        public GUIObject Parent
        {
            get
            {
                return _Parent;
            }
            set
            {
                _Parent = value;
                Transform.SetParent(_Parent.Transform);
                QueueDraw();
            }
        }

        private List<GUIObject> _Children;
        public List<GUIObject> Children
        {
            get
            {
                return _Children;
            }
            set
            {
                _Children = value;
                QueueDraw();
            }
        }



        public bool MouseInBounds()
        {
            return (MouseLocal >= 0f && MouseLocal <= Transform.Size);
        }

        public Vector2 MouseLocal
        {
            get
            {
                return (Mouse.Position - GlobalMatrix.TranslationVector2D);
            }
        }

        public float AspectRatio
        {
            get
            {
                return (float)Transform.Size.X / (float)Transform.Size.Y;
            }
        }

        private bool _FocusAble = true;
        public bool FocusAble
        {
            get
            {
                return _FocusAble;
            }

            protected set
            {
                _FocusAble = value;
            }
        }

        protected bool Focused => this == MyGameWorld.GUI.FocusedObject;
        protected bool FocusedLost => this == MyGameWorld.GUI.FocusedLostObject;
        protected bool MouseHover => this == MyGameWorld.GUI.MouseHoverObject;
        protected bool MouseEnter => this == MyGameWorld.GUI.MouseEnterObject;
        protected bool MouseLeave => this == MyGameWorld.GUI.MouseLeaveObject;
        protected bool LeftMousePressed = false;
        protected bool LeftMouseClicked = false;

        protected bool RightMousePressed = false;
        protected bool RightMouseClicked = false;

        protected bool MiddleMousePressed = false;
        protected bool MiddleMouseClicked = false;


        internal void UpdateTickInternal()
        {
            #region Left
            LeftMouseClicked = false;
            if (this.Focused && Mouse.LeftButtonDown())
            {
                LeftMouseClicked = true;
            }

            LeftMousePressed = false;
            if (this.Focused && Mouse.LeftButtonPressed())
            {
                LeftMousePressed = true;
            }
            #endregion Left

            #region Middle
            MiddleMouseClicked = false;
            if (this.Focused && Mouse.LeftButtonDown())
            {
                MiddleMouseClicked = true;
            }

            MiddleMousePressed = false;
            if (this.Focused && Mouse.LeftButtonPressed())
            {
                MiddleMousePressed = true;
            }
            #endregion Middle

            #region Right
            RightMouseClicked = false;
            if (this.Focused && Mouse.LeftButtonDown())
            {
                RightMouseClicked = true;
            }

            RightMousePressed = false;
            if (this.Focused && Mouse.LeftButtonPressed())
            {
                RightMousePressed = true;
            }
            #endregion Right
        }

        public virtual void UpdateFrame()
        {

        }

        public virtual void UpdateTick()
        {

        }
        public Matrix4x4 ObjectMatrix
        {
            get
            {
                Vector2 ParentSize;

                if (Parent == null)
                    ParentSize = new Vector2(MyGame.MainWindow.Width, MyGame.MainWindow.Height);
                else
                    ParentSize = Parent.Transform.Size;

                Vector2 PixelPosition = new Vector2(0, 0);
                Vector2 RotationPosition = new Vector2(0, 0);

                switch (PositionLayout)
                {
                    case (PositionLayout.Pixel):
                        {
                            PixelPosition.X = Transform.Position.X;
                            PixelPosition.Y = Transform.Position.Y;
                            break;
                        }
                    case (PositionLayout.UnitInterval):
                        {
                            PixelPosition.X = Transform.Position.X * ParentSize.X;
                            PixelPosition.Y = Transform.Position.Y * ParentSize.Y;
                            break;
                        }
                    case (PositionLayout.TopLeft):
                        {
                            PixelPosition.X = 0f + Transform.Position.X;
                            PixelPosition.Y = 0f + Transform.Position.Y;
                            break;
                        }
                    case (PositionLayout.TopMiddle):
                        {
                            PixelPosition.X = (0.5f * ParentSize.X - Transform.Size.X / 2f) + Transform.Position.X;
                            PixelPosition.Y = 0f + Transform.Position.Y;
                            break;
                        }
                    case (PositionLayout.TopRight):
                        {
                            PixelPosition.X = (1f * ParentSize.X - Transform.Size.X) + Transform.Position.X;
                            PixelPosition.Y = 0f + Transform.Position.Y;
                            break;
                        }
                    case (PositionLayout.MiddleLeft):
                        {
                            PixelPosition.X = 0f + Transform.Position.X;
                            PixelPosition.Y = (0.5f * ParentSize.Y - Transform.Size.Y / 2f) + Transform.Position.Y;
                            break;
                        }
                    case (PositionLayout.Center):
                        {
                            PixelPosition.X = (0.5f * ParentSize.X - Transform.Size.X / 2f) + Transform.Position.X;
                            PixelPosition.Y = (0.5f * ParentSize.Y - Transform.Size.Y / 2f) + Transform.Position.Y;
                            break;
                        }
                    case (PositionLayout.MiddleRight):
                        {
                            PixelPosition.X = (1f * ParentSize.X - Transform.Size.X) + Transform.Position.X;
                            PixelPosition.Y = (0.5f * ParentSize.Y - Transform.Size.Y / 2f) + Transform.Position.Y;
                            break;
                        }
                    case (PositionLayout.BottomLeft):
                        {
                            PixelPosition.X = 0f + Transform.Position.X;
                            PixelPosition.Y = (1f * ParentSize.Y - Transform.Size.Y) + Transform.Position.Y;
                            break;
                        }
                    case (PositionLayout.BottomMiddle):
                        {
                            PixelPosition.X = (0.5f * ParentSize.X - Transform.Size.X / 2f) + Transform.Position.X;
                            PixelPosition.Y = (1f * ParentSize.Y - Transform.Size.Y) + Transform.Position.Y;
                            break;
                        }
                    case (PositionLayout.BottomRight):
                        {
                            PixelPosition.X = (1f * ParentSize.X - Transform.Size.X) + Transform.Position.X;
                            PixelPosition.Y = (1f * ParentSize.Y - Transform.Size.Y) + Transform.Position.Y;
                            break;
                        }
                }

                switch (RotationLayout)
                {
                    case (RotationLayout.TopLeft):
                        {
                            RotationPosition.X = 0f;
                            RotationPosition.Y = 0f;
                            break;
                        }
                    case (RotationLayout.TopMiddle):
                        {
                            RotationPosition.X = (0.5f * Transform.Size.X);
                            RotationPosition.Y = 0f ;
                            break;
                        }
                    case (RotationLayout.TopRight):
                        {
                            RotationPosition.X = (1f * Transform.Size.X);
                            RotationPosition.Y = 0f;
                            break;
                        }
                    case (RotationLayout.MiddleLeft):
                        {
                            RotationPosition.X = 0f;
                            RotationPosition.Y = (0.5f * Transform.Size.Y);
                            break;
                        }
                    case (RotationLayout.Center):
                        {
                            RotationPosition.X = (0.5f *Transform.Size.X);
                            RotationPosition.Y = (0.5f *Transform.Size.Y);
                            break;
                        }
                    case (RotationLayout.MiddleRight):
                        {
                            RotationPosition.X = (1f   * Transform.Size.X);
                            RotationPosition.Y = (0.5f * Transform.Size.Y);
                            break;
                        }
                    case (RotationLayout.BottomLeft):
                        {
                            RotationPosition.X = 0f;
                            RotationPosition.Y = (1f * Transform.Size.Y);
                            break;
                        }
                    case (RotationLayout.BottomMiddle):
                        {
                            RotationPosition.X = (0.5f * Transform.Size.X);
                            RotationPosition.Y = (1f *   Transform.Size.Y);
                            break;
                        }
                    case (RotationLayout.BottomRight):
                        {
                            RotationPosition.X = (1f * Transform.Size.X);
                            RotationPosition.Y = (1f * Transform.Size.Y);
                            break;
                        }
                }

                Matrix4x4 t = Matrix4x4.Translation(PixelPosition.X, PixelPosition.Y , 0);
                Matrix4x4 negativerotationOffset = Matrix4x4.Translation(-RotationPosition.X, -RotationPosition.Y, 0);
                Matrix4x4 positiverotationOffset = Matrix4x4.Translation(RotationPosition.X, RotationPosition.Y, 0);
                Matrix4x4 r = Matrix4x4.RotationQuaternion(Quaternion.FromEuler(0, 0, Transform.Rotation));
                return ((negativerotationOffset * r) * positiverotationOffset) * t;
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


        public GUIObject()
        {
            Graphics = new PylonGameEngine.UI.Drawing.Graphics(this);
            GraphicsInitialized(Graphics);
            //Transform.PositionChange += QueueDraw;
            //Transform.RotationChange += QueueDraw;
            Transform.SizeChange += () => { _RecreateGraphics = true; QueueDraw(); };
            Transform.Size = new Vector2(100, 100);
            Children = new List<GUIObject>();
            scripts = new List<GameScript>();

            OnCreate();
        }

        internal bool _QueueDraw = false;
        internal bool _RecreateGraphics = false;
        public void QueueDraw()
        {
            _QueueDraw = true;
        }

        public void OnDrawInternal()
        {
            if (_QueueDraw == true)
            {
                _QueueDraw = false;

                if (Transform.Size.X < 1)
                    Transform.Size = new Vector2(1, Transform.Size.Y);
                if (Transform.Size.Y < 1)
                    Transform.Size = new Vector2(Transform.Size.X, 1);

                if (_RecreateGraphics)
                {
                    _RecreateGraphics = false;
                    Graphics = new PylonGameEngine.UI.Drawing.Graphics(this);
                    GraphicsInitialized(Graphics);
                }

                Graphics.BeginDraw();

                var Clip = GetClip();
                Graphics.CreateClip(Clip.Item1, Clip.Item2, Clip.Item3, Clip.Item4);

                OnDraw(Graphics);

                Graphics.ApplyClip();

                if (DebugSettings.UISettings.DrawLayoutRectangle)
                    DrawLayoutRectangle(Graphics);

                Graphics.EndDraw();
            }
        }

        public (float, float, float, float) GetClip()
        {
            Vector2 ParentSize = Parent != null ? Parent.Transform.Size : MyGameWorld.WindowRenderTarget.Size;

            float ClipX = Mathf.Clamp(Transform.Position.X, 0f, ParentSize.X) - Transform.Position.X;
            float ClipY = Mathf.Clamp(Transform.Position.Y, 0f, ParentSize.Y) - Transform.Position.Y;

            float ClipX2 = Mathf.Clamp((Transform.Position.X + Transform.Size.X), 0f, ParentSize.X) - Transform.Position.X;
            float ClipY2 = Mathf.Clamp((Transform.Position.Y + Transform.Size.Y), 0f, ParentSize.Y) - Transform.Position.Y;

            return (ClipX, ClipY, ClipX2, ClipY2);
        }

        public virtual void GraphicsInitialized(PylonGameEngine.UI.Drawing.Graphics g)
        {

        }

        public virtual void OnDraw(PylonGameEngine.UI.Drawing.Graphics g)
        {
            DrawLayoutRectangle(g);
        }

        protected void DrawLayoutRectangle(PylonGameEngine.UI.Drawing.Graphics g)
        {
            var p = g.CreatePen(RGBColor.Red);
            p.Width = 3f;

            g.DrawRectangle(p, new RectangleF(0, 0, this.Transform.Size.X, this.Transform.Size.Y));
            g.DrawLine(p, new Vector2(0, 0), new Vector2(this.Transform.Size.X, this.Transform.Size.Y));
            g.DrawLine(p, new Vector2(this.Transform.Size.X, 0), new Vector2(0, this.Transform.Size.Y));
        }

        public void SetName(string name)
        {
            Name = name;
        }

        public virtual void OnCreate()
        {

        }

        public void AddScript(GameScript script)
        {
            scripts.Add(script);
        }

        public void AddChild(GUIObject gameObject)
        {
            if (gameObject == this)
            {
                throw new ArgumentException("Cannot set myself as Child!", "gameObject");
            }

            gameObject.Parent = this;
            Children.Add(gameObject);
        }

        public void Destroy()
        {
            if (Parent != null)
            {
                Parent.Children.Remove(this);
                foreach (GUIObject child in Children)
                {
                    child.Parent = Parent;
                    Parent.Children.Add(child);
                }

            }
            else
            {
                MyGameWorld.GUI.Remove(this);
                foreach (GUIObject child in Children)
                {
                    child.Parent = null;
                }
            }


            OnDestroy();
        }

        public bool ContainsChild(GUIObject child)
        {
            return Children.Contains(child);
        }

        internal bool OnFocusCheck()
        {
            if (FocusAble)
            {
                if (Mouse.LeftButtonPressed())
                {
                    return true;
                }
            }

            return false;
        }

        public List<GUIObject> GetChildrenRecursive()
        {
            var objects = new List<GUIObject>();
            foreach (var item in Children)
            {
                objects.Add(item);
                objects.AddRange(item.GetChildrenRecursive());
            }
            return objects;
        }

        internal (GUIObject, GUIObject) CheckChildrenMouseBound()
        {
            GUIObject objhover = null;
            GUIObject objFocus = null;
            if (this.MouseInBounds())
            {

                objhover = this;
                if (OnFocusCheck())
                {
                    objFocus = this;
                }
            }
            foreach (var item in Children)
            {
                if (item.MouseInBounds())
                {
                    objhover = item;
                    if (OnFocusCheck())
                    {
                        objFocus = item;
                    }
                    item.CheckChildrenMouseBound();
                    break;
                }
            }

            return (objhover, objFocus);
        }


        public virtual void OnDestroy()
        {

        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Position: " + Transform.Position.ToString());
            sb.AppendLine("Position: " + Transform.Position.ToString());
            sb.Append("Rotation: " + Transform.Rotation.ToString());
            return sb.ToString();
        }
    }
}
