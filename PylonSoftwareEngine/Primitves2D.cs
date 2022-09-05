using PylonSoftwareEngine.Mathematics;
using System.Collections.Generic;

namespace PylonSoftwareEngine
{
    public static class Primitves2D
    {
        public static Mesh Quad(Vector2 Position, Vector2 Size, Material materialindex)
        {
            var m = new Mesh();

            var Position3D = (Vector3)Position;
            var Size3D = (Vector3)Size;
            m.Points.Add(Position3D);
            m.Points.Add(Position3D + new Vector3(Size3D.X, 0));
            m.Points.Add(Position3D + Size3D);
            m.Points.Add(Position3D + new Vector3(0, Size3D.Y));


            m.UVs.Add(new Vector2(0, 0));
            m.UVs.Add(new Vector2(1, 0));
            m.UVs.Add(new Vector2(1, 1));
            m.UVs.Add(new Vector2(0, 1));

            m.Normals.Add(Vector3.Forward);

            m.Triangles.Add(new TrianglePointer(materialindex, 0, 1, 2, 0, 1, 2, 0));
            m.Triangles.Add(new TrianglePointer(materialindex, 0, 2, 3, 0, 2, 3, 0));
            return m;
        }

        public static Mesh Quad(Vector2 Size, Material materialindex)
        {
            return Quad(new Vector2(0, 0), Size, materialindex);
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
    }
}
