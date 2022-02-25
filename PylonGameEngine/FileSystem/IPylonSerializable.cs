using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PylonGameEngine.FileSystem
{
    public interface IPylonSerializable
    {
        public bool Serialize(DataWriter writer);
        public bool DeSerialize(DataReader reader);
    }
}
