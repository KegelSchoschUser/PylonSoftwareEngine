using PylonGameEngine.Mathematics;
using Linearstar.Windows.RawInput.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PylonGameEngine.Input
{
    public static class Mouse
    {
        [DllImport("user32.dll")]
        static extern bool ClipCursor(ref RECT lpRect);
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out Point lpPoint);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int nIndex);

        public static bool IsTouchEnabled()
        {
            const int MAXTOUCHES_INDEX = 95;
            int maxTouches = GetSystemMetrics(MAXTOUCHES_INDEX);

            return maxTouches > 0;
        }

        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public Rectangle ToRectangle()
            {
                return new Rectangle(Left, Top, Right - Left, Bottom - Top);
            }
        }

        public static Vector2 GlobalPosition
        {
            get
            {
                GetCursorPos(out var P);
                return new Vector2(P.X, P.Y);
            }
        }

        public static Vector2 Position
        {
            get
            {
                GetCursorPos(out var P);
                return new Vector2(Mathf.Clamp(P.X - MyGame.MainWindow.Position.X, 0, MyGame.MainWindow.Size.X),
                                   Mathf.Clamp(P.Y - MyGame.MainWindow.Position.Y, 0, MyGame.MainWindow.Size.Y) );

            }
        }

        private static Vector2 DeltaBuffer = new Vector2();

        public static Vector2 Delta;
        public static float DeltaSpeed
        {
            get
            {
                return Mathf.Abs(Mouse.Delta.X) + Mathf.Abs(Mouse.Delta.Y);
            }
        }
        private static int ScrollDeltaBuffer = 0;
        public static int ScrollDelta;

        private static HashSet<RawMouseButtonFlags> DownButtons = new HashSet<RawMouseButtonFlags>();
        private static HashSet<RawMouseButtonFlags> PressedButtons = new HashSet<RawMouseButtonFlags>();
        private static HashSet<RawMouseButtonFlags> UpButtons = new HashSet<RawMouseButtonFlags>();

        private static HashSet<RawMouseButtonFlags> DownButtonsBuffer = new HashSet<RawMouseButtonFlags>();
        private static HashSet<RawMouseButtonFlags> UpButtonsBuffer = new HashSet<RawMouseButtonFlags>();
        public static bool CursorLockState { get; private set; }

        public static void LockCursor(Rectangle Rect)
        {
            CursorLockState = true;
            // ClipCursor(ref Rect);
            var rect = new Rectangle(Rect.X, Rect.Y, Rect.Width, Rect.Height);
            Cursor.Clip = rect;
        }

        public static void UnlockCursor()
        {
            CursorLockState = false;
            //RECT nothing = new RECT();
            //ClipCursor(ref nothing);
            Cursor.Clip = new Rectangle();
        }

        [DllImport("user32.dll")]
        static extern IntPtr LoadCursor(IntPtr hInstance, int lpCursorName);

        private static IntPtr CurrentCursor = IntPtr.Zero;
        public static void HideCursor()
        {
            Cursor.Hide();
        }

        public static void ShowCursor()
        {
            Cursor.Show();
        }

        public static void MouseButtonEvent(RawMouseButtonFlags Button, int Data)
        {
            switch (Button)
            {
                case RawMouseButtonFlags.LeftButtonDown:
                    {
                        PressedButtons.Add(RawMouseButtonFlags.LeftButtonDown);
                        DownButtonsBuffer.Add(RawMouseButtonFlags.LeftButtonDown);
                        break;
                    }
                case RawMouseButtonFlags.LeftButtonUp:
                    {
                        PressedButtons.Remove(RawMouseButtonFlags.LeftButtonDown);
                        UpButtonsBuffer.Add(RawMouseButtonFlags.LeftButtonUp);
                        break;
                    }

                case RawMouseButtonFlags.RightButtonDown:
                    {
                        PressedButtons.Add(RawMouseButtonFlags.RightButtonDown);
                        DownButtonsBuffer.Add(RawMouseButtonFlags.RightButtonDown);
                        break;
                    }
                case RawMouseButtonFlags.RightButtonUp:
                    {
                        PressedButtons.Remove(RawMouseButtonFlags.RightButtonDown);
                        UpButtonsBuffer.Add(RawMouseButtonFlags.RightButtonUp);
                        break;
                    }


                case RawMouseButtonFlags.MiddleButtonDown:
                    {
                       
                        PressedButtons.Add(RawMouseButtonFlags.MiddleButtonDown);
                        DownButtonsBuffer.Add(RawMouseButtonFlags.MiddleButtonDown);
                        break;
                    }
                case RawMouseButtonFlags.MiddleButtonUp:
                    {
                        PressedButtons.Remove(RawMouseButtonFlags.MiddleButtonDown);
                        UpButtonsBuffer.Add(RawMouseButtonFlags.MiddleButtonUp);
                        break;
                    }

            }
        }

        private static object LOCK = new object();
        public static void MouseDeltaEvent(int X, int Y)
        {
            lock (LOCK)
                DeltaBuffer += new Vector2(X, Y);

        }

        public static void MouseScrollEvent(int Y)
        {
            lock (LOCK)
                ScrollDeltaBuffer += Y;

        }

        

        public static bool LeftButtonDown()
        {
            return DownButtons.Contains(RawMouseButtonFlags.LeftButtonDown);
        }
        public static bool LeftButtonUp()
        {
            return UpButtons.Contains(RawMouseButtonFlags.LeftButtonUp);
        }
        public static bool LeftButtonPressed()
        {
            return PressedButtons.Contains(RawMouseButtonFlags.LeftButtonDown);
        }


        public static bool RightButtonDown()
        {
            return DownButtons.Contains(RawMouseButtonFlags.RightButtonDown);
        }
        public static bool RightButtonUp()
        {
            return UpButtons.Contains(RawMouseButtonFlags.RightButtonUp);
        }
        public static bool RightButtonPressed()
        {
            return PressedButtons.Contains(RawMouseButtonFlags.RightButtonDown);
        }


        public static bool MiddleButtonDown()
        {
            return DownButtons.Contains(RawMouseButtonFlags.MiddleButtonDown);
        }
        public static bool MiddleButtonUp()
        {
            return DownButtons.Contains(RawMouseButtonFlags.MiddleButtonUp);
        }
        public static bool MiddleButtonPressed()
        {
            return PressedButtons.Contains(RawMouseButtonFlags.MiddleButtonDown);
        }

        public static bool MouseMoving()
        {
            return Delta != 0f;
        }

        public static bool MouseDragging()
        {
            return MouseMoving() && LeftButtonPressed();
        }

        public static void Cycle()
        {
            DownButtons.Clear();
            DownButtons.UnionWith(DownButtonsBuffer);
            DownButtonsBuffer.Clear();

            UpButtons.Clear();
            UpButtons.UnionWith(UpButtonsBuffer);
            UpButtonsBuffer.Clear();
    
            lock (LOCK)
            {
                Delta = DeltaBuffer;
                DeltaBuffer = new Vector2();

                ScrollDelta = ScrollDeltaBuffer;
                ScrollDeltaBuffer = 0;     
            }
        }
    }
}
