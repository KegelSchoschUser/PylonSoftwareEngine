using PylonGameEngine.Mathematics;
using PylonGameEngine.Render11;
using PylonGameEngine.ShaderLibrary;
using PylonGameEngine.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Vortice.Direct3D11;

namespace PylonGameEngine
{
    public class Mesh
    {
        public ObservableList<Vector3> Points = new ObservableList<Vector3>();
        public ObservableList<Vector2> UVs = new ObservableList<Vector2>();
        public ObservableList<Vector3> Normals = new ObservableList<Vector3>();
        public ObservableList<TrianglePointer> Triangles = new ObservableList<TrianglePointer>();
        bool AutoUpdateBuffers;

        public Mesh(bool autoUpdateBuffers = true)
        {
            AutoUpdateBuffers = autoUpdateBuffers;

            Points.On_ItemAdded         += (i) => { if(AutoUpdateBuffers){ UpdateBuffers();}};
            Points.On_ItemRemoved       += (i) => { if(AutoUpdateBuffers){UpdateBuffers();}};
            Points.On_Item_Changed      += (i) => { if(AutoUpdateBuffers){UpdateBuffers();}};
                                           
            UVs.On_ItemAdded            += (i) => { if(AutoUpdateBuffers){UpdateBuffers();}};
            UVs.On_ItemRemoved          += (i) => { if(AutoUpdateBuffers){UpdateBuffers();}};
            UVs.On_Item_Changed         += (i) => { if(AutoUpdateBuffers){UpdateBuffers();}};
                                                  
            Normals.On_ItemAdded        += (i) => { if(AutoUpdateBuffers){UpdateBuffers();}};
            Normals.On_ItemRemoved      += (i) => { if(AutoUpdateBuffers){UpdateBuffers();}};
            Normals.On_Item_Changed     += (i) => { if(AutoUpdateBuffers){UpdateBuffers();}};
                                                  
            Triangles.On_ItemAdded      += (i) => { if(AutoUpdateBuffers){UpdateBuffers();}};
            Triangles.On_ItemRemoved    += (i) => { if(AutoUpdateBuffers){UpdateBuffers();}};
            Triangles.On_Item_Changed   += (i) => { if(AutoUpdateBuffers){UpdateBuffers();}};
        }

        public void UpdateBuffers()
        {
            {
                List<Triangle> output = new List<Triangle>();

                for (int i = 0; i < Triangles.Count; i++)
                {
                    output.Add(new Triangle(Points[Triangles[i].P1Index],
                                            Points[Triangles[i].P2Index],
                                            Points[Triangles[i].P3Index],
                                            UVs[Triangles[i].UV1Index],
                                            UVs[Triangles[i].UV2Index],
                                            UVs[Triangles[i].UV3Index],
                                            Normals[Triangles[i].NormalIndex]
                                            ));
                }

                TriangleDataBuffer = output;
            }

            {
                List<(Triangle, Material)> output = new List<(Triangle, Material)>();
                var triangledata = TriangleData;

                for (int i = 0; i < Triangles.Count; i++)
                {
                    output.Add((triangledata[i], Triangles[i].Material));
                }

                TriangleDataMaterialBuffer = output;
            }

            {
                //CAN BE OPTIMIZED

                List<Material> Materials = new List<Material>();

                foreach (var item in TriangleDataMaterialBuffer)
                {
                    if (Materials.Contains(item.Item2) == false)
                    {
                        Materials.Add(item.Item2);
                    }
                }

                MeshMaterialBuffer.Clear();

                foreach (var mat in Materials)
                {
                    List<Triangle> Triangles = new List<Triangle>();
                    MeshMaterialBuffer.Add((Triangles, mat));

                    foreach (var tri in TriangleDataMaterialBuffer)
                    {
                        if(tri.Item2 == mat)
                        {
                            Triangles.Add(tri.Item1);
                        }
                    }
                }
            }

            {
                foreach (var ValuePair in DirectXBuffersBuffer)
                {
                    ValuePair.Item2.Release();
                    ValuePair.Item3.Release();
                }

                DirectXBuffersBuffer.Clear();

                foreach (var ValuePair in MeshMaterialBuffer)
                {
                    var Material = ValuePair.Item2;
                    var Triangles = ValuePair.Item1;

                    var Vertices = new Span<RawVertex>(Triangle.ArrayToRawVertices(Triangles).ToArray());
                    var Indices = new Span<int>(Mesh.CreateOrderedIndicesList(Vertices.Length).ToArray());

                    ID3D11Buffer VertexBuffer = D3D11GraphicsDevice.Device.CreateBuffer(BindFlags.VertexBuffer, Vertices);
                    ID3D11Buffer IndexBuffer = D3D11GraphicsDevice.Device.CreateBuffer(BindFlags.IndexBuffer, Indices);

                    DirectXBuffersBuffer.Add((Material, VertexBuffer, IndexBuffer, Triangles.Count));
                }
            }
        }

        private List<Triangle> TriangleDataBuffer = new List<Triangle>();
        private List<(Triangle, Material)> TriangleDataMaterialBuffer = new List<(Triangle, Material)>();
        private List<(List<Triangle>, Material)> MeshMaterialBuffer = new List<(List<Triangle>, Material)>();
        private List<(Material, ID3D11Buffer, ID3D11Buffer, int)> DirectXBuffersBuffer = new List<(Material, ID3D11Buffer, ID3D11Buffer, int)>();


        public List<Triangle> TriangleData
        {
            get
            {
                return TriangleDataBuffer;
            }
        }

        public List<(Triangle, Material)> TriangleDataMaterial
        {
            get
            {
                return TriangleDataMaterialBuffer;
            }
        }

        public List<Triangle> GetTriangles(Material Material)
        {
            foreach (var item in MeshMaterialBuffer)
            {
                if (item.Item2 == Material)
                {
                    return item.Item1;
                }
            }

            return new List<Triangle>();
        }
        
        public List<(Material, ID3D11Buffer, ID3D11Buffer, int)> DirectXBuffers
        {
            get
            {
                return DirectXBuffersBuffer;
            }
        }

        public void AddTriangle(Triangle triangle, Material materialIndex)
        {
            Points.Add(triangle.P1);
            int p1index = Points.Count - 1;

            Points.Add(triangle.P2);
            int p2index = Points.Count - 1;

            Points.Add(triangle.P3);
            int p3index = Points.Count - 1;

            UVs.Add(triangle.UV1);
            int uv1index = UVs.Count - 1;

            UVs.Add(triangle.UV2);
            int uv2index = UVs.Count - 1;

            UVs.Add(triangle.UV3);
            int uv3index = UVs.Count - 1;

            Normals.Add(triangle.Normal);
            int normalindex = Normals.Count - 1;

            Triangles.Add(new TrianglePointer(materialIndex, p1index, p2index, p3index, uv1index, uv2index, uv3index, normalindex));
        }

        public void AddTriangles(List<Triangle> triangles, Material materialIndex)
        {
            bool UpdateState = AutoUpdateBuffers;

            AutoUpdateBuffers = false;
            foreach (var tri in triangles)
            {
                AddTriangle(tri, materialIndex);
            }
            if(UpdateState)
                UpdateBuffers();
            AutoUpdateBuffers = UpdateState;
        }

        public void AddQuad(Quad Quad, Material material)
        {
            var Triangles = Quad.ToTriangles();
            AddTriangle(Triangles.Item1, material);
            AddTriangle(Triangles.Item2, material);
        }

        public void FlipNormals()
        {
            for (int i = 0; i < Normals.Count; i++)
            {
                Normals[i] = -Normals[i];
            }

            for (int i = 0; i < Triangles.Count; i++)
            {
                Triangles[i] = Triangles[i].FlipFace();
            }
        }

        #region OldObjFileReader

        public static Mesh LoadFromObjectFile(string Filename, bool RightHanded = false)
        {
            Mesh Output = new Mesh();
            List<Material> materials = new List<Material>();
            Material currentMaterial = null;
            bool HasTexture = false;

            if (File.ReadAllText(Filename).Contains("vt "))
            {
                HasTexture = true;
            }

            foreach (string line in File.ReadAllLines(Filename))
            {
                if (line.StartsWith("mtllib "))
                {
                    string[] parts = line.Split(' ');
                    string fname = Path.GetDirectoryName(Path.GetFullPath(Filename)) + @"\" + parts[1];
                    fname.Replace(@"\\", @"\");
                    materials = ParseMTLFile(fname);
                }
                else if (line.StartsWith("v "))
                {
                    Vector3 vert = new Vector3();

                    string[] parts = line.Split(' ');
                    if (RightHanded)
                    {
                        vert.X = -float.Parse(parts[1], CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        vert.X = float.Parse(parts[1], CultureInfo.InvariantCulture);
                    }

                    vert.Y = float.Parse(parts[2], CultureInfo.InvariantCulture);
                    vert.Z = float.Parse(parts[3], CultureInfo.InvariantCulture);

                    Output.Points.Add(vert);
                }
                else if (line.StartsWith("vt "))
                {
                    Vector2 v = new Vector2();

                    string[] parts = line.Split(' ');


                    if (RightHanded)
                    {
                        v.X = float.Parse(parts[1], CultureInfo.InvariantCulture);
                        v.Y = 1 - float.Parse(parts[2], CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        v.X = float.Parse(parts[1], CultureInfo.InvariantCulture);
                        v.Y = float.Parse(parts[2], CultureInfo.InvariantCulture);
                    }


                    Output.UVs.Add(v);
                }
                else if (line.StartsWith("vn "))
                {
                    Vector3 normal = new Vector3();

                    string[] parts = line.Split(' ');
                    if (RightHanded)
                    {
                        normal.X = float.Parse(parts[1], CultureInfo.InvariantCulture);
                        normal.Z = float.Parse(parts[2], CultureInfo.InvariantCulture);
                        normal.Y = float.Parse(parts[3], CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        normal.X = float.Parse(parts[1], CultureInfo.InvariantCulture);
                        normal.Y = float.Parse(parts[2], CultureInfo.InvariantCulture);
                        normal.Z = float.Parse(parts[3], CultureInfo.InvariantCulture);
                    }


                    Output.Normals.Add(normal);
                }
                else if (line.StartsWith("usemtl "))
                {
                    string[] parts = line.Split(' ');
                    currentMaterial = materials.Find(x => x.Name == parts[1]);
                }

                if (!HasTexture)
                {
                    if (line.StartsWith("f "))
                    {
                        string[] parts = line.Split(' ');

                        int[] f = new int[3];

                        f[0] = int.Parse(parts[1], CultureInfo.InvariantCulture);
                        f[1] = int.Parse(parts[2], CultureInfo.InvariantCulture);
                        f[2] = int.Parse(parts[3], CultureInfo.InvariantCulture);

                        f[0] -= 1;
                        f[1] -= 1;
                        f[2] -= 1;

                        Output.Triangles.Add(new TrianglePointer(currentMaterial, f[0], f[1], f[2], 0, 0, 0, 0));
                    }
                }
                else
                {
                    if (line[0] == 'f')
                    {
                        string[] parts = line.Split(' ');

                        int[] v = new int[3];
                        int[] vt = new int[3];
                        int[] vn = new int[3];

                        for (int i = 0; i < parts.Length; i++)
                        {
                            if (!parts[i].StartsWith("f"))
                            {
                                string[] p1_p2 = parts[i].Split('/');
                                v[i - 1] = int.Parse(p1_p2[0]);
                                vt[i - 1] = int.Parse(p1_p2[1]);
                                vn[i - 1] = int.Parse(p1_p2[2]);
                            }
                        }

                        v[0] -= 1;
                        v[1] -= 1;
                        v[2] -= 1;
                        vt[0] -= 1;
                        vt[1] -= 1;
                        vt[2] -= 1;
                        vn[0] -= 1;

                        Output.Triangles.Add(new TrianglePointer(currentMaterial, v[0], v[1], v[2], vt[0], vt[1], vt[2], vn[0]));

                    }

                }
            }

            return Output;
        }
        public static List<Material> ParseMTLFile(string Filename)
        {
            List<Material> Output = new List<Material>();
            Material Current = null;
            foreach (string line in File.ReadAllLines(Filename))
            {

                if (line.StartsWith("newmtl "))
                {
                    string[] parts = line.Split(' ');
                    Current = new Material(parts[1]);
                    MyGame.Materials.Add(Current);
                }
                else if (line.StartsWith("map_Kd "))
                {
                    string[] parts = line.Split(' ');
                    if (line.Length > 10)
                    {
                        Current.Shader = new TextureShader(new Render11.Texture(new FileInfo(Path.Combine(Path.GetDirectoryName(Filename), parts[1])).FullName));

                    }
                    else
                    {
                        Current.Shader = new ColorShader(new RGBColor(0, 1, 1));
                    }
                    Output.Add(Current);
                }
                else if (line.StartsWith(""))
                {
                    if (Current != null)
                    {


                    }
                }
            }

            return Output;
        }

        #endregion OldObjFileReader

        public BoundingBox GetBoundingBox()
        {
            return BoundingBox.FromPoints(this.Points.ToArray());
        }

        public BoundingBox GetBoundingBox(Matrix4x4 Matrix)
        {
            return Matrix * GetBoundingBox();
        }

        public static List<int> CreateOrderedIndicesList(int n)
        {
            List<int> indices = new List<int>();

            for (int i = 0; i < n; i++)
            {
                indices.Add(i);
            }

            return indices;
        }

        public void Merge(Mesh m)
        {
            int PointOffset = this.Points.Count;
            int UVOffset = this.UVs.Count;
            int NormalOffset = this.Normals.Count;

            for (int i = 0; i < m.Points.Count; i++)
            {
                this.Points.Add(m.Points[i]);
            }
            this.UVs.AddRange(m.UVs);
            this.Normals.AddRange(m.Normals);

            for (int i = 0; i < m.Triangles.Count; i++)
            {
                TrianglePointer otherTriangle = m.Triangles[i];

                this.Triangles.Add(new TrianglePointer(otherTriangle.Material,
                                                       otherTriangle.P1Index + PointOffset,
                                                       otherTriangle.P2Index + PointOffset,
                                                       otherTriangle.P3Index + PointOffset,
                                                       otherTriangle.UV1Index + UVOffset,
                                                       otherTriangle.UV2Index + UVOffset,
                                                       otherTriangle.UV3Index + UVOffset,
                                                       otherTriangle.NormalIndex + NormalOffset));
            }
        }

        public void Destroy()
        {
            foreach (var buffer in DirectXBuffersBuffer)
            {
                var VertexBuffer = buffer.Item2;
                var IndexBuffer = buffer.Item3;

                VertexBuffer.Release();
                IndexBuffer.Release();
            }
        }
    }
}