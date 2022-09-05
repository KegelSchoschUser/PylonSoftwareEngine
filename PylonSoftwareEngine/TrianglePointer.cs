using PylonSoftwareEngine.Mathematics;
using System.Runtime.InteropServices;

namespace PylonSoftwareEngine
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TrianglePointer
    {
        public Material Material;

        public int P1Index;
        public int P2Index;
        public int P3Index;

        public int UV1Index;
        public int UV2Index;
        public int UV3Index;

        public int NormalIndex;

        public TrianglePointer(Material material, int p1index, int p2index, int p3index, int uv1index, int uv2index, int uv3index, int normalindex)
        {
            Material = material;

            P1Index = p1index;
            P2Index = p2index;
            P3Index = p3index;

            UV1Index = uv1index;
            UV2Index = uv2index;
            UV3Index = uv3index;

            NormalIndex = normalindex;
        }

        public TrianglePointer FlipFace()
        {
            return new TrianglePointer(Material, P3Index,
                                                 P2Index,
                                                 P1Index,

                                                 UV3Index,
                                                 UV2Index,
                                                 UV1Index,

                                                 NormalIndex



                );
        }

        public Triangle ToTriangle(Vector3[] Points, Vector2[] UVs, Vector3[] Normals)
        {
            return new Triangle(Points[P1Index], Points[P2Index], Points[P3Index],
                                UVs[UV1Index], UVs[UV2Index], UVs[UV3Index],
                                Normals[NormalIndex]);
        }
    }
}
