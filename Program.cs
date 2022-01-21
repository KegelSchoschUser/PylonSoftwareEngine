using GIWGameEngine;
using GIWGameEngine.GameWorld;
using GIWGameEngine.Mathematics;
using GIWGameEngine.Render11;
using GIWGameEngine.ShaderLibrary;
using GIWGameEngine.Utils;
using NAudio.CoreAudioApi;
using System;
using System.IO;
using System.Threading.Tasks;
using Vortice.Direct3D11;
using Vortice.Mathematics;
using Vortice.WIC;
using WICPixelFormat = Vortice.WIC.PixelFormat;

namespace MyTestGame
{
    public class MyScript : GameScript
    {
        public override void UpdateTick()
        {
            float multiplier = 4f;
            if (GIWGameEngine.Input.Keyboard.KeyPressed(KeyboardKey.LeftShift))
            {
                multiplier = 16f;
            }

            // ((ColorShader)((MeshObject)this.Parent).Mesh.Materials[0].Shader).Input.Color = RGBColor.GetRandomColor();
            if (GIWGameEngine.Input.Keyboard.KeyPressed(KeyboardKey.Numpad8))
            {
                float radians = this.Parent.Rotation.Y * 0.0174532925f;
                // Update the position.

                this.Parent.Position.X += (float)System.Math.Sin(radians) * MyGame.GameTickLoop.DeltaTime * multiplier;
                this.Parent.Position.Z += (float)System.Math.Cos(radians) * MyGame.GameTickLoop.DeltaTime * multiplier;
            }

            if (GIWGameEngine.Input.Keyboard.KeyPressed(KeyboardKey.Numpad2))
            {
                float radians = this.Parent.Rotation.Y * 0.0174532925f;
                // Update the position.
                this.Parent.Position.X -= (float)System.Math.Sin(radians) * MyGame.GameTickLoop.DeltaTime * multiplier;
                this.Parent.Position.Z -= (float)System.Math.Cos(radians) * MyGame.GameTickLoop.DeltaTime * multiplier;

            }

            if (GIWGameEngine.Input.Keyboard.KeyPressed(KeyboardKey.Numpad4))
            {
                float radians = this.Parent.Rotation.Y * 0.0174532925f;


                this.Parent.Position -= Vector3.left(new Vector3((float)System.Math.Sin(radians), 0, (float)System.Math.Cos(radians))) * MyGame.GameTickLoop.DeltaTime * multiplier;
            }

            if (GIWGameEngine.Input.Keyboard.KeyPressed(KeyboardKey.Numpad6))
            {
                float radians = this.Parent.Rotation.Y * 0.0174532925f;
                this.Parent.Position += Vector3.left(new Vector3((float)System.Math.Sin(radians), 0, (float)System.Math.Cos(radians))) * MyGame.GameTickLoop.DeltaTime * multiplier;
            }

            if (GIWGameEngine.Input.Keyboard.KeyPressed(KeyboardKey.Numpad9))
            {
                this.Parent.Rotation.Y += 5 * MyGame.GameTickLoop.DeltaTime * 7 * multiplier;
            }

            if (GIWGameEngine.Input.Keyboard.KeyPressed(KeyboardKey.Numpad7))
            {
                this.Parent.Rotation.Y -= 5 * MyGame.GameTickLoop.DeltaTime * 7 * multiplier;

            }

            if (GIWGameEngine.Input.Keyboard.KeyPressed(KeyboardKey.Numpad1))
            {
                this.Parent.Scale += new Vector3(-1, 0, 0) * MyGame.GameTickLoop.DeltaTime * multiplier;
            }

            if (GIWGameEngine.Input.Keyboard.KeyPressed(KeyboardKey.Numpad3))
            {
                this.Parent.Scale += new Vector3(1, 0, 0) * MyGame.GameTickLoop.DeltaTime * multiplier;

            }

            if (GIWGameEngine.Input.Keyboard.KeyPressed(KeyboardKey.W))
            {
                // Convert degrees to radians.
                float radians = MyGameWorld.ActiveCamera.Rotation.Y * 0.0174532925f;
                // Update the position.
               MyGameWorld.ActiveCamera.Position.X += (float)System.Math.Sin(radians) * MyGame.GameTickLoop.DeltaTime * multiplier;
               MyGameWorld.ActiveCamera.Position.Z += (float)System.Math.Cos(radians) * MyGame.GameTickLoop.DeltaTime * multiplier;
                //MyGame.Camera.Position.Z += .5f;

            }
            if (GIWGameEngine.Input.Keyboard.KeyPressed(KeyboardKey.S))
            {
                // Convert degrees to radians.
                float radians =MyGameWorld.ActiveCamera.Rotation.Y * 0.0174532925f;

                // Update the position.
               MyGameWorld.ActiveCamera.Position.X -= (float)System.Math.Sin(radians) * MyGame.GameTickLoop.DeltaTime * multiplier;
               MyGameWorld.ActiveCamera.Position.Z -= (float)System.Math.Cos(radians) * MyGame.GameTickLoop.DeltaTime * multiplier;

            }
            if (GIWGameEngine.Input.Keyboard.KeyPressed(KeyboardKey.A))
            {
                float radians =MyGameWorld.ActiveCamera.Rotation.Y * 0.0174532925f;


               MyGameWorld.ActiveCamera.Position -= Vector3.left(new Vector3((float)System.Math.Sin(radians), 0, (float)System.Math.Cos(radians))) * MyGame.GameTickLoop.DeltaTime * multiplier;


            }
            if (GIWGameEngine.Input.Keyboard.KeyPressed(KeyboardKey.D))
            {

                float radians =MyGameWorld.ActiveCamera.Rotation.Y * 0.0174532925f;


               MyGameWorld.ActiveCamera.Position -= Vector3.right(new Vector3((float)System.Math.Sin(radians), 0, (float)System.Math.Cos(radians))) * MyGame.GameTickLoop.DeltaTime * multiplier;

            }
            if (GIWGameEngine.Input.Keyboard.KeyPressed(KeyboardKey.R))
            {
               MyGameWorld.ActiveCamera.Position.Y += 1 * MyGame.GameTickLoop.DeltaTime * multiplier;


            }

            if (GIWGameEngine.Input.Keyboard.KeyPressed(KeyboardKey.F))
            {
               MyGameWorld.ActiveCamera.Position.Y -= 1 * MyGame.GameTickLoop.DeltaTime * multiplier;

            }


            if (GIWGameEngine.Input.Keyboard.KeyPressed(KeyboardKey.Right))
            {
               MyGameWorld.ActiveCamera.Rotation.Y += 5 * MyGame.GameTickLoop.DeltaTime * 7;

            }
            if (GIWGameEngine.Input.Keyboard.KeyPressed(KeyboardKey.Left))
            {
               MyGameWorld.ActiveCamera.Rotation.Y -= 5 * MyGame.GameTickLoop.DeltaTime * 7;



            }
            if (GIWGameEngine.Input.Keyboard.KeyPressed(KeyboardKey.Up))
            {
               MyGameWorld.ActiveCamera.Rotation.X -= 5 * MyGame.GameTickLoop.DeltaTime * 7;

            }
            if (GIWGameEngine.Input.Keyboard.KeyPressed(KeyboardKey.Down))
            {
               MyGameWorld.ActiveCamera.Rotation.X += 5 * MyGame.GameTickLoop.DeltaTime * 7;

            }
        }
    }

    public static class Program
    {
        public static void Main()
        {
#if !DEBUG
            MyGame.HideConsole();
#endif
            MyGame.Initialize(new MyGameOptions() { AppName = "GIW TEST", Version = new MyVersion(1, 0, 0), WindowSize = new System.Drawing.Size(1920, 1080) });
            new D3D11GraphicsDevice();

            MeshObject Object = new MeshObject(@"Cube.obj", true);
            ((ColorShader)Object.Mesh.Materials[0].Shader).Input.Color = RGBColor.GetRandomColor();
            Object.AddScript(new MyScript());
            MyGameWorld.Objects.Add(Object);

            MeshObject Object2 = new MeshObject(@"Cube.obj", true);
            ((ColorShader)Object2.Mesh.Materials[0].Shader).Input.Color = RGBColor.GetRandomColor();
            Object2.Position = new Vector3(5, 0, 0);
            Object.AddObject(Object2);

            CameraObject Camera1 = new CameraObject();
            //MyGameWorld.Objects.Add(Camera1);
            Object.AddObject(Camera1);
            Camera1.Activate();

            MeshObject Object3 = new MeshObject(@"Cube.obj", true);
            MyGameWorld.Objects.Add(Object3);

            MyGameWorld.ActiveCamera.Position = new Vector3(0, 85, 0);
            MyGameWorld.ActiveCamera.Rotation = new Vector3(33, -90, 0);

            // NAudio.Start();
            MyGame.Start();

            MyLog.Dafault.Write("PROGRAMM ENDED!", LogSeverity.Crash);
        }



        public static GIWGameEngine.Mathematics.Vector3 scale = new GIWGameEngine.Mathematics.Vector3();
        public static float fl = 0;

        public static MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
        public static float Abstand = 2.5f;


        public static float?[] Values = new float?[100];
        public static float Master = 0f;
        private static void NAudio_Tick(GameLoop sender)
        {
            Master = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console).AudioMeterInformation.MasterPeakValue * 20f;
            Parallel.For(0, enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console).AudioMeterInformation.PeakValues.Count, i =>
            {
                Values[i] = (enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console).AudioMeterInformation.PeakValues[i] * 1000f);
            });


            //   return;
            fl += 5f * MyGame.GameTickLoop.DeltaTime * Master;

           MyGameWorld.ActiveCamera.Position.Z = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console).AudioMeterInformation.PeakValues.Count * 20 / 4;

            float posX = (MyGameWorld.Objects.Count) * Abstand - 43;
           MyGameWorld.ActiveCamera.Position.X = (MyGameWorld.Objects.Count) * Abstand;
            Parallel.For(0, Values.Length, i =>
            {
                if (Values[i] != null)
                {
                    MeshObject Stripe = new MeshObject();
                    Stripe.Mesh = new Mesh(((MeshObject)MyGameWorld.Objects[0]).Mesh);


                    Stripe.Position = new Vector3(posX, 0, i * 20);

                    if (Stripe.Position.Z > 0f)
                    {
                        Stripe.Position.Z += Master * 8;
                    }
                    else
                    {
                        Stripe.Position.Z -= Master * 8;
                    }



                    Random r = new Random();
                    Stripe.Rotation.Y = (fl) % 360;
                    //scale.Y = ;
                    Stripe.Scale.Y = (float)(Values[i] / 10);

                    MyGameWorld.Objects.Add(Stripe);
                }
            });

            while (MyGameWorld.Objects.Count > 500)
            {
                MyGameWorld.Objects.RemoveAt(1);
            }

        }



        #region screenshot
        public static unsafe void SaveScreenshot(string path, ContainerFormat format = ContainerFormat.Png)
        {

            ID3D11Texture2D source = MyGame.GameOptions.Headless ? D3D11GraphicsDevice.OffscreenTexture : D3D11GraphicsDevice.BackBufferTexture;

            using (ID3D11Texture2D staging = D3D11GraphicsDevice.CaptureTexture(source))
            {

                staging.DebugName = "STAGING";

                var textureDesc = staging!.Description;

                // Determine source format's WIC equivalent
                Guid pfGuid = default;
                //bool sRGB = false;
                switch (textureDesc.Format)
                {
                    case Vortice.DXGI.Format.R32G32B32A32_Float:
                        pfGuid = WICPixelFormat.Format128bppRGBAFloat;
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
                        pfGuid = WICPixelFormat.Format32bppRGBA;
                        break;

                    case Vortice.DXGI.Format.R8G8B8A8_UNorm_SRgb:
                        pfGuid = WICPixelFormat.Format32bppRGBA;
                        //sRGB = true;
                        break;

                    case Vortice.DXGI.Format.B8G8R8A8_UNorm: // DXGI 1.1
                        pfGuid = WICPixelFormat.Format32bppBGRA;
                        break;

                    case Vortice.DXGI.Format.B8G8R8A8_UNorm_SRgb: // DXGI 1.1
                        pfGuid = WICPixelFormat.Format32bppBGRA;
                        //sRGB = true;
                        break;

                    case Vortice.DXGI.Format.B8G8R8X8_UNorm: // DXGI 1.1
                        pfGuid = WICPixelFormat.Format32bppBGR;
                        break;

                    case Vortice.DXGI.Format.B8G8R8X8_UNorm_SRgb: // DXGI 1.1
                        pfGuid = WICPixelFormat.Format32bppBGR;
                        //sRGB = true;
                        break;

                    case Vortice.DXGI.Format.D24_UNorm_S8_UInt:
                        pfGuid = WICPixelFormat.Format32bppRGBA;
                        //sRGB = true;
                        break;
                    default:
                        Console.WriteLine("Screenhot Error");
                        return;
                }
;
                // Screenshots don't typically include the alpha channel of the render target
                Guid targetGuid = default;
                switch (textureDesc.Format)
                {
                    case Vortice.DXGI.Format.R32G32B32A32_Float:
                    case Vortice.DXGI.Format.R16G16B16A16_Float:
                        //if (_IsWIC2())
                        {
                            targetGuid = WICPixelFormat.Format96bppRGBFloat;
                        }
                        //else
                        //{
                        //    targetGuid = WICPixelFormat.Format24bppBGR;
                        //}
                        break;

                    case Vortice.DXGI.Format.R16G16B16A16_UNorm:
                        targetGuid = WICPixelFormat.Format48bppBGR;
                        break;

                    case Vortice.DXGI.Format.B5G5R5A1_UNorm:
                        targetGuid = WICPixelFormat.Format16bppBGR555;
                        break;

                    case Vortice.DXGI.Format.B5G6R5_UNorm:
                        targetGuid = WICPixelFormat.Format16bppBGR565;
                        break;

                    case Vortice.DXGI.Format.R32_Float:
                    case Vortice.DXGI.Format.R16_Float:
                    case Vortice.DXGI.Format.R16_UNorm:
                    case Vortice.DXGI.Format.R8_UNorm:
                    case Vortice.DXGI.Format.A8_UNorm:
                        targetGuid = WICPixelFormat.Format8bppGray;
                        break;

                    default:
                        targetGuid = WICPixelFormat.Format24bppBGR;
                        break;
                }

                using var wicFactory = new IWICImagingFactory();
                //using IWICBitmapDecoder decoder = wicFactory.CreateDecoderFromFileName(path);


                using Stream stream = File.OpenWrite(path);
                using IWICStream wicStream = wicFactory.CreateStream(stream);
                using IWICBitmapEncoder encoder = wicFactory.CreateEncoder(format, wicStream);
                // Create a Frame encoder
                var props = new SharpGen.Runtime.Win32.PropertyBag();
                var frame = encoder.CreateNewFrame(props);
                frame.Initialize(props);
                frame.SetSize(textureDesc.Width, textureDesc.Height);
                frame.SetResolution(72, 72);
                frame.SetPixelFormat(targetGuid);

                var context = D3D11GraphicsDevice.DeviceContext;
                //var mapped = context.Map(staging, 0, MapMode.Read, MapFlags.None);
                Span<Color> colors = context.Map<Color>(staging, 0, 0, MapMode.Read, MapFlags.None);

                // Check conversion
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
                                context.Unmap(staging, 0);
                                return;
                            }

                            formatConverter.Initialize(bitmapSource, targetGuid, BitmapDitherType.None, null, 0, BitmapPaletteType.MedianCut);
                            frame.WriteSource(formatConverter, new System.Drawing.Rectangle(0, 0, textureDesc.Width, textureDesc.Height));
                        }
                    }
                }
                else
                {
                    // No conversion required
                    int stride = WICPixelFormat.GetStride(pfGuid, textureDesc.Width);
                    frame.WritePixels(textureDesc.Height, stride, colors);
                }

                context.Unmap(staging, 0);
                frame.Commit();
                encoder.Commit();

            }
        }
        #endregion
    }
}
