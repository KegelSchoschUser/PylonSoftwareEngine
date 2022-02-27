using BepuPhysics;
using BepuUtilities.Memory;
using PylonGameEngine.Mathematics;
using PylonGameEngine.Utilities;
using System;

namespace PylonGameEngine.Physics
{
    public static class MyPhysics
    {
        public static Simulation Simulation { get; internal set; }
        public static BufferPool BufferPool;
        internal static ThreadDispatcher ThreadDispatcher { get; private set; }
        private static object RigidLock = new object();
        private static object StaticLock = new object();
        private static object TriggerLock = new object();
        public static LockedList<RigidBody> RigidBodies { get; internal set; }
        public static LockedList<StaticBody> StaticBodies { get; internal set; }

        public static LockedList<TriggerBody> TriggerBodies { get; internal set; }
        public static bool Paused = false;
        public static Vector3 Gravity = new Vector3(0f, -9.81f, 0f);

        private static bool Initialized = false;
        public static void Initialize()
        {
            if (Initialized)
                return;

            RigidBodies = new LockedList<RigidBody>(ref RigidLock);
            StaticBodies = new LockedList<StaticBody>(ref StaticLock);
            TriggerBodies = new LockedList<TriggerBody>(ref TriggerLock);
            BufferPool = new BufferPool();

            var ThreadCount = Environment.ProcessorCount > 4 ? Environment.ProcessorCount - 2 : Environment.ProcessorCount - 1;
            ThreadDispatcher = new ThreadDispatcher(ThreadCount);

            Simulation = Simulation.Create(BufferPool, new DemoNarrowPhaseCallbacks(), new DemoPoseIntegratorCallbacks(), new SolveDescription(1, 4));
            Initialized = true;
        }

        public static void Update(float TickRate)
        {
            if (!Initialized)
                return;
            if (Paused)
                return;
            Simulation.Timestep(1f / TickRate, ThreadDispatcher);

            lock (RigidLock)
                foreach (RigidBody RigidBody in RigidBodies)
                {
                    RigidBody.Update();
                }
            lock (StaticLock)
                foreach (StaticBody StaticBody in StaticBodies)
                {
                    //if(StaticBody.Parent.Components.Count > 1)
                    //Console.WriteLine(StaticBody.Body.Pose.Position);
                    StaticBody.Update();
                }
        }

        public static void Update(GameLoop gameLoop)
        {
            Update(gameLoop.Tickrate);
        }
    }
}
