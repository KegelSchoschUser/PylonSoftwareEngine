using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace PylonSoftwareEngine.Mathematics
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

        public Triangle()
        {
            P1 = new Vector3();
            P2 = new Vector3();
            P3 = new Vector3();

            UV1 = new Vector2();
            UV2 = new Vector2();
            UV3 = new Vector2();

            Normal = new Vector3();
        }

        public Triangle(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;

            UV1 = new Vector2();
            UV2 = new Vector2();
            UV3 = new Vector2();

            Normal = new Vector3();
        }

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


        /*
        public static void ArrayToRawVertices(List<Triangle> Triangles, ref RawVertex[] Output)
        {
            for (int i = 0; i < Triangles.Count * 3; i+=3)
            {
                Output[i].Position = Triangles[i].P1;
                Output[i].UV = Triangles[i].UV1;
                Output[i].Normal = Triangles[i].Normal;


                Output[i + 1].Position = Triangles[i].P2;
                Output[i + 1].UV = Triangles[i].UV2;
                Output[i + 1].Normal = Triangles[i].Normal;

                Output[i + 2].Position = Triangles[i].P3;
                Output[i + 2].UV = Triangles[i].UV3;
                Output[i + 2].Normal = Triangles[i].Normal;
            }
        }
         */
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

        public static List<Triangle> GetFromVertexList(RawVertex[] Vertices, int[] Indices)
        {
            bool flag = Indices.Length % 3 != 0;
            if (flag)
            {
                throw new Exception("Indices List must be a multiple of 3!");
            }

            List<Triangle> Output = new List<Triangle>();

            for (int i = 0; i < Indices.Length; i += 3)
            {
                Triangle T = new Triangle(Vertices[Indices[i]].Position, Vertices[Indices[i + 1]].Position, Vertices[Indices[i + 2]].Position,
                                          Vertices[Indices[i]].UV, Vertices[Indices[i + 1]].UV, Vertices[Indices[i + 2]].UV);
                Output.Add(T);
            }
            return Output;
        }

        public static Triangle operator +(Triangle t, Vector3 v)
        {
            return new Triangle(t.P1 + v, t.P2 + v, t.P3 + v, t.UV1, t.UV2, t.UV3, t.Normal);
        }

        public static Triangle operator -(Triangle t, Vector3 v)
        {
            return new Triangle(t.P1 - v, t.P2 - v, t.P3 - v, t.UV1, t.UV2, t.UV3, t.Normal);
        }
    }
}
