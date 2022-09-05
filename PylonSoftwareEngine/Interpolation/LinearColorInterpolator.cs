using PylonSoftwareEngine.Mathematics;

namespace PylonSoftwareEngine.Interpolation
{
    public sealed class LinearColorInterpolator : Interpolator
    {
        public RGBColor First { get; private set; }
        public RGBColor Second { get; private set; }
        public RGBColor YFrame { get; private set; }
        public RGBColor YTick { get; private set; }


        public LinearColorInterpolator(RGBColor first, RGBColor second, int lengthTicks, int lengthFrames, bool loop = false) : base(lengthTicks, lengthFrames, loop)
        {
            First = first;
            Second = second;

            YFrame = first;
            YTick = first;
        }

        protected override void OnUpdateTick()
        {
            YTick = new RGBColor(
                Mathf.Lerp(First.R, Second.R, XTick),
                Mathf.Lerp(First.G, Second.G, XTick),
                Mathf.Lerp(First.B, Second.B, XTick),
                Mathf.Lerp(First.A, Second.A, XTick));
            
        }

        protected override void OnUpdateFrame()
        {
            YFrame = new RGBColor(
                Mathf.Lerp(First.R, Second.R, XFrame),
                Mathf.Lerp(First.G, Second.G, XFrame),
                Mathf.Lerp(First.B, Second.B, XFrame),
                Mathf.Lerp(First.A, Second.A, XFrame));
        }
    }
}
