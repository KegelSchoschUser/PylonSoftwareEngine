/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

namespace PylonSoftwareEngine
{
    public static class DebugSettings
    {
        public static class UISettings
        {
            private static bool _DrawLayoutRectangle = false;












            public static bool DrawLayoutRectangle
            {
                get { return _DrawLayoutRectangle; }
                set
                {
                    _DrawLayoutRectangle = value;
                    foreach (var scene in SceneManagement.SceneManager.Scenes)
                    {
                        scene.Gui.RefreshALL();
                    }
                }
            }
        }

        public static class CharacterSettings
        {
            public static bool ShowDebug = false;
        }
    }
}
