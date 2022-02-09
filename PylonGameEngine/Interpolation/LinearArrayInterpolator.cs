using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PylonGameEngine.Interpolation
{
    public sealed class LinearArrayInterpolator : Interpolator
    {
        public float[] Values { get; private set; }
        public float YFrame { get; private set; }
        public float YTick { get; private set; }


        public LinearArrayInterpolator(float[] values, int lengthTicks, int lengthFrames, bool loop = false) : base(lengthTicks, lengthFrames, loop)
        {
            if (values.Length < 2)
                throw new ArgumentOutOfRangeException("values");

            Values = values;

            YFrame = values[0];
            YTick = values[0];
        }

        protected override void OnUpdateTick()
        {
            
            YTick = Mathf.LerpArray(Values, Mathf.trun, XTick);
        }

        protected override void OnUpdateFrame()
        {
            YFrame = Mathf.LerpArray(Values, , XFrame);
        }
    }
}
