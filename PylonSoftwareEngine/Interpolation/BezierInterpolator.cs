/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

using PylonSoftwareEngine.Mathematics;

namespace PylonSoftwareEngine.Interpolation
{
    public sealed class BezierInterpolator : Interpolator
    {
        public QuadraticBezierCurve curve { get; private set; }
        public float YFrame { get; private set; }
        public float YTick { get; private set; }


        public BezierInterpolator(float first, float control, float third, int lengthTicks, int lengthFrames, bool loop = false) : base(lengthTicks, lengthFrames, loop)
        {
            curve = new QuadraticBezierCurve();
            curve.Points.Add(new Vector3(0.0f, first));
            curve.Points.Add(new Vector3(0.5f, control));
            curve.Points.Add(new Vector3(1.0f, third));

            YFrame = first;
            YTick = first;
        }

        protected override void OnUpdateTick()
        {
            YTick = curve.GetValue(0, XTick).Y;
        }

        protected override void OnUpdateFrame()
        {
            YFrame = curve.GetValue(0, XFrame).Y;
        }
    }
}
