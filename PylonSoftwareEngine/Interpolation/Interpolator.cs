/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

using System;
using System.Collections.Generic;

namespace PylonSoftwareEngine.Interpolation
{
    public class Interpolator
    {
        internal static List<Interpolator> Interpolators = new List<Interpolator>();
        public List<object> AdditionalObjects = new List<object>();

        private int _LengthTicks;
        public int LengthTicks
        {
            get { return _LengthTicks; }
            set
            {
                lock (MySoftware.RenderLock)
                    _LengthTicks = Math.Clamp(value, 1, int.MaxValue);
            }
        }
        public int TicksPassed { get; protected set; }

        private int _LengthFrames;
        public int LengthFrames
        {
            get { return _LengthFrames; }
            set
            {
                lock (MySoftware.RenderLock)
                    _LengthFrames = Math.Clamp(value, 1, int.MaxValue);
            }
        }
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

        public bool Ended
        {
            get
            {
                return TickDeactivated && FrameDeactivated;
            }
        }

        public delegate void OnFrame(Interpolator interpolator);
        public event OnFrame Frame;

        public delegate void OnTick(Interpolator interpolator);
        public event OnFrame Tick;

        public delegate void OnEnd(Interpolator interpolator);
        public event OnEnd End;

        public delegate void OnLoopTick(Interpolator interpolator);
        public event OnLoopTick LoopedTick;

        public delegate void OnLoopFrame(Interpolator interpolator);
        public event OnLoopFrame LoopedFrame;

        protected Interpolator(int lengthTicks, int lengthFrames, bool loop = false)
        {
            LengthTicks = lengthTicks;
            LengthFrames = lengthFrames;

            TicksPassed = 0;
            FramesPassed = 0;

            Loop = loop;

            Frame += (interpolator) => { };
            Tick += (interpolator) => { };
            End += (interpolator) => { };
            LoopedTick += (interpolator) => { };
            LoopedFrame += (interpolator) => { };

            Interpolators.Add(this);
        }

        internal void UpdateTick()
        {
            if (TickDeactivated && FrameDeactivated)
            {
                Interpolators.Remove(this);
                End(this);
            }

            if (TickDeactivated)
                return;

            if (TicksPassed >= LengthTicks)
            {
                if (Loop == true)
                {
                    TicksPassed = 0;
                    LoopedTick(this);
                }
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
            {
                Interpolators.Remove(this);
                End(this);
            }

            if (FrameDeactivated)
                return;

            if (FramesPassed >= LengthFrames)
            {
                if (Loop == true)
                {
                    FramesPassed = 0;
                    LoopedFrame(this);
                }
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
