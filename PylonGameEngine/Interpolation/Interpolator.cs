using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PylonGameEngine.Interpolation
{
    public class Interpolator
    {
        internal static List<Interpolator> Interpolators = new List<Interpolator>();

        public int LengthTicks { get; protected set; }
        public int TicksPassed { get; protected set; }
        public int LengthFrames { get; protected set; }
        public int FramesPassed { get; protected set; }

        public float XTick
        {
            get
            {
                return (float)TicksPassed / (float)LengthTicks;
            }
        }
        public float XFrame
        {
            get
            {
                return (float)FramesPassed / (float)LengthFrames;
            }
        }

        public bool Loop { get; protected set; }
        private bool TickDeactivated = false;
        private bool FrameDeactivated = false;

        public delegate void OnFrame(Interpolator interpolator);
        public event OnFrame Frame;

        public delegate void OnTick(Interpolator interpolator);
        public event OnFrame Tick;

        protected Interpolator(int lengthTicks, int lengthFrames, bool loop = false)
        {
            LengthTicks = lengthTicks;
            LengthFrames = lengthFrames;

            TicksPassed = 0;
            FramesPassed = 0;

            Loop = loop;

            Frame += (interpolator) => { };
            Tick += (interpolator) => { };

            Interpolators.Add(this);
        }

        internal void UpdateTick()
        {
            if(TickDeactivated && FrameDeactivated)
                Interpolators.Remove(this);

            if (TickDeactivated)
                return;

            if (TicksPassed >= LengthTicks)
            {
                if (Loop == true)
                    TicksPassed = 0;
                else
                {
                    TickDeactivated = true;
                    return;
                }
            }


            OnUpdateTick();

            Tick(this);
            TicksPassed++;
        }

        internal void UpdateFrame()
        {
            if (TickDeactivated && FrameDeactivated)
                Interpolators.Remove(this);

            if (FrameDeactivated)
                return;

            if (FramesPassed >= LengthFrames)
            {
                if (Loop == true)
                    FramesPassed = 0;
                else
                {
                    FrameDeactivated = true;
                    return;
                }
            }

            OnUpdateFrame();

            Frame(this);
            FramesPassed++;
        }

        protected virtual void OnUpdateTick()
        {

        }

        protected virtual void OnUpdateFrame()
        {

        }
    }
}
