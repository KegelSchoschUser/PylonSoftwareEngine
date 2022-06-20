using PylonGameEngine.Mathematics;
using PylonGameEngine.Render11;
using PylonGameEngine.SceneManagement.Objects;
using PylonGameEngine.ShaderLibrary;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Vortice.D3DCompiler;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.DXGI;

namespace PylonGameEngine.SceneManagement
{
    public class SceneRenderer
    {
        [StructLayout(LayoutKind.Sequential)]
        internal struct MatrixBufferStructure
        {
            public Mathematics.Matrix4x4 ViewMatrix;
            public Mathematics.Matrix4x4 ProjectionMatrix;
        }

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
        private ID3D11VertexShader VertexShader3D;
        private ID3D11VertexShader VertexShader2D;
        private ID3D11InputLayout InputLayout3D;
        private ID3D11InputLayout InputLayout2D;

        internal SceneRenderer(Scene scene)
        {
            Scene = scene;

            var VertexShader3DByteCode = CompileVertexShader3D();
            var VertexShader2DByteCode = CompileVertexShader2D();
            VertexShader3D = D3D11GraphicsDevice.Device.CreateVertexShader(VertexShader3DByteCode);
            VertexShader2D = D3D11GraphicsDevice.Device.CreateVertexShader(VertexShader2DByteCode);
            InputLayout3D = D3D11GraphicsDevice.Device.CreateInputLayout(CreateInputLayoutDescription3D(), VertexShader3DByteCode);
            InputLayout2D = D3D11GraphicsDevice.Device.CreateInputLayout(CreateInputLayoutDescription2D(), VertexShader2DByteCode);

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
            int verts = 0;
            D3D11GraphicsDevice.DeviceContext.OMSetDepthStencilState(DepthStencilStateEnabled);
            D3D11GraphicsDevice.DeviceContext.IASetInputLayout(InputLayout3D);
            D3D11GraphicsDevice.DeviceContext.VSSetShader(VertexShader3D);

            foreach (var material in MyGame.Materials)
            {
                List<Triangle> Triangles = new List<Triangle>();
                List<(int, Matrix4x4)> RawObjects = new System.Collections.Generic.List<(int, Matrix4x4)>();

                var GameObjects3D = Scene.GetRenderOrder3D();
                foreach (var obj in GameObjects3D)
                {
                    if (obj.Visible == false)
                        continue;
                    if (obj is MeshObject)
                    {
                        var meshobj = obj as MeshObject;
                        var triangles = meshobj.Mesh.GetTriangles(material);
                    
                        Triangles.AddRange(triangles);
                        RawObjects.Add((triangles.Count * 3, obj.Transform.GlobalMatrix));
                        verts += triangles.Count * 3;
                    }
                }

                if (RawObjects.Count == 0 || Triangles.Count == 0)
                    continue;


                material.Shader.InitializeShader(D3D11GraphicsDevice.Device, D3D11GraphicsDevice.DeviceContext);

                var Vertices = new Span<RawVertex>(Triangle.ArrayToRawVertices(Triangles).ToArray());
                var Indices = new Span<int>(Mesh.CreateOrderedIndicesList(Vertices.Length).ToArray());

                ID3D11Buffer VertexBuffer = D3D11GraphicsDevice.Device.CreateBuffer(BindFlags.VertexBuffer, Vertices);
                ID3D11Buffer IndexBuffer = D3D11GraphicsDevice.Device.CreateBuffer(BindFlags.IndexBuffer, Indices);

                D3D11GraphicsDevice.DeviceContext.IASetVertexBuffer(0, VertexBuffer, Marshal.SizeOf(new RawVertex()), 0);
                D3D11GraphicsDevice.DeviceContext.IASetIndexBuffer(IndexBuffer, Format.R32_UInt, 0);

                foreach (var camera in Scene.Cameras)
                {
                    if (camera != Scene.MainCamera && camera.Enabled == true)
                        RenderCamera3D(camera, RawObjects);
                }

                if (Scene.Cameras.Contains(Scene.MainCamera) && Scene.MainCamera.Enabled == true)
                    RenderCamera3D(Scene.MainCamera, RawObjects);


                VertexBuffer.Release();
                IndexBuffer.Release();
                Vertices.Clear();
                Indices.Clear();
                Triangles.Clear();
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct CameraPositionBufferStructure
        {
            public Mathematics.Vector3 CameraPosition;
            public float padding;
        }

        private void RenderCamera3D(Camera Camera, List<(int, Matrix4x4)> RawObjects)
        {
            Camera.RenderTarget.OnRender();
            D3D11GraphicsDevice.DeviceContext.OMSetRenderTargets(Camera.RenderTarget.InternalRenderTarget, Camera.RenderTarget.DepthStencilView);

            var MatrixBuffer = CreateCameraMatrixBuffer(Camera, true);
            D3D11GraphicsDevice.DeviceContext.VSSetConstantBuffer(0, MatrixBuffer);

            var CameraPositionBuffer = CreateStructBuffer(new CameraPositionBufferStructure() { CameraPosition = Camera.Transform.GlobalPosition });
            D3D11GraphicsDevice.DeviceContext.VSSetConstantBuffer(1, CameraPositionBuffer);

            int VertexOffset = 0;
            for (int i = 0; i < RawObjects.Count; i++)
            {
                Mathematics.Matrix4x4 ObjectMatrix = RawObjects[i].Item2;
                ObjectMatrix.Transpose();

                var ObjectMatrixBuffer = CreateStructBuffer(ObjectMatrix);
                D3D11GraphicsDevice.DeviceContext.VSSetConstantBuffer(2, ObjectMatrixBuffer);
                D3D11GraphicsDevice.DeviceContext.Draw(RawObjects[i].Item1, VertexOffset);
                VertexOffset += RawObjects[i].Item1;
                ObjectMatrixBuffer.Release();
            }

            MatrixBuffer.Release();
            CameraPositionBuffer.Release();
        }

        #endregion 3D

        #region 2D
        private TextureShader textureshader2D = new TextureShader();

        public void Render2D()
        {
            if (MainRenderTarget == null)
                return;
            MainRenderTarget.OnRender();
            D3D11GraphicsDevice.DeviceContext.OMSetRenderTargets(MainRenderTarget.InternalRenderTarget, MainRenderTarget.DepthStencilView);
            D3D11GraphicsDevice.DeviceContext.OMSetDepthStencilState(DepthStencilStateDisabled);
            D3D11GraphicsDevice.DeviceContext.IASetInputLayout(InputLayout2D);
            D3D11GraphicsDevice.DeviceContext.VSSetShader(VertexShader2D);

            var MatrixBuffer = CreateCameraMatrixBuffer(Scene.MainCamera, false);
            D3D11GraphicsDevice.DeviceContext.VSSetConstantBuffer(0, MatrixBuffer);

            List<Triangle> Triangles = new List<Triangle>();
            List<(int, Matrix4x4, Texture)> RawObjects = new System.Collections.Generic.List<(int, Matrix4x4, Texture)>();

            foreach (var obj in Scene.Gui.GetRenderOrder())
            {
                var triangles = Primitves2D.Quad(obj.Transform.Size, null).TriangleData;

                Triangles.AddRange(triangles);
                RawObjects.Add((triangles.Length * 3, obj.GlobalMatrix, obj.Graphics.Texture));
            }

            if (RawObjects.Count == 0 || Triangles.Count == 0)
                return;

            var Vertices = new Span<RawVertex>(Triangle.ArrayToRawVertices(Triangles).ToArray());
            var Indices = new Span<int>(Mesh.CreateOrderedIndicesList(Vertices.Length).ToArray());

            ID3D11Buffer VertexBuffer = D3D11GraphicsDevice.Device.CreateBuffer(BindFlags.VertexBuffer, Vertices);
            ID3D11Buffer IndexBuffer = D3D11GraphicsDevice.Device.CreateBuffer(BindFlags.IndexBuffer, Indices);

            D3D11GraphicsDevice.DeviceContext.IASetVertexBuffer(0, VertexBuffer, Marshal.SizeOf(new RawVertex()), 0);
            D3D11GraphicsDevice.DeviceContext.IASetIndexBuffer(IndexBuffer, Format.R32_UInt, 0);


            int VertexOffset = 0;
            for (int i = 0; i < RawObjects.Count; i++)
            {
                Mathematics.Matrix4x4 ObjectMatrix = RawObjects[i].Item2;
                ObjectMatrix.Transpose();

                var ObjectMatrixBuffer = CreateStructBuffer(ObjectMatrix);
                D3D11GraphicsDevice.DeviceContext.VSSetConstantBuffer(1, ObjectMatrixBuffer);


                if (textureshader2D.Textures.Count == 0)
                    textureshader2D.Textures.Add(RawObjects[i].Item3);
                else
                    textureshader2D.Textures[0] = RawObjects[i].Item3;
                textureshader2D.InitializeShader(D3D11GraphicsDevice.Device, D3D11GraphicsDevice.DeviceContext);
                textureshader2D.SetShaderTextures(D3D11GraphicsDevice.Device, D3D11GraphicsDevice.DeviceContext);

                D3D11GraphicsDevice.DeviceContext.Draw(RawObjects[i].Item1, VertexOffset);
                VertexOffset += RawObjects[i].Item1;

                ObjectMatrixBuffer.Release();
            }

            VertexBuffer.Release();
            IndexBuffer.Release();
            MatrixBuffer.Release();
            Vertices.Clear();
            Indices.Clear();
            Triangles.Clear();
        }

        #endregion 2D

        #region RenderUtilities

        private Blob CompileVertexShader3D()
        {
            Compiler.Compile(PylonGameEngine.Resources.Shaders.VertexShader3D, "EntryPoint3D", this.GetType().Name, "vs_4_0", out Blob ShaderByteCode, out Blob ErrorBlob);
            if (ErrorBlob != null)
            {
                Console.Write("ShaderCompileError (3D): " + Encoding.Default.GetString(ErrorBlob.AsBytes()));
            }

            return ShaderByteCode;
        }

        private Blob CompileVertexShader2D()
        {
            Compiler.Compile(PylonGameEngine.Resources.Shaders.VertexShader2D, "EntryPoint2D", this.GetType().Name, "vs_4_0", out Blob ShaderByteCode, out Blob ErrorBlob);
            if (ErrorBlob != null)
            {
                Console.Write("ShaderCompileError (3D): " + Encoding.Default.GetString(ErrorBlob.AsBytes()));
            }

            return ShaderByteCode;
        }

        private InputElementDescription[] CreateInputLayoutDescription3D()
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

        private InputElementDescription[] CreateInputLayoutDescription2D()
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
                    }

            };

            return InputElements;
        }
        private static ID3D11Buffer CreateCameraMatrixBuffer(Camera Camera, bool RenderMode3D)
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

            return CreateStructBuffer(Matrix);
        }
        private static ID3D11Buffer CreateStructBuffer<T>(T ObjectMatrix) where T : unmanaged
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

        private static ID3D11DepthStencilState DepthStencilStateEnabled;
        private static ID3D11DepthStencilState DepthStencilStateDisabled;
        #endregion RenderUtilities
    }
}
