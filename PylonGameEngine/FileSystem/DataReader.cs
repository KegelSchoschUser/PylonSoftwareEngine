using PylonGameEngine.FileSystem.Filetypes;
using PylonGameEngine.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PylonGameEngine.FileSystem
{
    public class DataReader
    {
        public int ReadOffset;
        private List<byte> Data;

        public DataReader(RawFile file)
        {
            Data = file.Data;
            ReadOffset = 0;
        }

        public DataReader(List<byte> data)
        {
            Data = data;
            ReadOffset = 0;
        }
        public DataReader(byte[] data)
        {
            Data = data.ToList();
            ReadOffset = 0;
        }

        public bool ReadBool()
        {
            return BitConverter.ToBoolean(ReadBytes(1), 0);
        }
        public bool[] ReadBoolArray()
        {
            int length = ReadInt();
            var array = new bool[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = ReadBool();
            }

            return array;
        }

        public short ReadShort()
        {
            return BitConverter.ToInt16(ReadBytes(2), 0);
        }

        public short[] ReadShortArray()
        {
            int length = ReadInt();
            var array = new short[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = ReadShort();
            }

            return array;
        }

        public ushort ReadUShort()
        {
            return BitConverter.ToUInt16(ReadBytes(2), 0);
        }
        public ushort[] ReadUShortArray()
        {
            int length = ReadInt();
            var array = new ushort[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = ReadUShort();
            }

            return array;
        }

        public int ReadInt()
        {
            return BitConverter.ToInt32(ReadBytes(4), 0);
        }
        public int[] ReadIntArray()
        {
            int length = ReadInt();
            var array = new int[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = ReadInt();
            }

            return array;
        }

        public bool CheckInt(int i)
        {
            return ReadInt() == i;
        }

        public uint ReadUInt()
        {
            return BitConverter.ToUInt32(ReadBytes(4), 0);
        }
        public uint[] ReadUIntArray()
        {
            int length = ReadInt();
            var array = new uint[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = ReadUInt();
            }

            return array;
        }

        public long ReadLong()
        {
            return BitConverter.ToInt64(ReadBytes(8), 0);
        }
        public long[] ReadLongArray()
        {
            int length = ReadInt();
            var array = new long[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = ReadLong();
            }

            return array;
        }

        public ulong ReadULong()
        {
            return BitConverter.ToUInt64(ReadBytes(8), 0);
        }
        public ulong[] ReadULongArray()
        {
            int length = ReadInt();
            var array = new ulong[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = ReadULong();
            }

            return array;
        }

        public float ReadFloat()
        {
            return BitConverter.ToSingle(ReadBytes(4), 0);
        }
        public float[] ReadFloatArray()
        {
            int length = ReadInt();
            var array = new float[length];

            Buffer.BlockCopy(ReadBytes(length * 4), 0, array, 0, length * 4);

            return array;
        }
        public double ReadDouble()
        {
            return BitConverter.ToDouble(ReadBytes(8), 0);
        }
        public double[] ReadDoubleArray()
        {
            int length = ReadInt();
            var array = new double[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = ReadDouble();
            }

            return array;
        }
        public char ReadChar()
        {
            return BitConverter.ToChar(ReadBytes(2), 0);
        }
        public char[] ReadCharArray()
        {
            int length = ReadInt();
            var array = new char[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = ReadChar();
            }

            return array;
        }
        public Vector2 ReadVector2()
        {
            return new Vector2(ReadFloat(), ReadFloat());
        }
        public Vector2[] ReadVector2Array()
        {
            int length = ReadInt();
            var array = new Vector2[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = ReadVector2();
            }

            return array;
        }

        public Vector3 ReadVector3()
        {
            return new Vector3(ReadFloat(), ReadFloat(), ReadFloat());
        }
        public Vector3[] ReadVector3Array()
        {
            int length = ReadInt();
            var array = new Vector3[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = ReadVector3();
            }

            return array;
        }

        public T ReadObject<T>() where T : IPylonSerializable, new()
        {
            T type = new T();
            return type.DeSerialize(this);
        }

        public dynamic ReadObject(IPylonSerializable type)
        {
            return type.DeSerialize(this);
        }

        public string ReadString()
        {
            int length = ReadInt();
            if (length == 0)
                return "";
            return Encoding.ASCII.GetString(ReadBytes(length), 0, length);
        }

        public bool CheckString(string str)
        {
            return Encoding.ASCII.GetString(ReadBytes(str.Length), 0, str.Length) == str;
        }
        public byte ReadByte()
        {
            byte Byte = Data[ReadOffset];
            ReadOffset++;
            return Byte;
        }

        public byte[] ReadByteArray()
        {
            int length = ReadInt();
            return ReadBytes(length);
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

    }
}
