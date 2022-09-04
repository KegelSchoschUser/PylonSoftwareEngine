using PylonGameEngine.Mathematics;
using PylonGameEngine.Render11;
using PylonGameEngine.SceneManagement.Objects;
using PylonGameEngine.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Effects;
using Vortice.D3DCompiler;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.DXGI;
using Vortice.Mathematics;

namespace PylonGameEngine.ShaderLibrary.Core
{
    public class ShaderStep
    {
        private ID3D11InputLayout VertexShaderInputLayout;
        private ID3D11VertexShader VertexShader;
        private ID3D11PixelShader PixelShader;
        private ID3D11SamplerState SamplerState;
        private RenderTexture RenderTexture;

        public Shader Shader;
        public string VertexShaderCode;
        public string PixelShaderCode;
        public bool Is2D = false;

        public ShaderStep(Shader shader, string vertexShaderCode, string pixelShaderCode, RenderTexture RenderOutput = null)
        {
            Shader = shader;
            VertexShaderCode = vertexShaderCode;
            PixelShaderCode = pixelShaderCode;
            RenderTexture = RenderOutput;
        }

        protected ShaderStep(Shader shader, string vertexShaderCode, string pixelShaderCode)
        {
            Shader = shader;
            VertexShaderCode = vertexShaderCode;
            PixelShaderCode = pixelShaderCode;
        }

        internal void Initialize()
        {
            CompileVertexShader(VertexShaderCode);
            CompilePixelShader(PixelShaderCode);

            SamplerDescription samplerDesc = new SamplerDescription()
            {
                Filter = Filter.MinMagMipPoint,
                AddressU = TextureAddressMode.Mirror,
                AddressV = TextureAddressMode.Mirror,
                AddressW = TextureAddressMode.Mirror,
                MipLODBias = 0,
                MaxAnisotropy = 1,
                ComparisonFunction = ComparisonFunction.Always,
                BorderColor = new Color4(0, 0, 0, 0),
                MinLOD = 0,
                MaxLOD = float.MaxValue
            };
            SamplerState = D3D11GraphicsDevice.Device.CreateSamplerState(samplerDesc);
        }

        internal void Activate()
        {
            D3D11GraphicsDevice.DeviceContext.IASetInputLayout(VertexShaderInputLayout);
            D3D11GraphicsDevice.DeviceContext.VSSetShader(VertexShader);
            D3D11GraphicsDevice.DeviceContext.PSSetShader(PixelShader);
            D3D11GraphicsDevice.DeviceContext.PSSetSampler(0, SamplerState);
        }

        internal virtual void Render(Camera camera)
        {

        }

        internal void Render(Camera camera, ID3D11Buffer VertexBuffer, ID3D11Buffer IndexBuffer, Matrix4x4 ObjectMatrix, int Vertices, int VerticesOffset = 0)
        {
            D3D11GraphicsDevice.DeviceContext.IASetVertexBuffer(0, VertexBuffer, Marshal.SizeOf(new RawVertex()), 0);
            D3D11GraphicsDevice.DeviceContext.IASetIndexBuffer(IndexBuffer, Format.R32_UInt, 0);

            if(Is2D == false)
                D3D11GraphicsDevice.DeviceContext.VSSetConstantBuffer(1, camera.CameraMatrixBuffer3D);
            else
                D3D11GraphicsDevice.DeviceContext.VSSetConstantBuffer(1, camera.CameraMatrixBuffer2D);
            D3D11GraphicsDevice.DeviceContext.VSSetConstantBuffer(2, camera.CameraPositionBuffer);



            if (RenderTexture == null)
            {
                camera.RenderTarget.OnRender();
                D3D11GraphicsDevice.DeviceContext.OMSetRenderTargets(camera.RenderTarget.InternalRenderTarget, camera.RenderTarget.DepthStencilView);
            }
            else
            {
                RenderTexture.OnRender();
                RenderTexture.Clear();
                D3D11GraphicsDevice.DeviceContext.OMSetRenderTargets(RenderTexture.InternalRenderTarget, RenderTexture.DepthStencilView);
            }

            var ObjectMatrixBuffer = D3D11GraphicsDevice.CreateStructBuffer(ObjectMatrix);
            D3D11GraphicsDevice.DeviceContext.VSSetConstantBuffer(3, ObjectMatrixBuffer);

            D3D11GraphicsDevice.DeviceContext.Draw(Vertices, VerticesOffset);

            ObjectMatrixBuffer.Release();

        }

        private void CompileVertexShader(string ShaderCode)
        {
            Compiler.Compile(ShaderCode, "Entry", GetType().Name, "vs_4_0", out Blob ShaderByteCode, out Blob ErrorBlob);
            if (ErrorBlob != null)
            {
                MyLog.Default.Write("ShaderCompileError: " + Encoding.Default.GetString(ErrorBlob.AsBytes()), LogSeverity.Critical);
                MyLog.Default.Write("" + ShaderCode, LogSeverity.Critical);

                throw new Exception();
            }

            VertexShaderInputLayout = D3D11GraphicsDevice.Device.CreateInputLayout(Shader.CreateInputLayoutDescription(), ShaderByteCode);

            VertexShader = D3D11GraphicsDevice.Device.CreateVertexShader(ShaderByteCode);
        }

        private void CompilePixelShader(string ShaderCode)
        {
            Compiler.Compile(ShaderCode, "Entry", GetType().Name, "ps_4_0", out Blob ShaderByteCode, out Blob ErrorBlob);
            if (ErrorBlob != null)
            {
                MyLog.Default.Write("ShaderCompileError: " + Encoding.Default.GetString(ErrorBlob.AsBytes()), LogSeverity.Critical);
                MyLog.Default.Write("" + ShaderCode, LogSeverity.Critical);

                throw new Exception();
            }

            PixelShader = D3D11GraphicsDevice.Device.CreatePixelShader(ShaderByteCode);
        }
    }
}
