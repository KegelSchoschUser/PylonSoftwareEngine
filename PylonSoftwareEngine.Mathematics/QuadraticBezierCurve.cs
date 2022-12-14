/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

namespace PylonSoftwareEngine.Mathematics
{
    public class QuadraticBezierCurve : Curve
    {
        public override int MinPoints => 3;
        public override Vector3 GetValue(int index, float k)
        {
            Vector3 P0 = Points[index];
            Vector3 P1 = Points[index + 1];
            Vector3 P2 = Points[index + 2];
            return Mathf.Pow(1f - k, 2f) * P0 + 2f * (1 - k) * k * P1 + Mathf.Pow(k, 2f) * P2;
        }
    }
}
