using PylonGameEngine.Audio;
using PylonGameEngine.Physics;
using PylonGameEngine.Render11;
using PylonGameEngine.Utils;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace PylonGameEngine
{
    public static class MyGame
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public static bool Initialized { get; private set; }

        public static GameLoop GameTickLoop;
        public static GameLoop RenderLoop;
        public static GameLoop PhysicsLoop;
        public static Window MainWindow;
        public static object RenderLock = new object();
        public static LockedList<Material> Materials = new LockedList<Material>(ref RenderLock);
        internal static bool RendererEnabled = true;
        internal static Unlicensed UnlicensedObject;
        internal static Utils.DiscordRPC RPC;

        public static void Initialize()
        {
            MyLog.Default = new MyLog(GameProperties.Roaming, GameProperties.GameName, GameProperties.Version.ToString());

            GameProperties.SplashScreen.ShowAsync();

            //"925124183044792341"
            RPC = new Utils.DiscordRPC("925124183044792341", GameProperties.GameName);
            MainWindow = new Window(GameProperties.GameName, new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), GameProperties.StartWindowSize), "PylonGameEngine");
            MainWindow.PlatformConstruct();

            RenderLoop = new GameLoop(GameProperties.RenderTickRate);
            RenderLoop.Tick += RenderLoop_Tick;

            GameTickLoop = new GameLoop(GameProperties.GameTickRate);
            GameTickLoop.Tick += GameTickLoop_Tick;

            StandardResources.AddResources();
            D3D11GraphicsDevice.INIT();

            UnlicensedObject = new Unlicensed();
            UnlicensedObject.Transform.Size = new Mathematics.Vector2(MainWindow.Size.Width, MainWindow.Size.Height);
            MyPhysics.Initialize();
            AudioEngine.Initialize();

            Initialized = true;
            MyLog.Default.Write("Game Initialized!");
        }



        public static void Start()
        {
            if (!Initialized)
            {
                throw new MyExceptions.EngineNotInitializedException();
            }

            GameTickLoop.Start();

            RenderLoop.Starting += () => { MyLog.Default.Write("Game Started!"); };

            GameProperties.SplashScreen.Close();
            RenderLoop.Start(false);
        }

        public static void Stop()
        {
            GameTickLoop.Stop();
            RenderLoop.Stop();
            MainWindow.Destroy();
        }

        private static void GameTickLoop_Tick()
        {
            //GC.Collect();
        }

        private static void RenderLoop_Tick()
        {
            Application.DoEvents();
            Input.Keyboard.Cycle();
            Input.Mouse.Cycle();
            RPC.Update();
        }

        public static void ShowConsole()
        {
            ShowWindow(GetConsoleWindow(), 5);
        }

        public static void HideConsole()
        {
            ShowWindow(GetConsoleWindow(), 0);
        }
    }
}
