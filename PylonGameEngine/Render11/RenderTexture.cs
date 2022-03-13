using PylonGameEngine.Mathematics;
using Vortice.Direct3D11;
using Vortice.DXGI;

namespace PylonGameEngine.Render11
{
    public class RenderTexture : Texture
    {
        internal ID3D11RenderTargetView InternalRenderTarget;
        internal ID3D11Texture2D DepthStencilBuffer;
        internal ID3D11DepthStencilView DepthStencilView;


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

            DepthStencilView = D3D11GraphicsDevice.Device.CreateDepthStencilView(DepthStencilBuffer);
        }

        private void CreateRenderTarget()
        {
            InternalRenderTarget = D3D11GraphicsDevice.Device.CreateRenderTargetView(InternalTexture);
        }

        internal virtual void OnRender()
        {

        }


        public void Clear()
        {
            D3D11GraphicsDevice.DeviceContext.ClearRenderTargetView(InternalRenderTarget, RGBColor.Transparent);
            D3D11GraphicsDevice.DeviceContext.ClearDepthStencilView(DepthStencilView, DepthStencilClearFlags.Depth, 1f, 0);
        }
    }
}