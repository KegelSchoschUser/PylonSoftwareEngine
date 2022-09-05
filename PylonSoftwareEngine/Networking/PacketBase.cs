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
