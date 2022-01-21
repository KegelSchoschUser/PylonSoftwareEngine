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


            InternalTexture = D3D11GraphicsDevice.Device.CreateTexture2D(width, height, Format.R32G32B32A32_Float, 1, 1, null, BindFlags.RenderTarget | BindFlags.ShaderResource);
        }

        public Texture(ID3D11Texture2D texture)
        {
            InternalTexture = texture;
        }

        public Texture(string FileName)
        {
            using (IWICFormatConverter bs = LoadBitmap(new Vortice.WIC.IWICImagingFactory(), FileName))
            {
                InternalTexture = CreateTexture2DFromBitmap(D3D11GraphicsDevice.Device, bs);
            }
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
            D3D11GraphicsDevice.DeviceContext.GenerateMips(Resource);

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
            InternalTexture.Dispose();
        }

        private static IWICFormatConverter LoadBitmap(IWICImagingFactory factory, string filename)
        {

            IWICBitmapDecoder bitmapDecoder = factory.CreateDecoderFromFileName(
                filename,
                FileAccess.Read,
                DecodeOptions.CacheOnDemand
                );

            IWICFormatConverter result = factory.CreateFormatConverter();


            result.Initialize(
                bitmapDecoder.GetFrame(0),
                PixelFormat.Format32bppRGBA,
                BitmapDitherType.None,
                null,
                0.0,
                BitmapPaletteType.Custom);

            return result;

        }
        private static ID3D11Texture2D CreateTexture2DFromBitmap(ID3D11Device1 device, IWICFormatConverter bitmapSource)
        {

            // Allocate DataStream to receive the WIC image pixels
            int stride = bitmapSource.Size.Width * 4;

            using (DataStream buffer = new DataStream(bitmapSource.Size.Height * stride, true, true))
            {
                // Copy the content of the WIC to the buffer
                bitmapSource.CopyPixels(stride, bitmapSource.Size.Height * stride, buffer.BasePointer);
                return device.CreateTexture2D(new Texture2DDescription()
                {
                    Format = Format.R8G8B8A8_UNorm,
                    ArraySize = 1,
                    MipLevels = 1,
                    Width = bitmapSource.Size.Width,
                    Height = bitmapSource.Size.Height,
                    SampleDescription = new SampleDescription(1, 0),
                    Usage = ResourceUsage.Default,
                    BindFlags = BindFlags.ShaderResource | BindFlags.RenderTarget,
                    CpuAccessFlags = CpuAccessFlags.None,
                    OptionFlags = ResourceOptionFlags.GenerateMips, // ResourceOptionFlags.GenerateMipMap

                }, new SubresourceData[] { new SubresourceData(buffer.BasePointer, stride) });
            }
        }
    }
}
