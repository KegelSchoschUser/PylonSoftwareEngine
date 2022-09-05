using PylonSoftwareEngine.Audio;
using PylonSoftwareEngine.Input;
using PylonSoftwareEngine.Interpolation;
using PylonSoftwareEngine.Physics;
using PylonSoftwareEngine.Render11;
using PylonSoftwareEngine.SceneManagement;
using PylonSoftwareEngine.Utilities;
using PylonSoftwareEngine.Utilities.Win32;
using PylonSoftwareEngine.Networking;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static PylonSoftwareEngine.Utilities.Win32.User32;

namespace PylonSoftwareEngine
{
    public static class MySoftware
    {
        public static bool Initialized { get; private set; }

        public static List<Window> Windows = new List<Window>();
        public static SoftwareLoop SoftwareTickLoop;
        public static SoftwareLoop RenderLoop;
        public static object RenderLock = new object();
        public static LockedList<Material> Materials = new LockedList<Material>(ref RenderLock);
        public static bool RendererEnabled = true;
        internal static Utilities.DiscordRPC RPC;

        [STAThread]
        public static void Initialize()
        {
            MyLog.Default = new MyLog(SoftwareProperties.Roaming, SoftwareProperties.SoftwareName, SoftwareProperties.Version.ToString());
            InitializeCrashLog();

            SetProcessDPIAware();
            SoftwareProperties.SplashScreen.ShowAsync();

            //"925124183044792341"
            RPC = new Utilities.DiscordRPC("925124183044792341", SoftwareProperties.SoftwareName);

            RenderLoop = new SoftwareLoop(SoftwareProperties.RenderTickRate, "RenderLoop");
            RenderLoop.Tick += RenderLoop_Tick;

            SoftwareTickLoop = new SoftwareLoop(SoftwareProperties.SoftwareTickRate, "SoftwareTickLoop");
            SoftwareTickLoop.Tick += SoftwareTickLoop_Tick;

            D3D11GraphicsDevice.INIT();
            StandardResources.AddResources();

            if(SoftwareProperties.UseAudioEngine)
                AudioEngine.Initialize();


            Touchscreen.DisableWPFTabletSupport();

            Initialized = true;
            MyLog.Default.Write("Software Initialized!");
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

            RenderLoop.Starting += () => { MyLog.Default.Write("Software Started!"); };
            SoftwareProperties.SplashScreen.Close();

            SoftwareTickLoop.Start();
            RenderLoop.Start(false);
        }

        public static void Stop()
        {
            Mouse.UnlockMouse();


            Window[] windows = new Window[Windows.Count];
            Windows.CopyTo(windows);
            foreach (var window in windows)
                window.Destroy();


            SoftwareTickLoop.Stop();
            RenderLoop.Stop();

            Environment.Exit(0);
        }

        private static void SoftwareTickLoop_Tick()
        {
            Input.Touchscreen.Cycle();


            lock (MySoftware.RenderLock)
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
            lock (MySoftware.RenderLock)
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
