using PylonGameEngine.Audio;
using PylonGameEngine.Input;
using PylonGameEngine.Interpolation;
using PylonGameEngine.Physics;
using PylonGameEngine.Render11;
using PylonGameEngine.SceneManagement;
using PylonGameEngine.Utilities;
using PylonGameEngine.Utilities.Win32;
using PylonGameEngine.Networking;
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
        public static object RenderLock = new object();
        public static LockedList<Material> Materials = new LockedList<Material>(ref RenderLock);
        public static bool RendererEnabled = true;
        internal static Utilities.DiscordRPC RPC;

        [STAThread]
        public static void Initialize()
        {
            MyLog.Default = new MyLog(GameProperties.Roaming, GameProperties.GameName, GameProperties.Version.ToString());
            InitializeCrashLog();

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

            if(GameProperties.UseAudioEngine)
                AudioEngine.Initialize();


            Touchscreen.DisableWPFTabletSupport();

            Initialized = true;
            MyLog.Default.Write("Game Initialized!");
        }

        #region InitializeCrashLog
        private static void InitializeCrashLog()
        {
            AppDomain currentDomain = default(AppDomain);
            currentDomain = AppDomain.CurrentDomain;
            // Handler for unhandled exceptions.
            currentDomain.UnhandledException += GlobalUnhandledExceptionHandler;
            // Handler for exceptions in threads behind forms.
            System.Windows.Forms.Application.ThreadException += GlobalThreadExceptionHandler;
        }

        private static void GlobalUnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = default(Exception);
            ex = (Exception)e.ExceptionObject;
            MyLog.Default.Write(ex, LogSeverity.Crash);
        }

        private static void GlobalThreadExceptionHandler(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Exception ex = default(Exception);
            ex = e.Exception;
            MyLog.Default.Write(ex, LogSeverity.Crash);
        }
        #endregion InitializeCrashLog


        public static void Start()
        {
            if (!Initialized)
            {
                throw new MyExceptions.EngineNotInitializedException();
            }

            RenderLoop.Starting += () => { MyLog.Default.Write("Game Started!"); };
            GameProperties.SplashScreen.Close();

            GameTickLoop.Start();
            RenderLoop.Start(false);
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

            Environment.Exit(0);
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

            SceneManager.UpdateTick();
            //GC.Collect();
        }

        [STAThread]
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

            SceneManager.UpdateFrame();

            //RPC.Update();
        }

        public static void ShowConsole()
        {
            ShowWindow(GetConsoleWindow(), ShowWindowCommand.Show);
        }

        public static void HideConsole()
        {
            ShowWindow(GetConsoleWindow(), ShowWindowCommand.Hide);
        }
    }
}
