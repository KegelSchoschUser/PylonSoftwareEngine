/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

using System;

namespace PylonSoftwareEngine.Utilities
{
    public static class MyFileSystem
    {
        public static string ROAMING => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    }
}
