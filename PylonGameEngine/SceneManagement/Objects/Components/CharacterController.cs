using PylonGameEngine.Input;
using PylonGameEngine.Mathematics;
using PylonGameEngine.Physics;
using PylonGameEngine.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PylonGameEngine.SceneManagement.Objects.Components
{
    public class CharacterController : RigidBody
    {
        /// <summary>
        /// Create a RigidBody with a Mesh collider
        /// </summary>
        /// <param name="Triangles">A list of Triangles of a Mesh</param>
        /// <param name="mass">Mass</param>
        public CharacterController(List<PylonGameEngine.Mathematics.Triangle> Triangles, float mass = 1f) : base(Triangles, mass) { }

        /// <summary>
        /// Create a RigidBody with a Triangle collider
        /// </summary>
        /// <param name="Triangles">A list of Triangles of a Triangle</param>
        /// <param name="mass">Mass</param>
        public CharacterController(PylonGameEngine.Mathematics.Triangle Triangle, float mass = 1f) : base(Triangle, mass) { }

        /// <summary>
        /// Create a RigidBody with a Box collider
        /// </summary>
        /// <param name="BoxSize">Size of the Box</param>
        /// <param name="mass">Mass</param>
        public CharacterController(Vector3 BoxSize, float mass = 1f) : base(BoxSize, mass) { }


        /// <summary>
        /// Create a RigidBody with a Cylinder collider
        /// </summary>
        /// <param name="radius">The Radius of the Cylinder</param>
        /// <param name="Length">The Length of the Cylinder</param>
        /// <param name="mass">Mass</param>
        public CharacterController(float radius, float length, bool Capsule = false, float mass = 1f) : base(radius, length, Capsule, mass) { }

        /// <summary>
        /// Create a RigidBody with a Cylinder collider
        /// </summary>
        /// <param name="radius">The Radius of the Cylinder</param>
        /// <param name="Length">The Length of the Cylinder</param>
        /// <param name="mass">Mass</param>
        public CharacterController(float radius, float mass = 1f) : base (radius, mass) { }

        public Camera Camera;
        public int CameraModes = 0;
        public float CameraHeight = 0.5f;

        public KeyboardKey MoveForward = KeyboardKey.W;
        public KeyboardKey MoveBackward = KeyboardKey.S;
        public KeyboardKey MoveLeft = KeyboardKey.A;
        public KeyboardKey MoveRight = KeyboardKey.D;
        public KeyboardKey Sprint = KeyboardKey.Shift;
        public KeyboardKey Jump = KeyboardKey.Space;
        MeshObject meshObject = new MeshObject();
        public bool OverrideOnGround = false;
        public bool OnGround
        {
            get
            {
                for (int i = 0; i < SceneContext.Objects.Count; i++)
                {
                    if (SceneContext.Objects[i].Tags.Contains("Ground") && SceneContext.Objects[i] is MeshObject)
                    {
             
                        MeshObject obj = SceneContext.Objects[i] as MeshObject;

                        BoundingBox boxobj = obj.Mesh.GetBoundingBox(obj.Transform.GlobalMatrix.Transposed);

                        Vector3 Size = new Vector3(0, ((MeshObject)Parent).Mesh.GetBoundingBox().Size.Y / 2f, 0);
                        Vector3 Point = Parent.Transform.GlobalPosition - Size + new Vector3(0, -0.1f, 0);


                        if (boxobj.PointInBox(Point))
                        {
                            float size = 2;
                            Vector3 Min = Point + new Vector3(-size, -size, -size);
                            Vector3 Max = Point + new Vector3(size, 0.1f, size);
                            BoundingBox PlayerFeet = new BoundingBox(Min, Max);

                            if(DebugSettings.CharacterSettings.ShowDebug)
                            {
                                meshObject.Mesh = Primitves3D.CreateCube(MyGame.Materials.Get("DEBUG_Blue"), Min, Max - Min, Quaternion.Identity, false);
                                SceneContext.Add(meshObject);
                            }


                            foreach (var item in obj.Mesh.Points)
                            {
                                var vert = obj.Transform.GlobalMatrix.Transposed * item;
                                if (PlayerFeet.PointInBox(vert))
                                {
                                    if (DebugSettings.CharacterSettings.ShowDebug)
                                    {
                                        meshObject.Mesh = Primitves3D.CreateCube(MyGame.Materials.Get("DEBUG_Red"), Min, Max - Min, Quaternion.Identity, false);
                                        SceneContext.Add(meshObject);
                                    }
                                    return true;
                                }
                            }
                        }

                        
                    }
                }
                return false;
            }
        }


        private Vector2 Rotation = Vector2.Zero;
        public override void UpdateTick()
        {
            lock (this)
            {
                Vector3 MoveDirection = Vector3.Zero;

                if (SceneContext.InputManager.Keyboard.KeyPressed(MoveForward))
                    MoveDirection += Parent.Transform.Forward;

                if (SceneContext.InputManager.Keyboard.KeyPressed(MoveBackward))
                    MoveDirection += Parent.Transform.Backward;

                if (SceneContext.InputManager.Keyboard.KeyPressed(MoveLeft))
                    MoveDirection += Parent.Transform.Left;

                if (SceneContext.InputManager.Keyboard.KeyPressed(MoveRight))
                    MoveDirection += Parent.Transform.Right;
                if (SceneContext.InputManager.Keyboard.KeyDown(Jump) && (OnGround || OverrideOnGround))
                    MoveDirection += Parent.Transform.Up;


                if ((OnGround || OverrideOnGround))
                    MoveDirection *= new Vector3(5, 5, 5);
                else
                    MoveDirection *= new Vector3(0.01f, 1, 0.01f);

                if (SceneContext.InputManager.Keyboard.KeyPressed(Sprint))
                    MoveDirection *= new Vector3(1.5f, 1, 1.5f);

                if (SceneContext.InputManager.Keyboard.KeyDown(KeyboardKey.V))
                    CameraModes++;

                if (Camera != null)
                {
                    if (Mouse.CursorLocked)
                    {
                        float mouseX = SceneContext.InputManager.Mouse.Delta.X / 4f;
                        float mouseY = SceneContext.InputManager.Mouse.Delta.Y / 4f;

                        Rotation.X += mouseY;
                        Rotation.X = Mathf.Clamp(Rotation.X, -90f, 90f);

                        Rotation.Y += mouseX;

                        Camera.Transform.Rotation = Quaternion.FromEuler(Rotation.X, 0f, 0f);
                        Parent.Transform.Rotation = Quaternion.FromEuler(0, Rotation.Y, 0);

                        if (CameraModes % 3 == 0)
                        {
                            Camera.Transform.Position = new Vector3(0, CameraHeight, 0);
                            Parent.Visible = false;
                        }
                        else if (CameraModes % 3 == 1)
                        {
                            Vector3 Position = (Camera.Transform.Rotation * Vector3.Backward * 5f);
                            Camera.Transform.Position = Position;
                            Camera.Transform.Rotation = Quaternion.LookAt(Position, new Vector3(0, CameraHeight, 0));
                            Parent.Visible = true;
                        }
                        else
                        {
                            Vector3 Position = (Camera.Transform.Rotation * Vector3.Forward * 5f);
                            Camera.Transform.Position = Position;
                            Camera.Transform.Rotation = Quaternion.LookAt(Position, new Vector3(0, CameraHeight, 0));
                            Camera.Transform.Rotation *= Quaternion.FromEuler(0f, 0f, 180f);
                            Parent.Visible = true;
                        }
                    }
                }

                if ((OnGround || OverrideOnGround))
                    Body.Velocity.Linear = new Vector3(MoveDirection.X, Body.Velocity.Linear.Y + MoveDirection.Y, MoveDirection.Z);
                else
                    Body.Velocity.Linear += MoveDirection;

                Body.Velocity.Angular = new Vector3();

                OverrideOnGround = false;
                base.UpdateTick();
            }
        }
    }
}
