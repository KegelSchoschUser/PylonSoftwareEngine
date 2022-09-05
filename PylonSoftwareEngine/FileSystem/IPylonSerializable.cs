/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

namespace PylonSoftwareEngine.FileSystem
{
    public interface IPylonSerializable
    {
        public bool Serialize(DataWriter writer);
        public bool DeSerialize(DataReader reader);
    }
}
