//using PylonGameEngine.GameWorld;
//using PylonGameEngine.Mathematics;
//using System;
//using System.Runtime.InteropServices;
//using System.Text;
//using Vortice.D3DCompiler;
//using Vortice.Direct3D;
//using Vortice.Direct3D11;
//using Vortice.DXGI;


//namespace PylonGameEngine.Render11
//{
//    internal class RenderPhaseSkybox : Renderphase
//    {
//        [StructLayout(LayoutKind.Sequential)]
//        internal struct CameraPositionBufferStructure
//        {
//            public Mathematics.Vector3 CameraPosition;
//            public float padding;
//        }

//        internal override Blob CompileVertexShader()
//        {
//            Compiler.Compile(PylonGameEngine.Resources.Shaders.VertexShader3D, "EntryPoint3D", this.GetType().Name, "vs_4_0", out Blob ShaderByteCode, out Blob ErrorBlob);
//            if (ErrorBlob != null)
//            {
//                Console.Write("ShaderCompileError (3D): " + Encoding.Default.GetString(ErrorBlob.AsBytes()));
//            }

//            return ShaderByteCode;
//        }

//        internal override InputElementDescription[] CreateInputLayout()
//        {
//            InputElementDescription[] InputElements = new InputElementDescription[]
//            {
//                    new InputElementDescription()
//                    {
//                        SemanticName = "POSITION",
//                        SemanticIndex = 0,
//                        Format = Format.R32G32B32_Float,
//                        Slot = 0,
//                        AlignedByteOffset = 0,
//                        Classification = InputClassification.PerVertexData,
//                        InstanceDataStepRate = 0
//                    },
//                    new InputElementDescription()
//                    {
//                        SemanticName = "TEXCOORD",
//                        SemanticIndex = 0,
//                        Format = Format.R32G32_Float,
//                        Slot = 0,
//                        AlignedByteOffset = InputElementDescription.AppendAligned,
//                        Classification = InputClassification.PerVertexData,
//                        InstanceDataStepRate = 0
//                    },
//                    new InputElementDescription()
//                    {
//                        SemanticName = "NORMAL",
//                        SemanticIndex = 0,
//                        Format = Format.R32G32B32_Float,
//                        Slot = 0,
//                        AlignedByteOffset = InputElementDescription.AppendAligned,
//                        Classification = InputClassification.PerVertexData,
//                        InstanceDataStepRate = 0
//                    }

//            };

//            return InputElements;
//        }

//        internal Material Material;

//        public RenderPhaseSkybox(RenderTexture output, CameraObject rendercamera) : base(ref output, rendercamera)
//        {

//        }

//        public RenderPhaseSkybox(Material material, RenderTexture output, CameraObject rendercamera) : base(ref output, rendercamera)
//        {
//            Material = material;
//        }

//        internal void SetSkyboxMaterial(Material material)
//        {
//            Material = material;
//        }

//        internal override void OnRender()
//        {
//            if (Material == null)
//                return;
//            var CameraPositionBuffer = CreateStructBuffer(new CameraPositionBufferStructure() { CameraPosition = MyGameWorld.ActiveCamera.Transform.WorldPosition });
//            D3D11GraphicsDevice.DeviceContext.VSSetConstantBuffer(1, CameraPositionBuffer);

//            Mesh Mesh = Primitves3D.CreateCube();
//            Mesh.UVs.Clear();
//            Mesh.FlipNormals();

//            float xu = 1f / 4f;
//            float yu = 1f / 3f;


//            //BACK
//            Mesh.UVs.Add(new Vector2(0, 0) + new Vector2(3 * xu, yu));
//            Mesh.UVs.Add(new Vector2(xu, 0) + new Vector2(3 * xu, yu));
//            Mesh.UVs.Add(new Vector2(xu, yu) + new Vector2(3 * xu, yu));
//            Mesh.UVs.Add(new Vector2(0, yu) + new Vector2(3 * xu, yu));

//            //FRONT
//            Mesh.UVs.Add(new Vector2(0, 0) + new Vector2(xu, yu));
//            Mesh.UVs.Add(new Vector2(xu, 0) + new Vector2(xu, yu));
//            Mesh.UVs.Add(new Vector2(xu, yu) + new Vector2(xu, yu));
//            Mesh.UVs.Add(new Vector2(0, yu) + new Vector2(xu, yu));

//            //LEFT
//            Mesh.UVs.Add(new Vector2(0, 0) + new Vector2(0, yu));
//            Mesh.UVs.Add(new Vector2(xu, 0) + new Vector2(0, yu));
//            Mesh.UVs.Add(new Vector2(xu, yu) + new Vector2(0, yu));
//            Mesh.UVs.Add(new Vector2(0, yu) + new Vector2(0, yu));

//            //RIGHT
//            Mesh.UVs.Add(new Vector2(0, 0) + new Vector2(2 * xu, yu));
//            Mesh.UVs.Add(new Vector2(xu, 0) + new Vector2(2 * xu, yu));
//            Mesh.UVs.Add(new Vector2(xu, yu) + new Vector2(2 * xu, yu));
//            Mesh.UVs.Add(new Vector2(0, yu) + new Vector2(2 * xu, yu));

//            //UP
//            Mesh.UVs.Add(new Vector2(0, 0) + new Vector2(xu, 0));
//            Mesh.UVs.Add(new Vector2(xu, 0) + new Vector2(xu, 0));
//            Mesh.UVs.Add(new Vector2(xu, yu) + new Vector2(xu, 0));
//            Mesh.UVs.Add(new Vector2(0, yu) + new Vector2(xu, 0));

//            //DOWN
//            Mesh.UVs.Add(new Vector2(0, 0) + new Vector2(xu, 2 * yu));
//            Mesh.UVs.Add(new Vector2(xu, 0) + new Vector2(xu, 2 * yu));
//            Mesh.UVs.Add(new Vector2(xu, yu) + new Vector2(xu, 2 * yu));
//            Mesh.UVs.Add(new Vector2(0, yu) + new Vector2(xu, 2 * yu));

//            for (int i = 0; i < Mesh.Triangles.Count; i++)
//            {
//                var T = Mesh.Triangles[i];
//                if (i % 2 == 0)
//                {
//                    T.UV1Index = i / 2 * 4 + 0;
//                    T.UV2Index = i / 2 * 4 + 1;
//                    T.UV3Index = i / 2 * 4 + 2;
//                }
//                else
//                {
//                    T.UV1Index = i / 2 * 4 + 3;
//                    T.UV2Index = i / 2 * 4 + 0;
//                    T.UV3Index = i / 2 * 4 + 2;
//                }


//                Mesh.Triangles[i] = T;
//            }

//            var Triangles = Mesh.TriangleData;

//            Material.Shader.InitializeShader(D3D11GraphicsDevice.Device, D3D11GraphicsDevice.DeviceContext);

//            var Vertices = new Span<RawVertex>(Triangle.ArrayToRawVertices(Triangles).ToArray());
//            ID3D11Buffer VertexBuffer = D3D11GraphicsDevice.Device.CreateBuffer(BindFlags.VertexBuffer, Vertices);
//            var indices = new Span<int>(Mesh.CreateOrderedIndicesList(Vertices.Length).ToArray());
//            ID3D11Buffer IndexBuffer = D3D11GraphicsDevice.Device.CreateBuffer(BindFlags.IndexBuffer, indices);

//            D3D11GraphicsDevice.DeviceContext.IASetVertexBuffer(0, VertexBuffer, Marshal.SizeOf(new RawVertex()), 0);
//            D3D11GraphicsDevice.DeviceContext.IASetIndexBuffer(IndexBuffer, Format.R32_UInt, 0);

//            Mathematics.Matrix4x4 ObjectMatrix = Matrix4x4.Translation(MyGameWorld.ActiveCamera.Transform.WorldPosition);
//            ObjectMatrix.Transpose();

//            var ObjectMatrixBuffer = CreateStructBuffer(ObjectMatrix);
//            D3D11GraphicsDevice.DeviceContext.VSSetConstantBuffer(2, ObjectMatrixBuffer);

//            D3D11GraphicsDevice.DeviceContext.Draw(Triangles.Count * 3, 0);

//            ObjectMatrixBuffer.Release();
//            VertexBuffer.Release();
//            IndexBuffer.Release();
//            Triangles.Clear();
//            CameraPositionBuffer.Release();
//        }
//    }
//}