/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

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
