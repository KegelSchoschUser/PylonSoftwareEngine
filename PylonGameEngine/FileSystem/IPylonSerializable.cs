using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PylonGameEngine.FileSystem
{
    public interface IPylonSerializable
    {
        public void Serialize(DataWriter writer);
        public dynamic DeSerialize(DataReader reader);
    }
}
