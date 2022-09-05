/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

using BepuPhysics;
using BepuUtilities.Memory;
using PylonSoftwareEngine.Mathematics;
using PylonSoftwareEngine.SceneManagement;
using PylonSoftwareEngine.Utilities;
using System;

namespace PylonSoftwareEngine.Physics
{
    public class MyPhysics
    {
        public Simulation Simulation { get; internal set; }
        public BufferPool BufferPool;
        internal BepuUtilities.ThreadDispatcher ThreadDispatcher { get; private set; }
        private object RigidLock = new object();
        private object StaticLock = new object();
        private object TriggerLock = new object();
        public LockedList<RigidBody> RigidBodies { get; internal set; }
        public LockedList<StaticBody> StaticBodies { get; internal set; }

        public LockedList<TriggerBody> TriggerBodies { get; internal set; }
        public bool Paused = false;
        public Vector3 Gravity = new Vector3(0f, -9.81f, 0f);

        internal Scene SceneContext;

        private bool Initialized = false;

        public DemoPoseIntegratorCallbacks demoPoseIntegratorCallbacks;
        public void Initialize()
        {
            if (Initialized)
                return;
            demoPoseIntegratorCallbacks = new DemoPoseIntegratorCallbacks();
            demoPoseIntegratorCallbacks.SceneContext = SceneContext;

            RigidBodies = new LockedList<RigidBody>(ref RigidLock);
            StaticBodies = new LockedList<StaticBody>(ref StaticLock);
            TriggerBodies = new LockedList<TriggerBody>(ref TriggerLock);
            BufferPool = new BufferPool();

            var ThreadCount = Environment.ProcessorCount > 4 ? Environment.ProcessorCount - 2 : Environment.ProcessorCount - 1;
            ThreadDispatcher = new BepuUtilities.ThreadDispatcher(ThreadCount);

            var demoNarrowPhaseCallbacks = new DemoNarrowPhaseCallbacks();
            demoNarrowPhaseCallbacks.SceneContext = SceneContext;
            Simulation = Simulation.Create(BufferPool, demoNarrowPhaseCallbacks, demoPoseIntegratorCallbacks, new SolveDescription(1, 4));
            Initialized = true;
        }

        public void Update(float TickRate)
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

        public void Update(SoftwareLoop SoftwareLoop)
        {
            Update(SoftwareLoop.Tickrate);
        }

        public void AddBody(RigidBody body)
        {
            RigidBodies.Add(body);
        }

        public void AddBody(StaticBody body)
        {
            StaticBodies.Add(body);
        }

        public void AddBody(TriggerBody body)
        {
            TriggerBodies.Add(body);
        }

        internal (CollisionType, PhysicsComponent) Found(int Handle)
        {
            var Rigid = RigidBodies.Find(x => x.Index == Handle);
            var Static = StaticBodies.Find(x => x.Index == Handle);
            var Trigger = TriggerBodies.Find(x => x.Index == Handle);

            if (Rigid != null)
            {
                return (CollisionType.Rigid, Rigid);
            }

            if (Static != null)
            {
                return (CollisionType.Static, Static);
            }

            if (Trigger != null)
            {
                return (CollisionType.Trigger, Trigger);
            }

            return (CollisionType.None, null);
        }
    }
}
