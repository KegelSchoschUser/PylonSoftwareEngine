/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

using PylonSoftwareEngine.General;
using PylonSoftwareEngine.Mathematics;
using PylonSoftwareEngine.SceneManagement;
using PylonSoftwareEngine.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace PylonSoftwareEngine.UI.GUIObjects
{
    public enum PositionLayout
    {
        Pixel,
        UnitInterval,

        TopLeft, TopMiddle, TopRight,
        MiddleLeft, Center, MiddleRight,
        BottomLeft, BottomMiddle, BottomRight
    }
    public enum SizeLayout
    {
        Pixel,
        UnitInterval,
        UnitIntervalX,
        UnitIntervalY,
    }

    public enum RotationLayout
    {
        TopLeft, TopMiddle, TopRight,
        MiddleLeft, Center, MiddleRight,
        BottomLeft, BottomMiddle, BottomRight
    }

    public class GUIObject : UniqueNameInterface, ISoftwareObject
    {
        public float DeltaTime => MySoftware.RenderLoop.DeltaTime;
        public float FixedDeltaTime => MySoftware.SoftwareTickLoop.DeltaTime;

        public List<SoftwareScript> scripts;
        public Transform2D Transform = new Transform2D();
        public Drawing.Graphics Graphics;

        public PositionLayout PositionLayout = PositionLayout.Pixel;
        public SizeLayout SizeLayout = SizeLayout.Pixel;
        public RotationLayout RotationLayout = RotationLayout.TopLeft;
        public bool Visible = true;

        private GUIObject _Parent;

        public Scene SceneContext = null;
        public GUIObject Parent
        {
            get
            {
                return _Parent;
            }
            set
            {
                _Parent = value;
                if (_Parent != null)
                    Transform.SetParent(_Parent.Transform);
                else
                    Transform.SetParent(null);
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

            return MouseLocal >= 0f && MouseLocal <= Transform.Size;
        }

        public Vector2 MouseLocal
        {
            get
            {
                return SceneContext.InputManager.SceneContext.InputManager.Mouse.Position - GlobalMatrix.TranslationVector2D;
            }
        }

        public float AspectRatio
        {
            get
            {
                return Transform.Size.X / Transform.Size.Y;
            }
        }

        public virtual bool FocusAble { get; set; }


        public bool Focused => this == SceneContext.Gui.FocusedObject && FocusedLost == false;
        protected bool FocusedLost => this == SceneContext.Gui.FocusedLostObject;
        protected bool MouseHover => this == SceneContext.Gui.MouseHoverObject;
        protected bool MouseEnter => this == SceneContext.Gui.MouseEnterObject;
        protected bool MouseLeave => this == SceneContext.Gui.MouseLeaveObject;
        protected bool LeftMousePressed = false;
        protected bool LeftMouseClicked = false;
        protected bool LeftMouseUp = false;

        protected bool RightMousePressed = false;
        protected bool RightMouseClicked = false;
        protected bool RightMouseUp = false;

        protected bool MiddleMousePressed = false;
        protected bool MiddleMouseClicked = false;
        protected bool MiddleMouseUp = false;


        internal void UpdateTickInternal()
        {
            #region Left
            LeftMouseClicked = false;
            if (Focused && SceneContext.InputManager.Mouse.LeftButtonDown() && MouseInBounds())
            {
                LeftMouseClicked = true;
            }

            LeftMouseUp = false;
            if (Focused && SceneContext.InputManager.Mouse.LeftButtonUp() || MouseLeave && LeftMousePressed)
            {
                LeftMouseUp = true;
            }
            LeftMousePressed = false;
            if (Focused && SceneContext.InputManager.Mouse.LeftButtonPressed() && MouseInBounds())
            {
                LeftMousePressed = true;
            }
            #endregion Left

            #region Middle
            MiddleMouseClicked = false;
            if (Focused && SceneContext.InputManager.Mouse.MiddleButtonDown() && MouseInBounds())
            {
                MiddleMouseClicked = true;
            }

            MiddleMouseUp = false;
            if (Focused && SceneContext.InputManager.Mouse.MiddleButtonUp() || MouseLeave && LeftMousePressed)
            {
                MiddleMouseUp = true;
            }

            MiddleMousePressed = false;

            if (Focused && SceneContext.InputManager.Mouse.MiddleButtonPressed() && MouseInBounds())
            {
                MiddleMousePressed = true;
            }
            #endregion Middle

            #region Right
            RightMouseClicked = false;
            if (Focused && SceneContext.InputManager.SceneContext.InputManager.Mouse.RightButtonDown() && MouseInBounds())
            {
                RightMouseClicked = true;
            }

            RightMouseUp = false;
            if (Focused && SceneContext.InputManager.SceneContext.InputManager.Mouse.RightButtonUp() || MouseLeave && LeftMousePressed)
            {
                RightMouseUp = true;
            }

            RightMousePressed = false;
            if (Focused && SceneContext.InputManager.SceneContext.InputManager.Mouse.RightButtonPressed() && MouseInBounds())
            {
                RightMousePressed = true;
            }
            #endregion Right
        }

        public virtual void OnAddScene()
        {

        }

        internal void UpdateFrameInternal()
        {
            if (SceneContext != null)
                if (SceneContext.InputManager != null)
                    if (SceneContext.InputManager.Window != null)
                    {
                        if (SizeLayout == SizeLayout.UnitInterval)
                        {
                            Vector2 size;

                            if (Parent != null)
                            {
                                size = Parent.Transform.Size * Transform.Size;
                            }
                            else
                            {
                                size = SceneContext.InputManager.Window.Size;
                            }

                            if (size != Transform.Size)
                                Transform.Size = size;
                        }
                        else if (SizeLayout == SizeLayout.UnitIntervalX)
                        {
                            Vector2 size;

                            if (Parent != null)
                            {
                                size = Parent.Transform.Size * Transform.Size;
                            }
                            else
                            {
                                size = SceneContext.InputManager.Window.Size;
                            }

                            size.Y = Transform.Size.Y;
                            if (size != Transform.Size)
                                Transform.Size = size;
                        }
                        else if (SizeLayout == SizeLayout.UnitIntervalY)
                        {
                            Vector2 size;

                            if (Parent != null)
                            {
                                size = Parent.Transform.Size * Transform.Size;
                            }
                            else
                            {
                                size = SceneContext.InputManager.Window.Size;
                            }

                            size.X = Transform.Size.X;
                            if (size != Transform.Size)
                                Transform.Size = size;
                        }
                    }

            UpdateFrame();
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
                    ParentSize = SceneContext.MainCamera.RenderTarget.Size;
                else
                    ParentSize = Parent.Transform.Size;

                Vector2 PixelPosition = new Vector2(0, 0);
                Vector2 RotationPosition = new Vector2(0, 0);

                switch (PositionLayout)
                {
                    case PositionLayout.Pixel:
                        {
                            PixelPosition.X = Transform.Position.X;
                            PixelPosition.Y = Transform.Position.Y;
                            break;
                        }
                    case PositionLayout.UnitInterval:
                        {
                            PixelPosition.X = Transform.Position.X * ParentSize.X;
                            PixelPosition.Y = Transform.Position.Y * ParentSize.Y;
                            break;
                        }
                    case PositionLayout.TopLeft:
                        {
                            PixelPosition.X = 0f + Transform.Position.X;
                            PixelPosition.Y = 0f + Transform.Position.Y;
                            break;
                        }
                    case PositionLayout.TopMiddle:
                        {
                            PixelPosition.X = 0.5f * ParentSize.X - Transform.Size.X / 2f + Transform.Position.X;
                            PixelPosition.Y = 0f + Transform.Position.Y;
                            break;
                        }
                    case PositionLayout.TopRight:
                        {
                            PixelPosition.X = 1f * ParentSize.X - Transform.Size.X + Transform.Position.X;
                            PixelPosition.Y = 0f + Transform.Position.Y;
                            break;
                        }
                    case PositionLayout.MiddleLeft:
                        {
                            PixelPosition.X = 0f + Transform.Position.X;
                            PixelPosition.Y = 0.5f * ParentSize.Y - Transform.Size.Y / 2f + Transform.Position.Y;
                            break;
                        }
                    case PositionLayout.Center:
                        {
                            PixelPosition.X = 0.5f * ParentSize.X - Transform.Size.X / 2f + Transform.Position.X;
                            PixelPosition.Y = 0.5f * ParentSize.Y - Transform.Size.Y / 2f + Transform.Position.Y;
                            break;
                        }
                    case PositionLayout.MiddleRight:
                        {
                            PixelPosition.X = 1f * ParentSize.X - Transform.Size.X + Transform.Position.X;
                            PixelPosition.Y = 0.5f * ParentSize.Y - Transform.Size.Y / 2f + Transform.Position.Y;
                            break;
                        }
                    case PositionLayout.BottomLeft:
                        {
                            PixelPosition.X = 0f + Transform.Position.X;
                            PixelPosition.Y = 1f * ParentSize.Y - Transform.Size.Y + Transform.Position.Y;
                            break;
                        }
                    case PositionLayout.BottomMiddle:
                        {
                            PixelPosition.X = 0.5f * ParentSize.X - Transform.Size.X / 2f + Transform.Position.X;
                            PixelPosition.Y = 1f * ParentSize.Y - Transform.Size.Y + Transform.Position.Y;
                            break;
                        }
                    case PositionLayout.BottomRight:
                        {
                            PixelPosition.X = 1f * ParentSize.X - Transform.Size.X + Transform.Position.X;
                            PixelPosition.Y = 1f * ParentSize.Y - Transform.Size.Y + Transform.Position.Y;
                            break;
                        }
                }

                switch (RotationLayout)
                {
                    case RotationLayout.TopLeft:
                        {
                            RotationPosition.X = 0f;
                            RotationPosition.Y = 0f;
                            break;
                        }
                    case RotationLayout.TopMiddle:
                        {
                            RotationPosition.X = 0.5f * Transform.Size.X;
                            RotationPosition.Y = 0f;
                            break;
                        }
                    case RotationLayout.TopRight:
                        {
                            RotationPosition.X = 1f * Transform.Size.X;
                            RotationPosition.Y = 0f;
                            break;
                        }
                    case RotationLayout.MiddleLeft:
                        {
                            RotationPosition.X = 0f;
                            RotationPosition.Y = 0.5f * Transform.Size.Y;
                            break;
                        }
                    case RotationLayout.Center:
                        {
                            RotationPosition.X = 0.5f * Transform.Size.X;
                            RotationPosition.Y = 0.5f * Transform.Size.Y;
                            break;
                        }
                    case RotationLayout.MiddleRight:
                        {
                            RotationPosition.X = 1f * Transform.Size.X;
                            RotationPosition.Y = 0.5f * Transform.Size.Y;
                            break;
                        }
                    case RotationLayout.BottomLeft:
                        {
                            RotationPosition.X = 0f;
                            RotationPosition.Y = 1f * Transform.Size.Y;
                            break;
                        }
                    case RotationLayout.BottomMiddle:
                        {
                            RotationPosition.X = 0.5f * Transform.Size.X;
                            RotationPosition.Y = 1f * Transform.Size.Y;
                            break;
                        }
                    case RotationLayout.BottomRight:
                        {
                            RotationPosition.X = 1f * Transform.Size.X;
                            RotationPosition.Y = 1f * Transform.Size.Y;
                            break;
                        }
                }

                Matrix4x4 t = Matrix4x4.Translation(PixelPosition.X, PixelPosition.Y, 0);
                Matrix4x4 negativerotationOffset = Matrix4x4.Translation(-RotationPosition.X, -RotationPosition.Y, 0);
                Matrix4x4 positiverotationOffset = Matrix4x4.Translation(RotationPosition.X, RotationPosition.Y, 0);
                Matrix4x4 r = Matrix4x4.RotationQuaternion(Quaternion.FromEuler(0, 0, Transform.Rotation));
                return negativerotationOffset * r * positiverotationOffset * t;
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
            FocusAble = true;

            Graphics = new Drawing.Graphics(this);
            GraphicsInitialized(Graphics);
            //Transform.PositionChange += QueueDraw;
            //Transform.RotationChange += QueueDraw;
            Transform.SizeChange += () => { _RecreateGraphics = true; QueueDraw(); };
            Transform.Size = new Vector2(100, 100);
            Children = new List<GUIObject>();
            scripts = new List<SoftwareScript>();

            OnCreate();
        }

        protected void RecreateGraphics()
        {
            _RecreateGraphics = true;
        }

        internal bool _QueueDraw = false;
        internal bool _RecreateGraphics = false;
        public void QueueDraw()
        {
            _QueueDraw = true;
        }

        internal void OnDrawInternal()
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
                    //Graphics.Destroy();
                    //Graphics = new PylonSoftwareEngine.UI.Drawing.Graphics(this);
                    Graphics.RecreateTexture(Transform.Size.X, Transform.Size.Y);
                    GraphicsInitialized(Graphics);
                }

                Graphics.BeginDraw();

                BeforeClip(Graphics);
                var Clip = GetClip();

                Graphics.CreateClip(Clip.Item1, Clip.Item2, Clip.Item3, Clip.Item4);
                OnDraw(Graphics);


                if (DebugSettings.UISettings.DrawLayoutRectangle)
                    DrawLayoutRectangle(Graphics);

                Graphics.ApplyClip();

                Graphics.EndDraw();
            }
        }


        public virtual void BeforeClip(Drawing.Graphics g)
        {

        }

        public (float, float, float, float) GetClip()
        {
            Vector2 ParentSize = Parent != null ? Parent.Transform.Size : SceneContext.MainCamera.RenderTarget.Size;

            float ClipX = Mathf.Clamp(Transform.Position.X, 0f, ParentSize.X) - Transform.Position.X;
            float ClipY = Mathf.Clamp(Transform.Position.Y, 0f, ParentSize.Y) - Transform.Position.Y;

            float ClipX2 = Mathf.Clamp(Transform.Position.X + Transform.Size.X, 0f, ParentSize.X) - Transform.Position.X;
            float ClipY2 = Mathf.Clamp(Transform.Position.Y + Transform.Size.Y, 0f, ParentSize.Y) - Transform.Position.Y;

            return (ClipX, ClipY, ClipX2, ClipY2);
        }

        public virtual void GraphicsInitialized(Drawing.Graphics g)
        {

        }

        public virtual void OnDraw(Drawing.Graphics g)
        {
            DrawLayoutRectangle(g);
        }

        protected void DrawLayoutRectangle(Drawing.Graphics g)
        {
            var p = g.CreatePen(RGBColor.Red);
            p.Width = 3f;

            g.DrawRectangle(p, new RectangleF(0, 0, Transform.Size.X, Transform.Size.Y));
            g.DrawLine(p, new Vector2(0, 0), new Vector2(Transform.Size.X, Transform.Size.Y));
            g.DrawLine(p, new Vector2(Transform.Size.X, 0), new Vector2(0, Transform.Size.Y));
        }

        public void SetName(string name)
        {
            Name = name;
        }

        public virtual void OnCreate()
        {

        }

        public void AddScript(SoftwareScript script)
        {
            script.SceneContext = SceneContext;
            scripts.Add(script);
        }

        public void AddChild(GUIObject SoftwareObject)
        {
            if (SoftwareObject == this)
            {
                throw new ArgumentException("Cannot set myself as Child!", "SoftwareObject");
            }

            SoftwareObject.Parent = this;
            SoftwareObject.SceneContext = SceneContext;
            SoftwareObject.OnAddScene();
            SoftwareObject.QueueDraw();

            Children.Add(SoftwareObject);
        }

        public void RemoveChild(GUIObject SoftwareObject)
        {
            if (SoftwareObject == this)
            {
                throw new ArgumentException("Cannot delete myself!", "SoftwareObject");
            }

            SoftwareObject.Parent = null;
            SoftwareObject.SceneContext = SceneContext;
            SoftwareObject.QueueDraw();

            Children.Remove(SoftwareObject);
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
                SceneContext.Gui.Destroy(this);
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
            if (FocusAble && Visible)
            {
                if (SceneContext.InputManager.Mouse.LeftButtonDown() == true && ANDExtendedFocusCheck() || ORExtendedFocusCheck()/*|| SceneContext.InputManager.Mouse.LeftButtonPressed() == true*/)
                {
                    return true;
                }
            }

            return false;
        }

        protected virtual bool ORExtendedFocusCheck()
        {
            return true;
        }

        protected virtual bool ANDExtendedFocusCheck()
        {
            return true;
        }

        protected virtual bool ExtendedHoverCheck()
        {
            return true;
        }

        public List<GUIObject> GetChildrenRecursive()
        {
            if (Visible == false)
                return new List<GUIObject>();

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
            if (Visible == false)
                return (null, null);

            GUIObject objhover = null;
            GUIObject objFocus = null;
            if (MouseInBounds())
            {
                if (FocusAble && ExtendedHoverCheck())
                    objhover = this;
                if (OnFocusCheck())
                {
                    objFocus = this;
                }

                foreach (var item in Children)
                {
                    if (item.MouseInBounds())
                    {
                        if (item.FocusAble && item.ExtendedHoverCheck())
                            objhover = item;
                        if (item.OnFocusCheck())
                        {
                            objFocus = item;
                        }
                        item.CheckChildrenMouseBound();
                        break;
                    }
                }
            }


            return (objhover, objFocus);
        }

        public void ToFront()
        {
            if (Parent == null)
            {
                SceneContext.Gui.GUIObjects.Remove(this);
                SceneContext.Gui.GUIObjects.Add(this);
            }
            else
            {
                Parent.Children.Remove(this);
                Parent.Children.Add(this);
            }
        }

        public void ToBack()
        {
            if (Parent == null)
            {
                SceneContext.Gui.GUIObjects.Remove(this);
                SceneContext.Gui.GUIObjects.Insert(0, this);
            }
            else
            {
                Parent.Children.Remove(this);
                Parent.Children.Insert(0, this);
            }
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
