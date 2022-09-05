/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

using System.Globalization;
using System.Runtime.InteropServices;

namespace PylonSoftwareEngine.Mathematics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector3
    {
        public float X;
        public float Y;
        public float Z;

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3(float x, float y)
        {
            X = x;
            Y = y;
            Z = 0f;
        }

        public static explicit operator Vector3(Vector2 v)
        {
            return new Vector3(v.X, v.Y, 0f);
        }

        public Vector3(Vector2 vec)
        {
            X = vec.X;
            Y = vec.Y;
            Z = 0f;
        }

        public Vector3(float Scale)
        {
            X = Scale;
            Y = Scale;
            Z = Scale;
        }

        public static Vector3 Min(Vector3 P1, Vector3 P2)
        {
            return new Vector3(
                 (P1.X < P2.X) ? P1.X : P2.X,
                 (P1.Y < P2.Y) ? P1.Y : P2.Y,
                 (P1.Z < P2.Z) ? P1.Z : P2.Z);
        }

        public static Vector3 Max(Vector3 P1, Vector3 P2)
        {
            return new Vector3(
                (P1.X > P2.X) ? P1.X : P2.X,
                (P1.Y > P2.Y) ? P1.Y : P2.Y,
                (P1.Z > P2.Z) ? P1.Z : P2.Z);
        }

        public Vector3(Quaternion q)
        {
            X = q.X;
            Y = q.Y;
            Z = q.Z;
        }


        public Vector2 ToVector2()
        {
            return new Vector2(X, Y);
        }

        public static readonly Vector3 Forward = new Vector3(0, 0, 1);
        public static readonly Vector3 Backward = new Vector3(0, 0, -1);

        public static readonly Vector3 Left = new Vector3(-1, 0, 0);
        public static readonly Vector3 Right = new Vector3(1, 0, 0);

        public static readonly Vector3 Up = new Vector3(0, 1, 0);
        public static readonly Vector3 Down = new Vector3(0, -1, 0);

        public static readonly Vector3 Zero = new Vector3(0, 0, 0);
        public static readonly Vector3 Unit = new Vector3(1, 1, 1);
        public static readonly Vector3 One = new Vector3(1, 1, 1);

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return X;
                    case 1:
                        return Y;
                    case 2:
                        return Z;
                    default:
                        return 0;
                }
            }

            set
            {
                switch (index)
                {
                    case 0:
                        X = value;
                        break;
                    case 1:
                        Y = value;
                        break;
                    case 2:
                        Z = value;
                        break;
                }
            }
        }
        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="left">The first vector to subtract.</param>
        /// <param name="right">The second vector to subtract.</param>
        /// <param name="result">When the method completes, contains the difference of the two vectors.</param>
        public static void Subtract(ref Vector3 left, ref Vector3 right, out Vector3 result)
        {
            result = new Vector3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }
        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="left">First source vector.</param>
        /// <param name="right">Second source vector.</param>
        /// <param name="result">When the method completes, contains the dot product of the two vectors.</param>
        public static float Dot(Vector3 left, Vector3 right)
        {
            return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z);
        }
        public float Length()
        {
            return (float)System.Math.Sqrt((X * X) + (Y * Y) + (Z * Z));
        }
        /// <summary>
        /// Converts the vector into a unit vector.
        /// </summary>
        public Vector3 Normalize()
        {
            float length = Length();
            if (length != 0)
            {
                float inv = 1.0f / length;
                X *= inv;
                Y *= inv;
                Z *= inv;
            }
            return this;
        }

        /// <summary>
        /// Calculates the cross product of two vectors.
        /// </summary>
        /// <param name="left">First source vector.</param>
        /// <param name="right">Second source vector.</param>
        /// <param name="result">When the method completes, contains he cross product of the two vectors.</param>
        public static void Cross(ref Vector3 left, ref Vector3 right, out Vector3 result)
        {
            result = new Vector3(
                (left.Y * right.Z) - (left.Z * right.Y),
                (left.Z * right.X) - (left.X * right.Z),
                (left.X * right.Y) - (left.Y * right.X));
        }

        public static Vector3 Cross(Vector3 left, Vector3 right)
        {
            var result = new Vector3(
                (left.Y * right.Z) - (left.Z * right.Y),
                (left.Z * right.X) - (left.X * right.Z),
                (left.X * right.Y) - (left.Y * right.X));
            return result;
        }

        public static Vector3 operator +(Vector3 v1)
        {
            return new Vector3(+v1.X, +v1.Y, +v1.Z);
        }

        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Vector3 operator -(Vector3 v1)
        {
            return new Vector3(-v1.X, -v1.Y, -v1.Z);
        }

        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }



        public static Vector3 operator *(Vector3 v1, float k)
        {
            return new Vector3(v1.X * k, v1.Y * k, v1.Z * k);
        }

        public static Vector3 operator *(float k, Vector3 v1)
        {
            return new Vector3(v1.X * k, v1.Y * k, v1.Z * k);
        }

        public static Vector3 operator *(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X * v2.X, v1.Y * v2.Y, v1.Z * v2.Z);
        }

        public static Vector3 operator /(Vector3 v1, float k)
        {
            return new Vector3(v1.X / k, v1.Y / k, v1.Z / k);
        }

        public static Vector3 operator /(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X / v2.X, v1.Y / v2.Y, v1.Z / v2.Z);
        }

        public static bool operator ==(Vector3 v1, Vector3 v2)
        {
            return v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z;
        }
        public static bool operator !=(Vector3 v1, Vector3 v2)
        {
            return (v1 == v2) == false;
        }

        public static bool operator <(Vector3 v1, Vector3 v2)
        {
            return (v1.X < v2.X && v1.Y < v2.Y && v1.Z < v2.Z);
        }

        public static bool operator >(Vector3 v1, Vector3 v2)
        {
            return (v1.X < v2.X && v1.Y < v2.Y && v1.Z > v2.Z);
        }
        public static bool operator <=(Vector3 v1, Vector3 v2)
        {
            return (v1.X <= v2.X && v1.Y <= v2.Y && v1.Z <= v2.Z);
        }

        public static bool operator >=(Vector3 v1, Vector3 v2)
        {
            return (v1.X <= v2.X && v1.Y <= v2.Y && v1.Z >= v2.Z);
        }
        public static float Vector_DotProduct(Vector3 v1, Vector3 v2)
        {
            return (v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z);
        }

        public static float Vector_Length(Vector3 v)
        {
            return ((float)System.Math.Sqrt(Vector_DotProduct(v, v)));
        }

        public static Vector3 Vector_Normalise(Vector3 v)
        {
            float l = Vector_Length(v);
            return new Vector3(v.X / l, v.Y / l, v.Z / l);
        }

        public static Vector3 right(Vector3 Forward)
        {
            return Vector3.Vector_CrossProduct(Forward, Vector3.Up);
        }

        public static Vector3 left(Vector3 Forward)
        {
            return -Vector3.Vector_CrossProduct(Forward.Normalize(), Vector3.Up.Normalize());
        }

        public static Vector3 Vector_CrossProduct(Vector3 v1, Vector3 v2)
        {
            Vector3 v = new Vector3
            {
                X = v1.Y * v2.Z - v1.Z * v2.Y,
                Y = v1.Z * v2.X - v1.X * v2.Z,
                Z = v1.X * v2.Y - v1.Y * v2.X
            };
            return v;
        }

        public static Vector3 Vector_IntersectPlane(Vector3 plane_p, Vector3 plane_n, Vector3 lineStart, Vector3 lineEnd, float t)
        {
            plane_n = Vector_Normalise(plane_n);
            float plane_d = -Vector_DotProduct(plane_n, plane_p);
            float ad = Vector_DotProduct(lineStart, plane_n);
            float bd = Vector_DotProduct(lineEnd, plane_n);
            t = (-plane_d - ad) / (bd - ad);
            Vector3 lineStartToEnd = lineEnd - lineStart;
            Vector3 lineToIntersect = lineStartToEnd * t;
            return lineStart + lineToIntersect;
        }

        public static Vector3 GetVector3Scaled(Vector3 originalVector, float newLength)
        {
            if (newLength == 0.0f)
            {
                //	V pripade ze chceme aby mal vektor nulovu dlzku, tak komponenty vektora zmenime
                //	na nulu rucne, lebo delenie nulou by sposobilo vznik neplatnych floatov a problemy!
                return Vector3.Zero;
            }
            else
            {
                float originalLength = originalVector.Length();

                //	Ak je dlzka povodneho vektora nulova, nema zmysel scalovat jeho dlzku
                if (originalLength == 0.0)
                {
                    return Vector3.Zero;
                }

                //  Return scaled vector
                float mul = newLength / originalLength;
                return new Vector3(originalVector.X * mul, originalVector.Y * mul, originalVector.Z * mul);
            }
        }
        public static Vector3 Lerp(Vector3 firstFloat, Vector3 secondFloat, float by)
        {
            return new Vector3(Mathf.Lerp(firstFloat.X, secondFloat.X, by),
                               Mathf.Lerp(firstFloat.Y, secondFloat.Y, by),
                               Mathf.Lerp(firstFloat.Z, secondFloat.Z, by));
        }
        public static Vector3 Lerp(Vector3 firstFloat, Vector3 secondFloat, Vector3 by)
        {
            return new Vector3(Mathf.Lerp(firstFloat.X, secondFloat.X, by.X),
                               Mathf.Lerp(firstFloat.Y, secondFloat.Y, by.Y),
                               Mathf.Lerp(firstFloat.Z, secondFloat.Z, by.Z));
        }

        /// <summary>
        /// Performs a coordinate transformation using the given <see cref="Matrix"/>.
        /// </summary>
        /// <param name="coordinate">The coordinate vector to transform.</param>
        /// <param name="transform">The transformation <see cref="Matrix"/>.</param>
        /// <param name="result">When the method completes, contains the transformed coordinates.</param>
        /// <remarks>
        /// A coordinate transform performs the transformation with the assumption that the w component
        /// is one. The four dimensional vector obtained from the transformation operation has each
        /// component in the vector divided by the w component. This forces the w component to be one and
        /// therefore makes the vector homogeneous. The homogeneous vector is often preferred when working
        /// with coordinates as the w component can safely be ignored.
        /// </remarks>
        public static void TransformCoordinate(ref Vector3 coordinate, ref Matrix4x4 transform, out Quaternion result)
        {
            Quaternion vector = new Quaternion
            {
                X = (coordinate.X * transform.M11) + (coordinate.Y * transform.M21) + (coordinate.Z * transform.M31) + transform.M41,
                Y = (coordinate.X * transform.M12) + (coordinate.Y * transform.M22) + (coordinate.Z * transform.M32) + transform.M42,
                Z = (coordinate.X * transform.M13) + (coordinate.Y * transform.M23) + (coordinate.Z * transform.M33) + transform.M43,
                W = 1f / ((coordinate.X * transform.M14) + (coordinate.Y * transform.M24) + (coordinate.Z * transform.M34) + transform.M44)
            };

            result = new Quaternion(vector.X * vector.W, vector.Y * vector.W, vector.Z * vector.W, vector.W);
        }

        /// <summary>
        /// Performs a coordinate transformation using the given <see cref="Matrix"/>.
        /// </summary>
        /// <param name="coordinate">The coordinate vector to transform.</param>
        /// <param name="transform">The transformation <see cref="Matrix"/>.</param>
        /// <returns>The transformed coordinates.</returns>
        /// <remarks>
        /// A coordinate transform performs the transformation with the assumption that the w component
        /// is one. The four dimensional vector obtained from the transformation operation has each
        /// component in the vector divided by the w component. This forces the w component to be one and
        /// therefore makes the vector homogeneous. The homogeneous vector is often preferred when working
        /// with coordinates as the w component can safely be ignored.
        /// </remarks>
        public static Vector3 TransformCoordinate(Vector3 coordinate, Matrix4x4 transform)
        {
            TransformCoordinate(ref coordinate, ref transform, out Quaternion result);
            return new Vector3(result);
        }

        public static Quaternion TransformCoordinateQuaternion(Vector3 coordinate, Matrix4x4 transform)
        {
            TransformCoordinate(ref coordinate, ref transform, out Quaternion result);
            return result;
        }

        public static Vector3 operator /(Vector3 vector, Vector2 divider)
        {
            return new Vector3(vector.X / divider.X, vector.Y / divider.Y, vector.Z);
        }

        public static Vector3 operator *(Vector3 vector, Vector2 multiplier)
        {
            return new Vector3(vector.X * multiplier.X, vector.Y * multiplier.Y, vector.Z);
        }


        public static Vector3 operator +(Vector3 vector, Vector2 second)
        {
            return new Vector3(vector.X + second.X, vector.Y + second.Y, vector.Z);
        }

        public static Vector3 operator +(Vector3 vector, float second)
        {
            return new Vector3(vector.X + second, vector.Y + second, vector.Z + second);
        }



        /// <summary>
        /// Calculates the distance between two vectors.
        /// </summary>
        /// <param name="value1">Source vector.</param><param name="value2">Source vector.</param>
        public static float Distance(Vector3 value1, Vector3 value2)
        {
            float num1 = value1.X - value2.X;
            float num2 = value1.Y - value2.Y;
            float num3 = value1.Z - value2.Z;
            return (float)System.Math.Sqrt(num1 * num1 + num2 * num2 + num3 * num3);
        }

        public float Combine()
        {
            return X + Y + Z;
        }

        public float Abs()
        {
            return Mathf.Abs(X + Y + Z);
        }


        public override string ToString()
        {
            return "{" + $"{X.ToString("0.000", CultureInfo.InvariantCulture)}, {Y.ToString("0.000", CultureInfo.InvariantCulture)}, {Z.ToString("0.000", CultureInfo.InvariantCulture)}" + "}";
        }


        #region System.Numerics

        public static Vector3 operator +(System.Numerics.Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Vector3 operator -(System.Numerics.Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        public static Vector3 operator *(System.Numerics.Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X * v2.X, v1.Y * v2.Y, v1.Z * v2.Z);
        }

        public static Vector3 operator /(System.Numerics.Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X / v2.X, v1.Y / v2.Y, v1.Z / v2.Z);
        }

        public System.Numerics.Vector3 ToSystemNumerics()
        {
            return new System.Numerics.Vector3(X, Y, Z);
        }

        public static Vector3 FromSystemNumerics(System.Numerics.Vector3 V)
        {
            return new Vector3(V.X, V.Y, V.Z);
        }

        public static Vector2[] ToVector2Array(Vector3[] array)
        {
            var output = new Vector2[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                output[i] = new Vector2(array[i]);
            }

            return output;
        }

        public static implicit operator System.Numerics.Vector3(Vector3 v) => new System.Numerics.Vector3(v.X, v.Y, v.Z);
        public static implicit operator Vector3(System.Numerics.Vector3 v) => new Vector3(v.X, v.Y, v.Z);
        #endregion

    }
}
