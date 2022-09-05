/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

using PylonSoftwareEngine.FileSystem.DataSources;
using PylonSoftwareEngine.FileSystem.Filetypes;
using PylonSoftwareEngine.Mathematics;
using System;
using System.Text;

namespace PylonSoftwareEngine.FileSystem
{
    public class DataWriter
    {
        public IDataSource DataSource;

        public DataWriter(IDataSource dataSource)
        {
            DataSource = dataSource;
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
            WriteBytes(BitConverter.GetBytes(value));
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
            WriteBytes(BitConverter.GetBytes(value));
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
            WriteBytes(BitConverter.GetBytes(value));
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
            WriteBytes(BitConverter.GetBytes(value));
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
            if (length != 0)
                WriteBytes(Encoding.ASCII.GetBytes(value));
        }
        public void WriteByte(byte Byte)
        {
            DataSource.WriteBytes(new byte[] { Byte });
        }

        public void WriteByteArray(byte[] value)
        {
            WriteInt(value.Length);
            DataSource.WriteBytes(value);
        }

        public void WriteBytes(byte[] bytes)
        {
            DataSource.WriteBytes(bytes);
        }
    }
}
