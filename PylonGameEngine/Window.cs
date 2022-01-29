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
using System.Windows.Threading;
using PylonGameEngine.Utilities;

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
        public Vector2 Size
        {
            get
            {
                User32.GetWindowRect(Handle, out var rect);
                int width = rect.Right - rect.Left;
                int height = rect.Bottom - rect.Top;
                return new Vector2(width, height);
            }
            set
            {
                User32.SetWindowPos(Handle, IntPtr.Zero, 0, 0, (int)value.X, (int)value.Y, SetWindowPosFlags.IgnoreMove);
            }
        }
        public Vector2 Position
        {
            get
            {
                User32.GetWindowRect(Handle, out var rect);
                return new Vector2(rect.Left, rect.Top);
            }
            set
            {
                User32.SetWindowPos(Handle, IntPtr.Zero, (int)value.X, (int)value.Y, 0, 0, SetWindowPosFlags.IgnoreResize);
            }
        }

        public float AspectRatio
        {
            get
            {
                return (float)Size.X / (float)Size.Y;
            }
        }

        Vector2 StartPosition;
        Vector2 StartSize;
        public Window(string title, Vector2 Position, Vector2 Size, string windowClassName)
        {
            Title = title;
            WindowClassName = windowClassName;
            StartPosition = Position;
            StartSize = Size;

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
            //AdjustWindowRectEx(ref rect, (uint)style, false, (uint)styleEx);

            fixed (char* lpszClassName = WindowClassName)
            {
                fixed (char* lpWindowName = Title)
                {
                    Handle = CreateWindowExW(
                    (uint)styleEx,
                    (ushort*)lpszClassName,
                    (ushort*)lpWindowName,
                    (uint)style,
                    (int)StartPosition.X,
                    (int)StartPosition.Y,
                    (int)StartSize.X,
                    (int)StartSize.Y,
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
            RawInputDevice.RegisterDevice(HidUsageAndPage.TouchScreen, RawInputDeviceFlags.ExInputSink , Handle);
            ShowWindow(Handle, (int)ShowWindowCommand.Normal);

            //var icon = LoadImage(IntPtr.Zero, @"CoreContent\Logo.ico", 1, 256, 256, 256);
            //Process process = Process.GetCurrentProcess();

            //SendMessage(process.Handle, 128, 0, icon);
            //SendMessage(process.Handle, 128, 1, icon);
        }

        [UnmanagedCallersOnly]
        private static nint WndProc(IntPtr hWnd, uint message, nuint wParam, nint lParam)
        {
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
                return 0;
            }
            else if (message == 576) // WM_TOUCH
            {
              //  Console.WriteLine("WM_TOUCH");
                return ProcessRawTouch(hWnd, wParam, lParam);
            }
            else if (message == WM_INPUT)
            {
                ProcessRawInput(hWnd, wParam, lParam);
                return 0;
            }

            else if (message == 281)
            {
                Console.WriteLine("WM_GESTURE");
                return 0;
            }
            return DefWindowProcW(hWnd, message, wParam, lParam);
        }

        private static nint ProcessRawTouch(IntPtr hWnd, nuint wParam, nint lParam)
        {
            var inputCount = LoWord((int)wParam);
            var inputs = new TOUCHINPUT[inputCount];

            if (!GetTouchInputInfo(lParam, inputCount, inputs, touchInputSize))
            {
                MyLog.Default.Write("GetTouchInputInfo failed");
                return -1;
            }

            //TouchQueue.Enqueue(inputs);
            Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
            {
                Touchscreen.ProcessTouchs(inputs, inputCount);
            }));

            CloseTouchInputHandle(lParam);

            return 0;
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
                case RawInputHidData hid:

                    //foreach (var item in hid.Hid.ToHidReports())
                    //{
                    //    string str = "";
                    //    for (int i = 0; i < item.Count; i++)
                    //    {
                    //        str += item[i] + " ";
                    //    }
                    //    str += "\n";
                    //    Console.WriteLine(str);
                    //}
                    //Console.WriteLine();
                    ////  12 0 2      4         1            0 6     38           195  14          5 2 0 73 12 245 10 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 104 9
                    ////  ?? ? Anzahl [5 = Down  Ánzahl Up?  ? PosX  MultiplierX  PosY MultiplierY] [Repeats]
                    ////       Down    4 = Up

                    //var Data = hid.Hid.RawData;
                    //Vector2 Location = Vector2.Zero;
                    //bool down = hid.Hid.RawData[1] == 5 ? true : false;
                    //Location.X = Data[6] + Data[7] * 255f;
                    //Location.Y = Data[8] + Data[9] * 255f;
                    //Console.WriteLine(Location +"   " + Data[5] + "   " + Data[6] + "   " + BitConverter.ToInt16(new byte[] { Data[5], Data[6] }));
                    //Touchscreen.TouchInput(Location, down);
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
