using System.Globalization;
using System.Runtime.InteropServices;

namespace PylonSoftwareEngine.Mathematics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Quaternion
    {
        public float X;
        public float Y;
        public float Z;
        public float W;

        public static readonly Quaternion Identity = new Quaternion(0, 0, 0, 1);

        public Quaternion(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
            W = 1f;
        }

        public Quaternion(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public Quaternion(float fl)
        {
            X = fl;
            Y = 0f;
            Z = 0f;
            W = 1f;
        }

        public Quaternion(Vector2 vec)
        {
            X = vec.X;
            Y = vec.Y;
            Z = 0f;
            W = 1f;
        }

        public Quaternion(Vector3 vec)
        {
            X = vec.X;
            Y = vec.Y;
            Z = vec.Z;
            W = 1f;
        }

        public static Quaternion FromEuler(Vector3 euler)
        {
            return FromEuler(euler.X, euler.Y, euler.Z);
        }

        public void Inverse()
        {
            float lengthSq = LengthSquared();
            if (lengthSq > 0.0001f)
            {
                lengthSq = 1.0f / lengthSq;

                X = -X * lengthSq;
                Y = -Y * lengthSq;
                Z = -Z * lengthSq;
                W = W * lengthSq;
            }
        }

        public Quaternion InverseGet()
        {
            Inverse();
            return this;
        }

        public float LengthSquared()
        {
            return (X * X) + (Y * Y) + (Z * Z) + (W * W);
        }

        public static Vector3 operator *(Quaternion quat, Vector3 vec)
        {
            float num = quat.X * 2f;
            float num2 = quat.Y * 2f;
            float num3 = quat.Z * 2f;
            float num4 = quat.X * num;
            float num5 = quat.Y * num2;
            float num6 = quat.Z * num3;
            float num7 = quat.X * num2;
            float num8 = quat.X * num3;
            float num9 = quat.Y * num3;
            float num10 = quat.W * num;
            float num11 = quat.W * num2;
            float num12 = quat.W * num3;
            Vector3 result;
            result.X = (1f - (num5 + num6)) * vec.X + (num7 - num12) * vec.Y + (num8 + num11) * vec.Z;
            result.Y = (num7 + num12) * vec.X + (1f - (num4 + num6)) * vec.Y + (num9 - num10) * vec.Z;
            result.Z = (num8 - num11) * vec.X + (num9 + num10) * vec.Y + (1f - (num4 + num5)) * vec.Z;
            return result;
        }

        public static Quaternion operator *(Quaternion left, Quaternion right)
        {
            float lx = left.X;
            float ly = left.Y;
            float lz = left.Z;
            float lw = left.W;
            float rx = right.X;
            float ry = right.Y;
            float rz = right.Z;
            float rw = right.W;
            float a = (ly * rz - lz * ry);
            float b = (lz * rx - lx * rz);
            float c = (lx * ry - ly * rx);
            float d = (lx * rx + ly * ry + lz * rz);
            Quaternion output;
            output.X = (lx * rw + rx * lw) + a;
            output.Y = (ly * rw + ry * lw) + b;
            output.Z = (lz * rw + rz * lw) + c;
            output.W = lw * rw - d;
            return output;
        }

        public static void FromRotationMatrix(ref Matrix4x4 matrix, out Quaternion result)
        {
            float sqrt;
            float half;
            float scale = matrix.M11 + matrix.M22 + matrix.M33;

            if (scale > 0.0f)
            {
                sqrt = (float)System.Math.Sqrt(scale + 1.0f);
                result.W = sqrt * 0.5f;
                sqrt = 0.5f / sqrt;

                result.X = (matrix.M23 - matrix.M32) * sqrt;
                result.Y = (matrix.M31 - matrix.M13) * sqrt;
                result.Z = (matrix.M12 - matrix.M21) * sqrt;
            }
            else if ((matrix.M11 >= matrix.M22) && (matrix.M11 >= matrix.M33))
            {
                sqrt = (float)System.Math.Sqrt(1.0f + matrix.M11 - matrix.M22 - matrix.M33);
                half = 0.5f / sqrt;

                result.X = 0.5f * sqrt;
                result.Y = (matrix.M12 + matrix.M21) * half;
                result.Z = (matrix.M13 + matrix.M31) * half;
                result.W = (matrix.M23 - matrix.M32) * half;
            }
            else if (matrix.M22 > matrix.M33)
            {
                sqrt = (float)System.Math.Sqrt(1.0f + matrix.M22 - matrix.M11 - matrix.M33);
                half = 0.5f / sqrt;

                result.X = (matrix.M21 + matrix.M12) * half;
                result.Y = 0.5f * sqrt;
                result.Z = (matrix.M32 + matrix.M23) * half;
                result.W = (matrix.M31 - matrix.M13) * half;
            }
            else
            {
                sqrt = (float)System.Math.Sqrt(1.0f + matrix.M33 - matrix.M11 - matrix.M22);
                half = 0.5f / sqrt;

                result.X = (matrix.M31 + matrix.M13) * half;
                result.Y = (matrix.M32 + matrix.M23) * half;
                result.Z = 0.5f * sqrt;
                result.W = (matrix.M12 - matrix.M21) * half;
            }
        }

        public static Quaternion FromRotationMatrix(Matrix4x4 matrix)
        {
            FromRotationMatrix(ref matrix, out var q);
            return q;
        }

        public static Quaternion FromEuler(float x, float y, float z)
        {
            float yaw = x * 0.0174532925f;
            float pitch = y * 0.0174532925f;
            float roll = z * 0.0174532925f;

            double cy = System.Math.Cos(yaw * 0.5);
            double sy = System.Math.Sin(yaw * 0.5);
            double cp = System.Math.Cos(pitch * 0.5);
            double sp = System.Math.Sin(pitch * 0.5);
            double cr = System.Math.Cos(roll * 0.5);
            double sr = System.Math.Sin(roll * 0.5);

            Quaternion q;
            q.W = (float)(cr * cp * cy + sr * sp * sy);
            q.Z = (float)(sr * cp * cy - cr * sp * sy);
            q.Y = (float)(cr * sp * cy + sr * cp * sy);
            q.X = (float)(cr * cp * sy - sr * sp * cy);

            return q;
        }

        public Vector3 ToEuler()
        {
            return ToEuler(this);
        }

        public static Vector3 ToEuler(Quaternion rotation)
        {
            float sqw = rotation.W * rotation.W;
            float sqx = rotation.X * rotation.X;
            float sqy = rotation.Y * rotation.Y;
            float sqz = rotation.Z * rotation.Z;
            float unit = sqx + sqy + sqz + sqw; // if normalised is one, otherwise is correction factor
            float test = rotation.X * rotation.W - rotation.Y * rotation.Z;
            Vector3 v;

            if (test > 0.4995f * unit)
            { // singularity at north pole
                v.Y = (float)(2f * System.Math.Atan2(rotation.Y, rotation.X));
                v.X = (float)(System.Math.PI / 2);
                v.Z = 0;
                return NormalizeAngles(v / 0.0174532925f);
            }
            if (test < -0.4995f * unit)
            { // singularity at south pole
                v.Y = (float)(-2f * System.Math.Atan2(rotation.Y, rotation.X));
                v.X = (float)(-System.Math.PI / 2);
                v.Z = 0;
                return NormalizeAngles(v / 0.0174532925f);
            }
            Quaternion q = new Quaternion(rotation.W, rotation.Z, rotation.X, rotation.Y);
            v.Y = (float)System.Math.Atan2(2f * q.X * q.W + 2f * q.Y * q.Z, 1 - 2f * (q.Z * q.Z + q.W * q.W));     // Yaw
            v.X = (float)System.Math.Asin(2f * (q.X * q.Z - q.W * q.Y));                                              // Pitch
            v.Z = (float)System.Math.Atan2(2f * q.X * q.Y + 2f * q.Z * q.W, 1 - 2f * (q.Y * q.Y + q.Z * q.Z));      // Roll
            return NormalizeAngles(v / 0.0174532925f);
        }

        public static Quaternion LookAt(Vector3 sourcePoint, Vector3 destPoint)
        {
            Vector3 forwardVector = (destPoint - sourcePoint).Normalize();

            float dot = Vector3.Dot(Vector3.Forward, forwardVector);

            if (System.Math.Abs(dot - (-1.0f)) < 0.000001f)
            {
                return new Quaternion(Vector3.Up.X, Vector3.Up.Y, Vector3.Up.Z, 3.1415926535897932f);
            }
            if (System.Math.Abs(dot - (1.0f)) < 0.000001f)
            {
                return Quaternion.Identity;
            }

            float rotAngle = (float)System.Math.Acos(dot);
            Vector3 rotAxis = Vector3.Cross(Vector3.Forward, forwardVector);
            rotAxis = rotAxis.Normalize();
            return CreateFromAxisAngle(rotAxis, rotAngle);
        }

        public static Quaternion CreateFromAxisAngle(Vector3 axis, float angle)
        {
            float halfAngle = angle * .5f;
            float s = (float)System.Math.Sin(halfAngle);
            Quaternion q;
            q.X = axis.X * s;
            q.Y = axis.Y * s;
            q.Z = axis.Z * s;
            q.W = (float)System.Math.Cos(halfAngle);
            return q;
        }

        public static Vector3 NormalizeAngles(Vector3 angles)
        {
            angles.X = NormalizeAngle(angles.X);
            angles.Y = NormalizeAngle(angles.Y);
            angles.Z = NormalizeAngle(angles.Z);
            return angles;
        }

        public static float NormalizeAngle(float angle)
        {
            while (angle > 360)
                angle -= 360;
            while (angle < 0)
                angle += 360;
            return angle;
        }

        #region System.Numerics


        public System.Numerics.Quaternion ToSystemNumerics()
        {
            return new System.Numerics.Quaternion(X, Y, Z, W);
        }

        public static Quaternion FromSystemNumerics(System.Numerics.Quaternion V)
        {
            return new Quaternion(V.X, V.Y, V.Z, V.W);
        }
        #endregion

        public override string ToString()
        {
            return "{" + $"{X.ToString("0.000", CultureInfo.InvariantCulture)}, {Y.ToString("0.000", CultureInfo.InvariantCulture)}, {Z.ToString("0.000", CultureInfo.InvariantCulture)}, {W.ToString("0.000", CultureInfo.InvariantCulture)}" + "}";
        }

        public static implicit operator System.Numerics.Quaternion(Quaternion v) => new System.Numerics.Quaternion(v.X, v.Y, v.Z, v.W);
        public static implicit operator Quaternion(System.Numerics.Quaternion v) => new Quaternion(v.X, v.Y, v.Z, v.W);
    }
}
