namespace PylonGameEngine.Interpolation
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
