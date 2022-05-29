using PylonGameEngine;
using PylonGameEngine.GUI.GUIObjects;
using PylonGameEngine.Input;
using PylonGameEngine.Mathematics;
using PylonGameEngine.Networking;
using PylonGameEngine.Render11;
using PylonGameEngine.SceneManagement;
using PylonGameEngine.SceneManagement.Objects;
using PylonGameEngine.Utilities;
using System;
using System.Drawing;
using System.Text;
using System.Threading;

namespace MyTestGame
{
    public class FlyScript : GameScript
    {
        public static bool Enabled = false;
        public override void UpdateTick()
        {
            if (Scene.InputManager.Keyboard.KeyPressed(PylonGameEngine.Input.KeyboardKey.Backspace))
                MyGame.Stop();

            if (Scene.InputManager.Keyboard.KeyDown(PylonGameEngine.Input.KeyboardKey.G))
            {
                if (Mouse.CursorLocked)
                    Mouse.UnlockMouse();
                else
                    Mouse.LockMouse(Scene.InputManager.Window.GlobalRectangle);
            }

            if (Enabled)
                return;
            if (Scene.InputManager.Keyboard.KeyPressed(PylonGameEngine.Input.KeyboardKey.W))
            {
                Parent.Transform.Position += Parent.Transform.Forward * 5f * FixedDeltaTime;
            }

            if (Scene.InputManager.Keyboard.KeyPressed(PylonGameEngine.Input.KeyboardKey.S))
                Parent.Transform.Position += Parent.Transform.Backward * 5f * FixedDeltaTime;

            if (Scene.InputManager.Keyboard.KeyPressed(PylonGameEngine.Input.KeyboardKey.A))
                Parent.Transform.Position += Parent.Transform.Left * 5f * FixedDeltaTime;

            if (Scene.InputManager.Keyboard.KeyPressed(PylonGameEngine.Input.KeyboardKey.D))
                Parent.Transform.Position += Parent.Transform.Right * 5f * FixedDeltaTime;

            if (Scene.InputManager.Keyboard.KeyPressed(PylonGameEngine.Input.KeyboardKey.Space))
                Parent.Transform.Position += Parent.Transform.Up * 5f * FixedDeltaTime;

            if (Scene.InputManager.Keyboard.KeyPressed(PylonGameEngine.Input.KeyboardKey.C))
                Parent.Transform.Position += Parent.Transform.Down * 5f * FixedDeltaTime;

            if (Scene.InputManager.Keyboard.KeyPressed(PylonGameEngine.Input.KeyboardKey.Q))
                Parent.Transform.Rotation *= Quaternion.FromEuler(0, 0, 2);

            if (Scene.InputManager.Keyboard.KeyPressed(PylonGameEngine.Input.KeyboardKey.E))
                Parent.Transform.Rotation *= Quaternion.FromEuler(0, 0, -2);

            if (Mouse.CursorLocked)
                Parent.Transform.Rotation *= Quaternion.FromEuler(Scene.InputManager.Mouse.Delta.Y / 4f, Scene.InputManager.Mouse.Delta.X / 4f, 0);
        }
    }
    public class CubeRotator : GameScript
    {
        public static Random r = new Random((int)(DateTime.Now.Ticks / 69));
        public override void UpdateFrame()
        {
            Parent.Transform.Rotation *= Quaternion.FromEuler(new Vector3((float)r.NextDouble() * 5f, (float)r.NextDouble() * 5f, (float)r.NextDouble() * 5f));
        }
    }

    public class LookAtScript : GameScript
    {

        private GameObject3D LookatObject;
        public LookAtScript(GameObject3D lookatObject)
        {
            LookatObject = lookatObject;
        }
        public override void UpdateFrame()
        {
            Parent.Transform.Rotation = Quaternion.LookAt(Parent.Transform.GlobalPosition, LookatObject.Transform.GlobalPosition);
        }
    }

    public class Program
    {
        static Random r = new Random((int)(DateTime.Now.Ticks));
        [STAThread]
        public static void Main(string[] args)
        {
            GameProperties.GameName = "GIW TEST";
            GameProperties.Version = new MyVersion(1, 0, 0);
            GameProperties.SplashScreen = new SplashScreen((Bitmap)Bitmap.FromFile("Splash.png"));
            GameProperties.RenderTickRate = 60;
            MyGame.Initialize();

            MyLog.Default.Write("test", LogSeverity.Info);
            MyLog.Default.Write("test", LogSeverity.Warning);
            MyLog.Default.Write("test", LogSeverity.Error);
            MyLog.Default.Write("test", LogSeverity.Critical);
            MyLog.Default.Write("test", LogSeverity.Crash);

            for (int x = 0; x < 2; x++)
            {
                Scene Scene1 = new Scene();
                SceneManager.AddScene(Scene1);
                Window MainWindow = Window.CreateWindow("Scene " + x, Vector2.Zero, new Vector2(1000, 1000), false, true);
                Scene1.SetInputWindow(MainWindow);

                WindowRenderTarget MainWindowRenderTarget = new WindowRenderTarget(MainWindow);

                Camera MainCamera = new Camera(MainWindowRenderTarget);
                Scene1.Add(MainCamera);
                MainCamera.AddComponent(new FlyScript());

                MeshObject Cube = new MeshObject();
                Scene1.Add(Cube);
                Cube.Mesh = Primitves3D.CreateCube(MyGame.Materials.Get("DEBUG_Red"), Vector3.Zero, new Vector3(1, 5, 1));
                Cube.Transform.Position = new Vector3(0, 0, 5);
                Cube.AddComponent(new CubeRotator());

                MeshObject blueCube = new MeshObject();
                Scene1.Add(blueCube);
                blueCube.Mesh = Primitves3D.CreateCube(MyGame.Materials.Get("DEBUG_Blue"));
                MainCamera.AddObject(blueCube);

                Button flySwitch = new Button();
                Scene1.Gui.Add(flySwitch);
                flySwitch.Text = "FlyMode: OFF";
                flySwitch.OnClick += (x) => { FlyScript.Enabled = !FlyScript.Enabled; x.Text = "FlyMode: " + (FlyScript.Enabled ? "OFF" : "ON"); };


                for (int i = 0; i < 1; i++)
                {
                    Window Window2 = Window.CreateWindow("Test2", Vector2.Zero, new Vector2(500, 500), false, true);
                    WindowRenderTarget WindowRenderTarget2 = new WindowRenderTarget(Window2);
                    Camera Camera2 = new Camera(WindowRenderTarget2);
                    Scene1.Add(Camera2);
                    Vector3 eulerangles = new Vector3(CubeRotator.r.Next(-180, 180), CubeRotator.r.Next(-180, 180), CubeRotator.r.Next(-180, 180));
                    Camera2.Transform.Rotation = Quaternion.FromEuler(eulerangles);
                    Camera2.Transform.Position = Camera2.Transform.Forward * 20f;
                    Camera2.Transform.Rotation = Quaternion.LookAt(Camera2.Transform.Position, Vector3.Zero);
                    //Camera2.AddComponent(new LookAtScript(MainCamera));

                    MeshObject greenCube = new MeshObject();
                    Scene1.Add(greenCube);
                    greenCube.Mesh = Primitves3D.CreateCube(MyGame.Materials.Get("DEBUG_Green"));
                    Camera2.AddObject(greenCube);
                }
            }

            MyGame.Start();
        }
    }
}