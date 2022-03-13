using PylonGameEngine.Mathematics;
using System.IO;
using Vortice;
using Vortice.Direct3D11;
using Vortice.DXGI;
using Vortice.WIC;

namespace PylonGameEngine.Render11
{
    //Vortice.DXGI.Format.R8G8B8A8_UNorm_SRgb
    public class WicBitmap
    {
        internal IWICBitmap InternalBitmap;
        public WicBitmap(string FileName)
        {
            var factory = new IWICImagingFactory();
            InternalBitmap = FormatConverterToWicBitmap(factory, CreateFormatConverter(FileName));
        }

        public WicBitmap(System.Drawing.Bitmap bitmap)
        {
            var factory = new IWICImagingFactory();
            InternalBitmap = FormatConverterToWicBitmap(factory, CreateFormatConverter(bitmap));
        }

        public Vector2 Size
        {
            get
            {
                return new Vector2(InternalBitmap.Size.Width, InternalBitmap.Size.Height);
            }
        }

        public void Destroy()
        {
            InternalBitmap.Release();
            InternalBitmap = null;
        }

        private static IWICFormatConverter CreateFormatConverter(System.Drawing.Bitmap bitmap)
        {
            var factory = new Vortice.WIC.IWICImagingFactory();
            var bitmapDecoder = CreateDecoder(factory, bitmap);

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
        private static IWICFormatConverter CreateFormatConverter(string filename)
        {
            var factory = new Vortice.WIC.IWICImagingFactory();
            var bitmapDecoder = CreateDecoder(factory, filename);

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

        private static IWICBitmapDecoder CreateDecoder(IWICImagingFactory factory, System.Drawing.Bitmap bitmap)
        {
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            return factory.CreateDecoderFromStream(
                ms,
                DecodeOptions.CacheOnDemand
                );
        }
        private static IWICBitmapDecoder CreateDecoder(IWICImagingFactory factory, string filename)
        {
            return factory.CreateDecoderFromFileName(
                filename,
                FileAccess.Read,
                DecodeOptions.CacheOnDemand
                );
        }

        private static unsafe IWICBitmap FormatConverterToWicBitmap(IWICImagingFactory factory, IWICFormatConverter formatconverter)
        {
            // Allocate DataStream to receive the WIC image pixels
            int stride = formatconverter.Size.Width * 4;

            using (DataStream buffer = new DataStream(formatconverter.Size.Height * stride, true, true))
            {
                // Copy the content of the WIC to the buffer
                formatconverter.CopyPixels(stride, formatconverter.Size.Height * stride, buffer.BasePointer);

                //  var bitmap = factory.CreateBitmap(formatconverter.Size.Width, formatconverter.Size.Height, BitmapCreateCacheOption.CacheOnDemand);
                return factory.CreateBitmapFromMemory(formatconverter.Size.Width, formatconverter.Size.Height, PixelFormat.Format32bppRGBA, stride, formatconverter.Size.Height * stride, buffer.BasePointer.ToPointer());
            }
        }

        private static ID3D11Texture2D FormatConverterToTexture2D(IWICFormatConverter formatconverter)
        {
            // Allocate DataStream to receive the WIC image pixels
            int stride = formatconverter.Size.Width * 4;

            using (DataStream buffer = new DataStream(formatconverter.Size.Height * stride, true, true))
            {
                // Copy the content of the WIC to the buffer
                formatconverter.CopyPixels(stride, formatconverter.Size.Height * stride, buffer.BasePointer);
                return D3D11GraphicsDevice.Device.CreateTexture2D(new Texture2DDescription()
                {
                    Format = Format.R8G8B8A8_UNorm,
                    ArraySize = 1,
                    MipLevels = 1,
                    Width = formatconverter.Size.Width,
                    Height = formatconverter.Size.Height,
                    SampleDescription = new SampleDescription(1, 0),
                    Usage = ResourceUsage.Default,
                    BindFlags = BindFlags.ShaderResource | BindFlags.RenderTarget,
                    CpuAccessFlags = CpuAccessFlags.None,
                    OptionFlags = ResourceOptionFlags.GenerateMips, // ResourceOptionFlags.GenerateMipMap

                }, new SubresourceData[] { new SubresourceData(buffer.BasePointer, stride) });
            }
        }

        internal static ID3D11Texture2D CreateTexture2D(System.Drawing.Bitmap bitmap)
        {
            return FormatConverterToTexture2D(CreateFormatConverter(bitmap));
        }

        internal static ID3D11Texture2D CreateTexture2D(string filename)
        {
            return FormatConverterToTexture2D(CreateFormatConverter(filename));
        }
    }
}
