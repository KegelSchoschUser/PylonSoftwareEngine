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
