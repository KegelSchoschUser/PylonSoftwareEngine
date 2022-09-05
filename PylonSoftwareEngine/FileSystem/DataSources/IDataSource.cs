/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PylonSoftwareEngine.FileSystem.DataSources
{
    public interface IDataSource : IDisposable
    {
        public byte[] ReadBytes(int length);

        public void WriteBytes(byte[] bytes);

        public void MovePosition(int offset);

        public void Close();
    }
}
