using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.DXGI;
using Vortice.Mathematics;

namespace PylonGameEngine.Render11
{
    public class RenderTextureold                   // 31 lines
    {
        // Propertues
        public ID3D11Texture2D RenderTargetTexture;
        public ID3D11RenderTargetView RenderTargetView;
        public ID3D11ShaderResourceView ShaderResourceView;

        public void Initialize(ID3D11Device Device)
        {
            // Initialize and set up the render target description.
            Texture2DDescription RenderTargetTextureDescription = new Texture2DDescription()
            {
                Width = (int)MyGame.MainWindow.Size.X,
                Height = (int)MyGame.MainWindow.Size.Y,
                MipLevels = 1,
                ArraySize = 1,
                Format = Format.R32G32B32A32_Float,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default,
                BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None
            };

            // Create the render target texture.
            RenderTargetTexture = Device.CreateTexture2D(RenderTargetTextureDescription);

            // Initialize and setup the render target view 
            RenderTargetViewDescription renderTargetViewDesc = new RenderTargetViewDescription()
            {
                Format = RenderTargetTextureDescription.Format,
                ViewDimension = RenderTargetViewDimension.Texture2D,
            };
            renderTargetViewDesc.Texture2D.MipSlice = 0;

            // Create the render target view.
            RenderTargetView = Device.CreateRenderTargetView(RenderTargetTexture, renderTargetViewDesc);

            // Initialize and setup the shader resource view 
            ShaderResourceViewDescription shaderResourceViewDesc = new ShaderResourceViewDescription()
            {
                Format = RenderTargetTextureDescription.Format,
                ViewDimension = ShaderResourceViewDimension.Texture2D,
            };
            shaderResourceViewDesc.Texture2D.MipLevels = 1;
            shaderResourceViewDesc.Texture2D.MostDetailedMip = 0;

            // Create the render target view.
            ShaderResourceView = Device.CreateShaderResourceView(RenderTargetTexture, shaderResourceViewDesc);
        }
        // Methods.
        public void Shutdown()
        {
            ShaderResourceView?.Dispose();
            ShaderResourceView = null;
            RenderTargetView?.Dispose();
            RenderTargetView = null;
            RenderTargetTexture?.Dispose();
            RenderTargetTexture = null;
        }


        public void ClearRenderTarget(ID3D11DeviceContext1 context)
        {
            //Color4 clearColor = new Color4(0.10f, 0.10f, 0.10f, 1f);
            //context.ClearRenderTargetView(RenderTargetView, clearColor);
        }
    }
}