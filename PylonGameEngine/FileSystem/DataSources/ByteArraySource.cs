using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PylonGameEngine.FileSystem.DataSources
{
    public class ByteArraySource : IDisposable, IDataSource
    {
        public List<byte> Data;
        public int ReadOffset;

        public ByteArraySource()
        {
            Data = new List<byte>();
            ReadOffset = 0;
        }

        public ByteArraySource(string FileName)
        {
            ReadFile(FileName);
            ReadOffset = 0;
        }

        public ByteArraySource(byte[] Bytes)
        {
            Data = Bytes.ToList();
            ReadOffset = 0;
        }

        //public DataReader ReadData()
        //{
        //    return new DataReader(this);
        //}

        //public DataWriter WriteData()
        //{
        //    return new DataWriter(this);
        //}

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

        public byte[] ReadBytes(int length)
        {
            byte[] buffer = new byte[length];

            for (int i = 0; i < length; i++)
            {
                buffer[i] = Data[i + ReadOffset];
            }

            ReadOffset += length;

            return buffer;
        }

        public void WriteBytes(byte[] bytes)
        {
           Data.AddRange(bytes);
        }

        public void MovePosition(int offset)
        {
            ReadOffset += offset;
        }

        public void Dispose()
        {
            Data = null;
        }

        public void Close()
        {

        }
    }
}
