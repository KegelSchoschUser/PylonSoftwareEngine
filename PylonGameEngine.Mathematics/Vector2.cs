using System;
using System.Drawing;

namespace PylonGameEngine.Mathematics
{
    [Serializable]
    public struct Vector2
    {
        public float X;
        public float Y;

        public static readonly Vector2 Zero = new Vector2(0, 0);
        public static readonly Vector2 One = new Vector2(1, 1);

        //public float w;


        public Vector2(float k)
        {
            X = k;
            Y = k;
        }

        public Vector2(Size s)
        {
            X = s.Width;
            Y = s.Height;
        }

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Vector2(double x, double y)
        {
            X = (float)x;
            Y = (float)y;
        }

        public Vector2(Vector3 v)
        {
            X = v.X;
            Y = v.Y;
        }

        public static readonly Vector2 Left = new Vector2(-1, 0);
        public static readonly Vector2 Right = new Vector2(1, 0);

        public static readonly Vector2 Up = new Vector2(0, 1);
        public static readonly Vector2 Down = new Vector2(0, -1);


        public static float Distance(Vector2 value1, Vector2 value2)
        {
            float num1 = value1.X - value2.X;
            float num2 = value1.Y - value2.Y;
            return (float)System.Math.Sqrt(num1 * num1 + num2 * num2);
        }


        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Vector2 operator +(Vector2 v1, float v2)
        {
            return new Vector2(v1.X + v2, v1.Y + v2);
        }

        public static Vector2 operator -(Vector2 v1, float v2)
        {
            return new Vector2(v1.X - v2, v1.Y - v2);
        }

        public static Vector2 operator -(Vector2 v1)
        {
            return new Vector2(-v1.X, -v1.Y);
        }

        public static Vector2 operator *(Vector2 v1, float k)
        {
            return new Vector2(v1.X * k, v1.Y * k);
        }

        public static Vector2 operator /(Vector2 v1, float k)
        {
            return new Vector2(v1.X / k, v1.Y / k);
        }

        public static Vector2 operator *(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X * v2.X, v1.Y * v2.Y);
        }

        public static Vector2 operator /(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X / v2.X, v1.Y / v2.Y);
        }

        public static bool operator >=(Vector2 v1, Vector2 v2)
        {
            return (v1.X >= v2.X && v1.Y >= v2.Y);
        }

        public static explicit operator Vector2(Vector3 v)
        {
            return new Vector2(v.X, v.Y);
        }

        public static bool operator <=(Vector2 v1, Vector2 v2)
        {
            return (v1.X <= v2.X && v1.Y <= v2.Y);
        }

        public static bool operator ==(Vector2 v1, float f)
        {
            return v1.X == f && v1.Y == f;
        }

        public static bool operator !=(Vector2 v1, float f)
        {
            return v1.X != f && v1.Y != f;
        }

        public static bool operator >(Vector2 v1, Vector2 v2)
        {
            return (v1.X > v2.X && v1.Y > v2.Y);
        }

        public static bool operator <(Vector2 v1, Vector2 v2)
        {
            return (v1.X < v2.X && v1.Y < v2.Y);
        }

        public static bool operator >=(Vector2 v1, float v2)
        {
            return (v1.X >= v2 && v1.Y >= v2);
        }

        public static bool operator <=(Vector2 v1, float v2)
        {
            return (v1.X <= v2 && v1.Y <= v2);
        }

        public static bool operator >(Vector2 v1, float v2)
        {
            return (v1.X > v2 && v1.Y > v2);
        }

        public static bool operator <(Vector2 v1, float v2)
        {
            return (v1.X < v2 && v1.Y < v2);
        }

        public static bool operator ==(Vector2 v1, Vector2 v2)
        {
            return v1.X == v2.X && v1.Y == v2.Y;
        }
        public static bool operator !=(Vector2 v1, Vector2 v2)
        {
            return (v1 == v2) == false;
        }

        public static float Vector_DotProduct(Vector2 v1, Vector2 v2)
        {
            return (v1.X * v2.X + v1.Y * v2.Y);
        }

        public static float Vector_Length(Vector2 v)
        {
            return ((float)System.Math.Sqrt(Vector_DotProduct(v, v)));
        }

        public static Vector2 Vector_Normalise(Vector2 v)
        {
            float l = Vector_Length(v);
            return new Vector2(v.X / l, v.Y / l);
        }

        public static Vector2 Vector_Positive(Vector2 v)
        {
            return new Vector2(+v.X, +v.Y);
        }

        public static Vector2 Vector_Negative(Vector2 v)
        {

            return new Vector2(-v.X, -v.Y);
        }

        public Point ToPoint()
        {
            return new Point((int)X, (int)Y);
        }

        public PointF ToPointF()
        {
            return new PointF(X, Y);
        }

        public Vortice.Mathematics.Point ToVorticePoint()
        {
            return new Vortice.Mathematics.Point(X, Y);
        }

        public System.Numerics.Vector2 ToSystemNumerics()
        {
            return new System.Numerics.Vector2(X, Y);
        }

        public static implicit operator System.Numerics.Vector2(Vector2 v) => new System.Numerics.Vector2(v.X, v.Y);
        public static explicit operator Vector2(System.Numerics.Vector2 v) => new Vector2(v.X,v.Y);

        public static implicit operator Vortice.Mathematics.Point(Vector2 v) => new Vortice.Mathematics.Point(v.X, v.Y);
        public static explicit operator Vector2(Vortice.Mathematics.Point v) => new Vector2(v.X, v.Y);


        public override string ToString()
        {
            return "{" + $"{X.ToString("0.000")}, {Y.ToString("0.000")}" + "}";
        }

        public static Vector2 Abs(Vector2 v)
        {
            return new Vector2(System.Math.Abs(v.X), System.Math.Abs(v.Y));
        }
    }
}
