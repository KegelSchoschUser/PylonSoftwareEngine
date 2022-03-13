using System;

using System.Runtime.InteropServices;
using System.Text;

namespace PylonGameEngine.Mathematics
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Matrix4x4
    {
        /// <summary>
        /// The size of the <see cref="Matrix4x4"/> type, in bytes.
        /// </summary>
        public static readonly int SizeInBytes = 4 * 4 * sizeof(float);

        /// <summary>
        /// A <see cref="Matrix4x4"/> with all of its components set to zero.
        /// </summary>
        public static readonly Matrix4x4 Zero = new Matrix4x4();

        /// <summary>
        /// The identity <see cref="Matrix4x4"/>.
        /// </summary>
        public static readonly Matrix4x4 Identity = new Matrix4x4() { M11 = 1.0f, M22 = 1.0f, M33 = 1.0f, M44 = 1.0f };

        /// <summary>
        /// Value at row 1 column 1 of the mat4x4.
        /// </summary>
        public float M11;

        /// <summary>
        /// Value at row 1 column 2 of the mat4x4.
        /// </summary>
        public float M12;

        /// <summary>
        /// Value at row 1 column 3 of the mat4x4.
        /// </summary>
        public float M13;

        /// <summary>
        /// Value at row 1 column 4 of the mat4x4.
        /// </summary>
        public float M14;

        /// <summary>
        /// Value at row 2 column 1 of the mat4x4.
        /// </summary>
        public float M21;

        /// <summary>
        /// Value at row 2 column 2 of the mat4x4.
        /// </summary>
        public float M22;

        /// <summary>
        /// Value at row 2 column 3 of the mat4x4.
        /// </summary>
        public float M23;

        /// <summary>
        /// Value at row 2 column 4 of the mat4x4.
        /// </summary>
        public float M24;

        /// <summary>
        /// Value at row 3 column 1 of the mat4x4.
        /// </summary>
        public float M31;

        /// <summary>
        /// Value at row 3 column 2 of the mat4x4.
        /// </summary>
        public float M32;

        /// <summary>
        /// Value at row 3 column 3 of the mat4x4.
        /// </summary>
        public float M33;

        /// <summary>
        /// Value at row 3 column 4 of the mat4x4.
        /// </summary>
        public float M34;

        /// <summary>
        /// Value at row 4 column 1 of the mat4x4.
        /// </summary>
        public float M41;

        /// <summary>
        /// Value at row 4 column 2 of the mat4x4.
        /// </summary>
        public float M42;

        /// <summary>
        /// Value at row 4 column 3 of the mat4x4.
        /// </summary>
        public float M43;

        /// <summary>
        /// Value at row 4 column 4 of the mat4x4.
        /// </summary>
        public float M44;

        // public float[][] m = new float[4][] { new float[4] { 0f, 0f, 0f, 0f }, new float[4] { 0f, 0f, 0f, 0f }, new float[4] { 0f, 0f, 0f, 0f }, new float[4] { 0f, 0f, 0f, 0f } };

        public Matrix4x4(float value)
        {
            M11 = M12 = M13 = M14 =
            M21 = M22 = M23 = M24 =
            M31 = M32 = M33 = M34 =
            M41 = M42 = M43 = M44 = value;
        }

        public Matrix4x4(float M11, float M12, float M13, float M14,
           float M21, float M22, float M23, float M24,
           float M31, float M32, float M33, float M34,
           float M41, float M42, float M43, float M44)
        {
            this.M11 = M11; this.M12 = M12; this.M13 = M13; this.M14 = M14;
            this.M21 = M21; this.M22 = M22; this.M23 = M23; this.M24 = M24;
            this.M31 = M31; this.M32 = M32; this.M33 = M33; this.M34 = M34;
            this.M41 = M41; this.M42 = M42; this.M43 = M43; this.M44 = M44;
        }

        public Matrix4x4(float[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            if (values.Length != 16)
            {
                throw new ArgumentOutOfRangeException("values", "There must be sixteen and only sixteen input values for mat4x4.");
            }

            M11 = values[0];
            M12 = values[1];
            M13 = values[2];
            M14 = values[3];

            M21 = values[4];
            M22 = values[5];
            M23 = values[6];
            M24 = values[7];

            M31 = values[8];
            M32 = values[9];
            M33 = values[10];
            M34 = values[11];

            M41 = values[12];
            M42 = values[13];
            M43 = values[14];
            M44 = values[15];
        }



        /// <summary>
        /// Creates a mat4x4 that rotates around the x-axis.
        /// </summary>
        /// <param name="angle">Angle of rotation in radians. Angles are measured clockwise when looking along the rotation axis toward the origin.</param>
        /// <param name="result">When the method completes, contains the created rotation mat4x4.</param>
        public static void RotationX(float angle, out Matrix4x4 result)
        {
            float cos = (float)System.Math.Cos(angle);
            float sin = (float)System.Math.Sin(angle);

            result = Matrix4x4.Identity;
            result.M22 = cos;
            result.M23 = sin;
            result.M32 = -sin;
            result.M33 = cos;
        }

        /// <summary>
        /// Creates a mat4x4 that rotates around the x-axis.
        /// </summary>
        /// <param name="angle">Angle of rotation in radians. Angles are measured clockwise when looking along the rotation axis toward the origin.</param>
        /// <returns>The created rotation mat4x4.</returns>
        public static Matrix4x4 RotationX(float angle)
        {
            RotationX(angle, out Matrix4x4 result);
            return result;
        }

        /// <summary>
        /// Creates a mat4x4 that rotates around the y-axis.
        /// </summary>
        /// <param name="angle">Angle of rotation in radians. Angles are measured clockwise when looking along the rotation axis toward the origin.</param>
        /// <param name="result">When the method completes, contains the created rotation mat4x4.</param>
        public static void RotationY(float angle, out Matrix4x4 result)
        {
            float cos = (float)System.Math.Cos(angle);
            float sin = (float)System.Math.Sin(angle);

            result = Matrix4x4.Identity;
            result.M11 = cos;
            result.M13 = -sin;
            result.M31 = sin;
            result.M33 = cos;
        }

        /// <summary>
        /// Creates a mat4x4 that rotates around the y-axis.
        /// </summary>
        /// <param name="angle">Angle of rotation in radians. Angles are measured clockwise when looking along the rotation axis toward the origin.</param>
        /// <returns>The created rotation mat4x4.</returns>
        public static Matrix4x4 RotationY(float angle)
        {
            RotationY(angle, out Matrix4x4 result);
            return result;
        }

        /// <summary>
        /// Creates a mat4x4 that rotates around the z-axis.
        /// </summary>
        /// <param name="angle">Angle of rotation in radians. Angles are measured clockwise when looking along the rotation axis toward the origin.</param>
        /// <param name="result">When the method completes, contains the created rotation mat4x4.</param>
        public static void RotationZ(float angle, out Matrix4x4 result)
        {
            float cos = (float)System.Math.Cos(angle);
            float sin = (float)System.Math.Sin(angle);

            result = Matrix4x4.Identity;
            result.M11 = cos;
            result.M12 = sin;
            result.M21 = -sin;
            result.M22 = cos;
        }

        /// <summary>
        /// Creates a mat4x4 that rotates around the z-axis.
        /// </summary>
        /// <param name="angle">Angle of rotation in radians. Angles are measured clockwise when looking along the rotation axis toward the origin.</param>
        /// <returns>The created rotation mat4x4.</returns>
        public static Matrix4x4 RotationZ(float angle)
        {
            RotationZ(angle, out Matrix4x4 result);
            return result;
        }

        /// <summary>
        /// Creates a mat4x4 that rotates around an arbitrary axis.
        /// </summary>
        /// <param name="axis">The axis around which to rotate. This parameter is assumed to be normalized.</param>
        /// <param name="angle">Angle of rotation in radians. Angles are measured clockwise when looking along the rotation axis toward the origin.</param>
        /// <param name="result">When the method completes, contains the created rotation mat4x4.</param>
        public static void RotationAxis(ref Vector3 axis, float angle, out Matrix4x4 result)
        {
            float x = axis.X;
            float y = axis.Y;
            float z = axis.Z;
            float cos = (float)System.Math.Cos(angle);
            float sin = (float)System.Math.Sin(angle);
            float xx = x * x;
            float yy = y * y;
            float zz = z * z;
            float xy = x * y;
            float xz = x * z;
            float yz = y * z;

            result = Matrix4x4.Identity;
            result.M11 = xx + (cos * (1.0f - xx));
            result.M12 = (xy - (cos * xy)) + (sin * z);
            result.M13 = (xz - (cos * xz)) - (sin * y);
            result.M21 = (xy - (cos * xy)) - (sin * z);
            result.M22 = yy + (cos * (1.0f - yy));
            result.M23 = (yz - (cos * yz)) + (sin * x);
            result.M31 = (xz - (cos * xz)) + (sin * y);
            result.M32 = (yz - (cos * yz)) - (sin * x);
            result.M33 = zz + (cos * (1.0f - zz));
        }

        /// <summary>
        /// Creates a mat4x4 that rotates around an arbitrary axis.
        /// </summary>
        /// <param name="axis">The axis around which to rotate. This parameter is assumed to be normalized.</param>
        /// <param name="angle">Angle of rotation in radians. Angles are measured clockwise when looking along the rotation axis toward the origin.</param>
        /// <returns>The created rotation mat4x4.</returns>
        public static Matrix4x4 RotationAxis(Vector3 axis, float angle)
        {
            RotationAxis(ref axis, angle, out Matrix4x4 result);
            return result;
        }

        public static Matrix4x4 RotationYawPitchRoll(float yaw, float pitch, float roll)
        {
            RotationYawPitchRoll(yaw, pitch, roll, out Matrix4x4 result);
            return result;
        }

        public static Matrix4x4 RotationYawPitchRoll(Vector3 rotation)
        {
            RotationYawPitchRoll(rotation.X, rotation.Y, rotation.Z, out Matrix4x4 result);
            return result;
        }

        public static Matrix4x4 R_active_x(float angle)
        {
            float c = (float)System.Math.Cos(angle);
            float s = (float)System.Math.Sin(angle);

            Matrix4x4 ret = Matrix4x4.Identity;
            ret.M22 = c;
            ret.M23 = -s;
            ret.M32 = s;
            ret.M33 = c;

            return ret;
        }

        public static Matrix4x4 R_active_y(float angle)
        {
            float c = (float)System.Math.Cos(angle);
            float s = (float)System.Math.Sin(angle);


            Matrix4x4 ret = Matrix4x4.Identity;
            ret.M11 = c;
            ret.M13 = s;
            ret.M31 = -s;
            ret.M33 = c;

            return ret;
        }

        public static Matrix4x4 R_active_z(float angle)
        {
            float c = (float)System.Math.Cos(angle);
            float s = (float)System.Math.Sin(angle);

            Matrix4x4 ret = Matrix4x4.Identity;
            ret.M11 = c;
            ret.M12 = -s;
            ret.M21 = s;
            ret.M22 = c;

            return ret;
        }

        public static Matrix4x4 R_active(char axis, float angle)
        {
            Matrix4x4 R = Matrix4x4.Identity;
            switch (axis)
            {
                case 'x':
                    R = R_active_x(angle);
                    break;
                case 'y':
                    R = R_active_y(angle);
                    break;
                case 'z':
                    R = R_active_z(angle);
                    break;
            }
            return R;
        }

        public static Quaternion RotationQuaternion(Matrix4x4 R)
        {
            Quaternion q;
            q.X = (float)(System.Math.Sqrt(System.Math.Max(0.0, 1.0 + R.M11 + R.M22 + R.M33)) / 2.0);

            q.Y = (float)(System.Math.Sqrt(System.Math.Max(0.0, 1.0 + R.M11 - R.M22 - R.M33)) / 2.0);
            q.Y = (float)System.Math.CopySign(q.Y, R.M23 - R.M32);

            q.Z = (float)(System.Math.Sqrt(System.Math.Max(0.0, 1.0 - R.M11 + R.M22 - R.M33)) / 2.0);
            q.Z = (float)System.Math.CopySign(q.Z, R.M21 - R.M13);

            q.W = (float)(System.Math.Sqrt(System.Math.Max(0.0, 1.0 - R.M11 - R.M22 + R.M33)) / 2.0);
            q.W = (float)System.Math.CopySign(q.W, R.M12 - R.M21);
            return q;

        }



        static Vector3 Multiply(Quaternion quat, Vector3 vec)
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

        public static Matrix4x4 Rotation(Vector3 Rotation)
        {
            Rotation *= -1f;
            Rotation *= 0.0174532925f;
            // Calculate rotation about x axis
            Matrix4x4 R_x = new Matrix4x4(
                       1, 0, 0, 0,
                       0, (float)System.Math.Cos(Rotation.X), (float)System.Math.Sin(Rotation.X), 0,
                       0, -(float)System.Math.Sin(Rotation.X), (float)System.Math.Cos(Rotation.X), 0,
                       0, 0, 0, 1
                       );

            // Calculate rotation about y axis
            Matrix4x4 R_y = new Matrix4x4(
                       (float)System.Math.Cos(Rotation.Y), 0, -(float)System.Math.Sin(Rotation.Y), 0,
                       0, 1, 0, 0,
                       (float)System.Math.Sin(Rotation.Y), 0, (float)System.Math.Cos(Rotation.Y), 0,
                       0, 0, 0, 1
                       );

            // Calculate rotation about z axis
            Matrix4x4 R_z = new Matrix4x4(
                       (float)System.Math.Cos(Rotation.Z), (float)System.Math.Sin(Rotation.Z), 0, 0,
                       -(float)System.Math.Sin(Rotation.Z), (float)System.Math.Cos(Rotation.Z), 0, 0,
                       0, 0, 1, 0,
                       0, 0, 0, 1);

            // Combined rotation matrix
            Matrix4x4 R = R_z * R_y * R_x;
            R.Transpose();
            return R;



            //Matrix4x4 R;

            //string Order = "zyx";

            //float yaw = rotationvector3.X;
            //float pitch = rotationvector3.Y;
            //float roll = rotationvector3.Z;

            //Quaternion q = new Quaternion();
            // q.X = (float)(System.Math.Sin(roll / 2) * System.Math.Cos(pitch / 2) * System.Math.Cos(yaw / 2)         -         System.Math.Cos(roll / 2) * System.Math.Sin(pitch / 2) * System.Math.Sin(yaw / 2));
            // q.Y = (float)(System.Math.Cos(roll / 2) * System.Math.Sin(pitch / 2) * System.Math.Cos(yaw / 2)         +         System.Math.Sin(roll / 2) * System.Math.Cos(pitch / 2) * System.Math.Sin(yaw / 2));
            // q.Z = (float)(System.Math.Cos(roll / 2) * System.Math.Cos(pitch / 2) * System.Math.Sin(yaw / 2)         -         System.Math.Sin(roll / 2) * System.Math.Sin(pitch / 2) * System.Math.Cos(yaw / 2));
            // q.W = (float)(System.Math.Cos(roll / 2) * System.Math.Cos(pitch / 2) * System.Math.Cos(yaw / 2)         +         System.Math.Sin(roll / 2) * System.Math.Sin(pitch / 2) * System.Math.Sin(yaw / 2));

            //var R_X = R_active_x(rotationvector3.X);
            //var R_Y = R_active_y(rotationvector3.Y);
            //var R_Z = R_active_z(rotationvector3.Z);
            //R = R_Z * R_Y * R_X;

            //    R = R_active(Order[0], rotationvector3.X) * R_active(Order[1], rotationvector3.Y) *
            //R_active(Order[2], rotationvector3.Z);
            //    R = R_active(Order[0], rotationvector3) * R_active(Order[1], rotationvector3) *
            //R_active(Order[2], rotationvector3);

            //return RotationQuaternion(q);
            //return R;


            //QuatRotationYawPitchRoll(rotationvector3.X, rotationvector3.Y, rotationvector3.Z, out Quaternion rotation);

            //float xx = rotation.X * rotation.X;
            //float yy = rotation.Y * rotation.Y;
            //float zz = rotation.Z * rotation.Z;
            //float xy = rotation.X * rotation.Y;
            //float zw = rotation.Z * rotation.W;
            //float zx = rotation.Z * rotation.X;
            //float yw = rotation.Y * rotation.W;
            //float yz = rotation.Y * rotation.Z;
            //float xw = rotation.X * rotation.W;

            //M11 += 1.0f - (2.0f * (yy + zz));
            //M12 += 2.0f * (xy + zw);
            //M13 += 2.0f * (zx - yw);
            //M21 += 2.0f * (xy - zw);
            //M22 += 1.0f - (2.0f * (zz + xx));
            //M23 += 2.0f * (yz + xw);
            //M31 += 2.0f * (zx + yw);
            //M32 += 2.0f * (yz - xw);
            //M33 += 1.0f - (2.0f * (yy + xx));
        }

        /// <summary>
        /// Scales a matrix by a given value.
        /// </summary>
        /// <param name="right">The matrix to scale.</param>
        /// <param name="left">The amount by which to scale.</param>
        /// <returns>The scaled matrix.</returns>
        public static Matrix4x4 operator *(float left, Matrix4x4 right)
        {
            Multiply(ref right, left, out Matrix4x4 result);
            return result;
        }
        /// <summary>
        /// Multiplies two matrices.
        /// </summary>
        /// <param name="left">The first Matrix4x4 to multiply.</param>
        /// <param name="right">The second Matrix4x4 to multiply.</param>
        /// <returns>The product of the two matrices.</returns>
        public static Matrix4x4 operator *(Matrix4x4 left, Matrix4x4 right)
        {
            Multiply(ref left, ref right, out Matrix4x4 result);
            return result;
        }

        /// <summary>
        /// Scales a Matrix4x4 by a given value.
        /// </summary>
        /// <param name="left">The Matrix4x4 to scale.</param>
        /// <param name="right">The amount by which to scale.</param>
        /// <returns>The scaled Matrix4x4.</returns>
        public static Matrix4x4 operator /(Matrix4x4 left, float right)
        {
            Divide(ref left, right, out Matrix4x4 result);
            return result;
        }

        /// <summary>
        /// Divides two matrices.
        /// </summary>
        /// <param name="left">The first Matrix4x4 to divide.</param>
        /// <param name="right">The second Matrix4x4 to divide.</param>
        /// <returns>The quotient of the two matrices.</returns>
        public static Matrix4x4 operator /(Matrix4x4 left, Matrix4x4 right)
        {
            Divide(ref left, ref right, out Matrix4x4 result);
            return result;
        }

        /// <summary>
        /// Scales a Matrix4x4 by the given value.
        /// </summary>
        /// <param name="left">The Matrix4x4 to scale.</param>
        /// <param name="right">The amount by which to scale.</param>
        /// <param name="result">When the method completes, contains the scaled Matrix4x4.</param>
        public static void Multiply(ref Matrix4x4 left, float right, out Matrix4x4 result)
        {
            result.M11 = left.M11 * right;
            result.M12 = left.M12 * right;
            result.M13 = left.M13 * right;
            result.M14 = left.M14 * right;
            result.M21 = left.M21 * right;
            result.M22 = left.M22 * right;
            result.M23 = left.M23 * right;
            result.M24 = left.M24 * right;
            result.M31 = left.M31 * right;
            result.M32 = left.M32 * right;
            result.M33 = left.M33 * right;
            result.M34 = left.M34 * right;
            result.M41 = left.M41 * right;
            result.M42 = left.M42 * right;
            result.M43 = left.M43 * right;
            result.M44 = left.M44 * right;
        }

        /// <summary>
        /// Scales a Matrix4x4 by the given value.
        /// </summary>
        /// <param name="left">The Matrix4x4 to scale.</param>
        /// <param name="right">The amount by which to scale.</param>
        /// <returns>The scaled Matrix4x4.</returns>
        public static Matrix4x4 Multiply(Matrix4x4 left, float right)
        {
            Multiply(ref left, right, out Matrix4x4 result);
            return result;
        }

        /// <summary>
        /// Determines the product of two matrices.
        /// </summary>
        /// <param name="left">The first Matrix4x4 to multiply.</param>
        /// <param name="right">The second Matrix4x4 to multiply.</param>
        /// <param name="result">The product of the two matrices.</param>
        public static void Multiply(ref Matrix4x4 left, ref Matrix4x4 right, out Matrix4x4 result)
        {
            Matrix4x4 temp = new Matrix4x4
            {
                M11 = (left.M11 * right.M11) + (left.M12 * right.M21) + (left.M13 * right.M31) + (left.M14 * right.M41),
                M12 = (left.M11 * right.M12) + (left.M12 * right.M22) + (left.M13 * right.M32) + (left.M14 * right.M42),
                M13 = (left.M11 * right.M13) + (left.M12 * right.M23) + (left.M13 * right.M33) + (left.M14 * right.M43),
                M14 = (left.M11 * right.M14) + (left.M12 * right.M24) + (left.M13 * right.M34) + (left.M14 * right.M44),
                M21 = (left.M21 * right.M11) + (left.M22 * right.M21) + (left.M23 * right.M31) + (left.M24 * right.M41),
                M22 = (left.M21 * right.M12) + (left.M22 * right.M22) + (left.M23 * right.M32) + (left.M24 * right.M42),
                M23 = (left.M21 * right.M13) + (left.M22 * right.M23) + (left.M23 * right.M33) + (left.M24 * right.M43),
                M24 = (left.M21 * right.M14) + (left.M22 * right.M24) + (left.M23 * right.M34) + (left.M24 * right.M44),
                M31 = (left.M31 * right.M11) + (left.M32 * right.M21) + (left.M33 * right.M31) + (left.M34 * right.M41),
                M32 = (left.M31 * right.M12) + (left.M32 * right.M22) + (left.M33 * right.M32) + (left.M34 * right.M42),
                M33 = (left.M31 * right.M13) + (left.M32 * right.M23) + (left.M33 * right.M33) + (left.M34 * right.M43),
                M34 = (left.M31 * right.M14) + (left.M32 * right.M24) + (left.M33 * right.M34) + (left.M34 * right.M44),
                M41 = (left.M41 * right.M11) + (left.M42 * right.M21) + (left.M43 * right.M31) + (left.M44 * right.M41),
                M42 = (left.M41 * right.M12) + (left.M42 * right.M22) + (left.M43 * right.M32) + (left.M44 * right.M42),
                M43 = (left.M41 * right.M13) + (left.M42 * right.M23) + (left.M43 * right.M33) + (left.M44 * right.M43),
                M44 = (left.M41 * right.M14) + (left.M42 * right.M24) + (left.M43 * right.M34) + (left.M44 * right.M44)
            };
            result = temp;
        }

        /// <summary>
        /// Determines the product of two matrices.
        /// </summary>
        /// <param name="left">The first Matrix4x4 to multiply.</param>
        /// <param name="right">The second Matrix4x4 to multiply.</param>
        /// <returns>The product of the two matrices.</returns>
        public static Matrix4x4 Multiply(Matrix4x4 left, Matrix4x4 right)
        {
            Multiply(ref left, ref right, out Matrix4x4 result);
            return result;
        }

        /// <summary>
        /// Scales a Matrix4x4 by the given value.
        /// </summary>
        /// <param name="left">The Matrix4x4 to scale.</param>
        /// <param name="right">The amount by which to scale.</param>
        /// <param name="result">When the method completes, contains the scaled Matrix4x4.</param>
        public static void Divide(ref Matrix4x4 left, float right, out Matrix4x4 result)
        {
            float inv = 1.0f / right;

            result.M11 = left.M11 * inv;
            result.M12 = left.M12 * inv;
            result.M13 = left.M13 * inv;
            result.M14 = left.M14 * inv;
            result.M21 = left.M21 * inv;
            result.M22 = left.M22 * inv;
            result.M23 = left.M23 * inv;
            result.M24 = left.M24 * inv;
            result.M31 = left.M31 * inv;
            result.M32 = left.M32 * inv;
            result.M33 = left.M33 * inv;
            result.M34 = left.M34 * inv;
            result.M41 = left.M41 * inv;
            result.M42 = left.M42 * inv;
            result.M43 = left.M43 * inv;
            result.M44 = left.M44 * inv;
        }

        /// <summary>
        /// Scales a Matrix4x4 by the given value.
        /// </summary>
        /// <param name="left">The Matrix4x4 to scale.</param>
        /// <param name="right">The amount by which to scale.</param>
        /// <returns>The scaled Matrix4x4.</returns>
        public static Matrix4x4 Divide(Matrix4x4 left, float right)
        {
            Divide(ref left, right, out Matrix4x4 result);
            return result;
        }

        /// <summary>
        /// Determines the quotient of two matrices.
        /// </summary>
        /// <param name="left">The first Matrix4x4 to divide.</param>
        /// <param name="right">The second Matrix4x4 to divide.</param>
        /// <param name="result">When the method completes, contains the quotient of the two matrices.</param>
        public static void Divide(ref Matrix4x4 left, ref Matrix4x4 right, out Matrix4x4 result)
        {
            result.M11 = left.M11 / right.M11;
            result.M12 = left.M12 / right.M12;
            result.M13 = left.M13 / right.M13;
            result.M14 = left.M14 / right.M14;
            result.M21 = left.M21 / right.M21;
            result.M22 = left.M22 / right.M22;
            result.M23 = left.M23 / right.M23;
            result.M24 = left.M24 / right.M24;
            result.M31 = left.M31 / right.M31;
            result.M32 = left.M32 / right.M32;
            result.M33 = left.M33 / right.M33;
            result.M34 = left.M34 / right.M34;
            result.M41 = left.M41 / right.M41;
            result.M42 = left.M42 / right.M42;
            result.M43 = left.M43 / right.M43;
            result.M44 = left.M44 / right.M44;
        }

        /// <summary>
        /// Determines the quotient of two matrices.
        /// </summary>
        /// <param name="left">The first Matrix4x4 to divide.</param>
        /// <param name="right">The second Matrix4x4 to divide.</param>
        /// <returns>The quotient of the two matrices.</returns>
        public static Matrix4x4 Divide(Matrix4x4 left, Matrix4x4 right)
        {
            Divide(ref left, ref right, out Matrix4x4 result);
            return result;
        }

        /// <summary>
        /// Creates a rotation mat4x4 with a specified yaw, pitch, and roll.
        /// </summary>
        /// <param name="yaw">Yaw around the y-axis, in radians.</param>
        /// <param name="pitch">Pitch around the x-axis, in radians.</param>
        /// <param name="roll">Roll around the z-axis, in radians.</param>
        /// <param name="result">When the method completes, contains the created rotation mat4x4.</param>
        public static void RotationYawPitchRoll(float yaw, float pitch, float roll, out Matrix4x4 result)
        {
            Quaternion quaternion = new Quaternion();
            QuatRotationYawPitchRoll(yaw, pitch, roll, out quaternion);
            RotationQuaternion(quaternion, out result);
        }


        /// <summary>
        /// Creates a quaternion given a yaw, pitch, and roll value.
        /// </summary>
        /// <param name="yaw">The yaw of rotation.</param>
        /// <param name="pitch">The pitch of rotation.</param>
        /// <param name="roll">The roll of rotation.</param>
        /// <param name="result">When the method completes, contains the newly created quaternion.</param>
        public static void QuatRotationYawPitchRoll(float yaw, float pitch, float roll, out Quaternion result)
        {
            float halfRoll = roll * 0.5f;
            float halfPitch = pitch * 0.5f;
            float halfYaw = yaw * 0.5f;

            float sinRoll = (float)System.Math.Sin(halfRoll);
            float cosRoll = (float)System.Math.Cos(halfRoll);
            float sinPitch = (float)System.Math.Sin(halfPitch);
            float cosPitch = (float)System.Math.Cos(halfPitch);
            float sinYaw = (float)System.Math.Sin(halfYaw);
            float cosYaw = (float)System.Math.Cos(halfYaw);

            result.X = (cosYaw * sinPitch * cosRoll) + (sinYaw * cosPitch * sinRoll);
            result.Y = (sinYaw * cosPitch * cosRoll) - (cosYaw * sinPitch * sinRoll);
            result.Z = (cosYaw * cosPitch * sinRoll) - (sinYaw * sinPitch * cosRoll);
            result.W = (cosYaw * cosPitch * cosRoll) + (sinYaw * sinPitch * sinRoll);
        }

        public static Matrix4x4 RotationQuaternion(Quaternion rotation)
        {
            Matrix4x4 result = Matrix4x4.Identity;
            RotationQuaternion(rotation, out result);
            return result;
        }

        /// <summary>
        /// Creates a rotation mat4x4 from a quaternion.
        /// </summary>
        /// <param name="rotation">The quaternion to use to build the mat4x4.</param>
        /// <param name="result">The created rotation mat4x4.</param>
        public static void RotationQuaternion(Quaternion rotation, out Matrix4x4 result)
        {
            float xx = rotation.X * rotation.X;
            float yy = rotation.Y * rotation.Y;
            float zz = rotation.Z * rotation.Z;
            float xy = rotation.X * rotation.Y;
            float zw = rotation.Z * rotation.W;
            float zx = rotation.Z * rotation.X;
            float yw = rotation.Y * rotation.W;
            float yz = rotation.Y * rotation.Z;
            float xw = rotation.X * rotation.W;

            result = Matrix4x4.Identity;
            result.M11 = 1.0f - (2.0f * (yy + zz));
            result.M12 = 2.0f * (xy + zw);
            result.M13 = 2.0f * (zx - yw);
            result.M21 = 2.0f * (xy - zw);
            result.M22 = 1.0f - (2.0f * (zz + xx));
            result.M23 = 2.0f * (yz + xw);
            result.M31 = 2.0f * (zx + yw);
            result.M32 = 2.0f * (yz - xw);
            result.M33 = 1.0f - (2.0f * (yy + xx));
        }

        public static Vector3 mat4x4_MultiplyVector(Matrix4x4 m, Vector3 i)
        {
            Vector3 v = new Vector3
            {
                X = i.X * m.M11 + i.Y * m.M21 + i.Z * m.M31 + 1 * m.M41,
                Y = i.X * m.M12 + i.Y * m.M22 + i.Z * m.M32 + 1 * m.M42,
                Z = i.X * m.M13 + i.Y * m.M23 + i.Z * m.M33 + 1 * m.M43
            };
            // v.w = i.x * m.M14 + i.y * m.M24 + i.z * m.m[2][3] + 1 * m.M44;
            return v;
        }

        public static Matrix4x4 mat4x4_MakeIdentity()
        {
            Matrix4x4 mat4x4 = new Matrix4x4
            {
                M11 = 1.0f,
                M22 = 1.0f,
                M33 = 1.0f,
                M44 = 1.0f
            };
            return mat4x4;
        }

        public static Matrix4x4 mat4x4_MakeRotationX(float fAngleRad)
        {
            Matrix4x4 mat4x4 = new Matrix4x4
            {
                M11 = 1.0f,
                M22 = (float)System.Math.Cos(fAngleRad),
                M23 = (float)System.Math.Sin(fAngleRad),
                M32 = -(float)System.Math.Sin(fAngleRad),
                M33 = (float)System.Math.Cos(fAngleRad),
                M44 = 1.0f
            };
            return mat4x4;
        }

        public static Matrix4x4 mat4x4_MakeRotationY(float fAngleRad)
        {
            Matrix4x4 mat4x4 = new Matrix4x4
            {
                M11 = (float)System.Math.Cos(fAngleRad),
                M13 = (float)System.Math.Sin(fAngleRad),
                M31 = -(float)System.Math.Sin(fAngleRad),
                M22 = 1.0f,
                M33 = (float)System.Math.Cos(fAngleRad),
                M44 = 1.0f
            };
            return mat4x4;
        }

        public static Matrix4x4 mat4x4_MakeRotationZ(float fAngleRad)
        {
            Matrix4x4 mat4x4 = new Matrix4x4
            {
                M11 = (float)System.Math.Cos(fAngleRad),
                M12 = (float)System.Math.Sin(fAngleRad),
                M21 = -(float)System.Math.Sin(fAngleRad),
                M22 = (float)System.Math.Cos(fAngleRad),
                M33 = 1.0f,
                M44 = 1.0f
            };
            return mat4x4;
        }

        public static Matrix4x4 mat4x4_MakeTranslation(float x, float y, float z)
        {
            Matrix4x4 mat4x4 = new Matrix4x4
            {
                M11 = 1.0f,
                M22 = 1.0f,
                M33 = 1.0f,
                M44 = 1.0f,
                M41 = x,
                M42 = y,
                M43 = z
            };
            return mat4x4;
        }

        /// <summary>
        /// Creates a left-handed, perspective projection mat4x4 based on a field of view.
        /// </summary>
        /// <param name="fov">Field of view in the y direction, in radians.</param>
        /// <param name="aspect">Aspect ratio, defined as view space width divided by height.</param>
        /// <param name="znear">Minimum z-value of the viewing volume.</param>
        /// <param name="zfar">Maximum z-value of the viewing volume.</param>
        /// <param name="result">When the method completes, contains the created projection mat4x4.</param>
        public static void PerspectiveFovLH(float fov, float aspect, float znear, float zfar, out Matrix4x4 result)
        {
            float yScale = (float)(1.0f / System.Math.Tan(fov * 0.5f));
            float q = zfar / (zfar - znear);

            result = new Matrix4x4
            {
                M11 = yScale / aspect,
                M22 = yScale,
                M33 = q,
                M34 = 1.0f,
                M43 = -q * znear
            };
        }

        /// <summary>
        /// Creates a left-handed, perspective projection mat4x4 based on a field of view.
        /// </summary>
        /// <param name="fov">Field of view in the y direction, in radians.</param>
        /// <param name="aspect">Aspect ratio, defined as view space width divided by height.</param>
        /// <param name="znear">Minimum z-value of the viewing volume.</param>
        /// <param name="zfar">Maximum z-value of the viewing volume.</param>
        /// <returns>The created projection mat4x4.</returns>
        public static Matrix4x4 PerspectiveFovLH(float fov, float aspect, float znear, float zfar)
        {
            PerspectiveFovLH(fov, aspect, znear, zfar, out Matrix4x4 result);
            return result;
        }

        //public static mat4x4 mat4x4_Multiplymat4x4(mat4x4 m1, mat4x4 m2)
        //{
        //    mat4x4 mat4x4 = new mat4x4();
        //    for (int c = 0; c < 4; c++)
        //        for (int r = 0; r < 4; r++)
        //            mat4x4.m[r][c] = m1.m[r][0] * m2.m[0][c] + m1.m[r][1] * m2.m[1][c] + m1.m[r][2] * m2.m[2][c] + m1.m[r][3] * m2.m[3][c];
        //    return mat4x4;
        //}


        /// <summary>
        /// Creates a left-handed, look-at mat4x4.
        /// </summary>
        /// <param name="eye">The position of the viewer's eye.</param>
        /// <param name="target">The camera look-at target.</param>
        /// <param name="up">The camera's up vector.</param>
        /// <returns>The created look-at mat4x4.</returns>
        public static Matrix4x4 LookAtLH(Vector3 eye, Vector3 target, Vector3 up)
        {
            LookAtLH(ref eye, ref target, ref up, out Matrix4x4 result);

            return result;
        }

        /// <summary>
        /// Creates a left-handed, look-at mat4x4.
        /// </summary>
        /// <param name="eye">The position of the viewer's eye.</param>
        /// <param name="target">The camera look-at target.</param>
        /// <param name="up">The camera's up vector.</param>
        /// <param name="result">When the method completes, contains the created look-at mat4x4.</param>
        public static void LookAtLH(ref Vector3 eye, ref Vector3 target, ref Vector3 up, out Matrix4x4 result)
        {
            Vector3.Subtract(ref target, ref eye, out Vector3 zaxis); zaxis.Normalize();
            Vector3.Cross(ref up, ref zaxis, out Vector3 xaxis); xaxis.Normalize();
            Vector3.Cross(ref zaxis, ref xaxis, out Vector3 yaxis);

            result = Matrix4x4.Identity;
            result.M11 = xaxis.X; result.M21 = xaxis.Y; result.M31 = xaxis.Z;
            result.M12 = yaxis.X; result.M22 = yaxis.Y; result.M32 = yaxis.Z;
            result.M13 = zaxis.X; result.M23 = zaxis.Y; result.M33 = zaxis.Z;

            result.M41 = Vector3.Dot(xaxis, eye);
            result.M42 = Vector3.Dot(yaxis, eye);
            result.M43 = Vector3.Dot(zaxis, eye);

            result.M41 = -result.M41;
            result.M42 = -result.M42;
            result.M43 = -result.M43;
        }


        public static Matrix4x4 mat4x4_QuickInverse(Matrix4x4 m) // Only for Rotation/Translation Matrices
        {
            Matrix4x4 mat4x4 = new Matrix4x4
            {
                M11 = m.M11,
                M12 = m.M21,
                M13 = m.M31,
                M14 = 0.0f,
                M21 = m.M12,
                M22 = m.M22,
                M23 = m.M32,
                M24 = 0.0f,
                M31 = m.M13,
                M32 = m.M23,
                M33 = m.M33,
                M34 = 0.0f
            };
            mat4x4.M41 = -(m.M41 * mat4x4.M11 + m.M42 * mat4x4.M21 + m.M43 * mat4x4.M31);
            mat4x4.M42 = -(m.M41 * mat4x4.M12 + m.M42 * mat4x4.M22 + m.M43 * mat4x4.M32);
            mat4x4.M43 = -(m.M41 * mat4x4.M13 + m.M42 * mat4x4.M23 + m.M43 * mat4x4.M33);
            mat4x4.M44 = 1.0f;
            return mat4x4;
        }

        /// <summary>
        /// Transposes the mat4x4.
        /// </summary>
        public void Transpose()
        {
            Transpose(ref this, out this);
        }

        /// <summary>
        /// Calculates the inverse of the specified matrix.
        /// </summary>
        /// <param name="value">The matrix whose inverse is to be calculated.</param>
        /// <param name="result">When the method completes, contains the inverse of the specified matrix.</param>
        public static void Invert(ref Matrix4x4 value, out Matrix4x4 result)
        {
            float b0 = (value.M31 * value.M42) - (value.M32 * value.M41);
            float b1 = (value.M31 * value.M43) - (value.M33 * value.M41);
            float b2 = (value.M34 * value.M41) - (value.M31 * value.M44);
            float b3 = (value.M32 * value.M43) - (value.M33 * value.M42);
            float b4 = (value.M34 * value.M42) - (value.M32 * value.M44);
            float b5 = (value.M33 * value.M44) - (value.M34 * value.M43);

            float d11 = value.M22 * b5 + value.M23 * b4 + value.M24 * b3;
            float d12 = value.M21 * b5 + value.M23 * b2 + value.M24 * b1;
            float d13 = value.M21 * -b4 + value.M22 * b2 + value.M24 * b0;
            float d14 = value.M21 * b3 + value.M22 * -b1 + value.M23 * b0;

            float det = value.M11 * d11 - value.M12 * d12 + value.M13 * d13 - value.M14 * d14;
            if (System.Math.Abs(det) == 0.0f)
            {
                result = Matrix4x4.Zero;
                return;
            }

            det = 1f / det;

            float a0 = (value.M11 * value.M22) - (value.M12 * value.M21);
            float a1 = (value.M11 * value.M23) - (value.M13 * value.M21);
            float a2 = (value.M14 * value.M21) - (value.M11 * value.M24);
            float a3 = (value.M12 * value.M23) - (value.M13 * value.M22);
            float a4 = (value.M14 * value.M22) - (value.M12 * value.M24);
            float a5 = (value.M13 * value.M24) - (value.M14 * value.M23);

            float d21 = value.M12 * b5 + value.M13 * b4 + value.M14 * b3;
            float d22 = value.M11 * b5 + value.M13 * b2 + value.M14 * b1;
            float d23 = value.M11 * -b4 + value.M12 * b2 + value.M14 * b0;
            float d24 = value.M11 * b3 + value.M12 * -b1 + value.M13 * b0;

            float d31 = value.M42 * a5 + value.M43 * a4 + value.M44 * a3;
            float d32 = value.M41 * a5 + value.M43 * a2 + value.M44 * a1;
            float d33 = value.M41 * -a4 + value.M42 * a2 + value.M44 * a0;
            float d34 = value.M41 * a3 + value.M42 * -a1 + value.M43 * a0;

            float d41 = value.M32 * a5 + value.M33 * a4 + value.M34 * a3;
            float d42 = value.M31 * a5 + value.M33 * a2 + value.M34 * a1;
            float d43 = value.M31 * -a4 + value.M32 * a2 + value.M34 * a0;
            float d44 = value.M31 * a3 + value.M32 * -a1 + value.M33 * a0;

            result.M11 = +d11 * det; result.M12 = -d21 * det; result.M13 = +d31 * det; result.M14 = -d41 * det;
            result.M21 = -d12 * det; result.M22 = +d22 * det; result.M23 = -d32 * det; result.M24 = +d42 * det;
            result.M31 = +d13 * det; result.M32 = -d23 * det; result.M33 = +d33 * det; result.M34 = -d43 * det;
            result.M41 = -d14 * det; result.M42 = +d24 * det; result.M43 = -d34 * det; result.M44 = +d44 * det;
        }

        /// <summary>
        /// Calculates the inverse of the specified matrix.
        /// </summary>
        /// <param name="value">The matrix whose inverse is to be calculated.</param>
        /// <returns>The inverse of the specified matrix.</returns>
        public static Matrix4x4 Invert(Matrix4x4 value)
        {
            value.Invert();
            return value;
        }

        /// <summary>
        /// Inverts the matrix.
        /// </summary>
        public void Invert()
        {
            Invert(ref this, out this);
        }

        /// <summary>
        /// Calculates the transpose of the specified mat4x4.
        /// </summary>
        /// <param name="value">The mat4x4 whose transpose is to be calculated.</param>
        /// <param name="result">When the method completes, contains the transpose of the specified mat4x4.</param>
        public static void Transpose(ref Matrix4x4 value, out Matrix4x4 result)
        {
            Matrix4x4 temp = new Matrix4x4
            {
                M11 = value.M11,
                M12 = value.M21,
                M13 = value.M31,
                M14 = value.M41,
                M21 = value.M12,
                M22 = value.M22,
                M23 = value.M32,
                M24 = value.M42,
                M31 = value.M13,
                M32 = value.M23,
                M33 = value.M33,
                M34 = value.M43,
                M41 = value.M14,
                M42 = value.M24,
                M43 = value.M34,
                M44 = value.M44
            };

            result = temp;
        }

        /// <summary>
        /// Calculates the transpose of the specified mat4x4.
        /// </summary>
        /// <param name="value">The mat4x4 whose transpose is to be calculated.</param>
        /// <param name="result">When the method completes, contains the transpose of the specified mat4x4.</param>
        public static void TransposeByRef(ref Matrix4x4 value, ref Matrix4x4 result)
        {
            result.M11 = value.M11;
            result.M12 = value.M21;
            result.M13 = value.M31;
            result.M14 = value.M41;
            result.M21 = value.M12;
            result.M22 = value.M22;
            result.M23 = value.M32;
            result.M24 = value.M42;
            result.M31 = value.M13;
            result.M32 = value.M23;
            result.M33 = value.M33;
            result.M34 = value.M43;
            result.M41 = value.M14;
            result.M42 = value.M24;
            result.M43 = value.M34;
            result.M44 = value.M44;
        }

        /// <summary>
        /// Gets or sets the translation of the mat4x4; that is M41, M42, and M43.
        /// </summary>
        public Vector3 TranslationVector
        {
            get => new Vector3(M41, M42, M43);
            set { M41 = value.X; M42 = value.Y; M43 = value.Z; }
        }

        public Vector2 TranslationVector2D
        {
            get => new Vector2(M41, M42);
            set { M41 = value.X; M42 = value.Y; }
        }

        /// <summary>
        /// Gets or sets the scale of the mat4x4; that is M11, M22, and M33.
        /// </summary>
        public Vector3 ScaleVector
        {
            get => new Vector3(M11, M22, M33);
            set { M11 = value.X; M22 = value.Y; M33 = value.Z; }
        }

        public Vector2 ScaleVector2D
        {
            get => new Vector2(M11, M22);
            set { M11 = value.X; M22 = value.Y; }
        }

        /// <summary>
        /// Creates a translation mat4x4 using the specified offsets.
        /// </summary>
        /// <param name="value">The offset for all three coordinate planes.</param>
        /// <param name="result">When the method completes, contains the created translation mat4x4.</param>
        public static void Translation(ref Vector3 value, out Matrix4x4 result)
        {
            Translation(value.X, value.Y, value.Z, out result);
        }

        /// <summary>
        /// Creates a translation mat4x4 using the specified offsets.
        /// </summary>
        /// <param name="value">The offset for all three coordinate planes.</param>
        /// <returns>The created translation mat4x4.</returns>
        public static Matrix4x4 Translation(Vector3 value)
        {
            Translation(ref value, out Matrix4x4 result);
            return result;
        }

        public void Translate(Vector3 value)
        {
            M41 += value.X;
            M42 += value.Y;
            M43 += value.Z;
        }

        /// <summary>
        /// Creates a translation mat4x4 using the specified offsets.
        /// </summary>
        /// <param name="x">X-coordinate offset.</param>
        /// <param name="y">Y-coordinate offset.</param>
        /// <param name="z">Z-coordinate offset.</param>
        /// <param name="result">When the method completes, contains the created translation mat4x4.</param>
        public static void Translation(float x, float y, float z, out Matrix4x4 result)
        {
            result = Matrix4x4.Identity;
            result.M41 = x;
            result.M42 = y;
            result.M43 = z;
        }

        /// <summary>
        /// Creates a translation mat4x4 using the specified offsets.
        /// </summary>
        /// <param name="x">X-coordinate offset.</param>
        /// <param name="y">Y-coordinate offset.</param>
        /// <param name="z">Z-coordinate offset.</param>
        /// <returns>The created translation mat4x4.</returns>
        public static Matrix4x4 Translation(float x, float y, float z)
        {
            Translation(x, y, z, out Matrix4x4 result);
            return result;
        }
        /// <summary>
        /// Calculates the transpose of the specified mat4x4.
        /// </summary>
        /// <param name="value">The mat4x4 whose transpose is to be calculated.</param>
        /// <returns>The transpose of the specified mat4x4.</returns>
        public static Matrix4x4 Transpose(Matrix4x4 value)
        {
            Transpose(ref value, out Matrix4x4 result);
            return result;
        }

        /// <summary>
        /// Creates a mat4x4 that scales along the x-axis, y-axis, and y-axis.
        /// </summary>
        /// <param name="scale">Scaling factor for all three axes.</param>
        /// <param name="result">When the method completes, contains the created scaling mat4x4.</param>
        public static void Scaling(ref Vector3 scale, out Matrix4x4 result)
        {
            Scaling(scale.X, scale.Y, scale.Z, out result);
        }

        public void Scale(ref Vector3 scale, out Matrix4x4 result)
        {
            Scaling(scale.X, scale.Y, scale.Z, out result);
        }

        public void Scale(Vector3 scale)
        {
            //M11 *= scale.X - 1;
            //M22 *= scale.Y - 1;
            //M33 *= scale.Z - 1;
            //BUG
            M11 *= scale.X;
            M22 *= scale.Y;
            M33 *= scale.Z;
        }

        /// <summary>
        /// Creates a mat4x4 that scales along the x-axis, y-axis, and y-axis.
        /// </summary>
        /// <param name="scale">Scaling factor for all three axes.</param>
        /// <returns>The created scaling mat4x4.</returns>
        public static Matrix4x4 Scaling(Vector3 scale)
        {
            Scaling(ref scale, out Matrix4x4 result);
            return result;
        }

        /// <summary>
        /// Creates a mat4x4 that scales along the x-axis, y-axis, and y-axis.
        /// </summary>
        /// <param name="x">Scaling factor that is applied along the x-axis.</param>
        /// <param name="y">Scaling factor that is applied along the y-axis.</param>
        /// <param name="z">Scaling factor that is applied along the z-axis.</param>
        /// <param name="result">When the method completes, contains the created scaling mat4x4.</param>
        public static void Scaling(float x, float y, float z, out Matrix4x4 result)
        {
            result = Matrix4x4.Identity;
            result.M11 = x;
            result.M22 = y;
            result.M33 = z;
        }

        /// <summary>
        /// Creates a mat4x4 that scales along the x-axis, y-axis, and y-axis.
        /// </summary>
        /// <param name="x">Scaling factor that is applied along the x-axis.</param>
        /// <param name="y">Scaling factor that is applied along the y-axis.</param>
        /// <param name="z">Scaling factor that is applied along the z-axis.</param>
        /// <returns>The created scaling mat4x4.</returns>
        public static Matrix4x4 Scaling(float x, float y, float z)
        {
            Scaling(x, y, z, out Matrix4x4 result);
            return result;
        }

        /// <summary>
        /// Creates a mat4x4 that uniformly scales along all three axis.
        /// </summary>
        /// <param name="scale">The uniform scale that is applied along all axis.</param>
        /// <param name="result">When the method completes, contains the created scaling mat4x4.</param>
        public static void Scaling(float scale, out Matrix4x4 result)
        {
            result = Matrix4x4.Identity;
            result.M11 = result.M22 = result.M33 = scale;
        }

        /// <summary>
        /// Creates a mat4x4 that uniformly scales along all three axis.
        /// </summary>
        /// <param name="scale">The uniform scale that is applied along all axis.</param>
        /// <returns>The created scaling mat4x4.</returns>
        public static Matrix4x4 Scaling(float scale)
        {
            Scaling(scale, out Matrix4x4 result);
            return result;
        }

        /// <summary>
        /// Adds two matrices.
        /// </summary>
        /// <param name="left">The first Matrix4x4 to add.</param>
        /// <param name="right">The second Matrix4x4 to add.</param>
        /// <returns>The sum of the two matrices.</returns>
        public static Matrix4x4 operator +(Matrix4x4 left, Matrix4x4 right)
        {
            Add(ref left, ref right, out Matrix4x4 result);
            return result;
        }

        /// <summary>
        /// Determines the sum of two matrices.
        /// </summary>
        /// <param name="left">The first Matrix4x4 to add.</param>
        /// <param name="right">The second Matrix4x4 to add.</param>
        /// <param name="result">When the method completes, contains the sum of the two matrices.</param>
        public static void Add(ref Matrix4x4 left, ref Matrix4x4 right, out Matrix4x4 result)
        {
            result.M11 = left.M11 + right.M11;
            result.M12 = left.M12 + right.M12;
            result.M13 = left.M13 + right.M13;
            result.M14 = left.M14 + right.M14;
            result.M21 = left.M21 + right.M21;
            result.M22 = left.M22 + right.M22;
            result.M23 = left.M23 + right.M23;
            result.M24 = left.M24 + right.M24;
            result.M31 = left.M31 + right.M31;
            result.M32 = left.M32 + right.M32;
            result.M33 = left.M33 + right.M33;
            result.M34 = left.M34 + right.M34;
            result.M41 = left.M41 + right.M41;
            result.M42 = left.M42 + right.M42;
            result.M43 = left.M43 + right.M43;
            result.M44 = left.M44 + right.M44;
        }

        /// <summary>
        /// Decomposes a matrix into a scale, rotation, and translation.
        /// </summary>
        /// <param name="scale">When the method completes, contains the scaling component of the decomposed matrix.</param>
        /// <param name="rotation">When the method completes, contains the rotation component of the decomposed matrix.</param>
        /// <param name="translation">When the method completes, contains the translation component of the decomposed matrix.</param>
        /// <remarks>
        /// This method is designed to decompose an SRT transformation matrix only.
        /// </remarks>
        public bool Decompose(out Vector3 scale, out Vector3 rotation, out Vector3 translation)
        {
            //Source: Unknown
            //References: http://www.gamedev.net/community/forums/topic.asp?topic_id=441695

            //Get the translation.
            translation.X = M41;
            translation.Y = M42;
            translation.Z = M43;

            //Scaling is the length of the rows.
            scale.X = (float)System.Math.Sqrt((M11 * M11) + (M12 * M12) + (M13 * M13));
            scale.Y = (float)System.Math.Sqrt((M21 * M21) + (M22 * M22) + (M23 * M23));
            scale.Z = (float)System.Math.Sqrt((M31 * M31) + (M32 * M32) + (M33 * M33));

            //If any of the scaling factors are zero, than the rotation matrix can not exist.
            if (System.Math.Abs(scale.X) < 1e-6f ||
                System.Math.Abs(scale.Y) < 1e-6f ||
                System.Math.Abs(scale.Z) < 1e-6f)
            {
                rotation = new Vector3(0, 0, 0);
                return false;
            }

            //The rotation is the left over matrix after dividing out the scaling.
            Matrix4x4 rotationmatrix = new Matrix4x4
            {
                M11 = M11 / scale.X,
                M12 = M12 / scale.X,
                M13 = M13 / scale.X,

                M21 = M21 / scale.Y,
                M22 = M22 / scale.Y,
                M23 = M23 / scale.Y,

                M31 = M31 / scale.Z,
                M32 = M32 / scale.Z,
                M33 = M33 / scale.Z,

                M44 = 1f
            };


            rotation = new Vector3(System.MathF.Atan2(rotationmatrix.M32, rotationmatrix.M33), System.MathF.Atan2(-rotationmatrix.M31, (float)System.Math.Sqrt(System.Math.Pow(rotationmatrix.M32, 2f) + System.Math.Pow(rotationmatrix.M33, 2f))), System.MathF.Atan2(rotationmatrix.M21, rotationmatrix.M11));
            return true;
        }

        public Matrix4x4 RotationMatrix()
        {
            Vector3 scale = new Vector3
            {
                X = (float)System.Math.Sqrt((M11 * M11) + (M12 * M12) + (M13 * M13)),
                Y = (float)System.Math.Sqrt((M21 * M21) + (M22 * M22) + (M23 * M23)),
                Z = (float)System.Math.Sqrt((M31 * M31) + (M32 * M32) + (M33 * M33))
            };

            if (System.Math.Abs(scale.X) < 1e-6f ||
                System.Math.Abs(scale.Y) < 1e-6f ||
                System.Math.Abs(scale.Z) < 1e-6f)
            {
                return Matrix4x4.Identity;
            }

            Matrix4x4 rotationmatrix = new Matrix4x4
            {
                M11 = M11 / scale.X,
                M12 = M12 / scale.X,
                M13 = M13 / scale.X,

                M21 = M21 / scale.Y,
                M22 = M22 / scale.Y,
                M23 = M23 / scale.Y,

                M31 = M31 / scale.Z,
                M32 = M32 / scale.Z,
                M33 = M33 / scale.Z,

                M44 = 1f
            };
            return rotationmatrix;
        }

        /// <summary>
        /// Creates a quaternion given a rotation matrix.
        /// </summary>
        /// <param name="matrix">The rotation matrix.</param>
        /// <param name="result">When the method completes, contains the newly created quaternion.</param>
        public static void RotationMatrix(ref Matrix4x4 matrix, out Quaternion result)
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

        public Vector3 Quaternion_to_Vector3(Quaternion quaternion)
        {
            float x = quaternion.X;
            float y = quaternion.Y;
            float z = quaternion.Z;
            float w = quaternion.W;


            float roll = (float)System.Math.Atan2(2 * y * w - 2 * x * z, 1 - 2 * y * y - 2 * z * z);
            float pitch = (float)System.Math.Atan2(2 * x * w - 2 * y * z, 1 - 2 * x * x - 2 * z * z);
            float yaw = (float)System.Math.Asin(2 * x * y + 2 * z * w);
            return new Vector3(roll, pitch, yaw);
        }

        /// <summary>
        /// Creates a left-handed, orthographic projection matrix.
        /// </summary>
        /// <param name="width">Width of the viewing volume.</param>
        /// <param name="height">Height of the viewing volume.</param>
        /// <param name="znear">Minimum z-value of the viewing volume.</param>
        /// <param name="zfar">Maximum z-value of the viewing volume.</param>
        /// <param name="result">When the method completes, contains the created projection matrix.</param>
        public static void OrthoLH(float width, float height, float znear, float zfar, out Matrix4x4 result)
        {
            float halfWidth = width * 0.5f;
            float halfHeight = height * 0.5f;

            OrthoOffCenterLH(-halfWidth, halfWidth, -halfHeight, halfHeight, znear, zfar, out result);
        }
        /// <summary>
        /// Creates a left-handed, customized orthographic projection matrix.
        /// </summary>
        /// <param name="left">Minimum x-value of the viewing volume.</param>
        /// <param name="right">Maximum x-value of the viewing volume.</param>
        /// <param name="bottom">Minimum y-value of the viewing volume.</param>
        /// <param name="top">Maximum y-value of the viewing volume.</param>
        /// <param name="znear">Minimum z-value of the viewing volume.</param>
        /// <param name="zfar">Maximum z-value of the viewing volume.</param>
        /// <param name="result">When the method completes, contains the created projection matrix.</param>
        public static void OrthoOffCenterLH(float left, float right, float bottom, float top, float znear, float zfar, out Matrix4x4 result)
        {
            float zRange = 1.0f / (zfar - znear);

            result = Matrix4x4.Identity;
            result.M11 = 2.0f / (right - left);
            result.M22 = 2.0f / (top - bottom);
            result.M33 = zRange;
            result.M41 = (left + right) / (left - right);
            result.M42 = (top + bottom) / (bottom - top);
            result.M43 = -znear * zRange;
        }

        public static Matrix4x4 OrthoOffCenterLH(float left, float right, float bottom, float top, float znear, float zfar)
        {
            OrthoOffCenterLH(left, right, bottom, top, znear, zfar, out Matrix4x4 result);
            return result;
        }
        /// <summary>
        /// Creates a left-handed, orthographic projection matrix.
        /// </summary>
        /// <param name="width">Width of the viewing volume.</param>
        /// <param name="height">Height of the viewing volume.</param>
        /// <param name="znear">Minimum z-value of the viewing volume.</param>
        /// <param name="zfar">Maximum z-value of the viewing volume.</param>
        /// <returns>The created projection matrix.</returns>
        public static Matrix4x4 OrthoLH(float width, float height, float znear, float zfar)
        {
            OrthoLH(width, height, znear, zfar, out Matrix4x4 result);
            return result;
        }


        public static Vector3 operator *(Matrix4x4 matrix, Vector3 vector)
        {
            Vector3 res;
            res.X = matrix.M11 * vector.X + matrix.M12 * vector.Y + matrix.M13 * vector.Z + matrix.M14;
            res.Y = matrix.M21 * vector.X + matrix.M22 * vector.Y + matrix.M23 * vector.Z + matrix.M24;
            res.Z = matrix.M31 * vector.X + matrix.M32 * vector.Y + matrix.M33 * vector.Z + matrix.M34;

            return res;
        }


        public static Vector2 operator *(Matrix4x4 matrix, Vector2 vector)
        {
            Vector2 res;
            res.X = matrix.M11 * vector.X + matrix.M12 * vector.Y + matrix.M14;
            res.Y = matrix.M21 * vector.X + matrix.M22 * vector.Y + matrix.M24;

            return res;
        }

        public Matrix4x4 Transposed
        {
            get
            {
                var output = Transpose(this);
                return output;
            }
        }


        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            output.Append("\n");
            output.Append($"[{M11}]"); output.Append($"[{M12}]"); output.Append($"[{M13}]"); output.Append($"[{M14}]"); output.Append($"\n");
            output.Append($"[{M21}]"); output.Append($"[{M22}]"); output.Append($"[{M23}]"); output.Append($"[{M24}]"); output.Append($"\n");
            output.Append($"[{M31}]"); output.Append($"[{M32}]"); output.Append($"[{M33}]"); output.Append($"[{M34}]"); output.Append($"\n");
            output.Append($"[{M41}]"); output.Append($"[{M42}]"); output.Append($"[{M43}]"); output.Append($"[{M44}]"); output.Append($"\n");
            output.Append("\n");

            return output.ToString();
        }
    }
}
