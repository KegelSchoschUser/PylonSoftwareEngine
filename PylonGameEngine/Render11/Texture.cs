﻿using PylonGameEngine.Mathematics;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.DXGI;

namespace PylonGameEngine.Render11
{
    public class Texture
    {
        public ID3D11Texture2D InternalTexture;
        private ID3D11ShaderResourceView ShaderResourceView;

        public Texture(int width, int height)
        {
            InternalTexture = D3D11GraphicsDevice.Device.CreateTexture2D(width, height, Format.R8G8B8A8_UNorm_SRgb, 1, 1, null, BindFlags.RenderTarget | BindFlags.ShaderResource);
            CreateShaderResourceView();
        }

        public Texture(ID3D11Texture2D texture)
        {
            InternalTexture = texture;
            CreateShaderResourceView();
        }

        public Texture(string FileName)
        {
            InternalTexture = WicBitmap.CreateTexture2D(FileName);
            CreateShaderResourceView();
        }

        public Texture(System.Drawing.Bitmap bitmap)
        {
            InternalTexture = WicBitmap.CreateTexture2D(bitmap);
            CreateShaderResourceView();
        }

        private ID3D11ShaderResourceView CreateShaderResourceView()
        {
            ShaderResourceViewDescription srvDesc = new ShaderResourceViewDescription()
            {
                Format = InternalTexture.Description.Format,
                ViewDimension = ShaderResourceViewDimension.Texture2D,
            };
            srvDesc.Texture2D.MostDetailedMip = 0;
            srvDesc.Texture2D.MipLevels = -1;
            
            var Resource = D3D11GraphicsDevice.Device.CreateShaderResourceView(InternalTexture, srvDesc);
            //D3D11GraphicsDevice.DeviceContext.GenerateMips(Resource);

            return Resource;
        }

        internal ID3D11ShaderResourceView GetShaderResourceView()
        {
            if (ShaderResourceView != null)
                ShaderResourceView.Release();
            ShaderResourceView = CreateShaderResourceView();
            return ShaderResourceView;
        }


        public Vector2 Size
        {
            get
            {
                return new Vector2(InternalTexture.Description.Width, InternalTexture.Description.Height);
            }
        }

        public void Destroy()
        {
            InternalTexture.Release();
            InternalTexture = null;
        }
    }
}
