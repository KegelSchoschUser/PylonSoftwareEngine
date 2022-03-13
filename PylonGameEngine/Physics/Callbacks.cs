using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.CollisionDetection;
using BepuPhysics.Constraints;
using BepuUtilities;
using PylonGameEngine.Utilities;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace PylonGameEngine.Physics
{
    public struct DemoPoseIntegratorCallbacks : IPoseIntegratorCallbacks
    {
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
            gravityWideDt = Vector3Wide.Broadcast(MyPhysics.Gravity.ToSystemNumerics() * dt);
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

            var RigidBody = MyPhysics.RigidBodies.Find(x => x.Index == bodyIndices[0]);
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
            //While the engine won't even try creating pairs between statics at all, it will ask about kinematic-kinematic pairs.
            //Those pairs cannot emit constraints since both involved bodies have infinite inertia. Since most of the demos don't need
            //to collect information about kinematic-kinematic pairs, we'll require that at least one of the bodies needs to be dynamic.
            if (a.Mobility == b.Mobility)
                return false;

            TriggerBody A_Trigger = null;
            TriggerBody B_Trigger = null;

            RigidBody A_Rigid = null;
            RigidBody B_Rigid = null;

            StaticBody A_Static = null;
            StaticBody B_Static = null;

            if (a.Mobility == CollidableMobility.Dynamic)
            {
                A_Rigid = MyPhysics.RigidBodies.Find(x => x.Index == a.BodyHandle.Value);
            }
            else if (a.Mobility == CollidableMobility.Static)
            {
                A_Trigger = MyPhysics.TriggerBodies.Find(x => x.Index == a.BodyHandle.Value);
                A_Static = MyPhysics.StaticBodies.Find(x => x.Index == a.BodyHandle.Value);
            }

            if (b.Mobility == CollidableMobility.Dynamic)
            {
                B_Rigid = MyPhysics.RigidBodies.Find(x => x.Index == b.BodyHandle.Value);
            }
            else if (b.Mobility == CollidableMobility.Static)
            {
                B_Trigger = MyPhysics.TriggerBodies.Find(x => x.Index == b.BodyHandle.Value);
                B_Static = MyPhysics.StaticBodies.Find(x => x.Index == b.BodyHandle.Value);
            }

            bool MobilityCheck = a.Mobility == CollidableMobility.Dynamic || b.Mobility == CollidableMobility.Dynamic;

            if (A_Trigger == null && B_Trigger == null
                && A_Rigid == null && B_Rigid == null
                && A_Static == null && B_Static == null || MobilityCheck == false)
                return false;

            if (A_Trigger != null || B_Trigger != null)
            {
                if ((A_Trigger != null && B_Trigger != null) == false)
                {
                    if (A_Trigger != null)
                        A_Trigger.InvokeEvent(B_Rigid != null ? B_Rigid : B_Static);

                    if (B_Trigger != null)
                        B_Trigger.InvokeEvent(A_Rigid != null ? A_Rigid : A_Static);
                }
                return false;
            }
            bool CollisionEnabledCheck = (A_Rigid != null ? A_Rigid.UseCollisions : A_Static.UseCollisions) && (B_Rigid != null ? B_Rigid.UseCollisions : B_Static.UseCollisions);

            return CollisionEnabledCheck;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool AllowContactGeneration(int workerIndex, CollidablePair pair, int childIndexA, int childIndexB)
        {
            //return AllowContactGeneration(workerIndex, pair.A, pair.B, )
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe bool ConfigureContactManifold<TManifold>(int workerIndex, CollidablePair pair, ref TManifold manifold, out PairMaterialProperties pairMaterial) where TManifold : unmanaged, IContactManifold<TManifold>
        {
            pairMaterial.FrictionCoefficient = 1f;
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
