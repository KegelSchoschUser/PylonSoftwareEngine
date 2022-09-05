using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.CollisionDetection;
using BepuPhysics.Constraints;
using BepuUtilities;
using PylonSoftwareEngine.SceneManagement;
using PylonSoftwareEngine.Utilities;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace PylonSoftwareEngine.Physics
{
    public struct DemoPoseIntegratorCallbacks : IPoseIntegratorCallbacks
    {
        internal Scene SceneContext;
        /// <summary>
        /// Fraction of dynamic body linear velocity to remove per unit of time. Values range from 0 to 1. 0 is fully undamped, while values very close to 1 will remove most velocity.
        /// </summary>
        public float LinearDamping;
        /// <summary>
        /// Fraction of dynamic body angular velocity to remove per unit of time. Values range from 0 to 1. 0 is fully undamped, while values very close to 1 will remove most velocity.
        /// </summary>
        public float AngularDamping;


        /// <summary>
        /// Gets how the pose integrator should handle angular velocity integration.
        /// </summary>
        public readonly AngularIntegrationMode AngularIntegrationMode => AngularIntegrationMode.Nonconserving;

        /// <summary>
        /// Gets whether the integrator should use substepping for unconstrained bodies when using a substepping solver.
        /// If true, unconstrained bodies will be integrated with the same number of substeps as the constrained bodies in the solver.
        /// If false, unconstrained bodies use a single step of length equal to the dt provided to Simulation.Timestep. 
        /// </summary>
        public readonly bool AllowSubstepsForUnconstrainedBodies => false;

        /// <summary>
        /// Gets whether the velocity integration callback should be called for kinematic bodies.
        /// If true, IntegrateVelocity will be called for bundles including kinematic bodies.
        /// If false, kinematic bodies will just continue using whatever velocity they have set.
        /// Most use cases should set this to false.
        /// </summary>
        public readonly bool IntegrateVelocityForKinematics => false;

        public void Initialize(Simulation simulation)
        {
            //In this demo, we don't need to initialize anything.
            //If you had a simulation with per body gravity stored in a CollidableProperty<T> or something similar, having the simulation provided in a callback can be helpful.
        }

        /// <summary>
        /// Creates a new set of simple callbacks for the demos.
        /// </summary>
        /// <param name="gravity">Gravity to apply to dynamic bodies in the simulation.</param>
        /// <param name="linearDamping">Fraction of dynamic body linear velocity to remove per unit of time. Values range from 0 to 1. 0 is fully undamped, while values very close to 1 will remove most velocity.</param>
        /// <param name="angularDamping">Fraction of dynamic body angular velocity to remove per unit of time. Values range from 0 to 1. 0 is fully undamped, while values very close to 1 will remove most velocity.</param>
        public DemoPoseIntegratorCallbacks(float linearDamping = .03f, float angularDamping = .03f) : this()
        {
            LinearDamping = linearDamping;
            AngularDamping = angularDamping;
        }

        Vector3Wide gravityWideDt;
        Vector<float> linearDampingDt;
        Vector<float> angularDampingDt;

        /// <summary>
        /// Callback invoked ahead of dispatches that may call into <see cref="IntegrateVelocity"/>.
        /// It may be called more than once with different values over a frame. For example, when performing bounding box prediction, velocity is integrated with a full frame time step duration.
        /// During substepped solves, integration is split into substepCount steps, each with fullFrameDuration / substepCount duration.
        /// The final integration pass for unconstrained bodies may be either fullFrameDuration or fullFrameDuration / substepCount, depending on the value of AllowSubstepsForUnconstrainedBodies. 
        /// </summary>
        /// <param name="dt">Current integration time step duration.</param>
        /// <remarks>This is typically used for precomputing anything expensive that will be used across velocity integration.</remarks>
        public void PrepareForIntegration(float dt)
        {
            //No reason to recalculate gravity * dt for every body; just cache it ahead of time.
            //Since these callbacks don't use per-body damping values, we can precalculate everything.
            linearDampingDt = new Vector<float>(MathF.Pow(MathHelper.Clamp(1 - LinearDamping, 0, 1), dt));
            angularDampingDt = new Vector<float>(MathF.Pow(MathHelper.Clamp(1 - AngularDamping, 0, 1), dt));
            gravityWideDt = Vector3Wide.Broadcast(SceneContext.Physics.Gravity.ToSystemNumerics() * dt);
        }

        /// <summary>
        /// Callback for a bundle of bodies being integrated.
        /// </summary>
        /// <param name="bodyIndices">Indices of the bodies being integrated in this bundle.</param>
        /// <param name="position">Current body positions.</param>
        /// <param name="orientation">Current body orientations.</param>
        /// <param name="localInertia">Body's current local inertia.</param>
        /// <param name="integrationMask">Mask indicating which lanes are active in the bundle. Active lanes will contain 0xFFFFFFFF, inactive lanes will contain 0.</param>
        /// <param name="workerIndex">Index of the worker thread processing this bundle.</param>
        /// <param name="dt">Durations to integrate the velocity over. Can vary over lanes.</param>
        /// <param name="velocity">Velocity of bodies in the bundle. Any changes to lanes which are not active by the integrationMask will be discarded.</param>
        public void IntegrateVelocity(Vector<int> bodyIndices, Vector3Wide position, QuaternionWide orientation, BodyInertiaWide localInertia, Vector<int> integrationMask, int workerIndex, Vector<float> dt, ref BodyVelocityWide velocity)
        {
            //This is a handy spot to implement things like position dependent gravity or per-body damping.
            //This implementation uses a single damping value for all bodies that allows it to be precomputed.
            //We don't have to check for kinematics; IntegrateVelocityForKinematics returns false, so we'll never see them in this callback.
            //Note that these are SIMD operations and "Wide" types. There are Vector<float>.Count lanes of execution being evaluated simultaneously.
            //The types are laid out in array-of-structures-of-arrays (AOSOA) format. That's because this function is frequently called from vectorized contexts within the solver.
            //Transforming to "array of structures" (AOS) format for the callback and then back to AOSOA would involve a lot of overhead, so instead the callback works on the AOSOA representation directly.

            var RigidBody = SceneContext.Physics.RigidBodies.Find(x => x.Index == bodyIndices[0]);
            if (RigidBody != null)
            {
                if (RigidBody.UseGravity && RigidBody.UsePhysics)
                {
                    velocity.Linear = (velocity.Linear + gravityWideDt) * linearDampingDt;
                    velocity.Angular = velocity.Angular * angularDampingDt;
                }
                else
                {
                    if (RigidBody.UsePhysics)
                    {
                        velocity.Linear = (velocity.Linear) * linearDampingDt;
                        velocity.Angular = velocity.Angular * angularDampingDt;
                    }
                    else
                    {
                        velocity.Linear = new Vector3Wide();
                        velocity.Angular = new Vector3Wide();
                    }
                }
            }
            else
            {
                MyLog.Default.Write("Couldn't find RigidBody", LogSeverity.Error);
            }

        }
    }
    public unsafe struct DemoNarrowPhaseCallbacks : INarrowPhaseCallbacks
    {
        internal Scene SceneContext;
        public SpringSettings ContactSpringiness;

        public void Initialize(Simulation simulation)
        {
            //Use a default if the springiness value wasn't initialized.
            if (ContactSpringiness.AngularFrequency == 0 && ContactSpringiness.TwiceDampingRatio == 0)
                ContactSpringiness = new SpringSettings(30, 1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool AllowContactGeneration(int workerIndex, CollidableReference a, CollidableReference b, ref float speculativeMargin)
        {
            var A_ValuePair = SceneContext.Physics.Found(a.BodyHandle.Value);
            var B_ValuePair = SceneContext.Physics.Found(b.BodyHandle.Value);

            bool AllowCollision = CollisionCheck.CanCollide(A_ValuePair.Item1, B_ValuePair.Item1, A_ValuePair.Item2.UseCollisions, B_ValuePair.Item2.UseCollisions);

            if (CollisionCheck.ObjectCollidedTrigger(A_ValuePair.Item1, B_ValuePair.Item1))
            {
                if (A_ValuePair.Item1 == CollisionType.Trigger)
                    (A_ValuePair.Item2 as TriggerBody).InvokeEvent(B_ValuePair.Item2);

                if (B_ValuePair.Item1 == CollisionType.Trigger)
                    (B_ValuePair.Item2 as TriggerBody).InvokeEvent(A_ValuePair.Item2);
            }

            return AllowCollision;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool AllowContactGeneration(int workerIndex, CollidablePair pair, int childIndexA, int childIndexB)
        {
            float f = 1.0f;
            return AllowContactGeneration(workerIndex, pair.A, pair.B, ref f);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe bool ConfigureContactManifold<TManifold>(int workerIndex, CollidablePair pair, ref TManifold manifold, out PairMaterialProperties pairMaterial) where TManifold : unmanaged, IContactManifold<TManifold>
        {
            var A_ValuePair = SceneContext.Physics.Found(pair.A.BodyHandle.Value);
            var B_ValuePair = SceneContext.Physics.Found(pair.B.BodyHandle.Value);

            bool AllowCollision = CollisionCheck.CanCollide(A_ValuePair.Item1, B_ValuePair.Item1, A_ValuePair.Item2.UseCollisions, B_ValuePair.Item2.UseCollisions);
            pairMaterial = default;

            if (AllowCollision)
                pairMaterial.FrictionCoefficient = A_ValuePair.Item2.Friction * B_ValuePair.Item2.Friction;

            pairMaterial.MaximumRecoveryVelocity = 2f;
            pairMaterial.SpringSettings = ContactSpringiness;

            return true;
        }



        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ConfigureContactManifold(int workerIndex, CollidablePair pair, int childIndexA, int childIndexB, ref ConvexContactManifold manifold)
        {
            return true;
        }

        public void Dispose()
        {
        }
    }
}
