using PylonGameEngine.Utilities;
using SharpGen.Runtime;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Vortice.Direct3D11;
using Vortice.DXGI;
using static PylonGameEngine.Utilities.Win32.User32;

namespace PylonGameEngine.Render11
{
    public class DesktopRenderTarget : RenderTexture
    {
        [Flags]
        public enum SendMessageTimeoutFlags : uint
        {
            SMTO_NORMAL = 0x0,
            SMTO_BLOCK = 0x1,
            SMTO_ABORTIFHUNG = 0x2,
            SMTO_NOTIMEOUTIFNOTHUNG = 0x8,
            SMTO_ERRORONEXIT = 0x20
        }

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessageTimeout(
            IntPtr hWnd,
            uint Msg,
            UIntPtr wParam,
            IntPtr lParam,
            SendMessageTimeoutFlags fuFlags,
            uint uTimeout,
            out UIntPtr lpdwResult);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessageTimeout(
            IntPtr windowHandle,
            uint Msg,
            IntPtr wParam,
            IntPtr lParam,
            SendMessageTimeoutFlags flags,
            uint timeout,
            out IntPtr result);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);
        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr hWndChildAfter, string className, string windowTitle);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        static extern bool ScreenToClient(IntPtr hWnd, ref POINT lpPoint);
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public static implicit operator System.Drawing.Point(POINT p)
            {
                return new System.Drawing.Point(p.X, p.Y);
            }

            public static implicit operator POINT(System.Drawing.Point p)
            {
                return new POINT(p.X, p.Y);
            }

            public override string ToString()
            {
                return X + "        " + Y;
            }
        }


        internal Window Window;
        internal IDXGISwapChain1 SwapChain;
        internal ID3D11Texture2D BackBufferTexture;

        public DesktopRenderTarget(Window window) : base((int)window.Size.X, (int)window.Size.Y)
        {
            Window = window;

            IntPtr result = IntPtr.Zero;

            IntPtr progman = FindWindow("Progman", null);
            // Send 0x052C to Progman. This message directs Progman to spawn a 
            // WorkerW behind the desktop icons. If it is already there, nothing 
            // happens.
            SendMessageTimeout(progman,
                                   0x052C,
                                   new IntPtr(0),
                                   IntPtr.Zero,
                                   SendMessageTimeoutFlags.SMTO_NORMAL,
                                   1000,
                                   out result);






            IntPtr workerw = IntPtr.Zero;


            // We enumerate all Windows, until we find one, that has the SHELLDLL_DefView 
            // as a child. 
            // If we found that window, we take its next sibling and assign it to workerw.
            EnumWindows(new EnumWindowsProc((tophandle, topparamhandle) =>
            {
                IntPtr p = FindWindowEx(tophandle,
                                            IntPtr.Zero,
                                            "SHELLDLL_DefView",
                                            null);

                if (p != IntPtr.Zero)
                {
                    // Gets the WorkerW Window after the current one.
                    workerw = FindWindowEx(IntPtr.Zero,
                                           tophandle,
                                           "WorkerW",
                                           null);
                }

                return true;
            }), IntPtr.Zero);

            CreateSwapChain();
            #region Magic
            //set right form style
            window.FormBorderStyle = FormBorderStyle.None;
            window.WindowState = FormWindowState.Normal;


            //Get the Position/Size of screen
            int posX = (int)MyScreen.CursorScreen_X;
            int posY = (int)MyScreen.CursorScreen_Y;
            int sizeX = (int)MyScreen.CursorScreen_Width;
            int sizeY = (int)MyScreen.CursorScreen_Height;

            //set window position to screen
            SetWindowPos(window.Handle, new IntPtr(1), posX, posY, sizeX, sizeY, SetWindowPosFlags.DoNotActivate);

            //Get window Position relative to WorkerW window
            RECT workerwpos = new RECT();
            MapWindowPoints(window.Handle, workerw, ref workerwpos, 2);

            //Set window Parent to WorkerW window
            SetParent(window.Handle, workerw);

            //set window Postion to position relative to WorkerW window
            SetWindowPos(window.Handle, new IntPtr(1), workerwpos.Left, workerwpos.Top, sizeX, sizeY, SetWindowPosFlags.DoNotActivate);

            //refresh Desktop
            SystemParametersInfo(SPI.SPI_SETDESKWALLPAPER, 0, IntPtr.Zero, SPIF.SPIF_UPDATEINIFILE);

            #endregion Magic

        }

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SystemParametersInfo(SPI uiAction, uint uiParam, IntPtr pvParam, SPIF fWinIni);

        private void CreateSwapChain()
        {
            SwapChainDescription1 swapChainDescription = new SwapChainDescription1()
            {
                Width = (int)Window.Size.X,
                Height = (int)Window.Size.Y,
                Format = Format.R8G8B8A8_UNorm_SRgb,
                BufferCount = 2,
                BufferUsage = Vortice.DXGI.Usage.RenderTargetOutput,
                SampleDescription = new SampleDescription(1, 0),
                Scaling = Scaling.Stretch,
                SwapEffect = SwapEffect.FlipDiscard,
                AlphaMode = AlphaMode.Ignore,
                Flags = SwapChainFlags.AllowModeSwitch
            };
            SwapChainFullscreenDescription fullscreenDescription = new SwapChainFullscreenDescription
            {
                Windowed = true
            };
            SwapChain = D3D11GraphicsDevice.Factory.CreateSwapChainForHwnd(D3D11GraphicsDevice.Device, Window.Handle, swapChainDescription, fullscreenDescription);
            D3D11GraphicsDevice.Factory.MakeWindowAssociation(Window.Handle, WindowAssociationFlags.IgnorePrintScreen);

            BackBufferTexture = SwapChain.GetBuffer<ID3D11Texture2D>(0);
            InternalTexture = BackBufferTexture;
            InternalRenderTarget = D3D11GraphicsDevice.Device.CreateRenderTargetView(BackBufferTexture);
        }

        public void Present()
        {
            Result result = SwapChain.Present(0, PresentFlags.None);

            if (result.Failure
                && result.Code == Vortice.DXGI.ResultCode.DeviceRemoved.Code)
            {
                throw new Exception();
            }
            else if (result.Failure == true)
            {
                //  Console.WriteLine(result.Code);
            }
        }
    }
}

