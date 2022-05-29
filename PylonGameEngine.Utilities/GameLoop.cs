using System;
using System.Collections.Generic;
using System.Threading;

namespace PylonGameEngine.Utilities
{
    public class GameLoop
    {
        public float Tickrate = 60;
        public string Name = "UNKNOWN";
        public int Frames { get; private set; }
        public float MillisecondsPerTick => 1000f / Tickrate;
        public bool Running { get; private set; }
        public bool Paused;
        public float DeltaTime { get; private set; }
        public float FPS => 1f / DeltaTime;

        public delegate void OnTick();
        public event OnTick Tick;

        public delegate void OnTickInfo(GameLoop sender);
        public event OnTickInfo TickInfo;

        public delegate void OnStart();
        public event OnStart Starting;

        public object LockObject;

        public GameLoop(float tickRate, string name = "UNKNOWN", object lockobject = null)
        {
            Frames = 0;
            Tickrate = tickRate;
            Name = name;
            LockObject = lockobject;
            Tick += GameLoop_Tick;
            TickInfo += GameLoop_TickInfo;
            Starting += GameLoop_Start;
        }

        private void GameLoop_Tick()
        {
            Frames++;
        }

        private void GameLoop_TickInfo(GameLoop sender)
        {
        }

        private void GameLoop_Start(/*GameLoop sender*/)
        {

        }

        private float previousStart;
        private Queue<Action> Invokes = new Queue<Action>();
        public void Start(bool NewThread = true)
        {
            Starting();
            Running = true;
            if (NewThread)
            {
                Thread t = new Thread(() =>
                {
                    previousStart = Environment.TickCount64;
                    float _nextLoop = Environment.TickCount64;
                    
                    while (true)
                    {
                        while (_nextLoop < Environment.TickCount64)
                        {

                            float now = Environment.TickCount64;
                            DeltaTime = (now - previousStart) / 1000f;

                            if (!Paused)
                            {
                                Tick();
                                TickInfo(this);
                                for (int i = 0; i < Invokes.Count; i++)
                                {
                                    Invokes.Dequeue().Invoke();
                                }
                            }

                            _nextLoop += MillisecondsPerTick;

                            //if (_nextLoop > Environment.TickCount64 && Tickrate != -1.0f)
                            //{
                                while((_nextLoop - Environment.TickCount64) > 0)
                                { }
                                //if ((_nextLoop - Environment.TickCount64).Milliseconds >= 0f)
                                //{
                                //    Thread.Sleep((_nextLoop - Environment.TickCount64).TotalMilliseconds);
                                //}
                            //}
                            previousStart = now;
                            if (!Running)
                            {
                                return;
                            }
                        }
                    }

                });
                t.Start();
            }
            else
            {
                previousStart = Environment.TickCount64;
                float _nextLoop = Environment.TickCount64;

                while (true)
                {
                    while (_nextLoop < Environment.TickCount64)
                    {
                        float now = Environment.TickCount64;
                        DeltaTime = (now - previousStart) / 1000f;
                        if (!Paused)
                        {

                            Tick();
                            TickInfo(this);
                            for (int i = 0; i < Invokes.Count; i++)
                            {
                                Invokes.Dequeue().Invoke();
                            }
                        }

                        if (Tickrate != -1.0f)
                        {
                            _nextLoop += MillisecondsPerTick;
                        }

                        //if (_nextLoop > Environment.TickCount64)
                        //{
                            while ((_nextLoop - Environment.TickCount64) > 0)
                            { }
                            //if ((_nextLoop - Environment.TickCount64).Milliseconds >= 0f)
                            //{
                            //    Thread.Sleep((_nextLoop - Environment.TickCount64).Milliseconds);
                            //}
                        //}
                        previousStart = now;
                        if (!Running)
                        {
                            return;
                        }
                    }
                }
            }
        }

        public void Invoke(Action action)
        {
            Invokes.Enqueue(action);
        }

        public void Stop()
        {
            Running = false;
        }

        public void Pause()
        {
            Paused = true;
        }

        public void Resume()
        {
            Paused = false;
        }
    }
}
