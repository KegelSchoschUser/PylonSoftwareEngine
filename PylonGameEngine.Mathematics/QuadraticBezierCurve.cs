using System;
using System.Collections.Generic;

namespace PylonGameEngine.Mathematics
{
    public class QuadraticBezierCurve : Curve
    {
        public override int MinPoints => 3;
        public override Vector3 GetValue(int index, float k)
        {
            Vector3 P0 = Points[index];
            Vector3 P1 = Points[index + 1];
            Vector3 P2 = Points[index + 2];
            Console.WriteLine(index  + "  " + k);
            return Mathf.Pow(1f - k, 2f) * P0 + 2f * (1 - k) * k * P1 + Mathf.Pow(k, 2f) * P2;              
        }
    }
}
