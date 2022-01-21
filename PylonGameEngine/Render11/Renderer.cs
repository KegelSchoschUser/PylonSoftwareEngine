
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
    public static class Renderer
    {
        public static Renderphase[] RenderPhases;
        internal static Unlicensed Unlicensed;





        public static void Initialize()
        {
            Unlicensed = new Unlicensed();
        }
        private static void UpdateUI()
        {
            var objects = MyGameWorld.GUI.GetRenderOrder();

            foreach (var obj in objects)
            {
                obj.UpdateFrame();
                obj.OnDrawInternal();
            }

            MyGameWorld.DirectController3D.UpdateFrame();
            MyGameWorld.DirectController3D.QueueDraw();
            MyGameWorld.DirectController3D.OnDrawInternal();
            Renderer.Unlicensed.UpdateFrame();
            Renderer.Unlicensed.QueueDraw();
            Renderer.Unlicensed.OnDrawInternal();

            objects.Clear();
        }

        public static void Render()
        {
            if (MyGameWorld.ActiveCamera == null)
                return;
            UpdateUI();
      
            foreach (var camera in WorldManager.CameraObjects)
            {
                if (camera == MyGameWorld.ActiveCamera)
                    continue;
                camera.CameraRender.Render();;
            }

            MyGameWorld.ActiveCamera.CameraRender.Render();

            BillBoard.BillboardObjects.Clear();
        }

        public static void SetSkyboxColor(RGBColor color)
        {
            ((RenderPhaseSkybox)RenderPhases[0]).SetSkyboxMaterial(new Material("SkyBoxMaterial", new ColorShader(color)));
        }
    }
}