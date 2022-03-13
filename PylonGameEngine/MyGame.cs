using PylonGameEngine.Audio;
using PylonGameEngine.Input;
using PylonGameEngine.Interpolation;
using PylonGameEngine.Physics;
using PylonGameEngine.Render11;
using PylonGameEngine.SceneManagement;
using PylonGameEngine.Utilities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static PylonGameEngine.Utilities.Win32.User32;

namespace PylonGameEngine
{
    public static class MyGame
    {
        public static bool Initialized { get; private set; }

        public static List<Window> Windows = new List<Window>();
        public static GameLoop GameTickLoop;
        public static GameLoop RenderLoop;
        public static GameLoop PhysicsLoop;
        public static object RenderLock = new object();
        public static LockedList<Material> Materials = new LockedList<Material>(ref RenderLock);
        internal static bool RendererEnabled = true;
        internal static Utilities.DiscordRPC RPC;

        [STAThread]
        public static void Initialize()
        {
            MyLog.Default = new MyLog(GameProperties.Roaming, GameProperties.GameName, GameProperties.Version.ToString());

            SetProcessDPIAware();
            GameProperties.SplashScreen.ShowAsync();

            //"925124183044792341"
            RPC = new Utilities.DiscordRPC("925124183044792341", GameProperties.GameName);

            RenderLoop = new GameLoop(GameProperties.RenderTickRate, "RenderLoop");
            RenderLoop.Tick += RenderLoop_Tick;

            GameTickLoop = new GameLoop(GameProperties.GameTickRate, "GameTickLoop");
            GameTickLoop.Tick += GameTickLoop_Tick;

            StandardResources.AddResources();
            D3D11GraphicsDevice.INIT();

            MyPhysics.Initialize();
            AudioEngine.Initialize();
            Touchscreen.DisableWPFTabletSupport();

            Initialized = true;
            MyLog.Default.Write("Game Initialized!");
        }



        public static void Start()
        {
            if (!Initialized)
            {
                throw new MyExceptions.EngineNotInitializedException();
            }

            RenderLoop.Starting += () => { MyLog.Default.Write("Game Started!"); };
            GameProperties.SplashScreen.Close();

            GameTickLoop.Start();
            RenderLoop.Start();
        }

        public static void Stop()
        {
            Mouse.UnlockMouse();


            Window[] windows = new Window[Windows.Count];
            Windows.CopyTo(windows);
            foreach (var window in windows)
                window.Destroy();


            GameTickLoop.Stop();
            RenderLoop.Stop();
        }

        private static void GameTickLoop_Tick()
        {
            Input.Touchscreen.Cycle();


            lock (MyGame.RenderLock)
            {
                for (int i = 0; i < Interpolator.Interpolators.Count; i++)
                {
                    Interpolator.Interpolators[i].UpdateTick();
                }

            }

            SceneManager.UpdateFrame();
            //GC.Collect();
        }

        private unsafe static void RenderLoop_Tick()
        {
            Application.DoEvents();
            lock (MyGame.RenderLock)
            {
                for (int i = 0; i < Interpolator.Interpolators.Count; i++)
                {
                    Interpolator.Interpolators[i].UpdateFrame();
                }
            }

            SceneManager.UpdateTick();

            //RPC.Update();
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
