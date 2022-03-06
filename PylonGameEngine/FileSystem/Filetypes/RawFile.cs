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
        internal List<byte> Data;

        public RawFile()
        {
            Data = new List<byte>();
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
            try
            {
            Directory.CreateDirectory(Path.GetDirectoryName(FileName));

            }
            catch (Exception)
            {


            }
            File.WriteAllBytes(FileName, Data.ToArray());
        }

        public void ReadFile(string FileName)
        {
            Data = File.ReadAllBytes(FileName).ToList();
        }

        public void Dispose()
        {
            Data = null;
        }
    }
}
