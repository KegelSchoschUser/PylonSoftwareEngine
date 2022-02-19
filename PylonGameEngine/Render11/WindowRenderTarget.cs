using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PylonGameEngine.GameWorld;
using PylonGameEngine.General;
using PylonGameEngine.Mathematics;
using System;
using System.Runtime.InteropServices;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.DXGI;
using Vortice.Mathematics;
using SharpGen.Runtime;
using System.Diagnostics;

namespace PylonGameEngine.Render11
{
    public class WindowRenderTarget : RenderTexture
    {
        internal Window Window;
        internal IDXGISwapChain1 SwapChain;
        internal ID3D11Texture2D BackBufferTexture;

        public WindowRenderTarget(Window window) : base((int)window.Size.X, (int)window.Size.Y)
        {
            Window = window;

            //D3D11GraphicsDevice.Factory.MakeWindowAssociation(hwnd, WindowAssociationFlags.IgnorePrintScreen);
            CreateSwapChain();
        }

        private void CreateSwapChain()
        {
            SwapChainDescription1 swapChainDescription = new SwapChainDescription1()
            {
                Width = (int)MyGame.MainWindow.Size.X,
                Height = (int)MyGame.MainWindow.Size.Y,
                Format = Format.R8G8B8A8_UNorm,
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
            SwapChain = D3D11GraphicsDevice.Factory.CreateSwapChainForHwnd(D3D11GraphicsDevice.Device, MyGame.MainWindow.Handle, swapChainDescription, fullscreenDescription);
            D3D11GraphicsDevice.Factory.MakeWindowAssociation(MyGame.MainWindow.Handle, WindowAssociationFlags.IgnorePrintScreen);

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

//Create Window for Desktop Background
/*          IntPtr result = IntPtr.Zero;

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


            hwnd = workerw;
            SetParent(hwnd, workerw);
*/