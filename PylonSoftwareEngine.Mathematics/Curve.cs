/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

using System;
using System.Collections.Generic;

namespace PylonSoftwareEngine.Mathematics
{
    public class Curve
    {
        public List<Vector3> Points = new List<Vector3>();
        public virtual int MinPoints
        {
            get { return 2; }
        }

        public Vector3 GetValue(float k)
        {
            if (Points.Count < MinPoints || k < 0f)
                throw new Exception();
            return GetValue(Mathf.Truncate(k), k - Mathf.Truncate(k));
        }
        public virtual Vector3 GetValue(int index, float k)
        {
            if (Points.Count < MinPoints || k < 0f || k > 1f)
                throw new Exception();


            return Vector3.Lerp(Points[index], Points[index + 1], k);
        }
    }
}
