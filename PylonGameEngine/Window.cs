using PylonGameEngine.Input;
using PylonGameEngine.Mathematics;
using PylonGameEngine.Utilities.Win32;
using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static PylonGameEngine.Utilities.Win32.Kernel32;
using static PylonGameEngine.Utilities.Win32.User32;
using System.Windows.Threading;
using PylonGameEngine.Utilities;
using System.Windows.Forms;
using System.Threading;

namespace PylonGameEngine
{
    public class Window : Form
    {

        public new Vector2 Size
        {
            get
            {
                return base.Size;
            }
            set
            {
                base.Size = value;
            }
        }

        public new Vector2 Position
        {
            get
            {
                return base.Location;
            }
            set
            {
                base.Location = value;
            }
        }

      

        public Window(string title, Vector2 position, Vector2 size, bool FullScreen = false, bool Titlebar = false)
        {
            
            AutoScaleMode = AutoScaleMode.Dpi;
            Text = title;
            Position = position;
            Size = size;
            

            if (FullScreen)
            {
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                if (Titlebar)
                    this.FormBorderStyle = FormBorderStyle.FixedSingle;
                else
                    this.FormBorderStyle = FormBorderStyle.None;
            }

        }

        internal unsafe void Start()
        {
            Application.Run(this);
        }

        protected override void OnShown(EventArgs e)
        {
            Location = GameProperties.StartWindowPosition;
            Size = GameProperties.StartWindowSize;
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            nint message = m.Msg;
            nint wParam = m.WParam;
            nint lParam = m.LParam;
     
            switch (message)
            {
                case WM_ACTIVATEAPP:
                    {
                        if (wParam == 1)
                        {
                            MyGame.RendererEnabled = true;

                            if (MyGame.GameTickLoop != null)
                                MyGame.GameTickLoop.Resume();
                        }
                        else if (wParam == 0)
                        {
                            //#if !DEBUG
                            //                    MyGame.RendererEnabled = false;
                            //                    MyGame.GameTickLoop.Pause();
                            //#endif
                        }
                    }
                    break;
                case WM_TOUCH:
                    {
                        ProcessRawTouch(Handle, wParam, lParam);
                    }
                    break;
                case WM_GESTURE:
                    {
                        ProcessRawGesture(Handle, wParam, lParam);
                    }
                    break;
                case WM_KEYDOWN:
                    {
                        Input.Keyboard.AddKey(KeyCodes.ConvertToKey((int)wParam));
                    }
                    break;
                case WM_CHAR:
                    {
                        Input.Keyboard.AddCharKey((char)wParam);
                    }
                    break;
                case WM_KEYUP:
                    {
                        Input.Keyboard.RemoveKey(KeyCodes.ConvertToKey((int)wParam));
                    }
                    break;
                default:
                    break;
            }
            base.WndProc(ref m);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            Mouse.MouseScrollEvent(e.Delta);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    Mouse.MouseButtonEvent(Mouse.MouseButton.LeftButton, true);
                    break;

                case MouseButtons.Middle:
                    Mouse.MouseButtonEvent(Mouse.MouseButton.MiddleButton, true);
                    break;

                case MouseButtons.Right:
                    Mouse.MouseButtonEvent(Mouse.MouseButton.RightButton, true);
                    break;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    Mouse.MouseButtonEvent(Mouse.MouseButton.LeftButton, false);
                    break;

                case MouseButtons.Middle:
                    Mouse.MouseButtonEvent(Mouse.MouseButton.MiddleButton, false);
                    break;

                case MouseButtons.Right:
                    Mouse.MouseButtonEvent(Mouse.MouseButton.RightButton, false);
                    break;
            }
        }


        private static void ProcessRawTouch(IntPtr hWnd, nint wParam, nint lParam)
        {
            var inputCount = LoWord((int)wParam);
            var inputs = new TOUCHINPUT[inputCount];

            if (!GetTouchInputInfo(lParam, inputCount, inputs, touchInputSize))
            {
                MyLog.Default.Write("GetTouchInputInfo failed");
            }

            Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
            {
                Touchscreen.ProcessTouchs(inputs, inputCount);
            }));

            CloseTouchInputHandle(lParam);
        }

        private static void ProcessRawGesture(IntPtr hWnd, nint wParam, nint lParam)
        {

        }

        /*
        private static void ProcessRawInput(IntPtr hWnd, nint wParam, nint lParam)
        {
            if (GetActiveWindow() != hWnd)
                return;

            var data = RawInputData.FromHandle(lParam);
            switch (data)
            {
                case RawInputMouseData mouse:
                    {
                        if (mouse.Mouse.Buttons == Linearstar.Windows.RawInput.Native.RawMouseButtonFlags.MouseWheel)
                        {
                            Mouse.MouseScrollEvent(mouse.Mouse.ButtonData);
                        }
                        else
                        {
                            Mouse.MouseButtonEvent(mouse.Mouse.Buttons, mouse.Mouse.ButtonData);
                                Mouse.MouseDeltaEvent(mouse.Mouse.LastX, mouse.Mouse.LastY);
                        }
                    }
                    break;
                case RawInputKeyboardData keyboard:
                    if (keyboard.Keyboard.Flags == Linearstar.Windows.RawInput.Native.RawKeyboardFlags.None)
                    {
                        OnKey(true, keyboard.Keyboard.VirutalKey);
                    }
                    else if (keyboard.Keyboard.Flags == Linearstar.Windows.RawInput.Native.RawKeyboardFlags.Up)
                    {

                        OnKey(false, keyboard.Keyboard.VirutalKey);
                    }
                    break;
            }
        }
        */

        internal void Destroy()
        {
            Close();
        }
    }
}
