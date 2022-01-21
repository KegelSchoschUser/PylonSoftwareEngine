using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PylonGameEngine.FileSystem.Filetypes
{
    public class DataReader
    {
        private int ReadOffset;
        private byte[] Data;

        public DataReader(RawFile file)
        {
            Data = file.Data;
            ReadOffset = 0;
        }

        public DataReader(byte[] data)
        {
            Data = data;
            ReadOffset = 0;
        }

        public short ReadShort()
        {
            return BitConverter.ToInt16(ReadBytes(2), 0);
        }

        public ushort ReadUShort()
        {
            return BitConverter.ToUInt16(ReadBytes(2), 0);
        }

        public int ReadInt()
        {
            return BitConverter.ToInt32(ReadBytes(4), 0);
        }

        public uint ReadUInt()
        {
            return BitConverter.ToUInt32(ReadBytes(4), 0);
        }

        public long ReadLong()
        {
            return BitConverter.ToInt64(ReadBytes(8), 0);
        }

        public ulong ReadULong()
        {
            return BitConverter.ToUInt64(ReadBytes(8), 0);
        }

        public float ReadFloat()
        {
            return BitConverter.ToSingle(ReadBytes(4), 0);
        }

        public double ReadDouble()
        {
            return BitConverter.ToDouble(ReadBytes(8), 0);
        }

        /// <summary>
        /// 2 Bytes Unicode
        /// </summary>
        /// <returns></returns>
        public char ReadChar()
        {
            return BitConverter.ToChar(ReadBytes(2), 0);
        }

        public string ReadString(int length)
        {
            return Encoding.ASCII.GetString(ReadBytes(length), 0, length);
        }

        public string ReadString()
        {
            int length = ReadInt();
            return Encoding.ASCII.GetString(ReadBytes(length), 0, length);
        }

        public bool CheckString(string str)
        {
            return Encoding.ASCII.GetString(ReadBytes(str.Length), 0, str.Length) == str;
        }

        public bool ReadBool()
        {
            return BitConverter.ToBoolean(ReadBytes(1), 0);
        }

        public byte[] ReadBytes(int length)
        {
            byte[] bytes = new byte[length];

            for (int i = 0; i < length; i++)
            {
                bytes[i] = ReadByte();
            }

            return bytes;
        }

        public byte ReadByte()
        {
            byte Byte = Data[ReadOffset];
            ReadOffset++;
            return Byte;
        }

    }
}
