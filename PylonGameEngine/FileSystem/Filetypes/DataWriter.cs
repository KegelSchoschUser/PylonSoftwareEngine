﻿using System;
using System.Text;

namespace PylonGameEngine.FileSystem.Filetypes
{
    public class DataWriter
    {
        public RawFile File;

        public DataWriter(RawFile file)
        {
            File = file;
        }

        public void WriteShort(short value)
        {
            WriteBytes( BitConverter.GetBytes(value));
        }

        public void WriteInt(int value)
        {
            WriteBytes( BitConverter.GetBytes(value));
        }

        public void WriteLong(long value)
        {
            WriteBytes( BitConverter.GetBytes(value));
        }

        public void WriteFloat(float value)
        {
            WriteBytes( BitConverter.GetBytes(value));
        }

        public void WriteDouble(double value)
        {
            WriteBytes( BitConverter.GetBytes(value));
        }

        /// <summary>
        /// 2 Bytes Unicode
        /// </summary>
        /// <WriteBytes(s></WriteBytes(s>
        public void WriteChar(char value)
        {
            WriteBytes( BitConverter.GetBytes(value));
        }

        public void WriteString(string value)
        {
            WriteBytes( Encoding.ASCII.GetBytes(value));
        }

        public void WriteBool(bool value)
        {
            WriteBytes( BitConverter.GetBytes(value));
        }

        public void WriteBytes(byte[] bytes)
        {
            Array.Resize(ref File.Data, File.Data.Length + bytes.Length);

            for (int i = 0; i < bytes.Length; i++)
            {
                File.Data[File.Data.Length - (bytes.Length - i)] = bytes[i];
            }
        }

        public void WriteByte(byte Byte)
        {
            Array.Resize(ref File.Data, File.Data.Length + 1);
            File.Data[File.Data.Length - 1] = Byte;
        }
    }
}
