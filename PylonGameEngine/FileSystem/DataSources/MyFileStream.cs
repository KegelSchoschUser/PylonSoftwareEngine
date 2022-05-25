using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PylonGameEngine.FileSystem.DataSources
{
    public class MyFileStream : IDataSource
    {
        internal FileStream FileStream;

        public MyFileStream(string FileName)
        {
            FileStream = new FileStream(FileName, FileMode.OpenOrCreate);
        }

        public byte[] ReadBytes(int length)
        {
            byte[] buffer = new byte[length];
            FileStream.Read(buffer, 0, length);

            return buffer;
        }

        public void WriteBytes(byte[] bytes)
        {
            FileStream.Write(bytes, 0, bytes.Length);
        }

        public void MovePosition(int offset)
        {
            FileStream.Position += offset;
        }

        public void Dispose()
        {
            FileStream.Close();
            FileStream.Dispose();
        }

        public void Close()
        {
            FileStream.Close();
        }
    }
}
