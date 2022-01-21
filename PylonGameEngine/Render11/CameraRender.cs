
using PylonGameEngine.Billboarding;
using PylonGameEngine.GameWorld;
using PylonGameEngine.GameWorld3D;
using PylonGameEngine.Mathematics;
using PylonGameEngine.ShaderLibrary;
using PylonGameEngine.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;
using Vortice.D3DCompiler;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.DXGI;

namespace PylonGameEngine.Render11
{
    public class CameraRender
    {
        public Renderphase[] RenderPhases;
        private CameraObject Camera;




        public CameraRender(CameraObject cameraObject, ref RenderTexture Rendertarget, bool HasUI = false)
        {
            Camera = cameraObject;

            if (HasUI)
            {
                RenderTexture OutputSkybox = new RenderTexture(MyGame.MainWindow.SizeVec2);
                RenderTexture Output3D = new RenderTexture(MyGame.MainWindow.SizeVec2);
                RenderTexture OutputBillboardOnTop = new RenderTexture(MyGame.MainWindow.SizeVec2);
                RenderTexture Output2D = new RenderTexture(MyGame.MainWindow.SizeVec2);

                List<Texture> Textures = new List<Texture>();
                Textures.Add(OutputSkybox);
                Textures.Add(Output3D);
                Textures.Add(OutputBillboardOnTop);
                Textures.Add(Output2D);

                RenderPhases = new Renderphase[]
                {
                 new RenderPhaseSkybox(new Material("SkyBoxMaterial", new ColorShader(RGBColor.Red)), OutputSkybox, cameraObject) { UseDepth = false },
                 new RenderPhase3D(Output3D, cameraObject),
                 new RenderPhaseBillboardOnTop(OutputBillboardOnTop, cameraObject) { UseDepth = false },
                 new RenderPhase2D(Output2D, cameraObject) { RenderMode3D = false},
                 new RenderPhaseMerger(ref Rendertarget, cameraObject, Textures){ RenderMode3D = false}
                };
            }
            else
            {
                RenderTexture OutputSkybox = new RenderTexture(MyGame.MainWindow.SizeVec2);
                RenderTexture Output3D = new RenderTexture(MyGame.MainWindow.SizeVec2);
                RenderTexture OutputBillboardOnTop = new RenderTexture(MyGame.MainWindow.SizeVec2);
                RenderTexture Output2D = new RenderTexture(MyGame.MainWindow.SizeVec2);

                List<Texture> Textures = new List<Texture>();
                Textures.Add(OutputSkybox);
                Textures.Add(Output3D);
                Textures.Add(OutputBillboardOnTop);
                //Textures.Add(Output2D);

                RenderPhases = new Renderphase[]
                {
                 new RenderPhaseSkybox(new Material("SkyBoxMaterial", new TextureShader(@"E:\Downloads\Skybox.png")), OutputSkybox, cameraObject) { UseDepth = false },
                 new RenderPhase3D(Output3D, cameraObject),
                 new RenderPhaseBillboardOnTop(OutputBillboardOnTop, cameraObject) { UseDepth = false },
                 new RenderPhase2D(Output2D, cameraObject) { RenderMode3D = false},
                 new RenderPhaseMerger(ref Rendertarget, cameraObject, Textures){ RenderMode3D = false}
                };
            }

           

        }

        public void Render()
        {
            if (Camera.Enabled == false)
                return;

            for (int i = 0; i < RenderPhases.Length; i++)
            {
                RenderPhases[i].Render(Camera);
                if (i == RenderPhases.Length - 1)
                {

                    //Console.WriteLine(RenderPhases[i].RenderTexture.InternalTexture.GetHashCode());
                    //Console.WriteLine(MyGame.Materials.Get("Mirror").Shader.Textures[0].InternalTexture.GetHashCode());
                    //MyGame.Materials.Get("Mirror").Shader.Textures[0].InternalTexture = RenderPhases[i].RenderTexture.InternalTexture;
                    //RenderPhases[i].RenderTexture.InternalTexture
                    //Bitmap bmp = new Bitmap(D3D11GraphicsDevice.ConvertToImage(MyGame.Materials.Get("Mirror").Shader.Textures[0].InternalTexture));
                    //bmp.Save(@"Temp\" + DateTime.Now.Ticks + ".bmp");

                }

            }
        }

        public void SetSkyboxColor(RGBColor color)
        {
            ((RenderPhaseSkybox)RenderPhases[0]).SetSkyboxMaterial(new Material("SkyBoxMaterial", new ColorShader(color)));
        }


        public void SetSkyboxMaterial(Material material)
        {
            ((RenderPhaseSkybox)RenderPhases[0]).SetSkyboxMaterial(material);
        }
    }
}