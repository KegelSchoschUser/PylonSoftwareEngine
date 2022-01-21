using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace PylonGameEngine.Mathematics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Triangle
    {
        public Vector3 P1;
        public Vector3 P2;
        public Vector3 P3;

        public Vector2 UV1;
        public Vector2 UV2;
        public Vector2 UV3;

        public Vector3 Normal;

        public Triangle(Vector3 p1, Vector3 p2, Vector3 p3, Vector2 uv1, Vector2 uv2, Vector2 uv3, Vector3 normal)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;

            UV1 = uv1;
            UV2 = uv2;
            UV3 = uv3;

            Normal = normal;
        }

        public Triangle(Vector3 p1, Vector3 p2, Vector3 p3, Vector2 uv1, Vector2 uv2, Vector2 uv3)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;

            UV1 = uv1;
            UV2 = uv2;
            UV3 = uv3;

            Normal = CalculateNormal(p1, p2, p3);
        }

        public Vector3 CalculateNormal()
        {
            Vector3 A = P2 - P1;
            Vector3 B = P3 - P1;

            Vector3 Normal = Vector3.Zero;
            Normal.X = A.Y * B.Z - A.Z * B.Y;
            Normal.Y = A.Z * B.X - A.X * B.Z;
            Normal.Z = A.X * B.Y - A.Y * B.X;

            return Normal;
        }

        public static Vector3 CalculateNormal(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            Vector3 A = p2 - p1;
            Vector3 B = p3 - p1;

            Vector3 Normal = Vector3.Zero;
            Normal.X = A.Y * B.Z - A.Z * B.Y;
            Normal.Y = A.Z * B.X - A.X * B.Z;
            Normal.Z = A.X * B.Y - A.Y * B.X;

            return Normal;
        }

        public (RawVertex, RawVertex, RawVertex) ToRawVertices()
        {
            return (new RawVertex(P1, UV1, Normal),
                    new RawVertex(P2, UV2, Normal),
                    new RawVertex(P3, UV3, Normal));
        }

        public static List<RawVertex> ArrayToRawVertices(List<Triangle> Triangles)
        {
            var output = new List<RawVertex>();
            foreach (var triangle in Triangles)
            {
                var vertices = triangle.ToRawVertices();
                output.Add(vertices.Item1);
                output.Add(vertices.Item2);
                output.Add(vertices.Item3);
            }
            return output;
        }

        public (RawVertex2D, RawVertex2D, RawVertex2D) ToRawVertices2D()
        {
            return (new RawVertex2D((Vector2)P1, UV1),
                    new RawVertex2D((Vector2)P2, UV2),
                    new RawVertex2D((Vector2)P3, UV3));
        }

        public static List<RawVertex2D> ArrayToRawVertices2D(List<Triangle> Triangles)
        {
            var output = new List<RawVertex2D>();
            foreach (var triangle in Triangles)
            {
                var vertices = triangle.ToRawVertices2D();
                output.Add(vertices.Item1);
                output.Add(vertices.Item2);
                output.Add(vertices.Item3);
            }
            return output;
        }
    }
}
