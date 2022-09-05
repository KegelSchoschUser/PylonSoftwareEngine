using SharpGen.Runtime;
using System;
using System.Diagnostics;
using Vortice.Direct3D11;
using Vortice.DXGI;
using Vortice.Mathematics;

namespace PylonSoftwareEngine.Render11
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
            Window.SizeChanged += (s, e) =>
            {
                lock (MySoftware.RenderLock)
                {
                    Refresh(window.Width, window.Height);
                }
            };
            CreateSwapChain();
        }

        protected override void Refresh(int width, int height)
        {
            lock (MySoftware.RenderLock)
            {
                base.Refresh(width, height);

                SwapChain.ResizeBuffers(0, width, height, Format.Unknown, 0);
                D3D11GraphicsDevice.Factory.MakeWindowAssociation(Window.Handle, WindowAssociationFlags.IgnorePrintScreen);

                if (InternalTexture != null)
                    InternalTexture.Release();

                BackBufferTexture = SwapChain.GetBuffer<ID3D11Texture2D>(0);
                InternalTexture = BackBufferTexture;

                if(InternalRenderTarget != null)
                    InternalRenderTarget.Release();
                InternalRenderTarget = D3D11GraphicsDevice.Device.CreateRenderTargetView(BackBufferTexture);
            }
        }

        private void CreateSwapChain()
        {
            lock (MySoftware.RenderLock)
            {
                SwapChainDescription1 swapChainDescription = new SwapChainDescription1()
                {
                    Width = (int)Window.Size.X,
                    Height = (int)Window.Size.Y,
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


                if (SwapChain != null)
                    SwapChain.Release();

    
                SwapChain = D3D11GraphicsDevice.Factory.CreateSwapChainForHwnd(D3D11GraphicsDevice.Device, Window.Handle, swapChainDescription, fullscreenDescription);
                D3D11GraphicsDevice.Factory.MakeWindowAssociation(Window.Handle, WindowAssociationFlags.IgnorePrintScreen);


                BackBufferTexture = SwapChain.GetBuffer<ID3D11Texture2D>(0);
                InternalTexture = BackBufferTexture;
                InternalRenderTarget = D3D11GraphicsDevice.Device.CreateRenderTargetView(BackBufferTexture);
            }
        }

        internal override void OnRender()
        {
            D3D11GraphicsDevice.DeviceContext.RSSetViewport(new Viewport(0, 0, Window.Size.X, Window.Size.Y));
        }

        internal void Present()
        {
            lock (MySoftware.RenderLock)
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