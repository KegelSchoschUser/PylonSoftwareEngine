using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PylonGameEngine.FileSystem.Filetypes
{
    public class RawFile : IDisposable
    {
        internal byte[] Data;

        public RawFile()
        {
            Data = new byte[0];
        }

        public RawFile(string FileName)
        {
            ReadFile(FileName);
        }

        public DataReader ReadData()
        {
            return new DataReader(this);
        }

        public DataWriter WriteData()
        {
            return new DataWriter(this);
        }

        public void SaveFile(string FileName)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FileName));
            File.WriteAllBytes(FileName, Data);
        }

        public void ReadFile(string FileName)
        {
            Data = File.ReadAllBytes(FileName);
        }

        public void Dispose()
        {
            Data = null;
        }
    }
}
