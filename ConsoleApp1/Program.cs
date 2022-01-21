using PylonGameEngine;
using PylonGameEngine.Audio;
using PylonGameEngine.Billboarding;
using PylonGameEngine.FileSystem.Filetypes;
using PylonGameEngine.FileSystem.Filetypes.GIW;
using PylonGameEngine.FileSystem.Filetypes.WAVE;
using PylonGameEngine.GameWorld;
using PylonGameEngine.GameWorld3D;
using PylonGameEngine.GUI.GUIObjects;
using PylonGameEngine.Input;
using PylonGameEngine.Mathematics;
using PylonGameEngine.Physics;
using PylonGameEngine.Render11;
using PylonGameEngine.ShaderLibrary;
using PylonGameEngine.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime;
using System.Runtime.CompilerServices;
using Vortice.Multimedia;
using Vortice.XAudio2;

//Simple flying "Game"


public class EnableMirrorCam : GameScript
{
    CameraObject camera;
    public EnableMirrorCam(CameraObject cam)
    {
        camera = cam;
    }

    public override void UpdateFrame()
    {
        if (PylonGameEngine.Input.Keyboard.KeyDown(KeyboardKey.Num1))
        {
            camera.Enabled = !camera.Enabled;
        }

        if (PylonGameEngine.Input.Keyboard.KeyPressed(KeyboardKey.Num2))
        {
            var MirrorTexture = camera.CameraRender.RenderPhases[camera.CameraRender.RenderPhases.Length - 1].RenderTexture;
            PylonGameEngine.UI.Drawing.Graphics g = new PylonGameEngine.UI.Drawing.Graphics(MirrorTexture);
            g.BeginDraw();

            g.FillRectangle(g.CreateSolidBrush(RGBColor.Black));

            var pen = g.CreatePen(RGBColor.White, 1);
            for (int i = 0; i < MirrorTexture.Size.X; i += 15)
            {
                g.DrawLine(pen, i, 0, i, MirrorTexture.Size.Y);
            }

            for (int i = 0; i < MirrorTexture.Size.Y; i += 15)
            {
                g.DrawLine(pen, 0, i, MirrorTexture.Size.X, i);
            }

            g.EndDraw();
            
        }

        if (PylonGameEngine.Input.Keyboard.KeyPressed(KeyboardKey.Num3))
        {
            var MirrorTexture = camera.CameraRender.RenderPhases[camera.CameraRender.RenderPhases.Length - 1].RenderTexture;
            PylonGameEngine.UI.Drawing.Graphics g = new PylonGameEngine.UI.Drawing.Graphics(MirrorTexture);
            g.BeginDraw();

            g.FillRectangle(g.CreateSolidBrush(RGBColor.Black));


            g.EndDraw();

        }

        var mesh = Primitves3D.CreateSphere(null, Vector3.Zero, 200, 15);

        //foreach (var item in mesh.TriangleData)
        //{
        //    BillBoard.DrawLine(item.P1, item.P2, MyGame.Materials.Get("DEBUG_Red"), true);
        //    BillBoard.DrawLine(item.P2, item.P3, MyGame.Materials.Get("DEBUG_Red"), true);
        //    BillBoard.DrawLine(item.P3, item.P1, MyGame.Materials.Get("DEBUG_Red"), true);
        //}
    }
}

public class CameraScript2 : GameScript
{
    CameraObject camera;
    public CameraScript2(CameraObject cam)
    {
        camera = cam;
    }

    public override void UpdateTick()
    {
        if (PylonGameEngine.Input.Keyboard.KeyPressed(KeyboardKey.Num2))
        {
            camera.Activate();
        }
    }
}

public class CameraScript3 : GameScript
{
    CameraObject camera;
    public CameraScript3(CameraObject cam)
    {
        camera = cam;
    }

    public override void UpdateTick()
    {
        if (PylonGameEngine.Input.Keyboard.KeyPressed(KeyboardKey.Num3))
        {
            camera.Activate();
        }
    }
}

//public class LookAtScript : GameScript
//{
//    GameObject3D LookAtObject;
//    public LookAtScript(GameObject3D lookAtObject)
//    {
//        LookAtObject = lookAtObject;
//    }

//    public override void UpdateTick()
//    {
//        //((GameObject3D)Parent).Transform.Rotation = Quaternion.LookAt(((GameObject3D)Parent).Transform.Position, LookAtObject.Transform.Position);
//        ((GameObject3D)Parent).Transform.Rotation = Quaternion.FromEuler(((GameObject3D)LookAtObject).Transform.Rotation.ToEuler());
//    }
//}


public class MyScript : GameScript
{
    bool Trackmouse = true;

    public override void UpdateFrame()
    {
        //Program.Cube.Mesh = Primitves3D.CreateCube(MyGame.Materials.Get("DEBUG_Red"), new Vector3(Mathf.Sin(MyGame.RenderLoop.Frames / 60f / Mathf.PI) * 10, 0, 0), new Vector3(10), Quaternion.FromEuler(45, 45, 0), true);
        //Program.Cube.Mesh.EnableBoundingBox = false;

        //BillBoard.DrawLine(new Vector3(0, 0, 0), new Vector3(100), MyGame.Materials.Get("DEBUG_Red"), false);
        //BillBoard.DrawLineCube(new Vector3(0, 0, 0), new Vector3(100), MyGame.Materials.Get("DEBUG_Red"), false);
        //Program.button.EdgeSize = 1f;
        //Program.button.Transform.Rotation = Program.slx.Value * 360f;
        //Program.button.Transform.Size = new Vector2(Program.button.Transform.Size.X, Program.button.Transform.Size.X * Program.sly.Value * 2f);
        //Program.gUIObject.x = Program.slx.Value;
        //Program.gUIObject.y = Program.sly.Value;
        //Program.slx.QueueDraw();
        //Program.sly.QueueDraw();
        //Program.gUIObject.QueueDraw();

        Program.RotObject.Transform.Rotation = Program.sly.Value * 360f;
        Program.RotObject.Transform.UnsafeSetSize(new Vector2(Program.slx.Value));
        float multiplier = 2f;

        //if (PylonGameEngine.Input.Keyboard.KeyPressed(KeyboardKey.LeftShift))
        //{
        //    multiplier = 4f;
        //}
        multiplier *= 1000f;

        //if (PylonGameEngine.Input.Keyboard.KeyDown(KeyboardKey.P))
        //{
        //    for (int i = 0; i < Renderer.RenderPhases.Length; i++)
        //    {
        //        var img = D3D11GraphicsDevice.ConvertToImage(D3D11GraphicsDevice.CaptureTexture(Renderer.RenderPhases[i].Output));

        //        var i2 = new Bitmap(img);
        //        string path = @"Temp\" + i.ToString() + " - " + DateTime.Now.Ticks.ToString() + ".png";
        //        i2.Save(path, System.Drawing.Imaging.ImageFormat.Png);
        //    }
        //}

        if (PylonGameEngine.Input.Keyboard.KeyPressed(KeyboardKey.W))
        {
            Program.Planerigid.Body.ApplyLinearImpulse((MyGameWorld.ActiveCamera.Transform.Forward * multiplier).ToSystemNumerics());
            //MyGameWorld.ActiveCamera.Transform.Position += MyGameWorld.ActiveCamera.Transform.Forward * multiplier;
        }
        if (PylonGameEngine.Input.Keyboard.KeyPressed(KeyboardKey.S))
        {
            Program.Planerigid.Body.ApplyLinearImpulse((MyGameWorld.ActiveCamera.Transform.Backward * multiplier).ToSystemNumerics());
            // MyGameWorld.ActiveCamera.Transform.Position += MyGameWorld.ActiveCamera.Transform.Backward * multiplier;
        }
        if (PylonGameEngine.Input.Keyboard.KeyPressed(KeyboardKey.A))
        {
            Program.Planerigid.Body.ApplyLinearImpulse((MyGameWorld.ActiveCamera.Transform.Left * multiplier).ToSystemNumerics());
            // MyGameWorld.ActiveCamera.Transform.Position += MyGameWorld.ActiveCamera.Transform.Left * multiplier;
        }
        if (PylonGameEngine.Input.Keyboard.KeyPressed(KeyboardKey.D))
        {
            Program.Planerigid.Body.ApplyLinearImpulse((MyGameWorld.ActiveCamera.Transform.Right * multiplier).ToSystemNumerics());
            //   MyGameWorld.ActiveCamera.Transform.Position += MyGameWorld.ActiveCamera.Transform.Right * multiplier;
        }
        if (PylonGameEngine.Input.Keyboard.KeyPressed(KeyboardKey.Space))
        {
            Program.Planerigid.Body.ApplyLinearImpulse((MyGameWorld.ActiveCamera.Transform.Up * multiplier).ToSystemNumerics());
            //MyGameWorld.ActiveCamera.Transform.Position += MyGameWorld.ActiveCamera.Transform.Up * multiplier;
        }
        if (PylonGameEngine.Input.Keyboard.KeyPressed(KeyboardKey.C))
        {
            Program.Planerigid.Body.ApplyLinearImpulse((MyGameWorld.ActiveCamera.Transform.Down * multiplier).ToSystemNumerics());
            // MyGameWorld.ActiveCamera.Transform.Position += MyGameWorld.ActiveCamera.Transform.Down * multiplier;
        }

        if (PylonGameEngine.Input.Keyboard.KeyPressed(KeyboardKey.Q))
        {
            MyGameWorld.ActiveCamera.Transform.Rotation *= Quaternion.FromEuler(0, 0, 3f);
        }
        if (PylonGameEngine.Input.Keyboard.KeyPressed(KeyboardKey.E))
        {
            MyGameWorld.ActiveCamera.Transform.Rotation *= Quaternion.FromEuler(0, 0, -3f);
        }
        if (Mouse.RightButtonPressed())
        {
            Program.Planerigid.AddVelocityLinear(Program.Planerigid.Parent.Transform.Forward * 100f);
        }

        if (PylonGameEngine.Input.Keyboard.KeyDown(KeyboardKey.F))
        {
            DebugSettings.UISettings.DrawLayoutRectangle = !DebugSettings.UISettings.DrawLayoutRectangle;
            // MyGameWorld.ActiveCamera.Transform.Position += MyGameWorld.ActiveCamera.Transform.Down * multiplier;
        }

        if (PylonGameEngine.Input.Keyboard.KeyDown(KeyboardKey.U))
        {
            Program.Planerigid.Parent.Transform.Position = new Vector3(0, 100, 9);
        }
        //  BillBoard.DrawLineCube(Vector3.FromSystemNumerics(Program.Planerigid.Body.BoundingBox.Min),Vector3.FromSystemNumerics (Program.Planerigid.Body.BoundingBox.Max - Program.Planerigid.Body.BoundingBox.Min), 0.1f, MyGame.Materials.Get("DEBUG_Red"), true) ;
        //  BillBoard.DrawLine(Program.Planerigid.Parent.Transform.Position, Program.Planerigid.Parent.Transform.Forward * 100000f, 0.1f, MyGame.Materials.Get("DEBUG_Blue"), true);
        if (PylonGameEngine.Input.Keyboard.KeyPressed(KeyboardKey.Alt) && PylonGameEngine.Input.Keyboard.KeyPressed(KeyboardKey.F4))
        {
            MyGame.Stop();
        }

        Program.Cube.Transform.Scale = new Vector3(1, 10, 1);

        var Matrix = Program.Cube.Transform.GlobalMatrix;
        Matrix.Transpose();

        //foreach (var item in MyGameWorld.Objects)
        //{
        //    if(item is MeshObject)
        //    {
        //        BillBoard.DrawLBoundingBox(((MeshObject)item).Mesh.GetBoundingBox(item.Transform.GlobalMatrix.Transposed), 0.3f, MyGame.Materials.Get("DEBUG_Blue"), true);
        //    }
        //}

        //BillBoard.DrawBoundingBox(Program.Cube.Mesh.GetBoundingBox(Matrix), 0.3f, MyGame.Materials.Get("DEBUG_Blue"), true);
        Program.Cube.Transform.Position = Program.Cube.Transform.Forward * 10f + new Vector3(Mathf.Sin(MyGame.RenderLoop.Frames / 30f) * 50f);
        Program.Cube.Transform.Rotation = Program.Cube.Transform.Rotation * Quaternion.FromEuler(Mathf.Sin(MyGame.RenderLoop.Frames / 40f), Mathf.Cos(MyGame.RenderLoop.Frames / 40f), Mathf.Tan(MyGame.RenderLoop.Frames / 40f));

        //lock (MyPhysics.RigidBodies.LOCK)
        //    foreach (var item in MyPhysics.RigidBodies)
        //    {
        //        BillBoard.DrawLineCube(Vector3.FromSystemNumerics(item.Body.BoundingBox.Min), Vector3.FromSystemNumerics(item.Body.BoundingBox.Max - item.Body.BoundingBox.Min), .5f, MyGame.Materials.Get("DEBUG_Red"), true);
        //    }

        //lock (MyPhysics.StaticBodies.LOCK)
        //    foreach (var item in MyPhysics.StaticBodies)
        //    {
        //        if (item.Parent.Name != "Terrain")
        //            BillBoard.DrawLineCube(Vector3.FromSystemNumerics(item.Body.BoundingBox.Min), Vector3.FromSystemNumerics(item.Body.BoundingBox.Max - item.Body.BoundingBox.Min), .5f, MyGame.Materials.Get("DEBUG_Green"), true);
        //        //else
        //        //    BillBoard.DrawLineCube(Vector3.FromSystemNumerics(item.Body.BoundingBox.Min), Vector3.FromSystemNumerics(item.Body.BoundingBox.Max - item.Body.BoundingBox.Min), .5f, MyGame.Materials.Get("DEBUG_Blue"), true);
        //    }
        if (Trackmouse)
        {
            //  Console.WriteLine(Mouse.Delta);
            // ((TextObject)MyGameWorld.GUIObjects[0]).Text = Mouse.Delta.ToString();

            //Random r = new Random();
            MyGameWorld.ActiveCamera.Transform.Rotation *= Quaternion.FromEuler(Mouse.Delta.Y / 4f, Mouse.Delta.X / 4f, 0);


        }
        if (PylonGameEngine.Input.Keyboard.KeyDown(KeyboardKey.G))
        {
            Mouse.LockCursor(MyGame.MainWindow.Rectangle);
            Console.WriteLine(Mouse.CursorLockState);
        }
        if (PylonGameEngine.Input.Keyboard.KeyDown(KeyboardKey.T))
        {
            Mouse.UnlockCursor();
            Console.WriteLine(Mouse.CursorLockState);

        }
        if (PylonGameEngine.Input.Keyboard.KeyDown(KeyboardKey.H))
        {
            Mouse.HideCursor();
        }
        if (PylonGameEngine.Input.Keyboard.KeyDown(KeyboardKey.J))
        {

            Mouse.ShowCursor();

        }

        if (PylonGameEngine.Input.Mouse.MiddleButtonDown())
        {
            MyPhysics.Paused = !MyPhysics.Paused;
            Program.Planerigid.Body.Velocity = new BepuPhysics.BodyVelocity();
        }

        if (PylonGameEngine.Input.Mouse.LeftButtonDown())
        {
            Program.audio.Play();
        }

        if (PylonGameEngine.Input.Keyboard.KeyDown(KeyboardKey.R))
        {
            Program.audio.Loop = !Program.audio.Loop;
            Console.WriteLine(Program.audio.Loop);
        }


        //if (PylonGameEngine.Input.Mouse.RightButtonDown())
        //{
        //    MyPhysics.Paused = !MyPhysics.Paused;
        //}

        if (PylonGameEngine.Input.Keyboard.KeyDown(KeyboardKey.Num0))
        {
            Trackmouse = !Trackmouse;
        }

        MyGameWorld.ActiveCamera.FoV -= Mouse.ScrollDelta / 60f;

        //Bitmap bmp = new Bitmap(D3D11GraphicsDevice.ConvertToImage(Program.MirrorTexture.InternalTexture));
        //bmp.Save(@"Temp\" + DateTime.Now.Ticks + ".bmp");

    }

    public override void UpdateTick()
    {
        //     ((TextObject)MyGameWorld.GUIObjects[0]).Text = Program.Planerigid.Parent.Transform.Position.ToString();
       
    }
    //public override void UpdateFrame()
    //{
    //   BillBoard.DrawLine(MyGameWorld.Objects[0].Position, MyGameWorld.Objects[0].Position + (MyGameWorld.Objects[0].Forward * 100f), 0.25f, GlobalResources.Materials[2]);
    //    BillBoard.DrawLine(new Vector3(0, 0, 0), new Vector3(1, 0, 0) * 100f, 0.25f, GlobalResources.Materials[2]);
    //    BillBoard.DrawLine(new Vector3(0, 0, 0), new Vector3(0, 1, 0) * 100f, 0.25f, GlobalResources.Materials[3]);
    //    BillBoard.DrawLine(new Vector3(0, 0, 0), new Vector3(0, 0, 1) * 100f, 0.25f, GlobalResources.Materials[4]);
    //}
}

public class GenerationScript : GameScript
{
    public GameObject3D ControllerObject;

    public GenerationScript(GameObject3D controllerobj)
    {
        ControllerObject = controllerobj;
    }

    private struct ChunkPos
    {
        public int X;
        public int Z;


        public ChunkPos(int x, int y)
        {
            X = x;
            Z = y;
        }
    }

    private ChunkPos curChunk = new ChunkPos(-1, -1);
    private readonly int chunkDist = 5;
    private readonly int chunkSize = 16;
    private readonly Dictionary<ChunkPos, GameObject3D> chunks = new Dictionary<ChunkPos, GameObject3D>();
    private readonly FastNoise f = new FastNoise();
    public override void UpdateTick()
    {
        return;
        int curChunkPosX = (int)System.MathF.Floor(ControllerObject.Transform.Position.X / (chunkSize)) * (chunkSize);
        int curChunkPosZ = (int)System.MathF.Floor(ControllerObject.Transform.Position.Z / (chunkSize)) * (chunkSize);
        var matindex = MyGame.Materials.Get("Grass");
        if (curChunk.X != curChunkPosX || curChunk.Z != curChunkPosZ || curChunk.Z != curChunkPosZ)
        {
            curChunk.X = curChunkPosX;
            curChunk.Z = curChunkPosZ;


            for (int chunkX = curChunkPosX - (chunkSize) * chunkDist; chunkX <= curChunkPosX + (chunkSize) * chunkDist; chunkX += (chunkSize))
            {
                for (int chunkZ = curChunkPosZ - (chunkSize) * chunkDist; chunkZ <= curChunkPosZ + (chunkSize) * chunkDist; chunkZ += (chunkSize))
                {
                    ChunkPos cp = new ChunkPos(chunkX, chunkZ);

                    if (!chunks.ContainsKey(cp))
                    {
                        MeshObject chunkobject = new MeshObject();
                        chunkobject.Mesh.EnableBoundingBox = false;

                        //chunkobject.Mesh.AddTriangle(new Triangle( new Vector3(0, 0, 0 + size), new Vector3(0 + size, 0, 0 + size),            new Vector3(0 + size, 0, 0),  new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1)), matindex);
                        //chunkobject.Mesh.AddTriangle(new Triangle( new Vector3(0, 0, 0 + size), new Vector3(0 + size, 0, 0),     new Vector3(0, 0, 0),         new Vector2(0, 0), new Vector2(1, 1), new Vector2(0, 1)), matindex);

                        for (int x = 0; x < chunkSize; x++)
                        {
                            for (int z = 0; z < chunkSize; z++)
                            {
                                Vector3 Offset = new Vector3(x, 0, z);
                                int y = (int)(f.GetSimplex(x + chunkX, z + chunkZ) * 10);
                                chunkobject.Mesh.AddQuad(new Quad(new Vector3(0, y, 1) + Offset, new Vector3(1, y, 1) + Offset, new Vector3(1, y, 0) + Offset, new Vector3(0, y, 0) + Offset, new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1)), matindex);
                            }
                        }

                        chunkobject.Transform.Position = new Vector3(chunkX, 0f, chunkZ);
        
                        MyGameWorld.Objects.Add(chunkobject);
                        this.chunks.Add(new GenerationScript.ChunkPos(chunkX, chunkZ), chunkobject);
                    }
                }
            }
            //unload chunks
            foreach (KeyValuePair<ChunkPos, GameObject3D> c in chunks)
            {
                ChunkPos cp = c.Key;
                if (System.MathF.Abs(curChunkPosX - cp.X) > (chunkSize) * (chunkDist + 3) ||
                    System.MathF.Abs(curChunkPosZ - cp.Z) > (chunkSize) * (chunkDist + 3) ||
                    System.MathF.Abs(curChunkPosZ - cp.Z) > (chunkSize) * (chunkDist + 3))
                {
                    chunks[c.Key].Destroy();
                    chunks.Remove(c.Key);
                }
            }
        }
    }
}

//public class DragObject : GameScript
//{
//    Vector3 mOffset;
//    float mZCoord;



//    public override void UpdateTick()
//    {
//        if (Mouse.LeftButtonDown())
//        {
//            mZCoord = MyGameWorld.ActiveCamera.WorldToScreenPoint(((GameObject3D)Parent).Transform.Position).Z;
//            mOffset = ((GameObject3D)Parent).Transform.Position - GetMouseWorldPosition();
//        }
//        if (Mouse.LeftButtonPressed())
//        {
//            ((GameObject3D)Parent).Transform.Position = GetMouseWorldPosition() + mOffset;
//        }
//    }

//    public Vector3 GetMouseWorldPosition()
//    {
//        Vector3 mousePoint = new Vector3(Mouse.Position);

//        mousePoint.Z = mZCoord;

//        return MyGameWorld.ActiveCamera.ScreenToWorldPoint(mousePoint);
//    }
//}



public static class Program
{
    public static Audio audio;
    public static TextureShader LandingColorShader;
    public static RigidBody Planerigid;
    public static Slider slx;
    public static Slider sly;
    public static Button button;
    public static MeshObject Cube = new MeshObject();
    public static GUIObject RotObject;
    public static RenderTexture MirrorTexture;
    public static void Main()
    {
        GameProperties.GameName = "GIW TEST";
        GameProperties.Version = new MyVersion(1, 0, 0);
        GameProperties.StartWindowSize = new Vector2(1920, 1080);
        GameProperties.SplashScreen = new SplashScreen((Bitmap)Bitmap.FromFile("Splash.png"));
        GameProperties.RenderTickRate = 60;
        MyGame.Initialize();

        MirrorTexture = new RenderTexture(1920, 1080);
        PylonGameEngine.UI.Drawing.Graphics g = new PylonGameEngine.UI.Drawing.Graphics(MirrorTexture);
        g.BeginDraw();

        g.FillRectangle(g.CreateSolidBrush(RGBColor.Black));

        var pen = g.CreatePen(RGBColor.White, 1);
        for (int i = 0; i < MirrorTexture.Size.X; i+= 15)
        {
            g.DrawLine(pen, i, 0, i, MirrorTexture.Size.Y);
        }

        for (int i = 0; i < MirrorTexture.Size.Y; i += 15)
        {
            g.DrawLine(pen, 0, i, MirrorTexture.Size.X, i);
        }

        g.EndDraw();


        var MirrorShader = new TextureShader(MirrorTexture);
        Material Mirror = new Material("Mirror", MirrorShader);
        MyGame.Materials.Add(Mirror);

        var WindowTarget = (RenderTexture)MyGameWorld.WindowRenderTarget;
        var MAINCAM = new CameraObject(WindowTarget, false);
 
        MAINCAM.Activate();
        MyPhysics.Gravity = Vector3.Zero;
        MeshObject Plane = new MeshObject();
        Plane.Transform.Position = new Vector3(0, 40f, 0);

        //MAINCAM.CameraRender.SetSkyboxMaterial(Mirror);
        MyGameWorld.ActiveCamera.Transform.Position = new Vector3(0, 50, 0);
        var rigid = new RigidBody(1, 1001);
        MAINCAM.AddComponent(rigid);
        Plane.AddObject(MAINCAM);
        Plane.AddComponent(new MyScript());
        Plane.AddComponent(new GenerationScript(MAINCAM));

        Planerigid = rigid;

        Label label = new Label("");
        label.Transform.Position = new Vector2(0, 200);
        label.Font.FontSize += 10f;
        MyGameWorld.GUI.Add(label);

        MAINCAM.AddComponent(new speedomeeter(label, rigid));

        CameraObject Camera2 = new CameraObject( MirrorTexture, false);
        Camera2.Enabled = false;

        MAINCAM.AddObject(Camera2);
        MAINCAM.AddComponent(new EnableMirrorCam(Camera2));
        MAINCAM.Far *= 100;
        Camera2.Far *= 100;
        Camera2.Transform.Rotation = Quaternion.FromEuler(0, 180, 0);

        /*
        {
            RawFile file = new RawFile();
            file.ReadFile(@"C:\Users\Endric\Desktop\filesystem\hello.txt");
            var reader = file.ReadData();
            //Console.WriteLine(reader.ReadString(38));
        }

        {
            RawFile file = new RawFile();
            var writer = file.WriteData();

            Mesh m = Primitves3D.CreateCube();

            writer.WriteInt(m.Triangles.Count);

            foreach (var item in m.Triangles)
            {
                writer.WriteInt(item.P1Index);
                writer.WriteInt(item.P2Index);
                writer.WriteInt(item.P3Index);
            }

            foreach (var item in m.TriangleData)
            {
                writer.WriteFloat(item.P1.X);
                writer.WriteFloat(item.P1.Y);
                writer.WriteFloat(item.P1.Z);

                writer.WriteFloat(item.P2.X);
                writer.WriteFloat(item.P2.Y);
                writer.WriteFloat(item.P2.Z);

                writer.WriteFloat(item.P3.X);
                writer.WriteFloat(item.P3.Y);
                writer.WriteFloat(item.P3.Z);


                writer.WriteFloat(item.UV1.X);
                writer.WriteFloat(item.UV1.Y);

                writer.WriteFloat(item.UV2.X);
                writer.WriteFloat(item.UV2.Y);

                writer.WriteFloat(item.UV3.X);
                writer.WriteFloat(item.UV3.Y);

                writer.WriteFloat(item.Normal.X);
                writer.WriteFloat(item.Normal.Y);
                writer.WriteFloat(item.Normal.Z);
            }

            file.SaveFile(@"C:\Users\Endric\Desktop\filesystem\test.txt");
        }

        {
            RawFile file = new RawFile();
            file.ReadFile(@"C:\Users\Endric\Desktop\filesystem\test.txt");
            var reader = file.ReadData();
            Mesh m = Primitves3D.CreateCube();

            int count = reader.ReadInt();
           // Console.WriteLine("HEADER: ");
            for (int i = 0; i < count; i++)
            {
                int P1Index = reader.ReadInt();
                int P2Index = reader.ReadInt();
                int P3Index = reader.ReadInt();

                //Console.WriteLine("Triangle: " + P1Index + " " + P2Index + " " + P3Index);
            }
           // Console.WriteLine();

            foreach (var item in m.TriangleData)
            {
                Vector3 P1 = new Vector3(
                reader.ReadFloat(),
                reader.ReadFloat(),
                reader.ReadFloat()
                );

                Vector3 P2 = new Vector3(
                reader.ReadFloat(),
                reader.ReadFloat(),
                reader.ReadFloat()
                );

                Vector3 P3 = new Vector3(
                reader.ReadFloat(),
                reader.ReadFloat(),
                reader.ReadFloat()
                );


                Vector2 UV1 = new Vector2(
                reader.ReadFloat(),
                reader.ReadFloat()
                );

                Vector2 UV2 = new Vector2(
               reader.ReadFloat(),
               reader.ReadFloat()
               );

                Vector2 UV3 = new Vector2(
                reader.ReadFloat(),
                reader.ReadFloat()
                );

                Vector3 Normal = new Vector3(
                reader.ReadFloat(),
                reader.ReadFloat(),
                reader.ReadFloat()
                );

                //Console.WriteLine("TRIANGLE: ");
                //Console.WriteLine("     P1: " + P1);
                //Console.WriteLine("     P2: " + P2);
                //Console.WriteLine("     P3: " + P3);
                //
                //Console.WriteLine("     UV1: " + UV1);
                //Console.WriteLine("     UV2: " + UV2);
                //Console.WriteLine("     UV3: " + UV3);
                //
                //Console.WriteLine("     Normal: " + Normal);
            }

            file.SaveFile(@"C:\Users\Endric\Desktop\filesystem\test.txt");
        }
        */

        //var WaveFile = new WaveFile(@"C:\Users\Endric\Desktop\filesystem\Audio.wav");
        // var GIWAudio = WaveFile.ConvertToGIWFormat();

        //for (int i = 0; i < GIWAudio.Samples.Rank; i++)
        //{
        //    string text = "";
        //    Enumerable.Range(0, GIWAudio.Samples.GetLength(1)).Select(x => GIWAudio.Samples[i, x]).ToArray().ToList().ForEach(y => text += Mathf.Abs(y).ToString("0.0") + " ");
        //    MyLog.Default.Write(text);
        //}

        //Console.ReadLine();

        MyGameWorld.SkyboxColor = new RGBColor(0, .1f, 0);


        //var GrassShader = new TextureShader("Grass.png");
        var GrassShader = new SpecularShader("Grass.png");
        GrassShader.Input.ambientColor = new RGBColor(0.15f, 0.15f, 0.15f, 1.0f);
        GrassShader.Input.diffuseColor = new RGBColor(1, 1, 1);
        GrassShader.Input.lightDirection = new Vector3(0.5f, -0.5f, 0.5f);
        GrassShader.Input.specularColor = new RGBColor(1, 1, 1, 1);
        GrassShader.Input.specularPower = 15f;


        //Material Grass = new Material("Grass", new ColorShader(RGBColor.Red) );
        Material Grass = new Material("Grass", GrassShader);


        MyGame.Materials.Add(Grass);







        var BBPLogo = new MeshObject();
        BBPLogo.SetName("BBPLogo");
       // BBPLogo.ExcludedCameras.Add(MAINCAM);
        MyGameWorld.Objects.Add(BBPLogo);
        BBPLogo.Mesh = Mesh.LoadFromObjectFile(@"Z:\Logo.obj", true);
        BBPLogo.Mesh.FlipNormals();
        BBPLogo.Transform.Scale = new Vector3(10f);
        BBPLogo.Transform.Position = new Vector3(0,0, 50);
        BBPLogo.Transform.Rotation = Quaternion.FromEuler(90, 0, 0);
        Camera2.AddObject(BBPLogo);

        //var RedCube = new MeshObject();
        //MyGameWorld.Objects.Add(RedCube);
        //RedCube.Mesh = Primitves3D.CreateCube(MyGame.Materials.Get("DEBUG_Red"), new Vector3(0, 0, 0), new Vector3(10, 25, 10), Quaternion.FromEuler(23, 54, 142));

        //var Green = new MeshObject();
        //MyGameWorld.Objects.Add(Green);
        //Green.Mesh = Primitves3D.CreateCube(MyGame.Materials.Get("DEBUG_Green"), new Vector3(0, 0, 0), new Vector3(25, 16, 10), Quaternion.FromEuler(253, 554, 1642));

        //var BlueCube = new MeshObject();
        //MyGameWorld.Objects.Add(BlueCube);
        //BlueCube.Mesh = Primitves3D.CreateCube(MyGame.Materials.Get("DEBUG_Blue"), new Vector3(0, 0, 0), new Vector3(50, 15, 5), Quaternion.FromEuler(213, 514, 1422));



        //var mirrorCube = new MeshObject();
        //MyGameWorld.Objects.Add(mirrorCube);
        //mirrorCube.Mesh = Primitves3D.CreateCube(Mirror, new Vector3(0, 0, 0), new Vector3(192, 108, 100));
        //mirrorCube.Mesh.FlipNormals();


        var MirrorObj = new MeshObject();
        MyGameWorld.Objects.Add(MirrorObj);
        MirrorObj.Mesh = Mesh.LoadFromObjectFile(@"Z:\Sphere.obj", true);
        MirrorObj.Transform.Scale = new Vector3(50f);

        for (int i = 0; i < MirrorObj.Mesh.Triangles.Count; i++)
        {
            var triangle = MirrorObj.Mesh.Triangles[i];
            triangle.Material = Mirror;
            MirrorObj.Mesh.Triangles[i] = triangle;
        }

        //var MirrorWall = new MeshObject();
        //MyGameWorld.Objects.Add(MirrorWall);
        //MirrorWall.Mesh = Mesh.LoadFromObjectFile("Wall.obj", true);
        //MirrorWall.Transform.Scale = new Vector3(50f);

        //var MatBlack = new Material("testqsdf", new ColorShader(RGBColor.Black));
        //MyGame.Materials.Add(MatBlack);
        //for (int i = 0; i < MirrorWall.Mesh.Triangles.Count; i++)
        //{
        //    var triangle = MirrorWall.Mesh.Triangles[i];
        //    triangle.Material = MatBlack;
        //    triangle.Material = Mirror;
        //    MirrorWall.Mesh.Triangles[i] = triangle;
        //}


        //mirrorSphere.Mesh.FlipNormals();
        //for (int x = 0; x < 100; x++)
        //{
        //    for (int z = 0; z < 100; z++)
        //    {
        //        MeshObject cube = new MeshObject();
        //        cube.Mesh = Primitves3D.CreateCube(MyGame.Materials.Get("Grass"), new Vector3(x * 50, 0, z * 50), new Vector3(50));
        //        MyGameWorld.Objects.Add(cube);
        //    }
        //}



        MyGameWorld.DirectController3D.Attach(Cube);

        button = new Button();
        button.SetName("btn1");
        button.Transform.Size = new Vector2(200, 100);
        button.PositionLayout = PositionLayout.BottomMiddle;
        button.Text = "TEST123\n Zeile2\nZeile3 ist mega geil komm in die Whatsapp Gruppe";
        MyGameWorld.GUI.Add(button);

        Button button2 = new Button();
        button2.SetName("btn2");
        button2.Transform.Position += new Vector2(100, 0);
        button2.Transform.Size = new Vector2(200, 100);
        button2.PositionLayout = PositionLayout.BottomMiddle;
        button.AddChild(button2);


        slx = new Slider();
        slx.Value = 1f;
        slx.Maximum = 1000f;
        sly = new Slider();
        slx.SetName("slX");
        sly.SetName("slY");
        slx.Transform.Size = new Vector2(200, 50);
        sly.Transform.Position = new Vector2(0, 100);
        sly.Transform.Size = new Vector2(200, 50);

        MyGameWorld.GUI.Add(slx);
        MyGameWorld.GUI.Add(sly);

        TextBox txt = new TextBox();
        txt.Transform.Position += new Vector2(100, 0);
        txt.Transform.Size = new Vector2(100, 100);
        txt.PositionLayout = PositionLayout.Center;
        MyGameWorld.GUI.Add(txt);




        cubetest cubetest = new cubetest();
        cubetest.Transform.Size = new Vector2(1920, 1080);
        cubetest.PositionLayout = PositionLayout.Center;
        //MyGameWorld.GUI.Add(cubetest);

        GIWAudioFile File = new WaveFile(@"E:\Downloads\SoundEffect.wav").ConvertToGIWFormat();
        audio = new Audio(File);


        //Oscilliscope oscilliscope = new Oscilliscope();
        //oscilliscope.Transform.Size = new Vector2(1920, 1080);
        //oscilliscope.PositionLayout = PositionLayout.Center;
        //MyGameWorld.GUI.Add(oscilliscope);


        RotObject = new GUIObject();
        RotObject.Transform.Size = new Vector2(300, 100);
        RotObject.Transform.Position = new Vector2(500,500);
        RotObject.RotationLayout = RotationLayout.Center;
        MyGameWorld.GUI.Add(RotObject);

        MyGame.Start();

        MyLog.Default.Write("PROGRAMM ENDED!", LogSeverity.Info);
    }

    public class Oscilliscope : RenderCanvas
    {
        PylonGameEngine.UI.Drawing.Pen p;
        float[] SamplesLeft;
        float[] SamplesRight;
        int Offset = 0;
        Audio AudioPlayer;
        List<List<Vector2>> OldFrames = new List<List<Vector2>>();

        public Oscilliscope()
        {
            WaveFile waveFile = new WaveFile(@"E:\Downloads\Track 5.wav"); //new WaveFile(@"E:\Downloads\yt5s.com - Jerobeam Fenderson - Shrooms (128 kbps).wav");
            
            var giwFile = waveFile.ConvertToGIWFormat();
            SamplesLeft = Enumerable.Range(0, giwFile.Samples.GetLength(0)).Select(x => giwFile.Samples[x, 0]).ToArray();
            SamplesRight = Enumerable.Range(0, giwFile.Samples.GetLength(0)).Select(x => giwFile.Samples[x, 1]).ToArray();
            AudioPlayer = new Audio(giwFile.SampleRate, giwFile.ChannelCount);
            AudioPlayer.Loop = true;
            AudioPlayer.SubmitBuffer(giwFile);
            AudioPlayer.Play();
            AudioPlayer.Volume = 0.5f;
        }

        public override void OnDraw(PylonGameEngine.UI.Drawing.Graphics g)
        {
            lock (MyGame.RenderLock)
            {
                g.Clear(RGBColor.Black);
                if (p == null)
                {
                    p = g.CreatePen(RGBColor.Green);
                }

                p.Color = RGBColor.Green;

                int readlength = (int)(48000 * MyGame.RenderLoop.DeltaTime);
                Offset = (int)AudioPlayer.SamplesPlayed;

                List<Vector2> Points = new List<Vector2>();

                for (int i = 0; i < readlength; i++)
                {
                    if(i + Offset < SamplesLeft.Length)
                    {
                        Vector2 P = new Vector2(SamplesLeft[i + Offset], -SamplesRight[i + Offset]) * 500f;
                        P += new Vector2(1920 / 2, 1080 / 2);
                        Points.Add(P);
                    }
                }
                if(Points.Count > 2)
                {
                    for (int i = 0; i < Points.Count - 1; i++)
                    {
                        //if(i % 100 == 33)
                        //{
                        //    p.Color = RGBColor.Red;
                        //} 
                        //else if(i % 100 == 66)
                        //{
                        //    p.Color = RGBColor.Green;
                        //}
                        //else if(i % 100 == 99)
                        //{
                        //    p.Color = RGBColor.Blue;
                        //}
                        g.DrawLine(p, Points[i], Points[i + 1]);
                    }
                    //g.DrawGeometry(p, Points.ToArray());
                }
                 

                if(OldFrames.Count > 10)
                {
                    OldFrames.RemoveAt(0);
                }

                for (int i = 0; i < OldFrames.Count; i++)
                {
                    var frame = OldFrames[i];
                    p.Color = new RGBColor(p.Color.R, p.Color.G, p.Color.B, ((float)OldFrames.Count / (float)i - 1f) / 2f);
                    if (frame.Count > 2)
                        g.DrawGeometry(p, frame.ToArray());
                }

                //  OldFrames.Add(Points);

                //readlength *= 2;
                //var leftSamples = Enumerable.Range(Offset, readlength).Select(x => SamplesLeft[x]).ToArray();
                //var rightSamples = Enumerable.Range(Offset, readlength).Select(x => SamplesRight[x]).ToArray();

                //AudioPlayer.SubmitBuffer(leftSamples, rightSamples);
                // AudioPlayer.Play();
                //  Console.WriteLine(AudioPlayer.SamplesPlayed);
                Offset += readlength;
                QueueDraw();
            }
        }
    }

    public class cubetest : RenderCanvas
    {
        IXAudio2SourceVoice voice;
        List<Vector2> Values;
        PylonGameEngine.UI.Drawing.Pen p;
        public cubetest()
        {
            voice = AudioEngine.Engine.CreateSourceVoice(new Vortice.Multimedia.WaveFormat(48000, 16, 2));
            Values = new List<Vector2>();
        }

        public override void OnDraw(PylonGameEngine.UI.Drawing.Graphics g)
        {
            g.Clear();

            int Offset = 1;

            if (p == null)
                p = g.CreatePen(RGBColor.Green);
            for (int FirstIndex = 0; FirstIndex < Values.Count; FirstIndex++)
            {
                int SecondIndex;
                if (FirstIndex == Values.Count - 1)
                    SecondIndex = 0;
                else
                    SecondIndex = FirstIndex + 1;
                g.DrawLine(p, (Values[FirstIndex] * Offset) + new Vector2(1920 / 2, 1080 / 2), (Values[SecondIndex] * Offset) + new Vector2(1920 / 2, 1080 / 2));
                // g.FillRectangle(p.ToSolidBrush(), (Values[FirstIndex] * Offset) + new Vector2(1920 / 2, 1080 / 2), new Vector2(1,1));
            }

            if (Values.Count > 1920 / Offset)
            {
                Values.RemoveRange(0, Values.Count - 1920 / Offset);
            }

            voice.Stop();
            voice.FlushSourceBuffers();

            //var WaveFile = new FileSystem.Filetypes.WAVE.WaveFile(@"C:\Users\Endric\Desktop\filesystem\Audio.wav");
            //var WaveFile = new PylonGameEngine.FileSystem.Filetypes.WAVE.WaveFile(@"C:\Users\Endric\Desktop\filesystem\Xmas song.wav");
            float[] Samples = new float[(int)(48000 * (1 / 60f))];

            int FrameLength = (int)(5f * 48000);

            List<Vector3> PointsList = new List<Vector3>();
            PointsList.Add(new Vector3(0, 0, 0));
            PointsList.Add(new Vector3(1, 0, 0));
            Vector3[] Points = Primitves3D.CreateCube().Points.ToArray();
            //  Vector3[] Points = PointsList.ToArray();//Primitves3D.CreateCube().Points.ToArray();
            Vector2[] Points2D = new Vector2[Points.Length];
            Vector2[] Points2DScreen = new Vector2[Points.Length];

            for (int i = 0; i < Samples.Length; i++)
            {

                if (i % (48000 / 2) == 0)
                {
                    // MyGameWorld.ActiveCamera.Transform.Rotation *= Quaternion.FromEuler(new Vector3(0.1f, 0.0f, 0f));
                    for (int x = 0; x < Points2D.Length; x++)
                    {
                        Points2D[x] = (Vector2)(MyGameWorld.ActiveCamera.ViewMatrix3D * MyGameWorld.ActiveCamera.ProjectionMatrix * Points[x]) * 10f;
                        Points2DScreen[x] = Points2D[x] * 10f + new Vector2(1920 / 2, 1080 / 2);
                    }
                }
                // g.DrawLine(g.CreatePen(RGBColor.Blue), Points2DScreen[0], Points2DScreen[1]);
                g.DrawGeometry(g.CreatePen(RGBColor.Blue), Points2DScreen, false);

                int CurrentPoint = (i / FrameLength) % Points2D.Length;
                int SecondPoint;

                if (CurrentPoint == Points2D.Length - 1)
                    SecondPoint = 0;
                else
                    SecondPoint = CurrentPoint + 1;

                Vector2 P1 = Points2D[CurrentPoint];
                Vector2 P2 = Points2D[SecondPoint];
                //g.DrawLine(g.CreatePen(RGBColor.Blue), P1 * 100f + new Vector2(1920 / 2, 1080 / 2), P2 * 100f + new Vector2(1920 / 2, 1080 / 2));

                //g.FillRectangle(g.CreateSolidBrush(RGBColor.Blue), new Vector2(Mathf.Lerp(P1.X, P2.X, (i % FrameLength)), Mathf.Lerp(P1.Y, P2.Y, (i % FrameLength))) + new Vector2(1920 / 2, 1080 / 2), new Vector2(1,1));

                if (i % 2 == 0)
                    Samples[i] = Mathf.Lerp(P1.X, P2.X, (i % FrameLength));
                else
                {
                    Samples[i] = Mathf.Lerp(P1.Y, P2.Y, (i % FrameLength));
                    //Values.Add(new Vector2(Samples[i - 1], Samples[i]));
                    Values.Add(new Vector2(P1.X, P1.Y));
                    Values.Add(new Vector2(P2.X, P2.Y) * 1000f);


                }

                g.DrawLine(g.CreatePen(RGBColor.Red), P1 * 10f + new Vector2(1920 / 2, 1080 / 2), P2 * 10f + new Vector2(1920 / 2, 1080 / 2));
            }

            var Bytes = new byte[Samples.Length * 2];

            for (int i = 0; i < Samples.Length; i++)
            {
                var bytes = BitConverter.GetBytes((short)(Samples[i] * short.MaxValue));

                int offset = i * 2;
                Bytes[offset] = bytes[0];
                Bytes[offset + 1] = bytes[1];
            }

            if (Bytes.Length != 0)
            {
                var Audiobuffer = new AudioBuffer(Bytes, BufferFlags.EndOfStream);

                voice.SubmitSourceBuffer(Audiobuffer);
            }


            QueueDraw();
        }
    }

    public static Random r = new Random(1337);
    //public static void CreateCollisionCube(Vector3 Position)
    //{
    //    MeshObject Cube = new MeshObject();
    //    Cube.Mesh = Primitves3D.CreateCube(new Vector3(1, 1, 1), 0);
    //    MyGameWorld.Objects.Add(Cube);
    //    Cube.Transform.Position = Position;
    //    var rigid = new RigidBody(Cube.Mesh.TriangleData);
    //    Cube.AddComponent(rigid);
    //    //  rigid.SetVelocity(new Vector3(rng, rng, rng), new Vector3(rng, rng, rng));

    //}

    public static float rng
    {
        get
        {
            return (float)r.Next(-100, 100);
        }
    }


    public class speedomeeter : GameScript
    {
        Label Label;
        RigidBody RigidBody;
        string fps = "";
        public speedomeeter(Label label, RigidBody rigid)
        {
            Label = label;
            RigidBody = rigid;
        }

        public override void UpdateTick()
        {
            string ms = Vector3.FromSystemNumerics(RigidBody.Body.Velocity.Linear).Abs().ToString("0.00");
            string kmh = (Vector3.FromSystemNumerics(RigidBody.Body.Velocity.Linear).Abs() * 3.6f).ToString("0.00");
            
            if(MyGame.GameTickLoop.Frames % 60 == 0)
            {
                fps = (1f / MyGame.RenderLoop.DeltaTime).ToString("0");
            }

            Label.Text = ms + @" m/s" + "\n" +
                          kmh + @" km/h" + "\n" +
                          fps + "FPS";

        }
    }
}

