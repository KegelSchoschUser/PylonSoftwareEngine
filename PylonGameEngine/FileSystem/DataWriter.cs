using PylonGameEngine.FileSystem.Filetypes;
using PylonGameEngine.Mathematics;
using System;
using System.Diagnostics;
using System.Text;

namespace PylonGameEngine.FileSystem
{
    public class DataWriter
    {
        public RawFile File;

        public DataWriter(RawFile file)
        {
            File = file;
        }

        public void WriteBool(bool value)
        {
            WriteBytes(BitConverter.GetBytes(value));
        }
        public void WriteBoolArray(bool[] value)
        {
            WriteInt(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                WriteBytes(BitConverter.GetBytes(value[i]));
            }
        }
        public void WriteShort(short value)
        {
            WriteBytes( BitConverter.GetBytes(value));
        }
        public void WriteShortArray(short[] value)
        {
            WriteInt(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                WriteBytes(BitConverter.GetBytes(value[i]));
            }
        }
        public void WriteUShort(ushort value)
        {
            WriteBytes(BitConverter.GetBytes(value));
        }
        public void WriteUShortArray(ushort[] value)
        {
            WriteInt(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                WriteBytes(BitConverter.GetBytes(value[i]));
            }
        }

        public void WriteInt(int value)
        {
            WriteBytes( BitConverter.GetBytes(value));
        }

        public void WriteIntArray(int[] value)
        {
            WriteInt(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                WriteBytes(BitConverter.GetBytes(value[i]));
            }
        }

        public void WriteLong(long value)
        {
            WriteBytes( BitConverter.GetBytes(value));
        }

        public void WriteLongArray(long[] value)
        {
            WriteInt(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                WriteBytes(BitConverter.GetBytes(value[i]));
            }
        }

        public void WriteFloat(float value)
        {
            WriteBytes( BitConverter.GetBytes(value));
        }

        public void WriteFloatArray(float[] value)
        {
            WriteInt(value.Length);
            var bytes = new byte[value.Length * 4];
            Buffer.BlockCopy(value, 0, bytes, 0, bytes.Length);
            WriteBytes(bytes);
        }

        public void WriteDouble(double value)
        {
            WriteBytes(BitConverter.GetBytes(value));
        }

        public void WriteDoubleArray(double[] value)
        {
            WriteInt(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                WriteBytes(BitConverter.GetBytes(value[i]));
            }
        }
        public void WriteChar(char value)
        {
            WriteBytes(BitConverter.GetBytes(value));
        }

        public void WriteCharArray(char[] value)
        {
            WriteInt(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                WriteBytes(BitConverter.GetBytes(value[i]));
            }
        }

        public void WriteVector2(Vector2 value)
        {
            WriteFloat(value.X);
            WriteFloat(value.Y);
        }

        public void WriteVector2Array(Vector2[] value)
        {
            WriteInt(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                WriteVector2(value[i]);
            }
        }
        public void WriteVector3(Vector3 value)
        {
            WriteFloat(value.X);
            WriteFloat(value.Y);
            WriteFloat(value.Z);
        }

        public void WriteVector3Array(Vector3[] value)
        {
            WriteInt(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                WriteVector3(value[i]);
            }
        }

        public void WriteObject(IPylonSerializable obj)
        {
            bool success = obj.Serialize(this);

            if (success == false)
                throw new Exception("Couldn't write Object of Type " + obj.GetType().FullName);
        }
        public void WriteString(string value)
        {
            var length = Encoding.ASCII.GetBytes(value).Length;
            WriteInt(length);
            if(length != 0)
                WriteBytes(Encoding.ASCII.GetBytes(value));
        }
        public void WriteByte(byte Byte)
        {
            File.Data.Add(Byte);
        }

        public void WriteByteArray(byte[] value)
        {
            WriteInt(value.Length);
            File.Data.AddRange(value);
        }

        public void WriteBytes(byte[] bytes)
        {
            File.Data.AddRange(bytes);
        }

    }
}
