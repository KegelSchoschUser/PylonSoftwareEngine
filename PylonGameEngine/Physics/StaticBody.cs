using BepuPhysics;
using BepuPhysics.Collidables;
using PylonGameEngine.General;
using PylonGameEngine.Mathematics;
using PylonGameEngine.SceneManagement;
using System;
using System.Collections.Generic;

namespace PylonGameEngine.Physics
{
    public class StaticBody : PhysicsComponent
    {
        public StaticReference Body;
        public int Index { get; private set; }
        public bool UseCollisions = true;
        private InitializationDescription InitDesc = new InitializationDescription();
        public BepuPhysics.Collidables.Mesh CollisionMesh;

        public StaticBody(List<PylonGameEngine.Mathematics.Triangle> Triangles, float mass = 1f)
        {
            InitDesc.Mass = mass;
            InitDesc.Shape = InitializationDescription._Shape.Mesh;
            InitDesc.Triangles = Triangles;
        }

        public StaticBody(Vector3 BoxSize, float mass = 1f)
        {
            InitDesc.Mass = mass;
            InitDesc.Shape = InitializationDescription._Shape.Box;
            InitDesc.BoxSize = BoxSize;
        }

        public override void Initialize()
        {
            Parent.Transform.PositionChange += Transform_PositionChange;
            Parent.Transform.RotationChange += Transform_RotationChange;
            TypedIndex meshIndex;
            switch (InitDesc.Shape)
            {
                case InitializationDescription._Shape.Mesh:
                    {
                        SceneContext.Physics.BufferPool.Take<BepuPhysics.Collidables.Triangle>(InitDesc.Triangles.Count, out var triangles);
                        for (int i = 0; i < InitDesc.Triangles.Count; ++i)
                        {
                            triangles[i] = new BepuPhysics.Collidables.Triangle(InitDesc.Triangles[i].P3.ToSystemNumerics(), InitDesc.Triangles[i].P2.ToSystemNumerics(), InitDesc.Triangles[i].P1.ToSystemNumerics());
                        }
                        //Parent.Transform.GlobalMatrix.TranslationVector
                        BepuPhysics.Collidables.Mesh collisionShape = new BepuPhysics.Collidables.Mesh(triangles, Parent.Transform.Scale.ToSystemNumerics(), SceneContext.Physics.BufferPool);
                        CollisionMesh = collisionShape;
                        meshIndex = SceneContext.Physics.Simulation.Shapes.Add(collisionShape);
                    }
                    break;
                case InitializationDescription._Shape.Box:
                    {

                        Box collisionShape = new Box(InitDesc.BoxSize.X, InitDesc.BoxSize.Y, InitDesc.BoxSize.Z);
                        var inertia = collisionShape.ComputeInertia(InitDesc.Mass);
                        meshIndex = SceneContext.Physics.Simulation.Shapes.Add(collisionShape);
                    }
                    break;
                default:
                    return;
            }


            StaticHandle Handle = SceneContext.Physics.Simulation.Statics.Add(new StaticDescription(Parent.Transform.GlobalMatrix.TranslationVector.ToSystemNumerics(), Matrix4x4.RotationQuaternion(Parent.Transform.GlobalMatrix).ToSystemNumerics(), new CollidableDescription(meshIndex, 0.1f).Shape));

            Index = Handle.Value;
            Body = new StaticReference(Handle, SceneContext.Physics.Simulation.Statics);
            Body.Pose.Position = -Parent.Transform.Position.ToSystemNumerics();
            SceneContext.Physics.StaticBodies.Add(this);
        }

        public override void OnDestroy()
        {
            SceneContext.Physics.Simulation.Statics.Remove(Body.Handle);
            SceneContext.Physics.StaticBodies.Remove(this);
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

        public void Update()
        {
            Body.GetDescription(out var desc);

            desc.Pose.Position = Parent.Transform.Position.ToSystemNumerics();
            desc.Pose.Orientation = Parent.Transform.Rotation.ToSystemNumerics();

            Body.ApplyDescription(desc);
        }

        private class InitializationDescription
        {
            internal enum _Shape
            {
                NONE,
                Mesh,
                Box
            }

            internal _Shape Shape = _Shape.NONE;
            internal float Mass = 1f;
            internal List<PylonGameEngine.Mathematics.Triangle> Triangles;
            internal Vector3 BoxSize;
        }
    }
}
