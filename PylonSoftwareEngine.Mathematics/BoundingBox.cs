/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

namespace PylonSoftwareEngine.Mathematics
{
    public struct BoundingBox
    {
        public Vector3 Min;
        public Vector3 Max;

        public Vector3 Size => Max - Min;

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

        public bool PointInBox(Vector3 Point)
        {
            if (Min <= Point && Point <= Max)
                return true;

            return false;
        }

        public static BoundingBox operator *(Matrix4x4 matrix, BoundingBox boundingBox)
        {
            return new BoundingBox(matrix * boundingBox.Min, matrix * boundingBox.Max);
        }
    }
}
