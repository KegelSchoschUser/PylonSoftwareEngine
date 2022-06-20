using BepuPhysics;
using BepuPhysics.Collidables;
using PylonGameEngine.General;
using PylonGameEngine.Mathematics;
using PylonGameEngine.SceneManagement;
using System.Collections.Generic;

namespace PylonGameEngine.Physics
{
    public class RigidBody : PhysicsComponent
    {
        public BodyReference Body;
        public bool UseGravity = true;
        public bool UsePhysics = true;
        public bool UseCollisions = true;
        private InitializationDescription InitDesc = new InitializationDescription();

        public BepuPhysics.Collidables.Mesh? CollisionMesh;
        public int Index { get; private set; }

        private void BeforeConstructor(float mass)
        {
            InitDesc.Mass = mass;
        }

        /// <summary>
        /// Create a RigidBody with a Mesh collider
        /// </summary>
        /// <param name="Triangles">A list of Triangles of a Mesh</param>
        /// <param name="mass">Mass</param>
        public RigidBody(List<PylonGameEngine.Mathematics.Triangle> Triangles, float mass = 1f)
        {
            BeforeConstructor(mass);
            InitDesc.Shape = InitializationDescription.Shapes.Mesh;
            InitDesc.Triangles = Triangles;
        }

        /// <summary>
        /// Create a RigidBody with a Triangle collider
        /// </summary>
        /// <param name="Triangles">A list of Triangles of a Triangle</param>
        /// <param name="mass">Mass</param>
        public RigidBody(PylonGameEngine.Mathematics.Triangle Triangle, float mass = 1f)
        {
            BeforeConstructor(mass);
            InitDesc.Shape = InitializationDescription.Shapes.Mesh;
            var Triangles = new List<Mathematics.Triangle>();
            Triangles.Add(Triangle);
            InitDesc.Triangles = Triangles;
        }

        /// <summary>
        /// Create a RigidBody with a Box collider
        /// </summary>
        /// <param name="BoxSize">Size of the Box</param>
        /// <param name="mass">Mass</param>
        public RigidBody(Vector3 BoxSize, float mass = 1f)
        {
            BeforeConstructor(mass);
            InitDesc.Shape = InitializationDescription.Shapes.Box;
            InitDesc.BoxSize = BoxSize;
        }


        /// <summary>
        /// Create a RigidBody with a Cylinder collider
        /// </summary>
        /// <param name="radius">The Radius of the Cylinder</param>
        /// <param name="Length">The Length of the Cylinder</param>
        /// <param name="mass">Mass</param>
        public RigidBody(float radius, float length, bool Capsule = false, float mass = 1f)
        {
            BeforeConstructor(mass);
            if (Capsule)
                InitDesc.Shape = InitializationDescription.Shapes.Capsule;
            else
                InitDesc.Shape = InitializationDescription.Shapes.Cylinder;
            InitDesc.Radius = radius;
            InitDesc.Length = length;
        }

        /// <summary>
        /// Create a RigidBody with a Cylinder collider
        /// </summary>
        /// <param name="radius">The Radius of the Cylinder</param>
        /// <param name="Length">The Length of the Cylinder</param>
        /// <param name="mass">Mass</param>
        public RigidBody(float radius, float mass = 1f)
        {
            BeforeConstructor(mass);
            InitDesc.Shape = InitializationDescription.Shapes.Sphere;
            InitDesc.Radius = radius;
        }

        public override void Initialize()
        {
            Parent.Transform.PositionChange += Transform_PositionChange;
            Parent.Transform.RotationChange += Transform_RotationChange;

            TypedIndex meshIndex;
            BodyInertia Inertia;
            switch (InitDesc.Shape)
            {
                case InitializationDescription.Shapes.Mesh:
                    {
                        SceneContext.Physics.BufferPool.Take<BepuPhysics.Collidables.Triangle>(InitDesc.Triangles.Count, out var triangles);
                        for (int i = 0; i < InitDesc.Triangles.Count; ++i)
                        {
                            triangles[i] = new BepuPhysics.Collidables.Triangle(InitDesc.Triangles[i].P3.ToSystemNumerics(), InitDesc.Triangles[i].P2.ToSystemNumerics(), InitDesc.Triangles[i].P1.ToSystemNumerics());
                        }
                        BepuPhysics.Collidables.Mesh collisionShape = new BepuPhysics.Collidables.Mesh(triangles, Parent.Transform.Scale.ToSystemNumerics(), SceneContext.Physics.BufferPool);
                        CollisionMesh = collisionShape;
                        Inertia = collisionShape.ComputeOpenInertia(InitDesc.Mass, out var center);
                        meshIndex = SceneContext.Physics.Simulation.Shapes.Add(collisionShape);
                    }
                    break;
                case InitializationDescription.Shapes.Triangle:
                    {

                        BepuPhysics.Collidables.Triangle collisionShape = new BepuPhysics.Collidables.Triangle(InitDesc.Triangles[0].P1.ToSystemNumerics(),
                                                                                                               InitDesc.Triangles[0].P2.ToSystemNumerics(),
                                                                                                               InitDesc.Triangles[0].P3.ToSystemNumerics());
                        Inertia = collisionShape.ComputeInertia(InitDesc.Mass);
                        meshIndex = SceneContext.Physics.Simulation.Shapes.Add(collisionShape);
                    }
                    break;
                case InitializationDescription.Shapes.Box:
                    {

                        Box collisionShape = new Box(InitDesc.BoxSize.X, InitDesc.BoxSize.Y, InitDesc.BoxSize.Z);
                        Inertia = collisionShape.ComputeInertia(InitDesc.Mass);
                        meshIndex = SceneContext.Physics.Simulation.Shapes.Add(collisionShape);
                    }
                    break;
                case InitializationDescription.Shapes.Capsule:
                    {

                        Capsule collisionShape = new Capsule(InitDesc.Radius, InitDesc.Length);
                        Inertia = collisionShape.ComputeInertia(InitDesc.Mass);
                        meshIndex = SceneContext.Physics.Simulation.Shapes.Add(collisionShape);
                    }
                    break;
                case InitializationDescription.Shapes.Cylinder:
                    {

                        Cylinder collisionShape = new Cylinder(InitDesc.Radius, InitDesc.Length);
                        Inertia = collisionShape.ComputeInertia(InitDesc.Mass);
                        meshIndex = SceneContext.Physics.Simulation.Shapes.Add(collisionShape);
                    }
                    break;
                case InitializationDescription.Shapes.Sphere:
                    {

                        Sphere collisionShape = new Sphere(InitDesc.Radius);
                        Inertia = collisionShape.ComputeInertia(InitDesc.Mass);
                        meshIndex = SceneContext.Physics.Simulation.Shapes.Add(collisionShape);
                    }
                    break;
                default:
                    return;
            }

            BodyHandle Handle = SceneContext.Physics.Simulation.Bodies.Add(BodyDescription.CreateDynamic(new RigidPose(Parent.Transform.GlobalMatrix.TranslationVector.ToSystemNumerics(), Matrix4x4.RotationQuaternion(Parent.Transform.GlobalMatrix).ToSystemNumerics()),
                            Inertia,
                            new CollidableDescription(meshIndex, 0.1f),
                            new BodyActivityDescription(0.01f)));
            Index = Handle.Value;
            Body = new BodyReference(Handle, SceneContext.Physics.Simulation.Bodies);
            SceneContext.Physics.RigidBodies.Add(this);
        }

        public override void OnDestroy()
        {
            SceneContext.Physics.Simulation.Bodies.Remove(Body.Handle);
            SceneContext.Physics.RigidBodies.Remove(this);
        }

        private void Transform_PositionChange()
        {
            Body.GetDescription(out var desc);

            desc.Pose.Position = Parent.Transform.Position.ToSystemNumerics();
            Body.ApplyDescription(desc);
        }


        private void Transform_RotationChange()
        {
            Body.GetDescription(out var desc);

            desc.Pose.Orientation = Parent.Transform.Rotation.ToSystemNumerics();

            Body.ApplyDescription(desc);
        }

        public void SetVelocityLinear(Vector3 linear)
        {
            Body.GetDescription(out var desc);

            desc.Velocity.Linear = linear.ToSystemNumerics();

            Body.ApplyDescription(desc);
        }

        public void SetVelocityAngular(Vector3 angular)
        {
            Body.GetDescription(out var desc);

            desc.Velocity.Angular = angular.ToSystemNumerics();

            Body.ApplyDescription(desc);
        }


        public void SetVelocity(Vector3 linear, Vector3 angular)
        {
            Body.GetDescription(out var desc);

            desc.Velocity.Linear = linear.ToSystemNumerics();
            desc.Velocity.Angular = angular.ToSystemNumerics();

            Body.ApplyDescription(desc);
        }

        public void AddVelocityLinear(Vector3 linear)
        {
            Body.GetDescription(out var desc);

            desc.Velocity.Linear = desc.Velocity.Linear + linear.ToSystemNumerics();

            Body.ApplyDescription(desc);
        }

        public void AddVelocityAngular(Vector3 angular)
        {
            Body.GetDescription(out var desc);

            desc.Velocity.Angular = desc.Velocity.Angular + angular.ToSystemNumerics();

            Body.ApplyDescription(desc);
        }

        public void AddVelocity(Vector3 linear, Vector3 angular)
        {
            Body.GetDescription(out var desc);

            desc.Velocity.Linear = desc.Velocity.Linear + linear.ToSystemNumerics();
            desc.Velocity.Angular = desc.Velocity.Angular + angular.ToSystemNumerics();

            Body.ApplyDescription(desc);
        }

        public void ResetVelocity()
        {
            Body.GetDescription(out var desc);

            desc.Velocity.Linear = Vector3.Zero.ToSystemNumerics();
            desc.Velocity.Angular = Vector3.Zero.ToSystemNumerics();

            Body.ApplyDescription(desc);
        }

        public void Update()
        {
            Parent.Transform.UnsafeSetPosition(Vector3.FromSystemNumerics(Body.Pose.Position));
            Parent.Transform.UnsafeSetRotation(Quaternion.FromSystemNumerics(Body.Pose.Orientation));
        }




        private class InitializationDescription
        {
            internal enum Shapes
            {
                NONE,
                Mesh,
                Triangle,
                Box,
                Capsule,
                Cylinder,
                Sphere
            }

            internal Shapes Shape = Shapes.NONE;
            internal float Mass = 1f;
            internal List<PylonGameEngine.Mathematics.Triangle> Triangles;
            internal Vector3 BoxSize;
            internal float Radius;
            internal float Length;
        }
    }
}
