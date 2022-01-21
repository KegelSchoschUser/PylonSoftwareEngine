using PylonGameEngine.Input;
using PylonGameEngine.Mathematics;
using PylonGameEngine.Utilities.Win32;
using Linearstar.Windows.RawInput;
using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static PylonGameEngine.Utilities.Win32.Kernel32;
using static PylonGameEngine.Utilities.Win32.User32;

namespace PylonGameEngine
{
    public unsafe class Window
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetActiveWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32", ExactSpelling = true)]
        private static extern int GetSystemMetrics(SystemMetrics smIndex);

        [DllImport("user32", ExactSpelling = true)]
        private static extern IntPtr CreateWindowExW(uint dwExStyle, ushort* lpClassName, ushort* lpWindowName, uint dwStyle, int X, int Y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, void* lpParam);

        [DllImport("user32", ExactSpelling = true, SetLastError = true)]
        private static extern int DestroyWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool AdjustWindowRectEx(ref RECT lpRect, uint dwStyle, bool bMenu, uint dwExStyle);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern IntPtr LoadImage(IntPtr hinst, string lpszName, uint uType,
           int cxDesired, int cyDesired, uint fuLoad);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, nuint wParam, nint lParam);

        [DllImport("user32.dll")]
        static extern IntPtr DefWindowProc(IntPtr hWnd, uint uMsg, nuint wParam, IntPtr lParam);

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



        public string Title { get; private set; }
        public IntPtr Handle { get; private set; }
        public string WindowClassName { get; private set; }
        public Rectangle Rectangle { get; private set; }
        public int Width
        {
            get
            {
                return Rectangle.Width;
            }
        }
        public int Height
        {
            get
            {
                return Rectangle.Height;
            }
        }
        public Size Size
        {
            get
            {
                return Rectangle.Size;
            }
        }

        public Vector2 SizeVec2
        {
            get
            {
                return new Vector2(Rectangle.Size);
            }
        }

        public float AspectRatio
        {
            get
            {
                return (float)Width / (float)Height;
            }
        }


        public Window(string title, Vector2 Position, Vector2 Size, string windowClassName)
        {
            Title = title;
            Rectangle = new Rectangle(Position.ToPoint(), new System.Drawing.Size(Position.ToPoint()));
            WindowClassName = windowClassName;

            fixed (char* lpszClassName = WindowClassName)
            {
                WNDCLASSEX wndClassEx = new WNDCLASSEX
                {
                    Size = Unsafe.SizeOf<WNDCLASSEX>(),
                    Styles = WindowClassStyles.CS_HREDRAW | WindowClassStyles.CS_VREDRAW | WindowClassStyles.CS_OWNDC,
                    WindowProc = &WndProc,
                    InstanceHandle = GetModuleHandle(null),
                    CursorHandle = LoadCursorW(IntPtr.Zero, IDC_ARROW),
                    BackgroundBrushHandle = IntPtr.Zero,
                    IconHandle = IntPtr.Zero,
                    ClassName = (ushort*)lpszClassName
                };

                ushort atom = RegisterClassExW(&wndClassEx);

                if (atom == 0)
                {
                    throw new InvalidOperationException(
                        $"Failed to register window class. Error: {Marshal.GetLastWin32Error()}"
                        );
                }

            }
        }

        internal unsafe void PlatformConstruct()
        {
            int Width;
            int Height;
            WindowStyles style = WindowStyles.WS_VISIBLE | WindowStyles.WS_POPUP;
            WindowExStyles styleEx = WindowExStyles.WS_EX_APPWINDOW | WindowExStyles.WS_EX_WINDOWEDGE;
            RECT rect = new RECT(Rectangle.Left, Rectangle.Top, Rectangle.Right, Rectangle.Bottom);
            //AdjustWindowRectEx(ref rect, (uint)style, false, (uint)styleEx);
            Rectangle = rect.ToRectangle();

            fixed (char* lpszClassName = WindowClassName)
            {
                fixed (char* lpWindowName = Title)
                {
                    Handle = CreateWindowExW(
                    (uint)styleEx,
                    (ushort*)lpszClassName,
                    (ushort*)lpWindowName,
                    (uint)style,
                    Rectangle.X,
                    Rectangle.Y,
                    Rectangle.Width,
                    Rectangle.Height,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    null
                    );
                    if (Handle == IntPtr.Zero)
                    {
                        return;
                    }
                }
            }

            RawInputDevice.RegisterDevice(HidUsageAndPage.Keyboard, RawInputDeviceFlags.ExInputSink | RawInputDeviceFlags.NoLegacy, Handle);
            RawInputDevice.RegisterDevice(HidUsageAndPage.Mouse, RawInputDeviceFlags.ExInputSink | RawInputDeviceFlags.NoLegacy, Handle);
            ShowWindow(Handle, (int)ShowWindowCommand.Normal);

            //var icon = LoadImage(IntPtr.Zero, @"CoreContent\Logo.ico", 1, 256, 256, 256);
            //Process process = Process.GetCurrentProcess();

            //SendMessage(process.Handle, 128, 0, icon);
            //SendMessage(process.Handle, 128, 1, icon);
        }

        [UnmanagedCallersOnly]
        private static nint WndProc(IntPtr hWnd, uint message, nuint wParam, nint lParam)
        {
            if (message == WM_LBUTTONDOWN)
            {
                

            }

            if (message == WM_NCHITTEST)
            {
                IntPtr hit = DefWindowProc(hWnd, message, wParam, lParam);
                Console.WriteLine(hit);
                //if (hit == HTCLIENT) hit = HTCAPTION;
                return hit;
            }


            if (message == WM_ACTIVATEAPP)
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
            else if (message == WM_INPUT)
            {
                ProcessRawInput(hWnd, wParam, lParam);
            }

            return DefWindowProcW(hWnd, message, wParam, lParam);
        }
        private static void ProcessRawInput(IntPtr hWnd, nuint wParam, nint lParam)
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

        private static void OnKey(bool down, int VK)
        {
            if (down)
            {
                Input.Keyboard.AddKey(KeyCodes.ConvertToKey(VK));
            }
            else if (!down)
            {
                Input.Keyboard.RemoveKey(KeyCodes.ConvertToKey(VK));
            }
        }

        internal void Destroy()
        {
            IntPtr hwnd = Handle;
            if (hwnd != IntPtr.Zero)
            {
                IntPtr destroyHandle = hwnd;
                Handle = IntPtr.Zero;
                DestroyWindow(destroyHandle);
            }
        }
    }
}
