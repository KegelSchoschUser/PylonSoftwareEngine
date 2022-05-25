using PylonGameEngine.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PylonGameEngine.Interpolation
{
    public sealed class LinearColorArrayInterpolator : Interpolator
    {
        private List<RGBColor> _Values;
        public List<RGBColor> Values
        {
            get { return _Values; }
            set { lock (MyGame.RenderLock) _Values = value; }
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
            float k = (float)XTick * (float)Values.Count;
            float R = Mathf.LerpArray(Values.ToArray().Select(x => x.R).ToArray(), Mathf.Truncate(k), k - Mathf.Truncate(k));
            float G = Mathf.LerpArray(Values.ToArray().Select(x => x.G).ToArray(), Mathf.Truncate(k), k - Mathf.Truncate(k));
            float B = Mathf.LerpArray(Values.ToArray().Select(x => x.B).ToArray(), Mathf.Truncate(k), k - Mathf.Truncate(k));
            float A = Mathf.LerpArray(Values.ToArray().Select(x => x.A).ToArray(), Mathf.Truncate(k), k - Mathf.Truncate(k));

            YTick = new RGBColor(R, G, B, A);
        }

        protected override void OnUpdateFrame()
        {
            float k = (float)XFrame * (float)Values.Count;
            float R = Mathf.LerpArray(Values.ToArray().Select(x => x.R).ToArray(), Mathf.Truncate(k), k - Mathf.Truncate(k));
            float G = Mathf.LerpArray(Values.ToArray().Select(x => x.G).ToArray(), Mathf.Truncate(k), k - Mathf.Truncate(k));
            float B = Mathf.LerpArray(Values.ToArray().Select(x => x.B).ToArray(), Mathf.Truncate(k), k - Mathf.Truncate(k));
            float A = Mathf.LerpArray(Values.ToArray().Select(x => x.A).ToArray(), Mathf.Truncate(k), k - Mathf.Truncate(k));

            YFrame = new RGBColor(R, G, B, A);
        }
    }
}
