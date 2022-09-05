/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

using PylonSoftwareEngine.FileSystem.DataSources;
using PylonSoftwareEngine.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PylonSoftwareEngine.Networking
{
    public class PacketBase
    {
        internal ByteArraySource Data;
        public DataWriter DataWriter;
        
        public PacketBase()
        {
            Data = new ByteArraySource();
            DataWriter = new DataWriter(Data);
        }

        public virtual void OnPacketReceived(DataReader dataReader, int Id) { }
    }
}
