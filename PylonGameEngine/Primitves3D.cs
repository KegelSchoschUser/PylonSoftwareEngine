using PylonGameEngine.Mathematics;
using System;
using System.Collections.Generic;

namespace PylonGameEngine
{
    public static class Primitves3D
    {
        public static Mesh Quad(Vector3 Position, Vector2 Size, Material materialindex)
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

            m.Normals.Add(Vector3.Zero);

            m.Triangles.Add(new TrianglePointer(materialindex, 0, 1, 2, 0, 1, 2, 0));
            m.Triangles.Add(new TrianglePointer(materialindex, 0, 2, 3, 0, 2, 3, 0));
            return m;
        }

        public static Mesh Quad(Vector2 Size, Material materialindex)
        {
            return Quad(new Vector3(0, 0, 0), Size, materialindex);
        }

        public static Vector3[] CreateHollowCircle(int Resolution = 32)
        {
            return CreateHollowCircle(Resolution, Vector3.Zero, new Vector3(1f), Quaternion.Identity);
        }

        public static Vector3[] CreateHollowCircle(int Resolution, Vector3 Position, Vector3 Scale, Quaternion Rotation)
        {
            if (Resolution < 3)
                throw new System.Exception("Resulotion must be greater than 2");
            List<Vector3> Vertices = new List<Vector3>();

            for (int i = 0; i < Resolution; i++)
            {
                Vertices.Add(new Vector3(Mathf.Cos((360f / Resolution * i) * Mathf.Deg2Rad),
                                         0f,
                                         Mathf.Sin((360f / Resolution * i) * Mathf.Deg2Rad)
                                         ));
            }

            for (int i = 0; i < Vertices.Count; i++)
            {
                Matrix4x4 t = Matrix4x4.Translation(Position);
                Matrix4x4 r = Matrix4x4.RotationQuaternion(Rotation);
                Matrix4x4 s = Matrix4x4.Scaling(Scale);
                var matrix = s * r * t;
                matrix.Transpose();

                Vertices[i] = matrix * Vertices[i];
            }

            return Vertices.ToArray();
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

        public static Mesh CreateCube()
        {
            return CreateCube(null, Vector3.Zero, Vector3.One, Quaternion.Identity, true);
        }

        public static Mesh CreateCube(Material MaterialID)
        {
            return CreateCube(MaterialID, Vector3.Zero, Vector3.One, Quaternion.Identity, true);
        }

        public static Mesh CreateCube(Material MaterialID, Vector3 Position)
        {
            return CreateCube(MaterialID, Position, Vector3.One, Quaternion.Identity, true);
        }

        public static Mesh CreateCube(Material MaterialID, Vector3 Position, Vector3 Size)
        {
            return CreateCube(MaterialID, Position, Size, Quaternion.Identity, true);
        }

        public static Mesh CreateCube(Material MaterialID, Vector3 Position, Vector3 Size, Quaternion Rotation)
        {
            return CreateCube(MaterialID, Position, Size, Rotation, true);
        }

        public static Mesh CreateCube(Material Material, Vector3 Position, Vector3 Size, Quaternion Rotation, bool Center)
        {
            var m = new Mesh();

            Vector3 Origin = Vector3.Zero;

            if (Center)
            {
                Origin = new Vector3(0.5f);
            }

            m.Points.Add(new Vector3(0, 0, 0));    //0
            m.Points.Add(new Vector3(0, 1, 0));    //1 //
            m.Points.Add(new Vector3(1, 1, 0));    //2 //
            m.Points.Add(new Vector3(1, 0, 0));    //3
                                                                                                                
            m.Points.Add(new Vector3(0, 0, 1));    //4
            m.Points.Add(new Vector3(0, 1, 1));    //5 //
            m.Points.Add(new Vector3(1, 1, 1));    //6 //
            m.Points.Add(new Vector3(1, 0, 1));    //7

            m.UVs.Add(new Vector2(0, 0));           //0
            m.UVs.Add(new Vector2(1, 0));           //1
            m.UVs.Add(new Vector2(1, 1));           //2
            m.UVs.Add(new Vector2(0, 1));           //3

            m.Normals.Add(Vector3.Backward);        //0
            m.Normals.Add(Vector3.Forward);         //1
            m.Normals.Add(Vector3.Left);            //2
            m.Normals.Add(Vector3.Right);           //3
            m.Normals.Add(Vector3.Up);              //4
            m.Normals.Add(Vector3.Down);            //5

            m.Triangles.Add(new TrianglePointer(Material,         0, 1, 2,        3, 0, 1,        0));    //Front
            m.Triangles.Add(new TrianglePointer(Material,         0, 2, 3,        3, 1, 2,        0));    //Front

            m.Triangles.Add(new TrianglePointer(Material,         7, 6, 5,        3, 0, 1,        1));    //Back
            m.Triangles.Add(new TrianglePointer(Material,         7, 5, 4,        3, 1, 2,        1));    //Back   

            m.Triangles.Add(new TrianglePointer(Material,         4, 5, 1,        3, 0, 1,        2));    //Left
            m.Triangles.Add(new TrianglePointer(Material,         4, 1, 0,        3, 1, 2,        2));    //Left

            m.Triangles.Add(new TrianglePointer(Material,         3, 2, 6,        3, 0, 1,        3));    //Right
            m.Triangles.Add(new TrianglePointer(Material,         3, 6, 7,        3, 1, 2,        3));    //Right  

            m.Triangles.Add(new TrianglePointer(Material,         6, 2, 1,        1, 2, 3,        4));    //Top
            m.Triangles.Add(new TrianglePointer(Material,         6, 1, 5,        1, 3, 0,        4));    //Top

            m.Triangles.Add(new TrianglePointer(Material,         3, 7, 4,        1, 2, 3,        5));    //Bottom
            m.Triangles.Add(new TrianglePointer(Material,         3, 4, 0,        1, 3, 0,        5));    //Bottom //  0, 3, 4


            ApplyTransform(m, Position, Size, Rotation, Origin);
            return m;
        }




        public static Mesh CreateSphere()
        {
            return CreateSphere(null, Vector3.Zero, 1, 32, true);
        }

        public static Mesh CreateSphere(Material MaterialID)
        {
            return CreateSphere(MaterialID, Vector3.Zero, 1, 32, true);
        }
        public static Mesh CreateSphere(Material MaterialID, Vector3 Position, float radius, int quality)
        {
            return CreateSphere(MaterialID, Position, radius, quality, true);
        }

        public static Mesh CreateSphere(Material Material, Vector3 Position, float radius, int quality, bool Center)
        {
            var m = new Mesh();

            List<Vector3> m_tmpVectorBuffer = new List<Vector3>();
            List<Vector3> vertices = new List<Vector3>();
            Vector3 Origin = Vector3.Zero;

            if (Center)
            {
                Origin = new Vector3(0.5f);
            }

            //Now are variables for this is as followed. n is the current vertex we are working
            //with. While a and b are used to control our loops.
            int n = 0;

            //Assign our b loop to go through 90 degrees in intervals of our variable space
            float space = 360 / quality;
            float limitBeta = 90 - space;
            float limitAlpha = 360 - space;
            Vector3 vctTmp;

            //@ generate hafSphere
            for (float beta = 0; beta <= limitBeta; beta += space)
            {
                //Assign our a loop to go through 360 degrees in intervals of our variable space
                for (float alpha = 0; alpha <= limitAlpha; alpha += space)
                {
                    //Start editing our vertex.
                    
                    vctTmp.X = (float)(radius * Mathf.Sin(Mathf.Deg2Rad * alpha) * Mathf.Sin(Mathf.Deg2Rad * beta));
                    vctTmp.Y = (float)(radius * Mathf.Cos(Mathf.Deg2Rad * alpha) * Mathf.Sin(Mathf.Deg2Rad * beta));
                    vctTmp.Z = (float)(radius * Mathf.Cos(Mathf.Deg2Rad * beta));
                    m_tmpVectorBuffer.Add(vctTmp);
                    //Then start working with the next vertex
                    n++;

                    //Then we do the same calculations as before, only adding the space variable
                    //to the b values.
                    vctTmp.X = (float)(radius * Mathf.Sin(Mathf.Deg2Rad * alpha) * Mathf.Sin(Mathf.Deg2Rad * beta + space));
                    vctTmp.Y = (float)(radius * Mathf.Cos(Mathf.Deg2Rad * alpha) * Mathf.Sin(Mathf.Deg2Rad * beta + space));
                    vctTmp.Z = (float)(radius * Mathf.Cos(Mathf.Deg2Rad * beta + space));
                    m_tmpVectorBuffer.Add(vctTmp);
                    n++;

                    //Then we do the same calculations as the first, only adding the space variable
                    //to the a values.
                    vctTmp.X = (float)(radius * Mathf.Sin(Mathf.Deg2Rad * alpha + space) * Mathf.Sin(Mathf.Deg2Rad * beta));
                    vctTmp.Y = (float)(radius * Mathf.Cos(Mathf.Deg2Rad * alpha + space) * Mathf.Sin(Mathf.Deg2Rad * beta));
                    vctTmp.Z = (float)(radius * Mathf.Cos(Mathf.Deg2Rad * beta));
                    m_tmpVectorBuffer.Add(vctTmp);
                    n++;

                    //Then we do the same calculations as the first again, only adding the space variable
                    //to both the b and the a values.
                    vctTmp.X = (float)(radius * Mathf.Sin(Mathf.Deg2Rad * alpha + space) * Mathf.Sin(Mathf.Deg2Rad * beta + space));
                    vctTmp.Y = (float)(radius * Mathf.Cos(Mathf.Deg2Rad * alpha + space) * Mathf.Sin(Mathf.Deg2Rad * beta + space));
                    vctTmp.Z = (float)(radius * Mathf.Cos(Mathf.Deg2Rad * beta + space));
                    m_tmpVectorBuffer.Add(vctTmp);
                    n++;
                }
            }

            int offset = m_tmpVectorBuffer.Count;
            foreach (Vector3 vct in m_tmpVectorBuffer)
                vertices.Add(vct);

            //@ coz stupid C#
            foreach (Vector3 vct in m_tmpVectorBuffer)
            {
                Vector3 vctTmpZNeg = new Vector3(vct.X, vct.Y, -vct.Z);
                vertices.Add(vctTmpZNeg);
            }

            for (int i = 0; i < vertices.Count; i += 4)
            {
                Quad quad = new Quad();
                quad.P1 = vertices[i + 1];
                quad.P2 = vertices[i + 3];
                quad.P3 = vertices[i + 2];
                quad.P4 = vertices[i];

                quad.CalculateNormal();
                quad.UV1 = new Vector2(0, 0);
                quad.UV2 = new Vector2(1, 0);
                quad.UV3 = new Vector2(1, 1);
                quad.UV4 = new Vector2(0, 1);

                m.AddQuad(quad, Material);
            }

                ApplyTransform(m, Position, Vector3.One, Quaternion.Identity, Origin);
            return m;
        }

        public static void ApplyTransform(Mesh m, Vector3 Translation, Vector3 Scale, Quaternion Rotation, Vector3 Center)
        {
            Matrix4x4 c = Matrix4x4.Translation(-Center);
            Matrix4x4 t = Matrix4x4.Translation(Translation);
            Matrix4x4 r = Matrix4x4.RotationQuaternion(Rotation);
            Matrix4x4 s = Matrix4x4.Scaling(Scale);

            Matrix4x4 TransfromMatrix = c * s * r * t;
            TransfromMatrix.Transpose();

            for (int i = 0; i < m.Points.Count; i++)
            {
                m.Points[i] = TransfromMatrix * m.Points[i];
            }
        }
    }
}