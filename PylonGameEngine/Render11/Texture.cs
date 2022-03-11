using System;
using System.Collections.Generic;
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
using Vortice.WIC;
using System.IO;
using Vortice;

namespace PylonGameEngine.Render11
{
    public class Texture
    {
        public ID3D11Texture2D InternalTexture;

        public Texture(int width, int height)
        {
            InternalTexture = D3D11GraphicsDevice.Device.CreateTexture2D(width, height, Format.R8G8B8A8_UNorm_SRgb, 1, 1, null, BindFlags.RenderTarget | BindFlags.ShaderResource);
        }

        public Texture(ID3D11Texture2D texture)
        {
            InternalTexture = texture;
        }

        public Texture(string FileName)
        {
            InternalTexture = WicBitmap.CreateTexture2D(FileName);
        }

        public Texture(System.Drawing.Bitmap bitmap)
        {
            InternalTexture = WicBitmap.CreateTexture2D(bitmap);
        }

        internal ID3D11ShaderResourceView GetShaderResourceView()
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
