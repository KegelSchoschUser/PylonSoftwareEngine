/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace PylonSoftwareEngine.Mathematics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Quad
    {
        public Vector3 P1;
        public Vector3 P2;
        public Vector3 P3;
        public Vector3 P4;

        public Vector2 UV1;
        public Vector2 UV2;
        public Vector2 UV3;
        public Vector2 UV4;

        public Vector3 Normal;

        public Quad(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, Vector2 uv1, Vector2 uv2, Vector2 uv3, Vector2 uv4, Vector3 normal)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
            P4 = p4;

            UV1 = uv1;
            UV2 = uv2;
            UV3 = uv3;
            UV4 = uv4;

            Normal = normal;
        }

        public Quad(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, Vector2 uv1, Vector2 uv2, Vector2 uv3, Vector2 uv4)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
            P4 = p4;

            UV1 = uv1;
            UV2 = uv2;
            UV3 = uv3;
            UV4 = uv4;

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

        public (RawVertex, RawVertex, RawVertex, RawVertex) ToRawVertices()
        {
            return (new RawVertex(P1, UV1, Normal),
                    new RawVertex(P2, UV2, Normal),
                    new RawVertex(P3, UV3, Normal),
                    new RawVertex(P4, UV4, Normal));
        }

        public static List<RawVertex> ArrayToRawVertices(List<Quad> Quads)
        {
            var output = new List<RawVertex>();
            foreach (var Quad in Quads)
            {
                var vertices = Quad.ToRawVertices();
                output.Add(vertices.Item1);
                output.Add(vertices.Item2);
                output.Add(vertices.Item3);
                output.Add(vertices.Item4);
            }
            return output;
        }

        public (RawVertex2D, RawVertex2D, RawVertex2D, RawVertex2D) ToRawVertices2D()
        {
            return (new RawVertex2D((Vector2)P1, UV1),
                    new RawVertex2D((Vector2)P2, UV2),
                    new RawVertex2D((Vector2)P3, UV3),
                    new RawVertex2D((Vector2)P4, UV4));
        }

        public static List<RawVertex2D> ArrayToRawVertices2D(List<Quad> Quads)
        {
            var output = new List<RawVertex2D>();
            foreach (var Quad in Quads)
            {
                var vertices = Quad.ToRawVertices2D();
                output.Add(vertices.Item1);
                output.Add(vertices.Item2);
                output.Add(vertices.Item3);
                output.Add(vertices.Item4);
            }
            return output;
        }

        public (Triangle, Triangle) ToTriangles()
        {
            return (new Triangle(P1, P2, P3, UV1, UV2, UV3, Normal), new Triangle(P1, P3, P4, UV1, UV3, UV4, Normal));
        }
    }
}
