using PylonGameEngine.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PylonGameEngine.Interpolation
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
