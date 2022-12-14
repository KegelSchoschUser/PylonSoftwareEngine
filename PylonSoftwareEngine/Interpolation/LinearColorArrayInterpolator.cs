/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

using PylonSoftwareEngine.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PylonSoftwareEngine.Interpolation
{
    public sealed class LinearColorArrayInterpolator : Interpolator
    {
        private List<RGBColor> _Values;
        public List<RGBColor> Values
        {
            get { return _Values; }
            set { lock (MySoftware.RenderLock) _Values = value; }
        }
        public RGBColor YFrame { get; private set; }
        public RGBColor YTick { get; private set; }


        public LinearColorArrayInterpolator(List<RGBColor> values, int lengthTicks, int lengthFrames, bool loop = false) : base(lengthTicks, lengthFrames, loop)
        {
            if (values.Count < 2)
                throw new ArgumentOutOfRangeException("values");

            Values = values;

            YFrame = values[0];
            YTick = values[0];
        }

        protected override void OnUpdateTick()
        {
            float k = (float)XTick * ((float)Values.Count - 1);
            float R = Mathf.LerpArray(Values.ToArray().Select(x => x.R).ToArray(), Mathf.Truncate(k), k - Mathf.Truncate(k));
            float G = Mathf.LerpArray(Values.ToArray().Select(x => x.G).ToArray(), Mathf.Truncate(k), k - Mathf.Truncate(k));
            float B = Mathf.LerpArray(Values.ToArray().Select(x => x.B).ToArray(), Mathf.Truncate(k), k - Mathf.Truncate(k));
            float A = Mathf.LerpArray(Values.ToArray().Select(x => x.A).ToArray(), Mathf.Truncate(k), k - Mathf.Truncate(k));

            YTick = new RGBColor(R, G, B, A);
        }

        protected override void OnUpdateFrame()
        {
            float k = (float)XFrame * ((float)Values.Count - 1);
            float R = Mathf.LerpArray(Values.ToArray().Select(x => x.R).ToArray(), Mathf.Truncate(k), k - Mathf.Truncate(k));
            float G = Mathf.LerpArray(Values.ToArray().Select(x => x.G).ToArray(), Mathf.Truncate(k), k - Mathf.Truncate(k));
            float B = Mathf.LerpArray(Values.ToArray().Select(x => x.B).ToArray(), Mathf.Truncate(k), k - Mathf.Truncate(k));
            float A = Mathf.LerpArray(Values.ToArray().Select(x => x.A).ToArray(), Mathf.Truncate(k), k - Mathf.Truncate(k));

            YFrame = new RGBColor(R, G, B, A);
        }
    }
}
