/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

using PylonSoftwareEngine.Mathematics;

namespace PylonSoftwareEngine.FileSystem
{
    public static class DataReaderExtensions
    {
        public static RGBColor ReadRGBColor(this DataReader reader)
        {
            RGBColor color = new RGBColor();
            color.R = reader.ReadFloat();
            color.G = reader.ReadFloat();
            color.B = reader.ReadFloat();
            color.A = reader.ReadFloat();

            return color;
        }
    }
}
