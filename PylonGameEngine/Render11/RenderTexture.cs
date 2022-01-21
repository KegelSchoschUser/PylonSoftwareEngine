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

namespace PylonGameEngine.Render11
{
    public class RenderTexture : Texture
    {
        internal ID3D11RenderTargetView InternalRenderTarget;
        internal ID3D11Texture2D DepthStencilBuffer;
        internal ID3D11DepthStencilView DepthStencilView;
        public ID3D11DepthStencilState DepthStencilStateEnabled;
        public ID3D11DepthStencilState DepthStencilStateDisabled;

        public RenderTexture(int width, int height) : base(width, height)
        {
            CreateDepth();
            CreateRenderTarget();
        }

        public RenderTexture(Vector2 size) : base((int)size.X, (int)size.Y)
        {
            CreateDepth();
            CreateRenderTarget();
        }

        private void CreateDepth()
        {
            DepthStencilBuffer = D3D11GraphicsDevice.Device.CreateTexture2D((int)Size.X, (int)Size.Y, Format.D32_Float, 1, 1, null, BindFlags.DepthStencil);

            DepthStencilDescription DepthStencilDesc = new DepthStencilDescription()
            {
                DepthEnable = true,
                DepthWriteMask = DepthWriteMask.All,
                StencilEnable = true,
                StencilReadMask = 0xFF,
                StencilWriteMask = 0xFF,
                DepthFunc = ComparisonFunction.Less,
                // Stencil operation if pixel front-facing.
                FrontFace = new DepthStencilOperationDescription()
                {
                    StencilFailOp = StencilOperation.Keep,
                    StencilDepthFailOp = StencilOperation.Increment,
                    StencilPassOp = StencilOperation.Keep,
                    StencilFunc = ComparisonFunction.Always,
                },
                // Stencil operation if pixel is back-facing.
                BackFace = new DepthStencilOperationDescription()
                {
                    StencilFailOp = StencilOperation.Keep,
                    StencilDepthFailOp = StencilOperation.Decrement,
                    StencilPassOp = StencilOperation.Keep,
                    StencilFunc = ComparisonFunction.Always,
                }
            };

            DepthStencilStateEnabled = D3D11GraphicsDevice.Device.CreateDepthStencilState(DepthStencilDesc);
            DepthStencilDesc.DepthEnable = false;
            DepthStencilStateDisabled = D3D11GraphicsDevice.Device.CreateDepthStencilState(DepthStencilDesc);
            DepthStencilView = D3D11GraphicsDevice.Device.CreateDepthStencilView(DepthStencilBuffer);
        }

        private void CreateRenderTarget()
        {
            InternalRenderTarget = D3D11GraphicsDevice.Device.CreateRenderTargetView(InternalTexture);
        }

        public void Clear()
        {
            D3D11GraphicsDevice.DeviceContext.ClearRenderTargetView(InternalRenderTarget, RGBColor.Transparent);
            D3D11GraphicsDevice.DeviceContext.ClearDepthStencilView(DepthStencilView, DepthStencilClearFlags.Depth, 1f, 0);
        }
    }
}