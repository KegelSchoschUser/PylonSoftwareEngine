﻿using PylonGameEngine.Mathematics;
using PylonGameEngine.Render11;
using PylonGameEngine.SceneManagement.Objects;
using PylonGameEngine.ShaderLibrary.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D11;
using Vortice.DXGI;

namespace PylonGameEngine.ShaderLibrary.CoreSteps
{
    internal class RenderToCameraStep : ShaderStep
    {
        private RenderTexture InputTexture;
        private Mesh Plane;

        public RenderToCameraStep(Shader shader, RenderTexture inputTexture)
                           : base(shader,
                                  File.ReadAllText(@"Shaders\VertexShader2D.hlsl"),
                                  File.ReadAllText(@"Shaders\Textureshader.hlsl"))
        {
            Plane = Primitves2D.Quad(new Vector2(), MyGame.Windows[0].Size, null);
            InputTexture = inputTexture;
        }

        internal override void Render(Camera camera)
        {
            var buffers = Plane.DirectXBuffers[0];
            D3D11GraphicsDevice.DeviceContext.IASetVertexBuffer(0, buffers.Item2, Marshal.SizeOf(new RawVertex()), 0);
            D3D11GraphicsDevice.DeviceContext.IASetIndexBuffer(buffers.Item3, Format.R32_UInt, 0);

            D3D11GraphicsDevice.DeviceContext.VSSetConstantBuffer(1, camera.CameraMatrixBuffer2D);
            D3D11GraphicsDevice.DeviceContext.VSSetConstantBuffer(2, camera.CameraPositionBuffer);


            camera.RenderTarget.OnRender();

            D3D11GraphicsDevice.DeviceContext.OMSetRenderTargets(camera.RenderTarget.InternalRenderTarget, camera.RenderTarget.DepthStencilView);

            D3D11GraphicsDevice.DeviceContext.PSSetShaderResource(0, InputTexture.GetShaderResourceView());

            var ObjectMatrixBuffer = D3D11GraphicsDevice.CreateStructBuffer(Matrix4x4.Identity);
            D3D11GraphicsDevice.DeviceContext.VSSetConstantBuffer(3, ObjectMatrixBuffer);

            D3D11GraphicsDevice.DeviceContext.Draw(buffers.Item4 * 3, 0);

            ObjectMatrixBuffer.Release();
        }
    }
}
