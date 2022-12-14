/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

using PylonSoftwareEngine.Mathematics;
using System;

namespace PylonSoftwareEngine.Interpolation
{
    public sealed class BezierAdvancedInterpolator : Interpolator
    {
        public QuadraticBezierCurve curve { get; private set; }
        public float YFrame { get; private set; }
        public float YTick { get; private set; }


        public BezierAdvancedInterpolator(Vector2[] values, int lengthTicks, int lengthFrames, bool loop = false) : base(lengthTicks, lengthFrames, loop)
        {
            if (values.Length < 2)
                throw new ArgumentOutOfRangeException("values");

            curve = new QuadraticBezierCurve();
            for (int i = 0; i < values.Length; i++)
            {
                curve.Points.Add(new Vector3(values[i]));
            }

            YFrame = values[0].Y;
            YTick = values[0].Y;
        }

        protected override void OnUpdateTick()
        {
            throw new NotImplementedException();

            float k = XTick * curve.Points.Count;
            YTick = curve.GetValue(k).Y;
        }

        protected override void OnUpdateFrame()
        {
            throw new NotImplementedException();
            float k = XFrame * curve.Points.Count;
            YFrame = curve.GetValue(k).Y;
        }
    }
}
