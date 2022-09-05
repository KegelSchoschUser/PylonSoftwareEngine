using PylonSoftwareEngine.Mathematics;
using System;
using System.Collections.Generic;

namespace PylonSoftwareEngine.Interpolation
{
    public sealed class LinearArrayInterpolator : Interpolator
    {
        private List<float> _Values;
        public List<float> Values
        {
            get { return _Values; }
            set { lock (MySoftware.RenderLock) _Values = value; }
        }
        public float YFrame { get; private set; }
        public float YTick { get; private set; }


        public LinearArrayInterpolator(List<float> values, int lengthTicks, int lengthFrames, bool loop = false) : base(lengthTicks, lengthFrames, loop)
        {
            if (values.Count < 2)
                throw new ArgumentOutOfRangeException("values");

            Values = values;

            YFrame = values[0];
            YTick = values[0];
        }

        protected override void OnUpdateTick()
        {
            float k = (float)XTick * ((float)Values.Count-1);
            YTick = Mathf.LerpArray(Values.ToArray(), Mathf.Truncate(k), k - Mathf.Truncate(k));
        }

        protected override void OnUpdateFrame()
        {
            float k = (float)XFrame * ((float)Values.Count - 1);
            YFrame = Mathf.LerpArray(Values.ToArray(), Mathf.Truncate(k), k - Mathf.Truncate(k));
        }
    }
}
