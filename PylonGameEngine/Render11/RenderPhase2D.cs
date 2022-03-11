﻿using PylonGameEngine.GameWorld;
using PylonGameEngine.GameWorld3D;
using PylonGameEngine.Mathematics;
using PylonGameEngine.ShaderLibrary;
using PylonGameEngine.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Vortice.D3DCompiler;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.DXGI;


namespace PylonGameEngine.Render11
{
    internal class RenderPhase2D : Renderphase
    {
        private TextureShader renderer = new TextureShader();

        public RenderPhase2D(RenderTexture output, CameraObject rendercamera) : base(ref output, rendercamera)
        {

        }

        internal override Blob CompileVertexShader()
        {
            Compiler.Compile(PylonGameEngine.Resources.Shaders.VertexShader2D, "EntryPoint2D", this.GetType().Name, "vs_4_0", out Blob ShaderByteCode, out Blob ErrorBlob);
            if (ErrorBlob != null)
            {
                Console.Write("ShaderCompileError (2D): " + Encoding.Default.GetString(ErrorBlob.AsBytes()));
            }

            return ShaderByteCode;
        }

        internal override InputElementDescription[] CreateInputLayout()
        {
            InputElementDescription[] InputElements = new InputElementDescription[]
            {
                    new InputElementDescription()
                    {
                        SemanticName = "POSITION",
                        SemanticIndex = 0,
                        Format = Format.R32G32B32_Float,
                        Slot = 0,
                        AlignedByteOffset = 0,
                        Classification = InputClassification.PerVertexData,
                        InstanceDataStepRate = 0
                    },
                    new InputElementDescription()
                    {
                        SemanticName = "TEXCOORD",
                        SemanticIndex = 0,
                        Format = Format.R32G32_Float,
                        Slot = 0,
                        AlignedByteOffset = InputElementDescription.AppendAligned,
                        Classification = InputClassification.PerVertexData,
                        InstanceDataStepRate = 0
                    },
                    new InputElementDescription()
                    {
                        SemanticName = "NORMAL",
                        SemanticIndex = 0,
                        Format = Format.R32G32B32_Float,
                        Slot = 0,
                        AlignedByteOffset = InputElementDescription.AppendAligned,
                        Classification = InputClassification.PerVertexData,
                        InstanceDataStepRate = 0
                    }

            };

            return InputElements;
        }

        internal override void OnRender()
        {
       
            var UIObjects = MyGameWorld.GUI.GetRenderOrder();
            UIObjects.Insert(0, MyGameWorld.DirectController3D);

            var RawObjects = new List<(int, Matrix4x4, Texture)>();
            var Triangles = new List<Triangle>();

            foreach (var obj in UIObjects)
            {
                Mesh mesh = Primitves2D.Quad(obj.Transform.Size, null);
                var triangles = mesh.TriangleData;

                RawObjects.Add((triangles.Count * 3, obj.GlobalMatrix, obj.Graphics.Texture));
                Triangles.AddRange(triangles);
            }
       
            if (RawObjects.Count > 0 && Triangles.Count > 0)
            {
                var Vertices = new Span<RawVertex>(Triangle.ArrayToRawVertices(Triangles).ToArray());
                ID3D11Buffer VertexBuffer = D3D11GraphicsDevice.Device.CreateBuffer(BindFlags.VertexBuffer, Vertices);
                var indices = new Span<int>(Mesh.CreateOrderedIndicesList(Vertices.Length).ToArray());
                ID3D11Buffer IndexBuffer = D3D11GraphicsDevice.Device.CreateBuffer(BindFlags.IndexBuffer, indices);

                D3D11GraphicsDevice.DeviceContext.IASetVertexBuffer(0, VertexBuffer, Marshal.SizeOf(new RawVertex()), 0);
                D3D11GraphicsDevice.DeviceContext.IASetIndexBuffer(IndexBuffer, Format.R32_UInt, 0);
          
                int VertexOffset = 0;
                for (int i = 0; i < RawObjects.Count; i++)
                {
                    Mathematics.Matrix4x4 ObjectMatrix = RawObjects[i].Item2;
                    ObjectMatrix.Transpose();

                    var ObjectMatrixBuffer = CreateStructBuffer(ObjectMatrix);
                    D3D11GraphicsDevice.DeviceContext.VSSetConstantBuffer(1, ObjectMatrixBuffer);

                //    Texture imageTexture = new Texture(RawObjects[i].Item3);
         
                    if (renderer.Textures.Count == 0)
                        renderer.Textures.Add(RawObjects[i].Item3);
                    else
                        renderer.Textures[0] = RawObjects[i].Item3;
                    renderer.InitializeShader(D3D11GraphicsDevice.Device, D3D11GraphicsDevice.DeviceContext);
                    renderer.SetShaderTextures(D3D11GraphicsDevice.Device, D3D11GraphicsDevice.DeviceContext);
                    D3D11GraphicsDevice.DeviceContext.Draw(RawObjects[i].Item1, VertexOffset);
                    VertexOffset += RawObjects[i].Item1;

                  
                   // imageTexture.Destroy();
                    ObjectMatrixBuffer.Release();
                }
                    VertexBuffer.Release();
                    IndexBuffer.Release();
                RawObjects.Clear();
                Triangles.Clear();
            }
        }
    }
}
