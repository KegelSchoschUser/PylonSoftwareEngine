using PylonGameEngine.Mathematics;
using PylonGameEngine.Render11;
using PylonGameEngine.SceneManagement.Objects;
using PylonGameEngine.ShaderLibrary.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Vortice.DXGI;

namespace PylonGameEngine.ShaderLibrary.UtilitySteps
{
    internal class VerticalBlur : ShaderStep
    {
        private Texture InputTexture;
        private RenderTexture OutputTexture;
        private Mesh Plane;

        public VerticalBlur(Shader shader, Texture inputTexture, RenderTexture outputTexture)
                           : base(shader,
                                  File.ReadAllText(@"Shaders\Neon\VSverticalblur.hlsl"),
                                  File.ReadAllText(@"Shaders\Neon\PSverticalblur.hlsl"))
        {
            Plane = Primitves2D.Quad(new Vector2(), MyGame.Windows[0].Size, null);
            InputTexture = inputTexture;
            OutputTexture = outputTexture;
        }

        internal override void Render(Camera camera)
        {
            var buffers = Plane.DirectXBuffers[0];

            D3D11GraphicsDevice.DeviceContext.IASetVertexBuffer(0, buffers.Item2, Marshal.SizeOf(new RawVertex()), 0);
            D3D11GraphicsDevice.DeviceContext.IASetIndexBuffer(buffers.Item3, Format.R32_UInt, 0);

            D3D11GraphicsDevice.DeviceContext.VSSetConstantBuffer(1, camera.CameraMatrixBuffer2D);
            D3D11GraphicsDevice.DeviceContext.VSSetConstantBuffer(2, camera.CameraPositionBuffer);


            OutputTexture.OnRender();
            OutputTexture.Clear();
            D3D11GraphicsDevice.DeviceContext.OMSetRenderTargets(OutputTexture.InternalRenderTarget, OutputTexture.DepthStencilView);

            D3D11GraphicsDevice.DeviceContext.PSSetShaderResource(0, InputTexture.GetShaderResourceView());
            //var b = new Bitmap(D3D11GraphicsDevice.ConvertToImage(OutputTexture.InternalTexture));
            //b.Save($@"c:\tmp\{DateTime.Now.Ticks}.png");
            var ObjectMatrixBuffer = D3D11GraphicsDevice.CreateStructBuffer(Matrix4x4.Identity);
            D3D11GraphicsDevice.DeviceContext.VSSetConstantBuffer(3, ObjectMatrixBuffer);

            D3D11GraphicsDevice.DeviceContext.Draw(buffers.Item4 * 3, 0);


            ObjectMatrixBuffer.Release();
        }
    }
}
