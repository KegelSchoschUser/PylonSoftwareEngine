using PylonGameEngine.Utilities;

namespace PylonGameEngine
{
    public static class GameProperties
    {
        public static string GameName = "My Game";
        public static MyVersion Version = new MyVersion(0, 0, 0);

        public static int GameTickRate = 60;
        public static int RenderTickRate = -1;
        public static SplashScreen SplashScreen = new SplashScreen(null, false);

        public static string Roaming => MyFileSystem.ROAMING + @"\" + GameName;
    }
}
