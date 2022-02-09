using PylonGameEngine.GameWorld;
using PylonGameEngine.General;
using PylonGameEngine.Mathematics;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.DXGI;
using Vortice.Mathematics;

namespace PylonGameEngine.Render11
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct MatrixBufferStructure
    {
        public Mathematics.Matrix4x4 ViewMatrix;
        public Mathematics.Matrix4x4 ProjectionMatrix;
    }

    public class Renderphase
    {
        internal bool RenderMode3D = true;
        internal bool UseDepth = true;
        internal ID3D11Buffer MatrixBuffer;
        internal ID3D11VertexShader VertexShader;
        internal ID3D11InputLayout InputLayout;

        public RenderTexture RenderTexture;
        public GameObject3D RenderCamera;

        internal Renderphase(ref RenderTexture output, CameraObject renderCamera)
        {
            var VertexShaderByteCode = CompileVertexShader();
            VertexShader = D3D11GraphicsDevice.Device.CreateVertexShader(VertexShaderByteCode);
            InputLayout = D3D11GraphicsDevice.Device.CreateInputLayout(CreateInputLayout(), VertexShaderByteCode);
            VertexShaderByteCode.Dispose();

            RenderTexture = output;
            RenderCamera = renderCamera;
        }

        internal virtual Blob CompileVertexShader()
        {
            return null;
        }

        internal virtual InputElementDescription[] CreateInputLayout()
        {
            return new InputElementDescription[0];
        }

        //public void SetRenderOutput(ref RenderTexture output)
        //{
        //    RenderTexture = output;
        //}

        internal void Render(CameraObject Camera)
        {
            lock (MyGame.RenderLock)
            {
                CreateMatrixBuffer(Camera);

                D3D11GraphicsDevice.DeviceContext.OMSetRenderTargets(RenderTexture.InternalRenderTarget, RenderTexture.DepthStencilView);

                RenderTexture.Clear();

                if (RenderMode3D && UseDepth)
                {
                    D3D11GraphicsDevice.DeviceContext.OMSetDepthStencilState(RenderTexture.DepthStencilStateEnabled);
                }
                else
                {
                    D3D11GraphicsDevice.DeviceContext.OMSetDepthStencilState(RenderTexture.DepthStencilStateDisabled);
                }

                D3D11GraphicsDevice.DeviceContext.IASetInputLayout(InputLayout);
                D3D11GraphicsDevice.DeviceContext.VSSetShader(VertexShader);
                D3D11GraphicsDevice.DeviceContext.VSSetConstantBuffer(0, MatrixBuffer);

                OnRender();
            }
        }

        internal virtual void OnRender()
        {

        }

        private void CreateMatrixBuffer(CameraObject Camera)
        {
            MatrixBufferStructure Matrix = new MatrixBufferStructure();

            if (RenderMode3D)
            {
                var Viewmatrix = Camera.ViewMatrix3D;
                var ProjectionMatrix = Camera.ProjectionMatrix;
                Viewmatrix.Transpose();
                ProjectionMatrix.Transpose();
                Matrix.ViewMatrix = Viewmatrix;
                Matrix.ProjectionMatrix = ProjectionMatrix;
            }
            else
            {
                var Viewmatrix = Camera.ViewMatrix2D;
                var ProjectionMatrix = Camera.OrthographicMatrix;
                Viewmatrix.Transpose();
                ProjectionMatrix.Transpose();
                Matrix.ViewMatrix = Viewmatrix;
                Matrix.ProjectionMatrix = ProjectionMatrix;
            }

            BufferDescription MatrixBufferDescription = new BufferDescription()
            {
                Usage = ResourceUsage.Default,
                SizeInBytes = System.Runtime.InteropServices.Marshal.SizeOf(Matrix),
                BindFlags = BindFlags.ConstantBuffer,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                StructureByteStride = 0
            };
            MatrixBuffer = CreateStructBuffer(Matrix);
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
