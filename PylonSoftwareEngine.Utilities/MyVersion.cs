/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

namespace PylonSoftwareEngine.Utilities
{
    public class MyVersion
    {
        public int Major { get; private set; }
        public int Minor { get; private set; }
        public int Patch { get; private set; }
        public int Build { get; private set; }

        public MyVersion(int major, int minor, int minorbuild, int build = -1)
        {
            Major = major;
            Minor = minor;
            Patch = minorbuild;
            Build = build;
        }

        public override string ToString()
        {
            return Major + "." + Minor + "." + Patch + ":" + Build;
        }
    }
}
