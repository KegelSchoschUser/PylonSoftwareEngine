﻿using PylonGameEngine.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PylonGameEngine.Interpolation
{
    public sealed class LinearArrayAdvancedInterpolator : Interpolator
    {
        private List<Vector2> _Values;
        public List<Vector2> Values
        {
            get { return _Values; }
            set {lock(MyGame.RenderLock) _Values = value; }
        }
        public float YFrame { get; private set; }
        public float YTick { get; private set; }


        public LinearArrayAdvancedInterpolator(List<Vector2> values, int lengthTicks, int lengthFrames, bool loop = false) : base(lengthTicks, lengthFrames, loop)
        {
            if (values.Count < 2)
                throw new ArgumentOutOfRangeException("values");

            Values = values;

            YFrame = values[0].Y;
            YTick = values[0].Y;
        }

        protected override void OnUpdateTick()
        {
            Values.Sort((a, b) => a.X < b.X ? -1 : 1);

            int index = 0;
            for (int i = 0; i < Values.Count; i++)
            {
                var current = Values[i];
                if (XTick - current.X < 0)
                {
                    index = i - 1;
                    break;
                }
            }

            float firstX = Values[index].X;
            float firstY = Values[index].Y;
            float secondY;
            float secondX;



            if (index == Values.Count)
            {
                secondX = Values[index].X;
                secondY = Values[index].Y;
            }
            else
            {
                secondX = Values[index + 1].X;
                secondY = Values[index + 1].Y;
            }

            float by = Mathf.Abs(1 / ((firstX - secondX) / (XTick - firstX)));
            YTick = Mathf.Lerp(firstY, secondY, by);
        }

        protected override void OnUpdateFrame()
        {
            Values.Sort((a, b) => a.X < b.X ? -1 : 1);

            int index = 0;
            for (int i = 0; i < Values.Count; i++)
            {
                var current = Values[i];
                if(XFrame - current.X < 0)
                {
                    index = i - 1;
                    break;
                }
            }

            float firstX = Values[index].X;
            float firstY = Values[index].Y;
            float secondY;
            float secondX;



            if (index == Values.Count)
            {
                 secondX = Values[index].X;
                 secondY = Values[index].Y;
            }
            else
            {
                secondX = Values[index + 1].X;
                secondY = Values[index + 1].Y;
            }

            float by = Mathf.Abs( 1 / ((firstX-secondX) / (XFrame - firstX)) );
            YFrame = Mathf.Lerp(firstY, secondY, by);
        }
    }
}
