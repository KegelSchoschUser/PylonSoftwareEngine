using PylonGameEngine.Billboarding;
using PylonGameEngine.GameWorld;
using PylonGameEngine.GameWorld3D;
using PylonGameEngine.Mathematics;
using PylonGameEngine.ShaderLibrary;
using PylonGameEngine.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Vortice.D3DCompiler;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.DXGI;


namespace PylonGameEngine.Render11
{
    internal class RenderPhase3D : Renderphase
    {
        public RenderPhase3D(RenderTexture output, CameraObject rendercamera) : base(ref output, rendercamera)
        {

        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct CameraPositionBufferStructure
        {
            public Mathematics.Vector3 CameraPosition;
            public float padding;
        }


        internal override Blob CompileVertexShader()
        {
            Compiler.Compile(PylonGameEngine.Resources.Shaders.VertexShader3D, "EntryPoint3D", this.GetType().Name, "vs_4_0", out Blob ShaderByteCode, out Blob ErrorBlob);
            if (ErrorBlob != null)
            {
                Console.Write("ShaderCompileError (3D): " + Encoding.Default.GetString(ErrorBlob.GetBytes()));
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
            var CameraPositionBuffer = CreateStructBuffer(new CameraPositionBufferStructure() { CameraPosition = MyGameWorld.ActiveCamera.Transform.Position });
            D3D11GraphicsDevice.DeviceContext.VSSetConstantBuffer(1, CameraPositionBuffer);

            var GameObjects = MyGameWorld.GetRenderOrder();


            foreach (var CurrentMaterial in MyGame.Materials)
            {
                var RawObjects = new List<(int, Matrix4x4)>();
                var Triangles = new List<Triangle>();
                foreach (var obj in GameObjects)
                {
                    if (((MeshObject)obj).ExcludedCameras.Contains(RenderCamera))
                        continue;
                    if (obj is MeshObject)
                    {
                        Mesh mesh = ((MeshObject)obj).Mesh;
                        
                        if (mesh == null)
                            continue;
                        var triangles = mesh.GetTriangles(CurrentMaterial);
                        if (triangles.Count == 0)
                            continue;
                        if (mesh.EnableBoundingBox == true)
                        {
                            BoundingBox boundingBox = mesh.GetBoundingBox(obj.Transform.GlobalMatrix.Transposed);

                            bool FirstinView = MyGameWorld.ActiveCamera.PointInView(boundingBox.Min);
                            bool SecondinView = MyGameWorld.ActiveCamera.PointInView(boundingBox.Max);

                            if (FirstinView == false && SecondinView == false)
                            {
                               // continue;
                            }
                        }
                        RawObjects.Add((triangles.Count * 3, obj.Transform.GlobalMatrix));
                        Triangles.AddRange(triangles);
                    }
                }

                foreach (var obj in BillBoard.BillboardObjects)
                {
                    if (obj.OnTop == true)
                        continue;
                    Mesh mesh = ((BillboardObject)obj).GetMesh(MyGameWorld.ActiveCamera.Transform.WorldPosition);
                    if (obj.Material != CurrentMaterial)
                        continue;
                    if (mesh == null)
                        continue;
                    var triangles = mesh.GetTriangles(CurrentMaterial);


                    RawObjects.Add((triangles.Count * 3, Matrix4x4.Identity));
                    Triangles.AddRange(triangles);

                }

          
                if (RawObjects.Count > 0 && Triangles.Count > 0)
                {
                    CurrentMaterial.Shader.InitializeShader(D3D11GraphicsDevice.Device, D3D11GraphicsDevice.DeviceContext);

                    var Vertices = Triangle.ArrayToRawVertices(Triangles).ToArray();
                    ID3D11Buffer VertexBuffer = D3D11GraphicsDevice.Device.CreateBuffer(BindFlags.VertexBuffer, Vertices);
                    var indices = Mesh.CreateOrderedIndicesList(Vertices.Length).ToArray();
                    ID3D11Buffer IndexBuffer = D3D11GraphicsDevice.Device.CreateBuffer(BindFlags.IndexBuffer, indices);

                    D3D11GraphicsDevice.DeviceContext.IASetVertexBuffer(0, VertexBuffer, Marshal.SizeOf(new RawVertex()), 0);
                    D3D11GraphicsDevice.DeviceContext.IASetIndexBuffer(IndexBuffer, Format.R32_UInt, 0);

                    int VertexOffset = 0;
                    for (int i = 0; i < RawObjects.Count; i++)
                    {
                        Mathematics.Matrix4x4 ObjectMatrix = RawObjects[i].Item2;
                        ObjectMatrix.Transpose();

                        var ObjectMatrixBuffer = CreateStructBuffer(ObjectMatrix);
                        D3D11GraphicsDevice.DeviceContext.VSSetConstantBuffer(2, ObjectMatrixBuffer);
                        D3D11GraphicsDevice.DeviceContext.Draw(RawObjects[i].Item1, VertexOffset);
                        VertexOffset += RawObjects[i].Item1;
                        MatrixBuffer.Dispose();
                        ObjectMatrixBuffer.Dispose();
                        VertexBuffer.Dispose();
                        IndexBuffer.Dispose();
                    }
                    RawObjects.Clear();
                    Triangles.Clear();
                }
            }
        }
    }
}