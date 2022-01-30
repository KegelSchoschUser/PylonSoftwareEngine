using PylonGameEngine.Mathematics;
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
        public static extern bool GetCursorPos(out Point lpPoint);

        public enum MouseButton
        {
            LeftButton,
            RightButton,
            MiddleButton,
            Button4,
            Button5,
        }

        private static object LOCK = new object();

        public static Vector2 Delta = Vector2.Zero;
        public static float DeltaSpeed
        {
            get
            {
                return Mathf.Abs(Mouse.Delta.X) + Mathf.Abs(Mouse.Delta.Y);
            }
        }
        private static Vector2 LastGlobalMouse = Vector2.Zero;
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
                if (MyGame.MainWindow.IsDisposed == false)
                    return (Vector2)MyGame.MainWindow.PointToClient(GlobalPosition) - new Vector2(0, SystemInformation.CaptionHeight);
                else
                    return Vector2.Zero;
            }
        }

        internal static int ScrollDeltaBuffer = 0;
        public static int ScrollDelta { get;private set; }

        private static HashSet<MouseButton> DownButtons = new HashSet<MouseButton>();
        private static HashSet<MouseButton> PressedButtons = new HashSet<MouseButton>();
        private static HashSet<MouseButton> UpButtons = new HashSet<MouseButton>();

        private static HashSet<MouseButton> DownButtonsBuffer = new HashSet<MouseButton>();
        private static HashSet<MouseButton> UpButtonsBuffer = new HashSet<MouseButton>();

        public static void MouseButtonEvent(MouseButton Button, bool down)
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

        public static void MouseScrollEvent(int Y)
        {
            lock (LOCK)
                ScrollDeltaBuffer += Y;

        }      

        public static bool LeftButtonDown()
        {
            return DownButtons.Contains(MouseButton.LeftButton);
        }
        public static bool LeftButtonUp()
        {
            return UpButtons.Contains(MouseButton.LeftButton);
        }
        public static bool LeftButtonPressed()
        {
            return PressedButtons.Contains(MouseButton.LeftButton);
        }


        public static bool RightButtonDown()
        {
            return DownButtons.Contains(MouseButton.RightButton);
        }
        public static bool RightButtonUp()
        {
            return UpButtons.Contains(MouseButton.RightButton);
        }
        public static bool RightButtonPressed()
        {
            return PressedButtons.Contains(MouseButton.RightButton);
        }


        public static bool MiddleButtonDown()
        {
            return DownButtons.Contains(MouseButton.MiddleButton);
        }
        public static bool MiddleButtonUp()
        {
            return DownButtons.Contains(MouseButton.MiddleButton);
        }
        public static bool MiddleButtonPressed()
        {
            return PressedButtons.Contains(MouseButton.MiddleButton);
        }

        public static bool MouseMoving()
        {
            return Mathf.Abs(DeltaSpeed) >= 0.00001f;
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
                Delta = GlobalPosition - LastGlobalMouse;
                LastGlobalMouse = GlobalPosition;

                ScrollDelta = ScrollDeltaBuffer;
                ScrollDeltaBuffer = 0;     
            }
        }
    }
}
