using System;
using System.Drawing;
using System.Runtime.InteropServices;
//test
namespace PylonGameEngine.Utilities.Win32
{
    #region Enums
    [Flags]
    public enum WindowStyles
    {
        WS_BORDER = 0x00800000,
        WS_CAPTION = 0x00C00000,
        WS_CHILD = 0x40000000,
        WS_CHILDWINDOW = 0x40000000,
        WS_CLIPCHILDREN = 0x02000000,
        WS_CLIPSIBLINGS = 0x04000000,
        WS_DISABLED = 0x08000000,
        WS_DLGFRAME = 0x00400000,
        WS_GROUP = 0x00020000,
        WS_HSCROLL = 0x00100000,
        WS_ICONIC = 0x20000000,
        WS_MAXIMIZE = 0x01000000,
        WS_MAXIMIZEBOX = 0x00010000,
        WS_MINIMIZE = 0x20000000,
        WS_MINIMIZEBOX = 0x00020000,
        WS_OVERLAPPED = 0x00000000,
        WS_OVERLAPPEDWINDOW =
            WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,
        WS_POPUP = unchecked((int)0x80000000),
        WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,
        WS_SIZEBOX = 0x00040000,
        WS_SYSMENU = 0x00080000,
        WS_TABSTOP = 0x00010000,
        WS_THICKFRAME = 0x00040000,
        WS_TILED = 0x00000000,
        WS_TILEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,
        WS_VISIBLE = 0x10000000,
        WS_VSCROLL = 0x00200000
    }

    [Flags]
    public enum WindowExStyles : uint
    {
        WS_EX_LEFT = 0x00000000,
        WS_EX_LTRREADING = 0x00000000,
        WS_EX_RIGHTSCROLLBAR = 0x00000000,
        WS_EX_DLGMODALFRAME = 0x00000001,
        WS_EX_NOPARENTNOTIFY = 0x00000004,
        WS_EX_TOPMOST = 0x00000008,
        WS_EX_ACCEPTFILES = 0x00000010,
        WS_EX_TRANSPARENT = 0x00000020,
        WS_EX_MDICHILD = 0x00000040,
        /// <summary>
        ///     The window is intended to be used as a floating toolbar. A tool window has a title bar that is shorter than a
        ///     normal title bar, and the window title is drawn using a smaller font. A tool window does not appear in the taskbar
        ///     or in the dialog that appears when the user presses ALT+TAB. If a tool window has a system menu, its icon is not
        ///     displayed on the title bar. However, you can display the system menu by right-clicking or by typing ALT+SPACE.
        /// </summary>
        WS_EX_TOOLWINDOW = 0x00000080,
        WS_EX_WINDOWEDGE = 0x00000100,
        WS_EX_PALETTEWINDOW = WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST,
        WS_EX_CLIENTEDGE = 0x00000200,
        WS_EX_OVERLAPPEDWINDOW = WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE,
        WS_EX_CONTEXTHELP = 0x00000400,
        WS_EX_RIGHT = 0x00001000,
        WS_EX_RTLREADING = 0x00002000,
        WS_EX_LEFTSCROLLBAR = 0x00004000,
        WS_EX_CONTROLPARENT = 0x00010000,
        WS_EX_STATICEDGE = 0x00020000,
        WS_EX_APPWINDOW = 0x00040000,
        WS_EX_LAYERED = 0x00080000,
        WS_EX_NOINHERITLAYOUT = 0x00100000,
        WS_EX_NOREDIRECTIONBITMAP = 0x00200000,
        WS_EX_LAYOUTRTL = 0x00400000,
        WS_EX_COMPOSITED = 0x02000000,
        WS_EX_NOACTIVATE = 0x08000000
    }

    [Flags]
    public enum WindowClassStyles
    {
        CS_BYTEALIGNCLIENT = 0x1000,
        CS_BYTEALIGNWINDOW = 0x2000,
        CS_CLASSDC = 0x0040,
        CS_DBLCLKS = 0x0008,
        CS_DROPSHADOW = 0x00020000,
        CS_GLOBALCLASS = 0x4000,
        CS_HREDRAW = 0x0002,
        CS_NOCLOSE = 0x0200,
        CS_OWNDC = 0x0020,
        CS_PARENTDC = 0x0080,
        CS_SAVEBITS = 0x0800,
        CS_VREDRAW = 0x0001
    }

    public enum ShowWindowCommand
    {
        /// <summary>
        /// Hides the window and activates another window.
        /// </summary>
        Hide = 0,

        /// <summary>
        /// Activates and displays a window. If the window is minimized or
        /// maximized, the system restores it to its original size and position.
        /// An application should specify this flag when displaying the window
        /// for the first time.
        /// </summary>
        Normal = 1,

        /// <summary>
        /// Activates the window and displays it as a minimized window.
        /// </summary>
        ShowMinimized = 2,

        /// <summary>
        /// Maximizes the specified window.
        /// </summary>
        Maximize = 3,

        /// <summary>
        /// Activates the window and displays it as a maximized window.
        /// </summary>
        ShowMaximized = 3,

        /// <summary>
        /// Displays a window in its most recent size and position. This value
        /// is similar to <see cref="ShowWindowCommand.Normal"/>, except
        /// the window is not activated.
        /// </summary>
        ShowNoActivate = 4,

        /// <summary>
        /// Activates the window and displays it in its current size and position.
        /// </summary>
        Show = 5,

        /// <summary>
        /// Minimizes the specified window and activates the next top-level
        /// window in the Z order.
        /// </summary>
        Minimize = 6,

        /// <summary>
        /// Displays the window as a minimized window. This value is similar to
        /// <see cref="ShowMinimized"/>, except the
        /// window is not activated.
        /// </summary>
        ShowMinNoActive = 7,

        /// <summary>
        /// Displays the window in its current size and position. This value is
        /// similar to <see cref="Show"/>, except the
        /// window is not activated.
        /// </summary>
        ShowNA = 8,

        /// <summary>
        /// Activates and displays the window. If the window is minimized or
        /// maximized, the system restores it to its original size and position.
        /// An application should specify this flag when restoring a minimized window.
        /// </summary>
        Restore = 9,

        /// <summary>
        /// Sets the show state based on the SW_* value specified in the
        /// STARTUPINFO structure passed to the CreateProcess function by the
        /// program that started the application.
        /// </summary>
        ShowDefault = 10,

        /// <summary>
        ///  <b>Windows 2000/XP:</b> Minimizes a window, even if the thread
        /// that owns the window is not responding. This flag should only be
        /// used when minimizing windows from a different thread.
        /// </summary>
        ForceMinimize = 11
    }

    public enum SystemMetrics
    {
        SM_CXSCREEN = 0,  // 0x00
        SM_CYSCREEN = 1,  // 0x01
        SM_CXVSCROLL = 2,  // 0x02
        SM_CYHSCROLL = 3,  // 0x03
        SM_CYCAPTION = 4,  // 0x04
        SM_CXBORDER = 5,  // 0x05
        SM_CYBORDER = 6,  // 0x06
        SM_CXDLGFRAME = 7,  // 0x07
        SM_CXFIXEDFRAME = 7,  // 0x07
        SM_CYDLGFRAME = 8,  // 0x08
        SM_CYFIXEDFRAME = 8,  // 0x08
        SM_CYVTHUMB = 9,  // 0x09
        SM_CXHTHUMB = 10, // 0x0A
        SM_CXICON = 11, // 0x0B
        SM_CYICON = 12, // 0x0C
        SM_CXCURSOR = 13, // 0x0D
        SM_CYCURSOR = 14, // 0x0E
        SM_CYMENU = 15, // 0x0F
        SM_CXFULLSCREEN = 16, // 0x10
        SM_CYFULLSCREEN = 17, // 0x11
        SM_CYKANJIWINDOW = 18, // 0x12
        SM_MOUSEPRESENT = 19, // 0x13
        SM_CYVSCROLL = 20, // 0x14
        SM_CXHSCROLL = 21, // 0x15
        SM_DEBUG = 22, // 0x16
        SM_SWAPBUTTON = 23, // 0x17
        SM_CXMIN = 28, // 0x1C
        SM_CYMIN = 29, // 0x1D
        SM_CXSIZE = 30, // 0x1E
        SM_CYSIZE = 31, // 0x1F
        SM_CXSIZEFRAME = 32, // 0x20
        SM_CXFRAME = 32, // 0x20
        SM_CYSIZEFRAME = 33, // 0x21
        SM_CYFRAME = 33, // 0x21
        SM_CXMINTRACK = 34, // 0x22
        SM_CYMINTRACK = 35, // 0x23
        SM_CXDOUBLECLK = 36, // 0x24
        SM_CYDOUBLECLK = 37, // 0x25
        SM_CXICONSPACING = 38, // 0x26
        SM_CYICONSPACING = 39, // 0x27
        SM_MENUDROPALIGNMENT = 40, // 0x28
        SM_PENWINDOWS = 41, // 0x29
        SM_DBCSENABLED = 42, // 0x2A
        SM_CMOUSEBUTTONS = 43, // 0x2B
        SM_SECURE = 44, // 0x2C
        SM_CXEDGE = 45, // 0x2D
        SM_CYEDGE = 46, // 0x2E
        SM_CXMINSPACING = 47, // 0x2F
        SM_CYMINSPACING = 48, // 0x30
        SM_CXSMICON = 49, // 0x31
        SM_CYSMICON = 50, // 0x32
        SM_CYSMCAPTION = 51, // 0x33
        SM_CXSMSIZE = 52, // 0x34
        SM_CYSMSIZE = 53, // 0x35
        SM_CXMENUSIZE = 54, // 0x36
        SM_CYMENUSIZE = 55, // 0x37
        SM_ARRANGE = 56, // 0x38
        SM_CXMINIMIZED = 57, // 0x39
        SM_CYMINIMIZED = 58, // 0x3A
        SM_CXMAXTRACK = 59, // 0x3B
        SM_CYMAXTRACK = 60, // 0x3C
        SM_CXMAXIMIZED = 61, // 0x3D
        SM_CYMAXIMIZED = 62, // 0x3E
        SM_NETWORK = 63, // 0x3F
        SM_CLEANBOOT = 67, // 0x43
        SM_CXDRAG = 68, // 0x44
        SM_CYDRAG = 69, // 0x45
        SM_SHOWSOUNDS = 70, // 0x46
        SM_CXMENUCHECK = 71, // 0x47
        SM_CYMENUCHECK = 72, // 0x48
        SM_SLOWMACHINE = 73, // 0x49
        SM_MIDEASTENABLED = 74, // 0x4A
        SM_MOUSEWHEELPRESENT = 75, // 0x4B
        SM_XVIRTUALSCREEN = 76,
        SM_YVIRTUALSCREEN = 77,
        SM_CXVIRTUALSCREEN = 78, // 0x4E
        SM_CYVIRTUALSCREEN = 79, // 0x4F
        SM_CMONITORS = 80, // 0x50
        SM_SAMEDISPLAYFORMAT = 81, // 0x51
        SM_IMMENABLED = 82, // 0x52
        SM_CXFOCUSBORDER = 83, // 0x53
        SM_CYFOCUSBORDER = 84, // 0x54
        SM_TABLETPC = 86,
        SM_MEDIACENTER = 87,
        SM_STARTER = 88,
        SM_SERVERR2 = 89,
        SM_MOUSEHORIZONTALWHEELPRESENT = 91,
        SM_CXPADDEDBORDER = 92,
        SM_DIGITIZER = 94,
        SM_MAXIMUMTOUCHES = 95,

        SM_REMOTESESSION = 0x1000,
        SM_SHUTTINGDOWN = 0x2000,
        SM_REMOTECONTROL = 0x2001,

        SM_CONVERTABLESLATEMODE = 0x2003,
        SM_SYSTEMDOCKED = 0x2004,
    }
    #endregion

    #region Structures
    [StructLayout(LayoutKind.Sequential)]
    public struct Message
    {
        public IntPtr Hwnd;
        public uint Value;
        public IntPtr WParam;
        public IntPtr LParam;
        public uint Time;
        public Point Point;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public unsafe struct WNDCLASSEX
    {
        public int Size;
        public WindowClassStyles Styles;

        public delegate* unmanaged<IntPtr, uint, nuint, nint, nint> WindowProc;
        public int ClassExtraBytes;
        public int WindowExtraBytes;
        public IntPtr InstanceHandle;
        public IntPtr IconHandle;
        public IntPtr CursorHandle;
        public IntPtr BackgroundBrushHandle;
        public ushort* MenuName;
        public ushort* ClassName;
        public IntPtr SmallIconHandle;
    }
    #endregion Structures

    public static unsafe class User32
    {
        public static readonly unsafe ushort* IDC_ARROW = (ushort*)32512;
        
        public const int WM_NULL = 0x0000;
        public const int WM_QUIT = 18;
        public const int WM_ACTIVATEAPP = 28;
        public const int WM_NCHITTEST = 132;
        public const int WM_KEYDOWN = 256;
        public const int WM_KEYUP = 257;

        public const int WM_CHAR = 258;

        public const int WM_DEADCHAR = 259;

        public const int WM_SYSKEYDOWN = 260;

        public const int WM_SYSKEYUP = 261;

        public const int WM_SYSCHAR = 262;

        public const int WM_SYSDEADCHAR = 263;

        public const int WM_UNICHAR = 265;

        public const int WM_KEYLAST = 265;

        public const int WM_DESTROY = 2;

        public const int VK_LBUTTON = 1;

        public const int VK_RBUTTON = 2;

        public const int VK_CANCEL = 3;

        public const int VK_MBUTTON = 4;

        public const int VK_XBUTTON1 = 5;

        public const int VK_XBUTTON2 = 6;

        public const int VK_BACK = 8;

        public const int VK_TAB = 9;

        public const int VK_CLEAR = 12;

        public const int VK_RETURN = 13;

        public const int VK_SHIFT = 16;

        public const int VK_CONTROL = 17;

        public const int VK_MENU = 18;

        public const int VK_PAUSE = 19;

        public const int VK_CAPITAL = 20;

        public const int VK_KANA = 21;

        public const int VK_HANGEUL = 21;

        public const int VK_HANGUL = 21;

        public const int VK_IME_ON = 22;

        public const int VK_JUNJA = 23;

        public const int VK_FINAL = 24;

        public const int VK_HANJA = 25;

        public const int VK_KANJI = 25;

        public const int VK_IME_OFF = 26;

        public const int VK_ESCAPE = 27;

        public const int VK_CONVERT = 28;

        public const int VK_NONCONVERT = 29;

        public const int VK_ACCEPT = 30;

        public const int VK_MODECHANGE = 31;

        public const int VK_SPACE = 32;

        public const int VK_PRIOR = 33;

        public const int VK_NEXT = 34;

        public const int VK_END = 35;

        public const int VK_HOME = 36;

        public const int VK_LEFT = 37;

        public const int VK_UP = 38;

        public const int VK_RIGHT = 39;

        public const int VK_DOWN = 40;

        public const int VK_SELECT = 41;

        public const int VK_PRINT = 42;

        public const int VK_EXECUTE = 43;

        public const int VK_SNAPSHOT = 44;

        public const int VK_INSERT = 45;

        public const int VK_DELETE = 46;

        public const int VK_HELP = 47;

        public const int VK_LWIN = 91;

        public const int VK_RWIN = 92;

        public const int VK_APPS = 93;

        public const int VK_SLEEP = 95;

        public const int VK_NUMPAD0 = 96;

        public const int VK_NUMPAD1 = 97;

        public const int VK_NUMPAD2 = 98;

        public const int VK_NUMPAD3 = 99;

        public const int VK_NUMPAD4 = 100;

        public const int VK_NUMPAD5 = 101;

        public const int VK_NUMPAD6 = 102;

        public const int VK_NUMPAD7 = 103;

        public const int VK_NUMPAD8 = 104;

        public const int VK_NUMPAD9 = 105;

        public const int VK_MULTIPLY = 106;

        public const int VK_ADD = 107;

        public const int VK_SEPARATOR = 108;

        public const int VK_SUBTRACT = 109;

        public const int VK_DECIMAL = 110;

        public const int VK_DIVIDE = 111;

        public const int VK_F1 = 112;

        public const int VK_F2 = 113;

        public const int VK_F3 = 114;

        public const int VK_F4 = 115;

        public const int VK_F5 = 116;

        public const int VK_F6 = 117;

        public const int VK_F7 = 118;

        public const int VK_F8 = 119;

        public const int VK_F9 = 120;

        public const int VK_F10 = 121;

        public const int VK_F11 = 122;

        public const int VK_F12 = 123;

        public const int VK_F13 = 124;

        public const int VK_F14 = 125;

        public const int VK_F15 = 126;

        public const int VK_F16 = 127;

        public const int VK_F17 = 128;

        public const int VK_F18 = 129;

        public const int VK_F19 = 130;

        public const int VK_F20 = 131;

        public const int VK_F21 = 132;

        public const int VK_F22 = 133;

        public const int VK_F23 = 134;

        public const int VK_F24 = 135;

        public const int VK_NAVIGATION_VIEW = 136;

        public const int VK_NAVIGATION_MENU = 137;

        public const int VK_NAVIGATION_UP = 138;

        public const int VK_NAVIGATION_DOWN = 139;

        public const int VK_NAVIGATION_LEFT = 140;

        public const int VK_NAVIGATION_RIGHT = 141;

        public const int VK_NAVIGATION_ACCEPT = 142;

        public const int VK_NAVIGATION_CANCEL = 143;

        public const int VK_NUMLOCK = 144;

        public const int VK_SCROLL = 145;

        public const int VK_OEM_NEC_EQUAL = 146;

        public const int VK_OEM_FJ_JISHO = 146;

        public const int VK_OEM_FJ_MASSHOU = 147;

        public const int VK_OEM_FJ_TOUROKU = 148;

        public const int VK_OEM_FJ_LOYA = 149;

        public const int VK_OEM_FJ_ROYA = 150;

        public const int VK_LSHIFT = 160;

        public const int VK_RSHIFT = 161;

        public const int VK_LCONTROL = 162;

        public const int VK_RCONTROL = 163;

        public const int VK_LMENU = 164;

        public const int VK_RMENU = 165;

        public const int VK_BROWSER_BACK = 166;

        public const int VK_BROWSER_FORWARD = 167;

        public const int VK_BROWSER_REFRESH = 168;

        public const int VK_BROWSER_STOP = 169;

        public const int VK_BROWSER_SEARCH = 170;

        public const int VK_BROWSER_FAVORITES = 171;

        public const int VK_BROWSER_HOME = 172;

        public const int VK_VOLUME_MUTE = 173;

        public const int VK_VOLUME_DOWN = 174;

        public const int VK_VOLUME_UP = 175;

        public const int VK_MEDIA_NEXT_TRACK = 176;

        public const int VK_MEDIA_PREV_TRACK = 177;

        public const int VK_MEDIA_STOP = 178;

        public const int VK_MEDIA_PLAY_PAUSE = 179;

        public const int VK_LAUNCH_MAIL = 180;

        public const int VK_LAUNCH_MEDIA_SELECT = 181;

        public const int VK_LAUNCH_APP1 = 182;

        public const int VK_LAUNCH_APP2 = 183;

        public const int VK_OEM_1 = 186;

        public const int VK_OEM_PLUS = 187;

        public const int VK_OEM_COMMA = 188;

        public const int VK_OEM_MINUS = 189;

        public const int VK_OEM_PERIOD = 190;

        public const int VK_OEM_2 = 191;

        public const int VK_OEM_3 = 192;

        public const int VK_ATTN = 246;

        public const int VK_CRSEL = 247;

        public const int VK_EXSEL = 248;

        public const int VK_EREOF = 249;

        public const int VK_PLAY = 250;

        public const int VK_ZOOM = 251;

        public const int VK_NONAME = 252;

        public const int VK_PA1 = 253;

        public const int VK_OEM_CLEAR = 254;
        public const int VK_OEM_102 = 226;

        public const int WM_LBUTTONDOWN = 513;
        public const int WM_LBUTTONUP = 514;
        public const int WM_LBUTTONDBLCLK = 515;
        public const int WM_RBUTTONDOWN = 516;
        public const int WM_RBUTTONUP = 517;
        public const int WM_RBUTTONDBLCLK = 518;
        public const int WM_MBUTTONDOWN = 519;
        public const int WM_MBUTTONUP = 520;
        public const int WM_MBUTTONDBLCLK = 521;

        public const int WM_NCMOUSELEAVE = 674;
        public const int WM_MOUSELEAVE = 675;
        public const int WM_INPUT = 255;

        [DllImport("user32", ExactSpelling = true)]
        public static extern unsafe ushort RegisterClassExW(WNDCLASSEX* lpwcx);

        [DllImport("user32", ExactSpelling = true)]
        public static extern nint DefWindowProcW(IntPtr hWnd, uint msg, nuint wParam, nint lParam);

        [DllImport("user32", ExactSpelling = true)]
        public static extern IntPtr LoadCursorW(IntPtr hInstance, ushort* lpCursorName);

        [DllImport("user32", ExactSpelling = true, SetLastError = true)]
        public static extern unsafe int GetMessageW(Message* lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

        [DllImport("user32", ExactSpelling = true)]
        public static extern unsafe int PeekMessageW(Message* lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax, uint wRemoveMsg);

        [DllImport("user32", ExactSpelling = true)]
        public static extern int TranslateMessage(Message* lpMsg);

        [DllImport("user32", ExactSpelling = true)]
        public static extern unsafe nint DispatchMessageW(Message* lpMsg);


        [DllImport("user32", ExactSpelling = true)]
        public static extern int ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32", ExactSpelling = true)]
        public static extern void PostQuitMessage(int nExitCode);

        [DllImport("user32.dll")]
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern int GetMessage(out Message lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

        // This static method is required because legacy OSes do not support
        // SetWindowLongPtr
        public static IntPtr SetWindowLongPtr(HandleRef hWnd, int nIndex, IntPtr dwNewLong)
        {
            if (IntPtr.Size == 8)
                return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
            else
                return new IntPtr(SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32()));
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        private static extern int SetWindowLong32(HandleRef hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        private static extern IntPtr SetWindowLongPtr64(HandleRef hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);


        [Flags()]
        public enum SetWindowPosFlags : uint
        {
            /// <summary>If the calling thread and the thread that owns the window are attached to different input queues,
            /// the system posts the request to the thread that owns the window. This prevents the calling thread from
            /// blocking its execution while other threads process the request.</summary>
            /// <remarks>SWP_ASYNCWINDOWPOS</remarks>
            AsynchronousWindowPosition = 0x4000,
            /// <summary>Prevents generation of the WM_SYNCPAINT message.</summary>
            /// <remarks>SWP_DEFERERASE</remarks>
            DeferErase = 0x2000,
            /// <summary>Draws a frame (defined in the window's class description) around the window.</summary>
            /// <remarks>SWP_DRAWFRAME</remarks>
            DrawFrame = 0x0020,
            /// <summary>Applies new frame styles set using the SetWindowLong function. Sends a WM_NCCALCSIZE message to
            /// the window, even if the window's size is not being changed. If this flag is not specified, WM_NCCALCSIZE
            /// is sent only when the window's size is being changed.</summary>
            /// <remarks>SWP_FRAMECHANGED</remarks>
            FrameChanged = 0x0020,
            /// <summary>Hides the window.</summary>
            /// <remarks>SWP_HIDEWINDOW</remarks>
            HideWindow = 0x0080,
            /// <summary>Does not activate the window. If this flag is not set, the window is activated and moved to the
            /// top of either the topmost or non-topmost group (depending on the setting of the hWndInsertAfter
            /// parameter).</summary>
            /// <remarks>SWP_NOACTIVATE</remarks>
            DoNotActivate = 0x0010,
            /// <summary>Discards the entire contents of the client area. If this flag is not specified, the valid
            /// contents of the client area are saved and copied back into the client area after the window is sized or
            /// repositioned.</summary>
            /// <remarks>SWP_NOCOPYBITS</remarks>
            DoNotCopyBits = 0x0100,
            /// <summary>Retains the current position (ignores X and Y parameters).</summary>
            /// <remarks>SWP_NOMOVE</remarks>
            IgnoreMove = 0x0002,
            /// <summary>Does not change the owner window's position in the Z order.</summary>
            /// <remarks>SWP_NOOWNERZORDER</remarks>
            DoNotChangeOwnerZOrder = 0x0200,
            /// <summary>Does not redraw changes. If this flag is set, no repainting of any kind occurs. This applies to
            /// the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent
            /// window uncovered as a result of the window being moved. When this flag is set, the application must
            /// explicitly invalidate or redraw any parts of the window and parent window that need redrawing.</summary>
            /// <remarks>SWP_NOREDRAW</remarks>
            DoNotRedraw = 0x0008,
            /// <summary>Same as the SWP_NOOWNERZORDER flag.</summary>
            /// <remarks>SWP_NOREPOSITION</remarks>
            DoNotReposition = 0x0200,
            /// <summary>Prevents the window from receiving the WM_WINDOWPOSCHANGING message.</summary>
            /// <remarks>SWP_NOSENDCHANGING</remarks>
            DoNotSendChangingEvent = 0x0400,
            /// <summary>Retains the current size (ignores the cx and cy parameters).</summary>
            /// <remarks>SWP_NOSIZE</remarks>
            IgnoreResize = 0x0001,
            /// <summary>Retains the current Z order (ignores the hWndInsertAfter parameter).</summary>
            /// <remarks>SWP_NOZORDER</remarks>
            IgnoreZOrder = 0x0004,
            /// <summary>Displays the window.</summary>
            /// <remarks>SWP_SHOWWINDOW</remarks>
            ShowWindow = 0x0040,
        }
        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        public static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);

        public static readonly int GWL_WNDPROC = -4;
        public static readonly int GWL_HINSTANCE = -6;
        public static readonly int GWL_HWNDPARENT = -8;
        public static readonly int GWL_STYLE = -16;
        public static readonly int GWL_EXSTYLE = -20;
        public static readonly int GWL_USERDATA = -21;
        public static readonly int GWL_ID = -12;

        public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        public static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        public static readonly IntPtr HWND_TOP = new IntPtr(0);
        public static readonly IntPtr HWND_BOTTOM = new IntPtr(1);

        /// <summary>
        /// Window handles (HWND) used for hWndInsertAfter
        /// </summary>
        public static class HWND
        {
            public static IntPtr
            NoTopMost = new IntPtr(-2),
            TopMost = new IntPtr(-1),
            Top = new IntPtr(0),
            Bottom = new IntPtr(1);
        }

        /// <summary>
        /// SetWindowPos Flags
        /// </summary>
        public static class SWP
        {
            public static readonly int
            NOSIZE = 0x0001,
            NOMOVE = 0x0002,
            NOZORDER = 0x0004,
            NOREDRAW = 0x0008,
            NOACTIVATE = 0x0010,
            DRAWFRAME = 0x0020,
            FRAMECHANGED = 0x0020,
            SHOWWINDOW = 0x0040,
            HIDEWINDOW = 0x0080,
            NOCOPYBITS = 0x0100,
            NOOWNERZORDER = 0x0200,
            NOREPOSITION = 0x0200,
            NOSENDCHANGING = 0x0400,
            DEFERERASE = 0x2000,
            ASYNCWINDOWPOS = 0x4000;
        }

        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(SystemMetric smIndex);

        /// <summary>
        /// Flags used with the Windows API (User32.dll):GetSystemMetrics(SystemMetric smIndex)
        ///  
        /// This Enum and declaration signature was written by Gabriel T. Sharp
        /// ai_productions@verizon.net or osirisgothra@hotmail.com
        /// Obtained on pinvoke.net, please contribute your code to support the wiki!
        /// </summary>
        public enum SystemMetric : int
        {
            /// <summary>
            /// The flags that specify how the system arranged minimized windows. For more information, see the Remarks section in this topic.
            /// </summary>
            SM_ARRANGE = 56,

            /// <summary>
            /// The value that specifies how the system is started:
            /// 0 Normal boot
            /// 1 Fail-safe boot
            /// 2 Fail-safe with network boot
            /// A fail-safe boot (also called SafeBoot, Safe Mode, or Clean Boot) bypasses the user startup files.
            /// </summary>
            SM_CLEANBOOT = 67,

            /// <summary>
            /// The number of display monitors on a desktop. For more information, see the Remarks section in this topic.
            /// </summary>
            SM_CMONITORS = 80,

            /// <summary>
            /// The number of buttons on a mouse, or zero if no mouse is installed.
            /// </summary>
            SM_CMOUSEBUTTONS = 43,

            /// <summary>
            /// The width of a window border, in pixels. This is equivalent to the SM_CXEDGE value for windows with the 3-D look.
            /// </summary>
            SM_CXBORDER = 5,

            /// <summary>
            /// The width of a cursor, in pixels. The system cannot create cursors of other sizes.
            /// </summary>
            SM_CXCURSOR = 13,

            /// <summary>
            /// This value is the same as SM_CXFIXEDFRAME.
            /// </summary>
            SM_CXDLGFRAME = 7,

            /// <summary>
            /// The width of the rectangle around the location of a first click in a double-click sequence, in pixels. ,
            /// The second click must occur within the rectangle that is defined by SM_CXDOUBLECLK and SM_CYDOUBLECLK for the system
            /// to consider the two clicks a double-click. The two clicks must also occur within a specified time.
            /// To set the width of the double-click rectangle, call SystemParametersInfo with SPI_SETDOUBLECLKWIDTH.
            /// </summary>
            SM_CXDOUBLECLK = 36,

            /// <summary>
            /// The number of pixels on either side of a mouse-down point that the mouse pointer can move before a drag operation begins.
            /// This allows the user to click and release the mouse button easily without unintentionally starting a drag operation.
            /// If this value is negative, it is subtracted from the left of the mouse-down point and added to the right of it.
            /// </summary>
            SM_CXDRAG = 68,

            /// <summary>
            /// The width of a 3-D border, in pixels. This metric is the 3-D counterpart of SM_CXBORDER.
            /// </summary>
            SM_CXEDGE = 45,

            /// <summary>
            /// The thickness of the frame around the perimeter of a window that has a caption but is not sizable, in pixels.
            /// SM_CXFIXEDFRAME is the height of the horizontal border, and SM_CYFIXEDFRAME is the width of the vertical border.
            /// This value is the same as SM_CXDLGFRAME.
            /// </summary>
            SM_CXFIXEDFRAME = 7,

            /// <summary>
            /// The width of the left and right edges of the focus rectangle that the DrawFocusRectdraws.
            /// This value is in pixels.
            /// Windows 2000:  This value is not supported.
            /// </summary>
            SM_CXFOCUSBORDER = 83,

            /// <summary>
            /// This value is the same as SM_CXSIZEFRAME.
            /// </summary>
            SM_CXFRAME = 32,

            /// <summary>
            /// The width of the client area for a full-screen window on the primary display monitor, in pixels.
            /// To get the coordinates of the portion of the screen that is not obscured by the system taskbar or by application desktop toolbars,
            /// call the SystemParametersInfofunction with the SPI_GETWORKAREA value.
            /// </summary>
            SM_CXFULLSCREEN = 16,

            /// <summary>
            /// The width of the arrow bitmap on a horizontal scroll bar, in pixels.
            /// </summary>
            SM_CXHSCROLL = 21,

            /// <summary>
            /// The width of the thumb box in a horizontal scroll bar, in pixels.
            /// </summary>
            SM_CXHTHUMB = 10,

            /// <summary>
            /// The default width of an icon, in pixels. The LoadIcon function can load only icons with the dimensions
            /// that SM_CXICON and SM_CYICON specifies.
            /// </summary>
            SM_CXICON = 11,

            /// <summary>
            /// The width of a grid cell for items in large icon view, in pixels. Each item fits into a rectangle of size
            /// SM_CXICONSPACING by SM_CYICONSPACING when arranged. This value is always greater than or equal to SM_CXICON.
            /// </summary>
            SM_CXICONSPACING = 38,

            /// <summary>
            /// The default width, in pixels, of a maximized top-level window on the primary display monitor.
            /// </summary>
            SM_CXMAXIMIZED = 61,

            /// <summary>
            /// The default maximum width of a window that has a caption and sizing borders, in pixels.
            /// This metric refers to the entire desktop. The user cannot drag the window frame to a size larger than these dimensions.
            /// A window can override this value by processing the WM_GETMINMAXINFO message.
            /// </summary>
            SM_CXMAXTRACK = 59,

            /// <summary>
            /// The width of the default menu check-mark bitmap, in pixels.
            /// </summary>
            SM_CXMENUCHECK = 71,

            /// <summary>
            /// The width of menu bar buttons, such as the child window close button that is used in the multiple document interface, in pixels.
            /// </summary>
            SM_CXMENUSIZE = 54,

            /// <summary>
            /// The minimum width of a window, in pixels.
            /// </summary>
            SM_CXMIN = 28,

            /// <summary>
            /// The width of a minimized window, in pixels.
            /// </summary>
            SM_CXMINIMIZED = 57,

            /// <summary>
            /// The width of a grid cell for a minimized window, in pixels. Each minimized window fits into a rectangle this size when arranged.
            /// This value is always greater than or equal to SM_CXMINIMIZED.
            /// </summary>
            SM_CXMINSPACING = 47,

            /// <summary>
            /// The minimum tracking width of a window, in pixels. The user cannot drag the window frame to a size smaller than these dimensions.
            /// A window can override this value by processing the WM_GETMINMAXINFO message.
            /// </summary>
            SM_CXMINTRACK = 34,

            /// <summary>
            /// The amount of border padding for captioned windows, in pixels. Windows XP/2000:  This value is not supported.
            /// </summary>
            SM_CXPADDEDBORDER = 92,

            /// <summary>
            /// The width of the screen of the primary display monitor, in pixels. This is the same value obtained by calling 
            /// GetDeviceCaps as follows: GetDeviceCaps( hdcPrimaryMonitor, HORZRES).
            /// </summary>
            SM_CXSCREEN = 0,

            /// <summary>
            /// The width of a button in a window caption or title bar, in pixels.
            /// </summary>
            SM_CXSIZE = 30,

            /// <summary>
            /// The thickness of the sizing border around the perimeter of a window that can be resized, in pixels.
            /// SM_CXSIZEFRAME is the width of the horizontal border, and SM_CYSIZEFRAME is the height of the vertical border.
            /// This value is the same as SM_CXFRAME.
            /// </summary>
            SM_CXSIZEFRAME = 32,

            /// <summary>
            /// The recommended width of a small icon, in pixels. Small icons typically appear in window captions and in small icon view.
            /// </summary>
            SM_CXSMICON = 49,

            /// <summary>
            /// The width of small caption buttons, in pixels.
            /// </summary>
            SM_CXSMSIZE = 52,

            /// <summary>
            /// The width of the virtual screen, in pixels. The virtual screen is the bounding rectangle of all display monitors.
            /// The SM_XVIRTUALSCREEN metric is the coordinates for the left side of the virtual screen.
            /// </summary>
            SM_CXVIRTUALSCREEN = 78,

            /// <summary>
            /// The width of a vertical scroll bar, in pixels.
            /// </summary>
            SM_CXVSCROLL = 2,

            /// <summary>
            /// The height of a window border, in pixels. This is equivalent to the SM_CYEDGE value for windows with the 3-D look.
            /// </summary>
            SM_CYBORDER = 6,

            /// <summary>
            /// The height of a caption area, in pixels.
            /// </summary>
            SM_CYCAPTION = 4,

            /// <summary>
            /// The height of a cursor, in pixels. The system cannot create cursors of other sizes.
            /// </summary>
            SM_CYCURSOR = 14,

            /// <summary>
            /// This value is the same as SM_CYFIXEDFRAME.
            /// </summary>
            SM_CYDLGFRAME = 8,

            /// <summary>
            /// The height of the rectangle around the location of a first click in a double-click sequence, in pixels.
            /// The second click must occur within the rectangle defined by SM_CXDOUBLECLK and SM_CYDOUBLECLK for the system to consider
            /// the two clicks a double-click. The two clicks must also occur within a specified time. To set the height of the double-click
            /// rectangle, call SystemParametersInfo with SPI_SETDOUBLECLKHEIGHT.
            /// </summary>
            SM_CYDOUBLECLK = 37,

            /// <summary>
            /// The number of pixels above and below a mouse-down point that the mouse pointer can move before a drag operation begins.
            /// This allows the user to click and release the mouse button easily without unintentionally starting a drag operation.
            /// If this value is negative, it is subtracted from above the mouse-down point and added below it.
            /// </summary>
            SM_CYDRAG = 69,

            /// <summary>
            /// The height of a 3-D border, in pixels. This is the 3-D counterpart of SM_CYBORDER.
            /// </summary>
            SM_CYEDGE = 46,

            /// <summary>
            /// The thickness of the frame around the perimeter of a window that has a caption but is not sizable, in pixels.
            /// SM_CXFIXEDFRAME is the height of the horizontal border, and SM_CYFIXEDFRAME is the width of the vertical border.
            /// This value is the same as SM_CYDLGFRAME.
            /// </summary>
            SM_CYFIXEDFRAME = 8,

            /// <summary>
            /// The height of the top and bottom edges of the focus rectangle drawn byDrawFocusRect.
            /// This value is in pixels.
            /// Windows 2000:  This value is not supported.
            /// </summary>
            SM_CYFOCUSBORDER = 84,

            /// <summary>
            /// This value is the same as SM_CYSIZEFRAME.
            /// </summary>
            SM_CYFRAME = 33,

            /// <summary>
            /// The height of the client area for a full-screen window on the primary display monitor, in pixels.
            /// To get the coordinates of the portion of the screen not obscured by the system taskbar or by application desktop toolbars,
            /// call the SystemParametersInfo function with the SPI_GETWORKAREA value.
            /// </summary>
            SM_CYFULLSCREEN = 17,

            /// <summary>
            /// The height of a horizontal scroll bar, in pixels.
            /// </summary>
            SM_CYHSCROLL = 3,

            /// <summary>
            /// The default height of an icon, in pixels. The LoadIcon function can load only icons with the dimensions SM_CXICON and SM_CYICON.
            /// </summary>
            SM_CYICON = 12,

            /// <summary>
            /// The height of a grid cell for items in large icon view, in pixels. Each item fits into a rectangle of size
            /// SM_CXICONSPACING by SM_CYICONSPACING when arranged. This value is always greater than or equal to SM_CYICON.
            /// </summary>
            SM_CYICONSPACING = 39,

            /// <summary>
            /// For double byte character set versions of the system, this is the height of the Kanji window at the bottom of the screen, in pixels.
            /// </summary>
            SM_CYKANJIWINDOW = 18,

            /// <summary>
            /// The default height, in pixels, of a maximized top-level window on the primary display monitor.
            /// </summary>
            SM_CYMAXIMIZED = 62,

            /// <summary>
            /// The default maximum height of a window that has a caption and sizing borders, in pixels. This metric refers to the entire desktop.
            /// The user cannot drag the window frame to a size larger than these dimensions. A window can override this value by processing
            /// the WM_GETMINMAXINFO message.
            /// </summary>
            SM_CYMAXTRACK = 60,

            /// <summary>
            /// The height of a single-line menu bar, in pixels.
            /// </summary>
            SM_CYMENU = 15,

            /// <summary>
            /// The height of the default menu check-mark bitmap, in pixels.
            /// </summary>
            SM_CYMENUCHECK = 72,

            /// <summary>
            /// The height of menu bar buttons, such as the child window close button that is used in the multiple document interface, in pixels.
            /// </summary>
            SM_CYMENUSIZE = 55,

            /// <summary>
            /// The minimum height of a window, in pixels.
            /// </summary>
            SM_CYMIN = 29,

            /// <summary>
            /// The height of a minimized window, in pixels.
            /// </summary>
            SM_CYMINIMIZED = 58,

            /// <summary>
            /// The height of a grid cell for a minimized window, in pixels. Each minimized window fits into a rectangle this size when arranged.
            /// This value is always greater than or equal to SM_CYMINIMIZED.
            /// </summary>
            SM_CYMINSPACING = 48,

            /// <summary>
            /// The minimum tracking height of a window, in pixels. The user cannot drag the window frame to a size smaller than these dimensions.
            /// A window can override this value by processing the WM_GETMINMAXINFO message.
            /// </summary>
            SM_CYMINTRACK = 35,

            /// <summary>
            /// The height of the screen of the primary display monitor, in pixels. This is the same value obtained by calling 
            /// GetDeviceCaps as follows: GetDeviceCaps( hdcPrimaryMonitor, VERTRES).
            /// </summary>
            SM_CYSCREEN = 1,

            /// <summary>
            /// The height of a button in a window caption or title bar, in pixels.
            /// </summary>
            SM_CYSIZE = 31,

            /// <summary>
            /// The thickness of the sizing border around the perimeter of a window that can be resized, in pixels.
            /// SM_CXSIZEFRAME is the width of the horizontal border, and SM_CYSIZEFRAME is the height of the vertical border.
            /// This value is the same as SM_CYFRAME.
            /// </summary>
            SM_CYSIZEFRAME = 33,

            /// <summary>
            /// The height of a small caption, in pixels.
            /// </summary>
            SM_CYSMCAPTION = 51,

            /// <summary>
            /// The recommended height of a small icon, in pixels. Small icons typically appear in window captions and in small icon view.
            /// </summary>
            SM_CYSMICON = 50,

            /// <summary>
            /// The height of small caption buttons, in pixels.
            /// </summary>
            SM_CYSMSIZE = 53,

            /// <summary>
            /// The height of the virtual screen, in pixels. The virtual screen is the bounding rectangle of all display monitors.
            /// The SM_YVIRTUALSCREEN metric is the coordinates for the top of the virtual screen.
            /// </summary>
            SM_CYVIRTUALSCREEN = 79,

            /// <summary>
            /// The height of the arrow bitmap on a vertical scroll bar, in pixels.
            /// </summary>
            SM_CYVSCROLL = 20,

            /// <summary>
            /// The height of the thumb box in a vertical scroll bar, in pixels.
            /// </summary>
            SM_CYVTHUMB = 9,

            /// <summary>
            /// Nonzero if User32.dll supports DBCS; otherwise, 0.
            /// </summary>
            SM_DBCSENABLED = 42,

            /// <summary>
            /// Nonzero if the debug version of User.exe is installed; otherwise, 0.
            /// </summary>
            SM_DEBUG = 22,

            /// <summary>
            /// Nonzero if the current operating system is Windows 7 or Windows Server 2008 R2 and the Tablet PC Input
            /// service is started; otherwise, 0. The return value is a bitmask that specifies the type of digitizer input supported by the device.
            /// For more information, see Remarks.
            /// Windows Server 2008, Windows Vista, and Windows XP/2000:  This value is not supported.
            /// </summary>
            SM_DIGITIZER = 94,

            /// <summary>
            /// Nonzero if Input Method Manager/Input Method Editor features are enabled; otherwise, 0.
            /// SM_IMMENABLED indicates whether the system is ready to use a Unicode-based IME on a Unicode application.
            /// To ensure that a language-dependent IME works, check SM_DBCSENABLED and the system ANSI code page.
            /// Otherwise the ANSI-to-Unicode conversion may not be performed correctly, or some components like fonts
            /// or registry settings may not be present.
            /// </summary>
            SM_IMMENABLED = 82,

            /// <summary>
            /// Nonzero if there are digitizers in the system; otherwise, 0. SM_MAXIMUMTOUCHES returns the aggregate maximum of the
            /// maximum number of contacts supported by every digitizer in the system. If the system has only single-touch digitizers,
            /// the return value is 1. If the system has multi-touch digitizers, the return value is the number of simultaneous contacts
            /// the hardware can provide. Windows Server 2008, Windows Vista, and Windows XP/2000:  This value is not supported.
            /// </summary>
            SM_MAXIMUMTOUCHES = 95,

            /// <summary>
            /// Nonzero if the current operating system is the Windows XP, Media Center Edition, 0 if not.
            /// </summary>
            SM_MEDIACENTER = 87,

            /// <summary>
            /// Nonzero if drop-down menus are right-aligned with the corresponding menu-bar item; 0 if the menus are left-aligned.
            /// </summary>
            SM_MENUDROPALIGNMENT = 40,

            /// <summary>
            /// Nonzero if the system is enabled for Hebrew and Arabic languages, 0 if not.
            /// </summary>
            SM_MIDEASTENABLED = 74,

            /// <summary>
            /// Nonzero if a mouse is installed; otherwise, 0. This value is rarely zero, because of support for virtual mice and because
            /// some systems detect the presence of the port instead of the presence of a mouse.
            /// </summary>
            SM_MOUSEPRESENT = 19,

            /// <summary>
            /// Nonzero if a mouse with a horizontal scroll wheel is installed; otherwise 0.
            /// </summary>
            SM_MOUSEHORIZONTALWHEELPRESENT = 91,

            /// <summary>
            /// Nonzero if a mouse with a vertical scroll wheel is installed; otherwise 0.
            /// </summary>
            SM_MOUSEWHEELPRESENT = 75,

            /// <summary>
            /// The least significant bit is set if a network is present; otherwise, it is cleared. The other bits are reserved for future use.
            /// </summary>
            SM_NETWORK = 63,

            /// <summary>
            /// Nonzero if the Microsoft Windows for Pen computing extensions are installed; zero otherwise.
            /// </summary>
            SM_PENWINDOWS = 41,

            /// <summary>
            /// This system metric is used in a Terminal Services environment to determine if the current Terminal Server session is
            /// being remotely controlled. Its value is nonzero if the current session is remotely controlled; otherwise, 0.
            /// You can use terminal services management tools such as Terminal Services Manager (tsadmin.msc) and shadow.exe to
            /// control a remote session. When a session is being remotely controlled, another user can view the contents of that session
            /// and potentially interact with it.
            /// </summary>
            SM_REMOTECONTROL = 0x2001,

            /// <summary>
            /// This system metric is used in a Terminal Services environment. If the calling process is associated with a Terminal Services
            /// client session, the return value is nonzero. If the calling process is associated with the Terminal Services console session,
            /// the return value is 0.
            /// Windows Server 2003 and Windows XP:  The console session is not necessarily the physical console.
            /// For more information, seeWTSGetActiveConsoleSessionId.
            /// </summary>
            SM_REMOTESESSION = 0x1000,

            /// <summary>
            /// Nonzero if all the display monitors have the same color format, otherwise, 0. Two displays can have the same bit depth,
            /// but different color formats. For example, the red, green, and blue pixels can be encoded with different numbers of bits,
            /// or those bits can be located in different places in a pixel color value.
            /// </summary>
            SM_SAMEDISPLAYFORMAT = 81,

            /// <summary>
            /// This system metric should be ignored; it always returns 0.
            /// </summary>
            SM_SECURE = 44,

            /// <summary>
            /// The build number if the system is Windows Server 2003 R2; otherwise, 0.
            /// </summary>
            SM_SERVERR2 = 89,

            /// <summary>
            /// Nonzero if the user requires an application to present information visually in situations where it would otherwise present
            /// the information only in audible form; otherwise, 0.
            /// </summary>
            SM_SHOWSOUNDS = 70,

            /// <summary>
            /// Nonzero if the current session is shutting down; otherwise, 0. Windows 2000:  This value is not supported.
            /// </summary>
            SM_SHUTTINGDOWN = 0x2000,

            /// <summary>
            /// Nonzero if the computer has a low-end (slow) processor; otherwise, 0.
            /// </summary>
            SM_SLOWMACHINE = 73,

            /// <summary>
            /// Nonzero if the current operating system is Windows 7 Starter Edition, Windows Vista Starter, or Windows XP Starter Edition; otherwise, 0.
            /// </summary>
            SM_STARTER = 88,

            /// <summary>
            /// Nonzero if the meanings of the left and right mouse buttons are swapped; otherwise, 0.
            /// </summary>
            SM_SWAPBUTTON = 23,

            /// <summary>
            /// Nonzero if the current operating system is the Windows XP Tablet PC edition or if the current operating system is Windows Vista
            /// or Windows 7 and the Tablet PC Input service is started; otherwise, 0. The SM_DIGITIZER setting indicates the type of digitizer
            /// input supported by a device running Windows 7 or Windows Server 2008 R2. For more information, see Remarks.
            /// </summary>
            SM_TABLETPC = 86,

            /// <summary>
            /// The coordinates for the left side of the virtual screen. The virtual screen is the bounding rectangle of all display monitors.
            /// The SM_CXVIRTUALSCREEN metric is the width of the virtual screen.
            /// </summary>
            SM_XVIRTUALSCREEN = 76,

            /// <summary>
            /// The coordinates for the top of the virtual screen. The virtual screen is the bounding rectangle of all display monitors.
            /// The SM_CYVIRTUALSCREEN metric is the height of the virtual screen.
            /// </summary>
            SM_YVIRTUALSCREEN = 77,
        }
    }
}