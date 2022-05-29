using PylonGameEngine.FileSystem.DataSources;
using PylonGameEngine.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PylonGameEngine.Networking
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

        public virtual void OnPacketReceived(DataReader dataReader, string Id) { }
    }
}
