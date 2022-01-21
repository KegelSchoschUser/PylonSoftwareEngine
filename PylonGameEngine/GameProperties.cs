using PylonGameEngine.Mathematics;
using PylonGameEngine.Utils;
using System.Drawing;

namespace PylonGameEngine
{
    public static class GameProperties
    {
        public static string GameName = "My Game";
        public static MyVersion Version = new MyVersion(0, 0, 0);

        public static Size StartWindowSize = new System.Drawing.Size(1920, 1080);

        public static int GameTickRate = 60;
        public static int RenderTickRate = -1;
        public static SplashScreen SplashScreen = new SplashScreen(null, false);

        public static string Roaming => MyFileSystem.ROAMING + @"\" + GameName;
    }
}
