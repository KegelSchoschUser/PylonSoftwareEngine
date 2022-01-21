using PylonGameEngine.FileSystem.Filetypes.GIW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PylonGameEngine.FileSystem.Filetypes.WAVE
{
    public class WaveFile
    {
        private RawFile RAWFile;


        public ushort Format;
        public ushort Channels;
        public uint SampleRate;
        public ushort BitsPerSample;
        public float[] Samples;


        public WaveFile(string FileName)
        {
            RAWFile = new RawFile(FileName);
            Initialize();
        }

        private bool Initialize()
        {
            if (RAWFile.Data.Length == 0)
                throw new IndexOutOfRangeException();

            DataReader Reader = RAWFile.ReadData();

            bool HeaderChecked = CheckHeader(Reader);
            if (HeaderChecked)
            {
                bool FormatChecked = ReadFormat(Reader);
                if (FormatChecked)
                {
                    bool DataChecked = ReadData(Reader);
                    if (DataChecked)
                    {
                        RAWFile.Dispose();
                        return true;
                    }
                }
            }

            return false;
        }

        private bool CheckHeader(DataReader Reader)
        {
            if (Reader.CheckString("RIFF"))
            {
                Reader.ReadUInt(); //Size ?!
                if (Reader.CheckString("WAVE"))
                {
                    return true;
                }
            }

            return false;
        }

        private bool ReadFormat(DataReader Reader)
        {
            if (Reader.CheckString("fmt "))
            {
                Reader.ReadUInt(); //Size ?!
                Format = Reader.ReadUShort();
                Channels = Reader.ReadUShort();
                SampleRate = Reader.ReadUInt();
                uint avgBytesSec = Reader.ReadUInt();
                ushort blockAlign = Reader.ReadUShort();
                BitsPerSample = Reader.ReadUShort();

                return true;
            }

            return false;
        }

        private bool ReadData(DataReader Reader)
        {
            if (Reader.CheckString("data"))
            {
                uint chunksize = Reader.ReadUInt();

                uint SamplesLength = chunksize / (uint)(BitsPerSample / 8);
                Samples = new float[SamplesLength];

                if (Format == 1)
                {
                    for (int i = 0; i < SamplesLength; i++)
                    {
                        Samples[i] = Reader.ReadShort() / short.MaxValue;
                    }
                }
                else if (Format == 3)
                {
                    for (int i = 0; i < SamplesLength; i++)
                    {
                        Samples[i] = Reader.ReadFloat();
                    }
                }

                return true;
            }

            return false;
        }

        public GIWAudioFile ConvertToGIWFormat()
        {
            GIWAudioFile File = new GIWAudioFile(Channels, Samples.Length / Channels, (int)SampleRate);

            for (int i = 0; i < Samples.Length; i++)
            {
                File.Samples[i / Channels, i % Channels] = Samples[i];
            }

            return File;
        }
    }
}
