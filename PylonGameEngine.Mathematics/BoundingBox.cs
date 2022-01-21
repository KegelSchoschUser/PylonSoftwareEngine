using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PylonGameEngine.Mathematics
{
    public struct BoundingBox
    {
        public Vector3 Min;
        public Vector3 Max;

        public BoundingBox(Vector3 min, Vector3 max)
        {
            Min = min;
            Max = max;
        }

        public static BoundingBox FromPoints(Vector3[] Points)
        {
            return FromPoints(Points, Matrix4x4.Identity);
        }

        public static BoundingBox FromPoints(Vector3[] Points, Matrix4x4 Matrix)
        {
            Vector3 min;
            Vector3 max;
            if (Points.Length == 0)
            {
                min = Vector3.Zero;
                max = Vector3.Zero;
            }
            else
            {
                min = Matrix * Points[0];
                max = Matrix * Points[0];

                for (int i = 0; i < Points.Length; i++)
                {
                    Vector3 P = Matrix * Points[i];
                    min = Vector3.Min(P, min);
                    max = Vector3.Max(P, max);

                }
            }

            return new BoundingBox(min, max);
        }

        public static BoundingBox operator *(Matrix4x4 matrix, BoundingBox boundingBox)
        {
            return new BoundingBox(matrix * boundingBox.Min, matrix * boundingBox.Max);
        }
    }
}
