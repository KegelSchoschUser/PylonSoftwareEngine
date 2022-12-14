/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

using System;
using System.Drawing;
using System.IO;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.DXGI;
using Vortice.DXGI.Debug;
using Vortice.Mathematics;
using Vortice.WIC;
using static Vortice.Direct3D11.D3D11;
using static Vortice.DXGI.DXGI;

namespace PylonSoftwareEngine.Render11
{
    public unsafe static class D3D11GraphicsDevice
    {
        private static readonly FeatureLevel[] s_featureLevels = new[]
        {
            FeatureLevel.Level_10_0,
            FeatureLevel.Level_10_1,
            FeatureLevel.Level_11_0,
            FeatureLevel.Level_11_1,
        };

        public static IDXGIFactory2 Factory;
        public static Vortice.Direct2D1.ID2D1Factory7 Factory2D;
        public static ID3D11Device1 Device;

        public static FeatureLevel FeatureLevel;
        public static ID3D11DeviceContext1 DeviceContext;
        public static ID3D11Texture2D OffscreenTexture;

        public static ID3D11RasterizerState RasterState;

        public static ID3D11BlendState AlphaDisableBlendingState;
        public static ID3D11BlendState AlphaEnableBlendingState;


        public static bool IsSupported()
        {
            return true;
        }

        public static void StartListeningForInit()
        {
            // GlobalManager.RenderLoop.Starting += () => { INIT(); };
        }

        public static void INIT()
        {
            if (CreateDXGIFactory1(out Factory).Failure)
            {
                throw new InvalidOperationException("Cannot create IDXGIFactory1");
            }

            Factory2D = Vortice.Direct2D1.D2D1.D2D1CreateFactory<Vortice.Direct2D1.ID2D1Factory7>();

            using (IDXGIAdapter1 adapter = GetHardwareAdapter())
            {
                DeviceCreationFlags creationFlags = DeviceCreationFlags.BgraSupport;

                if (SdkLayersAvailable())
                {
                    creationFlags |= DeviceCreationFlags.Debug;
                }

                if (D3D11CreateDevice(
                    adapter!,
                    DriverType.Unknown,
                    creationFlags,
                    s_featureLevels,
                    out ID3D11Device tempDevice, out FeatureLevel, out ID3D11DeviceContext tempContext).Failure)
                {
                    // If the initialization fails, fall back to the WARP device.
                    // For more information on WARP, see:
                    // http://go.microsoft.com/fwlink/?LinkId=286690
                    D3D11CreateDevice(
                        null,
                        DriverType.Warp,
                        creationFlags,
                        s_featureLevels,
                        out tempDevice, out FeatureLevel, out tempContext).CheckError();
                }

                Device = tempDevice.QueryInterface<ID3D11Device1>();
                DeviceContext = tempContext.QueryInterface<ID3D11DeviceContext1>();
                tempContext.Dispose();
                tempDevice.Dispose();

                // Get the adapter (video card) description.
                AdapterDescription adapterDescription = adapter.Description;


                // Convert the name of the video card to a character array and store it.
                string VideoCardDescription = adapterDescription.Description;
            }


            // Setup the raster description which will determine how and what polygon will be drawn.
            RasterizerDescription rasterDesc = new RasterizerDescription()
            {
                AntialiasedLineEnable = false,
                CullMode = CullMode.Back,
                DepthBias = 0,
                DepthBiasClamp = 0.0f,
                DepthClipEnable = true,
                FillMode = FillMode.Solid,
                FrontCounterClockwise = false,
                MultisampleEnable = false,
                ScissorEnable = false,
                SlopeScaledDepthBias = 0.0f
            };

            // Create the rasterizer state from the description we just filled out.
            RasterState = Device.CreateRasterizerState(rasterDesc);

            // Now set the rasterizer state.
            DeviceContext.RSSetState(RasterState);
            // Setup and create the viewport for rendering.

            BlendDescription blendDescription = new BlendDescription();
            blendDescription.RenderTarget[0].IsBlendEnabled = true;
            //blendDescription.RenderTarget[0].SourceBlend = Blend.SourceAlpha;
            //blendDescription.RenderTarget[0].DestinationBlend = Blend.InverseSourceAlpha;
            //blendDescription.RenderTarget[0].BlendOperation = BlendOperation.Add;
            //blendDescription.RenderTarget[0].SourceBlendAlpha = Blend.One;
            //blendDescription.RenderTarget[0].DestinationBlendAlpha = Blend.Zero;
            //blendDescription.RenderTarget[0].BlendOperationAlpha = BlendOperation.Add;
            //blendDescription.RenderTarget[0].RenderTargetWriteMask = ColorWriteEnable.All;

            blendDescription.RenderTarget[0].SourceBlend = Blend.One;
            blendDescription.RenderTarget[0].DestinationBlend = Blend.InverseSourceAlpha;
            blendDescription.RenderTarget[0].BlendOperation = BlendOperation.Add;
            blendDescription.RenderTarget[0].SourceBlendAlpha = Blend.One;
            blendDescription.RenderTarget[0].DestinationBlendAlpha = Blend.InverseSourceAlpha;
            blendDescription.RenderTarget[0].BlendOperationAlpha = BlendOperation.Add;
            blendDescription.RenderTarget[0].RenderTargetWriteMask = ColorWriteEnable.All;

            // Create the blend state using the description.
            AlphaEnableBlendingState = Device.CreateBlendState(blendDescription);

            // Modify the description to create an disabled blend state description.
            blendDescription.RenderTarget[0].IsBlendEnabled = false;

            // Create the blend state using the description.
            AlphaDisableBlendingState = Device.CreateBlendState(blendDescription);
        }

        public static void TurnOnAlphaBlending()
        {
            // Setup the blend factor.
            Color4 blendFactor = new Color4(0, 0, 0, 0);

            // Turn on the alpha blending.
            DeviceContext.OMSetBlendState(AlphaEnableBlendingState, blendFactor);
        }

        public static void TurnOffAlphaBlending()
        {
            // Setup the blend factor.
            Color4 blendFactor = new Color4(0, 0, 0, 0);

            // Turn on the alpha blending.
            DeviceContext.OMSetBlendState(AlphaDisableBlendingState, blendFactor);
        }

        public static void Dispose()
        {
            AlphaEnableBlendingState?.Dispose();
            AlphaEnableBlendingState = null;
            AlphaDisableBlendingState?.Dispose();
            AlphaDisableBlendingState = null;
            OffscreenTexture?.Dispose();
            DeviceContext.ClearState();
            DeviceContext.Flush();
            DeviceContext.Dispose();
            Device.Dispose();
            Factory.Dispose();

            if (DXGIGetDebugInterface1(out IDXGIDebug1 dxgiDebug).Success)
            {
                dxgiDebug!.ReportLiveObjects(DebugAll, ReportLiveObjectFlags.Summary | ReportLiveObjectFlags.IgnoreInternal);
                dxgiDebug!.Dispose();
            }
        }

        private static IDXGIAdapter1 GetHardwareAdapter()
        {
            IDXGIAdapter1 adapter = null;
            IDXGIFactory6 factory6 = Factory.QueryInterfaceOrNull<IDXGIFactory6>();
            if (factory6 != null)
            {
                for (int adapterIndex = 0;
                    factory6.EnumAdapterByGpuPreference(adapterIndex, GpuPreference.HighPerformance, out adapter).Success;
                    adapterIndex++)
                {
                    if (adapter == null)
                    {
                        continue;
                    }

                    AdapterDescription1 desc = adapter.Description1;

                    if ((desc.Flags & AdapterFlags.Software) != AdapterFlags.None)
                    {
                        // Don't select the Basic Render Driver adapter.
                        adapter.Dispose();
                        continue;
                    }

                    return adapter;
                }


                factory6.Dispose();
            }

            if (adapter == null)
            {
                for (int adapterIndex = 0;
                    Factory.EnumAdapters1(adapterIndex, out adapter).Success;
                    adapterIndex++)
                {
                    AdapterDescription1 desc = adapter.Description1;

                    if ((desc.Flags & AdapterFlags.Software) != AdapterFlags.None)
                    {
                        // Don't select the Basic Render Driver adapter.
                        adapter.Dispose();
                        continue;
                    }

                    return adapter;
                }
            }

            return adapter;
        }





        public static ID3D11Texture2D CaptureTexture(ID3D11Texture2D source)
        {
            ID3D11Texture2D stagingTexture;
            Texture2DDescription desc = source.Description;

            if (desc.ArraySize > 1 || desc.MipLevels > 1)
            {
                Console.WriteLine("WARNING: ScreenGrab does not support 2D arrays, cubemaps, or mipmaps; only the first surface is written. Consider using DirectXTex instead.");
                return null;
            }

            if (desc.SampleDescription.Count > 1)
            {
                // MSAA content must be resolved before being copied to a staging texture
                desc.SampleDescription.Count = 1;
                desc.SampleDescription.Quality = 0;

                ID3D11Texture2D temp = Device.CreateTexture2D(desc);
                Format format = desc.Format;

                FormatSupport formatSupport = Device.CheckFormatSupport(format);

                if ((formatSupport & FormatSupport.MultisampleResolve) == FormatSupport.None)
                {
                    return null;
                }

                for (int item = 0; item < desc.ArraySize; ++item)
                {
                    for (int level = 0; level < desc.MipLevels; ++level)
                    {
                        int index = ID3D11Resource.CalculateSubResourceIndex(level, item, desc.MipLevels);
                        DeviceContext.ResolveSubresource(temp, index, source, index, format);
                    }
                }

                desc.BindFlags = BindFlags.None;
                desc.OptionFlags &= ResourceOptionFlags.TextureCube;
                desc.CpuAccessFlags = CpuAccessFlags.Read;
                desc.Usage = ResourceUsage.Staging;

                stagingTexture = Device.CreateTexture2D(desc);

                DeviceContext.CopyResource(stagingTexture, temp);
            }
            else if ((desc.Usage == ResourceUsage.Staging) && ((desc.CpuAccessFlags & CpuAccessFlags.Read) != CpuAccessFlags.None))
            {
                // Handle case where the source is already a staging texture we can use directly
                stagingTexture = source;
            }
            else
            {
                // Otherwise, create a staging texture from the non-MSAA source
                desc.BindFlags = 0;
                desc.OptionFlags &= ResourceOptionFlags.TextureCube;
                desc.CpuAccessFlags = CpuAccessFlags.Read;
                desc.Usage = ResourceUsage.Staging;

                stagingTexture = Device.CreateTexture2D(desc);

                DeviceContext.CopyResource(stagingTexture, source);
            }

            return stagingTexture;
        }

        public static Image ConvertToImage(ID3D11Texture2D source)
        {
            using (ID3D11Texture2D staging = CaptureTexture(source))
            {
                staging.DebugName = "STAGING";

                var textureDesc = staging!.Description;

                // Determine source format's WIC equivalent
                Guid pfGuid = default;
                bool sRGB = false;
                switch (textureDesc.Format)
                {
                    case Vortice.DXGI.Format.R8G8B8A8_UNorm_SRgb:
                        pfGuid = PixelFormat.Format128bppRGBAFloat;
                        break;

                    //case DXGI_FORMAT_R16G16B16A16_FLOAT: pfGuid = GUID_WICPixelFormat64bppRGBAHalf; break;
                    //case DXGI_FORMAT_R16G16B16A16_UNORM: pfGuid = GUID_WICPixelFormat64bppRGBA; break;
                    //case DXGI_FORMAT_R10G10B10_XR_BIAS_A2_UNORM: pfGuid = GUID_WICPixelFormat32bppRGBA1010102XR; break; // DXGI 1.1
                    //case DXGI_FORMAT_R10G10B10A2_UNORM: pfGuid = GUID_WICPixelFormat32bppRGBA1010102; break;
                    //case DXGI_FORMAT_B5G5R5A1_UNORM: pfGuid = GUID_WICPixelFormat16bppBGRA5551; break;
                    //case DXGI_FORMAT_B5G6R5_UNORM: pfGuid = GUID_WICPixelFormat16bppBGR565; break;
                    //case DXGI_FORMAT_R32_FLOAT: pfGuid = GUID_WICPixelFormat32bppGrayFloat; break;
                    //case DXGI_FORMAT_R16_FLOAT: pfGuid = GUID_WICPixelFormat16bppGrayHalf; break;
                    //case DXGI_FORMAT_R16_UNORM: pfGuid = GUID_WICPixelFormat16bppGray; break;
                    //case DXGI_FORMAT_R8_UNORM: pfGuid = GUID_WICPixelFormat8bppGray; break;
                    //case DXGI_FORMAT_A8_UNORM: pfGuid = GUID_WICPixelFormat8bppAlpha; break;

                    case Vortice.DXGI.Format.R8G8B8A8_UNorm:
                        pfGuid = PixelFormat.Format32bppRGBA;
                        break;

                    case Vortice.DXGI.Format.R32G32B32_Float:
                        pfGuid = PixelFormat.Format32bppRGBA;
                        sRGB = true;
                        break;

                    case Vortice.DXGI.Format.B8G8R8A8_UNorm: // DXGI 1.1
                        pfGuid = PixelFormat.Format32bppBGRA;
                        break;

                    case Vortice.DXGI.Format.B8G8R8A8_UNorm_SRgb: // DXGI 1.1
                        pfGuid = PixelFormat.Format32bppBGRA;
                        sRGB = true;
                        break;

                    case Vortice.DXGI.Format.B8G8R8X8_UNorm: // DXGI 1.1
                        pfGuid = PixelFormat.Format32bppBGR;
                        break;

                    case Vortice.DXGI.Format.B8G8R8X8_UNorm_SRgb: // DXGI 1.1
                        pfGuid = PixelFormat.Format32bppBGR;
                        sRGB = true;
                        break;

                    default:
                        //Console.WriteLine("ERROR: ScreenGrab does not support all DXGI formats (%u). Consider using DirectXTex.\n", static_cast<uint32_t>(desc.Format));
                        throw new Exception("Error in CaptureFrame() !");
                }

                // Screenshots don't typically include the alpha channel of the render target
                Guid targetGuid = default;
                switch (textureDesc.Format)
                {
                    case Vortice.DXGI.Format.R8G8B8A8_UNorm_SRgb:
                    case Vortice.DXGI.Format.R16G16B16A16_Float:
                        //if (_IsWIC2())
                        {
                            targetGuid = PixelFormat.Format96bppRGBFloat;
                        }
                        //else
                        //{
                        //    targetGuid = WICPixelFormat.Format24bppBGR;
                        //}
                        break;

                    case Vortice.DXGI.Format.R16G16B16A16_UNorm:
                        targetGuid = PixelFormat.Format48bppBGR;
                        break;

                    case Vortice.DXGI.Format.B5G5R5A1_UNorm:
                        targetGuid = PixelFormat.Format16bppBGR555;
                        break;

                    case Vortice.DXGI.Format.B5G6R5_UNorm:
                        targetGuid = PixelFormat.Format16bppBGR565;
                        break;

                    case Vortice.DXGI.Format.R32_Float:
                    case Vortice.DXGI.Format.R16_Float:
                    case Vortice.DXGI.Format.R16_UNorm:
                    case Vortice.DXGI.Format.R8_UNorm:
                    case Vortice.DXGI.Format.A8_UNorm:
                        targetGuid = PixelFormat.Format8bppGray;
                        break;

                    default:
                        targetGuid = PixelFormat.Format24bppBGR;
                        break;

                }


                using var wicFactory = new IWICImagingFactory();
                //using IWICBitmapDecoder decoder = wicFactory.CreateDecoderFromFileName(path);

                using Stream stream = new MemoryStream();
                using IWICStream wicStream = wicFactory.CreateStream(stream);
                using IWICBitmapEncoder encoder = wicFactory.CreateEncoder(ContainerFormat.Bmp, wicStream);
                // Create a Frame encoder
                // var props = new SharpGen.Runtime.Win32.PropertyBag();
                var frame = encoder.CreateNewFrame(out var props);
                frame.Initialize(props);
                frame.SetSize(textureDesc.Width, textureDesc.Height);
                frame.SetResolution(72, 72);
                frame.SetPixelFormat(targetGuid);

                //var mapped = context.Map(staging, 0, MapMode.Read, MapFlags.None);
                ReadOnlySpan<Vortice.Mathematics.Color> colors = DeviceContext.Map<Vortice.Mathematics.Color>(staging, 0, 0, MapMode.Read, Vortice.Direct3D11.MapFlags.None);

                // Check conversion
                //R8G8B8A8_UNorm_SRgb
                pfGuid = PixelFormat.Format32bppRGBA;
                if (targetGuid != pfGuid)
                {
                    // Conversion required to write
                    using (IWICBitmap bitmapSource = wicFactory.CreateBitmapFromMemory(
                        textureDesc.Width,
                        textureDesc.Height,
                        pfGuid,
                        colors))
                    {
                        using (IWICFormatConverter formatConverter = wicFactory.CreateFormatConverter())
                        {
                            if (!formatConverter.CanConvert(pfGuid, targetGuid))
                            {
                                DeviceContext.Unmap(staging, 0);
                                throw new Exception("Error in CaptureFrame() !"); ;
                            }

                            formatConverter.Initialize(bitmapSource, targetGuid, BitmapDitherType.None, null, 0, BitmapPaletteType.MedianCut);
                            frame.WriteSource(formatConverter, new Vortice.RawRect(0, 0, textureDesc.Width, textureDesc.Height));
                        }
                    }
                }
                else
                {
                    // No conversion required
                    int stride = PixelFormat.GetStride(pfGuid, textureDesc.Width);
                    frame.WritePixels(textureDesc.Height, stride, colors);
                }

                DeviceContext.Unmap(staging, 0);
                frame.Commit();
                encoder.Commit();
                return Bitmap.FromStream(stream);
            }
        }

        internal static ID3D11Buffer CreateStructBuffer<T>(T ObjectMatrix) where T : unmanaged
        {
            BufferDescription BufferDescription = new BufferDescription()
            {
                Usage = ResourceUsage.Default,
                SizeInBytes = System.Runtime.InteropServices.Marshal.SizeOf(ObjectMatrix),
                BindFlags = BindFlags.ConstantBuffer,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                StructureByteStride = 0
            };

            ID3D11Buffer MatrixBuffer = D3D11GraphicsDevice.Device.CreateBuffer(in ObjectMatrix, BufferDescription);

            return MatrixBuffer;
        }
    }
}