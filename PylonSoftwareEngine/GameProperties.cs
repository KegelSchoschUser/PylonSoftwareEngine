using PylonSoftwareEngine.Utilities;

namespace PylonSoftwareEngine
{
    public static class SoftwareProperties
    {
        public static string SoftwareName = "My Software";
        public static MyVersion Version = new MyVersion(0, 0, 0);

        public static int SoftwareTickRate = 60;
        public static int RenderTickRate = -1;
        public static SplashScreen SplashScreen = new SplashScreen(null, false);

        public static string Roaming => MyFileSystem.ROAMING + @"\" + SoftwareName;

        public static bool UseAudioEngine = true;
    }
}
