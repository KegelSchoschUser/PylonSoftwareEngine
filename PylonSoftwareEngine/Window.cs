/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

using PylonSoftwareEngine.Input;
using PylonSoftwareEngine.Mathematics;
using PylonSoftwareEngine.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Threading;
using static PylonSoftwareEngine.Utilities.Win32.User32;

namespace PylonSoftwareEngine
{
    public class Window : Form
    {
        internal List<InputManager> InputManagers = new List<InputManager>();

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

        public Rectangle GlobalRectangle => base.RectangleToScreen(base.ClientRectangle);

        public static Window CreateWindow(string title, Vector2 position, Vector2 size, bool FullScreen = false, bool Titlebar = false)
        {
            Window window = null;
            Thread t = new Thread(() =>
            {
                window = new Window(title, position, size, FullScreen, Titlebar);
                Application.Run(window);
            });
            t.Start();
            while (window == null)
                ;

            return window;
        }
        internal Window(string title, Vector2 position, Vector2 size, bool FullScreen = false, bool Titlebar = false)
        {
            CheckForIllegalCrossThreadCalls = false;
            Touchscreen.RegisterTouchEvent(Handle);
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

            SetWindowPos(Handle, new IntPtr(1), (int)position.X, (int)position.Y, (int)size.X, (int)size.Y, SetWindowPosFlags.DoNotActivate);
            MySoftware.Windows.Add(this);


        }


        protected override void OnShown(EventArgs e)
        {

        }

        protected override void OnClosed(EventArgs e)
        {

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
                            //MySoftware.RendererEnabled = true;

                            //if (MySoftware.SoftwareTickLoop != null)
                            //    MySoftware.SoftwareTickLoop.Resume();
                        }
                        else if (wParam == 0)
                        {
                            //#if !DEBUG
                            //                    MySoftware.RendererEnabled = false;
                            //                    MySoftware.SoftwareTickLoop.Pause();
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

                        foreach (var inputmanager in InputManagers)
                        {
                            //Console.WriteLine(wParam);
                            inputmanager.KeyDown(KeyCodes.ConvertToKey((int)wParam));
                        }
                    }
                    break;
                case WM_CHAR:
                    {
                        foreach (var inputmanager in InputManagers)
                        {
                            inputmanager.CharKey((char)wParam);
                        }
                    }
                    break;
                case WM_KEYUP:
                    {
                        foreach (var inputmanager in InputManagers)
                        {
                            inputmanager.KeyUp(KeyCodes.ConvertToKey((int)wParam));
                        }
                    }
                    break;
                //case WM_INPUT:
                //    {
                //        Vector2 delta = new Vector2();
                //        .
                //        Input.Mouse.MouseDeltaEvent(delta);
                //    }
                //    break;
                default:
                    break;
            }
            base.WndProc(ref m);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Menu)
                foreach (var inputmanager in InputManagers)
                {
                    inputmanager.KeyDown(KeyboardKey.Alt);
                }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Menu)
                foreach (var inputmanager in InputManagers)
                {
                    inputmanager.KeyUp(KeyboardKey.Alt);
                }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            foreach (var inputmanager in InputManagers)
            {
                inputmanager.MouseScrollEvent(e.Delta);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            foreach (var inputmanager in InputManagers)
            {
                switch (e.Button)
                {
                    case MouseButtons.Left:
                        inputmanager.MouseButtonEvent(Mouse.MouseButton.LeftButton, true);
                        break;

                    case MouseButtons.Middle:
                        inputmanager.MouseButtonEvent(Mouse.MouseButton.MiddleButton, true);
                        break;

                    case MouseButtons.Right:
                        inputmanager.MouseButtonEvent(Mouse.MouseButton.RightButton, true);
                        break;
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            foreach (var inputmanager in InputManagers)
            {
                switch (e.Button)
                {
                    case MouseButtons.Left:
                        inputmanager.MouseButtonEvent(Mouse.MouseButton.LeftButton, false);
                        break;

                    case MouseButtons.Middle:
                        inputmanager.MouseButtonEvent(Mouse.MouseButton.MiddleButton, false);
                        break;

                    case MouseButtons.Right:
                        inputmanager.MouseButtonEvent(Mouse.MouseButton.RightButton, false);
                        break;
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            foreach (var inputmanager in InputManagers)
            {
                if(FormBorderStyle == FormBorderStyle.None)
                    inputmanager.MouseMove(e.X + 5, e.Y);
                else
                {
                    Rectangle screenRectangle = this.RectangleToScreen(this.ClientRectangle);

                    int titleHeight = screenRectangle.Top - this.Top;
                    inputmanager.MouseMove(e.X + 5, e.Y + titleHeight);
                }
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

        public void Destroy()
        {
            MySoftware.Windows.Remove(this);
            Close();
            DestroyHandle();
        }
    }
}
