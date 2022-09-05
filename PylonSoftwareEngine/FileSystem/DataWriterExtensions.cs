/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

using PylonSoftwareEngine.Mathematics;

namespace PylonSoftwareEngine.FileSystem
{
    public static class DataWriterExtensions
    {
        public static void WriteRGBColor(this DataWriter writer, RGBColor value)
        {
            writer.WriteFloat(value.R);
            writer.WriteFloat(value.G);
            writer.WriteFloat(value.B);
            writer.WriteFloat(value.A);
        }
    }
}
