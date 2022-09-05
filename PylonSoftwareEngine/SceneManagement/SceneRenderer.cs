/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

using PylonSoftwareEngine.Mathematics;
using PylonSoftwareEngine.Render11;
using PylonSoftwareEngine.SceneManagement.Objects;
using PylonSoftwareEngine.ShaderLibrary.Core;
using PylonSoftwareEngine.ShaderLibrary.CoreShaders;
using PylonSoftwareEngine.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Media.Media3D;
using Vortice.D3DCompiler;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.DXGI;
using Camera = PylonSoftwareEngine.SceneManagement.Objects.Camera;

namespace PylonSoftwareEngine.SceneManagement
{
    public class SceneRenderer
    {
        public RenderTexture MainRenderTarget
        {
            get
            {
                if(Scene != null)
                    if(Scene.MainCamera != null)
                        if(Scene.MainCamera.RenderTarget != null)
                            return Scene.MainCamera.RenderTarget;
                return null;
            }
        }

        private Scene Scene;

        internal SceneRenderer(Scene scene)
        {
            Scene = scene;

            DepthStencilDescription DepthStencilDesc = new DepthStencilDescription()
            {
                DepthEnable = true,
                DepthWriteMask = DepthWriteMask.All,
                StencilEnable = true,
                StencilReadMask = 0xFF,
                StencilWriteMask = 0xFF,
                DepthFunc = ComparisonFunction.Less,
                // Stencil operation if pixel front-facing.
                FrontFace = new DepthStencilOperationDescription()
                {
                    StencilFailOp = StencilOperation.Keep,
                    StencilDepthFailOp = StencilOperation.Increment,
                    StencilPassOp = StencilOperation.Keep,
                    StencilFunc = ComparisonFunction.Always,
                },
                // Stencil operation if pixel is back-facing.
                BackFace = new DepthStencilOperationDescription()
                {
                    StencilFailOp = StencilOperation.Keep,
                    StencilDepthFailOp = StencilOperation.Decrement,
                    StencilPassOp = StencilOperation.Keep,
                    StencilFunc = ComparisonFunction.Always,
                }
            };

            DepthStencilStateEnabled = D3D11GraphicsDevice.Device.CreateDepthStencilState(DepthStencilDesc);
            DepthStencilDesc.DepthEnable = false;
            DepthStencilStateDisabled = D3D11GraphicsDevice.Device.CreateDepthStencilState(DepthStencilDesc);
        }

        public void Render()
        {
            D3D11GraphicsDevice.DeviceContext.IASetPrimitiveTopology(PrimitiveTopology.TriangleList);
            D3D11GraphicsDevice.TurnOnAlphaBlending();

            foreach (var camera in Scene.Cameras)
            {
                camera.RenderTarget.Clear();
                camera.UpdateBuffers();
            }

            Render3D();
            Render2D();

            foreach (var camera in Scene.Cameras)
            {
                if (camera.Enabled == true)
                    if (camera.RenderTarget is WindowRenderTarget)
                        ((WindowRenderTarget)camera.RenderTarget).Present();
                    else if (camera.RenderTarget is DesktopRenderTarget)
                        ((DesktopRenderTarget)camera.RenderTarget).Present();
            }
        }

        #region 3D
        public void Render3D()
        {
            D3D11GraphicsDevice.TurnOnAlphaBlending();
            D3D11GraphicsDevice.DeviceContext.OMSetDepthStencilState(DepthStencilStateEnabled);

            List<(Material, List<(ID3D11Buffer, ID3D11Buffer, Matrix4x4, int)>)> MaterialsBuffers = new List<(Material, List<(ID3D11Buffer, ID3D11Buffer, Matrix4x4, int)>)>();
            foreach (var material in MySoftware.Materials)
            {
                MaterialsBuffers.Add((material, new List<(ID3D11Buffer, ID3D11Buffer, Matrix4x4, int)>()));
            }

            var SoftwareObjects3D = Scene.GetRenderOrder3D();
            foreach (var obj in SoftwareObjects3D)
            {
                if (obj.Visible == false)
                    continue;
                if (obj is MeshObject)
                {
                    var meshobj = obj as MeshObject;

                    var DirectXBuffers = meshobj.Mesh.DirectXBuffers;

                    foreach (var ValuePair in DirectXBuffers)
                    {
                        var Entry = MaterialsBuffers.Find(x => x.Item1 == ValuePair.Item1);
                        if (Entry.Item1 is null)
                            continue;
                        Entry.Item2.Add((ValuePair.Item2, ValuePair.Item3, obj.Transform.GlobalMatrix, ValuePair.Item4));
                    }
                }
            }

           foreach (var camera in Scene.Cameras)
           {
               if (camera != Scene.MainCamera && camera.Enabled == true)
                   RenderMaterials(camera, MaterialsBuffers); 
           }

            if (Scene.Cameras.Contains(Scene.MainCamera) && Scene.MainCamera.Enabled == true)
                RenderMaterials(Scene.MainCamera, MaterialsBuffers);
        }

        private void RenderMaterials(Camera camera, List<(Material, List<(ID3D11Buffer, ID3D11Buffer, Matrix4x4, int)>)> MaterialsBuffers)
        {
            foreach (var entry in MaterialsBuffers)
            {
                if (entry.Item2.Count == 0)
                    continue;

                entry.Item1.Shader.Render();

                foreach (var obj in entry.Item2)
                {
                    foreach (var step in entry.Item1.Shader.ShaderSteps)
                    {
                        step.Activate();
                        if(step.GetType() == typeof(ShaderStep))
                            step.Render(camera, obj.Item1, obj.Item2, obj.Item3.Transposed, obj.Item4 * 3);
                        else
                            step.Render(camera);
                    }
                }
            }
        }

        #endregion 3D
        
        #region 2D
        private GUIShader textureshader2D = new GUIShader();

        public void Render2D()
        {
            if (MainRenderTarget == null)
                return;

            D3D11GraphicsDevice.DeviceContext.OMSetDepthStencilState(DepthStencilStateDisabled);

            MainRenderTarget.OnRender();

            List<Triangle> Triangles = new List<Triangle>();
            List<(int, Matrix4x4, Texture)> RawObjects = new System.Collections.Generic.List<(int, Matrix4x4, Texture)>();

            foreach (var obj in Scene.Gui.GetRenderOrder())
            {
                var triangles = Primitves2D.Quad(obj.Transform.Size, null).TriangleData;

                Triangles.AddRange(triangles);
                RawObjects.Add((triangles.Count * 3, obj.GlobalMatrix, obj.Graphics.Texture));
            }

            if (RawObjects.Count == 0 || Triangles.Count == 0)
                return;

            var Vertices = new Span<RawVertex>(Triangle.ArrayToRawVertices(Triangles).ToArray());
            var Indices = new Span<int>(Mesh.CreateOrderedIndicesList(Vertices.Length).ToArray());

            ID3D11Buffer VertexBuffer = D3D11GraphicsDevice.Device.CreateBuffer(BindFlags.VertexBuffer, Vertices);
            ID3D11Buffer IndexBuffer = D3D11GraphicsDevice.Device.CreateBuffer(BindFlags.IndexBuffer, Indices);


            textureshader2D.Render();
            textureshader2D.ShaderSteps[0].Activate();

            int VertexOffset = 0;
            for (int i = 0; i < RawObjects.Count; i++)
            {
                Mathematics.Matrix4x4 ObjectMatrix = RawObjects[i].Item2;
                ObjectMatrix.Transpose();

                var ObjectMatrixBuffer = D3D11GraphicsDevice.CreateStructBuffer(ObjectMatrix);
                D3D11GraphicsDevice.DeviceContext.VSSetConstantBuffer(1, ObjectMatrixBuffer);


                if (textureshader2D.Textures.Count == 0)
                    textureshader2D.Textures.Add(RawObjects[i].Item3);
                else
                    textureshader2D.Textures[0] = RawObjects[i].Item3;

                textureshader2D.ShaderSteps[0].Render(Scene.MainCamera, VertexBuffer, IndexBuffer, ObjectMatrix, RawObjects[i].Item1, VertexOffset);
                VertexOffset += RawObjects[i].Item1;

                ObjectMatrixBuffer.Release();
            }

            VertexBuffer.Release();
            IndexBuffer.Release();
            Vertices.Clear();
            Indices.Clear();
            Triangles.Clear();
        }

        #endregion 2D
        
        #region RenderUtilities

        private static ID3D11DepthStencilState DepthStencilStateEnabled;
        private static ID3D11DepthStencilState DepthStencilStateDisabled;
        #endregion RenderUtilities
    }
}
