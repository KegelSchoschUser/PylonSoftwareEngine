/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

namespace PylonSoftwareEngine.Utilities
{
    public static class Utilities
    {
        public static void Swap<T>(ref T left, ref T right)
        {
            T Buffer = left;
            left = right;
            right = Buffer;
        }
    }
}
