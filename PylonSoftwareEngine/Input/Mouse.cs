/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

using PylonSoftwareEngine.Mathematics;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PylonSoftwareEngine.Input
{
    public class Mouse
    {
        public enum MouseButton
        {
            LeftButton,
            RightButton,
            MiddleButton,
            Button4,
            Button5,
        }
        internal InputManager InputManager { get; private set; }

        private object LOCK = new object();

        public Vector2 Delta = Vector2.Zero;
        public float DeltaSpeed
        {
            get
            {
                return Mathf.Abs(Delta.X) + Mathf.Abs(Delta.Y);
            }
        }

        private int ScrollDeltaBuffer = 0;
        public int ScrollDelta { get; private set; }



        private Vector2 MousePostionBuffer = Vector2.Zero;
        private Vector2 LastMousePosition = Vector2.Zero;
        public Vector2 Position { get; private set; }
        public Vector2 GlobalPosition => Cursor.Position;

        private HashSet<MouseButton> DownButtons = new HashSet<MouseButton>();
        private HashSet<MouseButton> PressedButtons = new HashSet<MouseButton>();
        private HashSet<MouseButton> UpButtons = new HashSet<MouseButton>();

        private HashSet<MouseButton> DownButtonsBuffer = new HashSet<MouseButton>();
        private HashSet<MouseButton> UpButtonsBuffer = new HashSet<MouseButton>();
        public Mouse(InputManager manager)
        {
            InputManager = manager;
        }

        public void MouseButtonEvent(MouseButton Button, bool down)
        {
            lock (LOCK)
            {
                if (down)
                {
                    PressedButtons.Add(Button);
                    DownButtonsBuffer.Add(Button);

                }
                else
                {
                    PressedButtons.Remove(Button);
                    UpButtonsBuffer.Add(Button);
                }
            }
        }

        internal void MouseMoveEvent(int X, int Y)
        {
            lock (LOCK)
            {
                if(InputManager.Window != null)
                {
                    if(InputManager.Window.FormBorderStyle == FormBorderStyle.None)
                        MousePostionBuffer = new Vector2(X, Y);
                    else
                        MousePostionBuffer = new Vector2(X, Y - SystemInformation.CaptionHeight);
                }
                //MousePostionBuffer = new Vector2(X, Y);
            }
                

        }
        internal void MouseScrollEvent(int Y)
        {
            lock (LOCK)
                ScrollDeltaBuffer += Y;

        }

        public static bool CursorLocked => LockedRectange.IsEmpty == false;
        private static Rectangle LockedRectange;
        public static void LockMouse(Rectangle rect)
        {
            LockedRectange = rect;
            Cursor.Clip = rect;
            Cursor.Hide(); //TODO: BUG NOT WORKING
        }

        public static void UnlockMouse()
        {
            LockedRectange = new Rectangle();
            Cursor.Clip = new Rectangle();
            Cursor.Show();//TODO: BUG NOT WORKING
        }

        #region CheckButtons

        public bool LeftButtonDown()
        {
            return DownButtons.Contains(MouseButton.LeftButton);
        }
        public bool LeftButtonUp()
        {
            return UpButtons.Contains(MouseButton.LeftButton);
        }
        public bool LeftButtonPressed()
        {
            return PressedButtons.Contains(MouseButton.LeftButton);
        }


        public bool RightButtonDown()
        {
            return DownButtons.Contains(MouseButton.RightButton);
        }
        public bool RightButtonUp()
        {
            return UpButtons.Contains(MouseButton.RightButton);
        }
        public bool RightButtonPressed()
        {
            return PressedButtons.Contains(MouseButton.RightButton);
        }


        public bool MiddleButtonDown()
        {
            return DownButtons.Contains(MouseButton.MiddleButton);
        }
        public bool MiddleButtonUp()
        {
            return DownButtons.Contains(MouseButton.MiddleButton);
        }
        public bool MiddleButtonPressed()
        {
            return PressedButtons.Contains(MouseButton.MiddleButton);
        }

        public bool MouseMoving()
        {
            return Mathf.Abs(DeltaSpeed) >= 0.00001f;
        }

        public bool MouseDragging()
        {
            return MouseMoving() && LeftButtonPressed();
        }

        #endregion CheckButtons


        public void Update()
        {
            lock (LOCK)
            {
                DownButtons.Clear();
                DownButtons.UnionWith(DownButtonsBuffer);
                DownButtonsBuffer.Clear();

                UpButtons.Clear();
                UpButtons.UnionWith(UpButtonsBuffer);
                UpButtonsBuffer.Clear();

                if(InputManager.Window.IsDisposed == false)
                if (LockedRectange.IsEmpty == false && InputManager.Window.GlobalRectangle == LockedRectange)
                {
                    var Pos = LockedRectange.Location + new Size(LockedRectange.Size.Width / 2, LockedRectange.Size.Height / 2);
                    Position = Cursor.Position;
                    Delta = Position - Pos;
                    System.Windows.Forms.Cursor.Position = Pos;
                }
                else
                {
                    Position = MousePostionBuffer;
                    //Delta broken (= 0) when mouse isn't locked
                    Delta = Position - LastMousePosition;
                }
                LastMousePosition = Position;

                ScrollDelta = ScrollDeltaBuffer;
                ScrollDeltaBuffer = 0;
            }
        }
    }
}
