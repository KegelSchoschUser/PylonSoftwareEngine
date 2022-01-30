using PylonGameEngine.Mathematics;
using PylonGameEngine.Utilities;
using System.Drawing;

namespace PylonGameEngine
{
    public static class GameProperties
    {
        public static string GameName = "My Game";
        public static MyVersion Version = new MyVersion(0, 0, 0);
        
        public static Vector2 StartWindowSize = new Vector2(1920, 1080);
        public static Vector2 StartWindowPosition = new Vector2(0, 0);

        public static int GameTickRate = 60;
        public static int RenderTickRate = -1;
        public static SplashScreen SplashScreen = new SplashScreen(null, false);

        public static bool FullScreen = false;
        public static bool Titlebar = true;

        public static string Roaming => MyFileSystem.ROAMING + @"\" + GameName;
    }
}
