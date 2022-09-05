/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

using PylonSoftwareEngine.Mathematics;

namespace PylonSoftwareEngine.Interpolation
{
    public sealed class LinearInterpolator : Interpolator
    {
        public float First { get; private set; }
        public float Second { get; private set; }
        public float YFrame { get; private set; }
        public float YTick { get; private set; }


        public LinearInterpolator(float first, float second, int lengthTicks, int lengthFrames, bool loop = false) : base(lengthTicks, lengthFrames, loop)
        {
            First = first;
            Second = second;

            YFrame = first;
            YTick = first;
        }

        protected override void OnUpdateTick()
        {
            YTick = Mathf.Lerp(First, Second, XTick);
        }

        protected override void OnUpdateFrame()
        {
            YFrame = Mathf.Lerp(First, Second, XFrame);
        }
    }
}
