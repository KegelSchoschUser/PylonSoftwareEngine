/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

using PylonSoftwareEngine.FileSystem.DataSources;
using PylonSoftwareEngine.FileSystem.Filetypes.Pylon;
using System;

namespace PylonSoftwareEngine.FileSystem.Filetypes.WAVE
{
    public class WaveFile
    {

        private IDataSource DataSource;

        public ushort Format;
        public ushort Channels;
        public uint SampleRate;
        public ushort BitsPerSample;
        public float[] Samples;


        [Obsolete]
        public WaveFile()
        {

        }

        public WaveFile(string FileName)
        {
            DataSource = new MyFileStream(FileName);
            Initialize();
        }

        public static bool Verify(string FileName)
        {
            WaveFile wav = new WaveFile();
            wav.DataSource = new MyFileStream(FileName);
            return wav.Initialize();
        }

        private bool Initialize()
        {
            //if (RAWFile.Data.Count == 0)
            //    throw new IndexOutOfRangeException();

            DataReader Reader = new DataReader(DataSource);

            bool HeaderChecked = CheckHeader(Reader);
            if (HeaderChecked)
            {
                bool FormatChecked = ReadFormat(Reader);
                if (FormatChecked)
                {
                    bool DataChecked = ReadData(Reader);
                    if (DataChecked)
                    {
                        Reader.Dispose();
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
                    if (Reader.CheckString("JUNK"))
                    {
                        var junksize = Reader.ReadUInt();
                        Reader.ReadBytes((int)junksize);
                    }
                    else
                    {
                        Reader.MovePosition(-4);
                    }
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

        public PylonAudioFile ConvertToPylonFormat()
        {
            PylonAudioFile File = new PylonAudioFile(Channels, Samples.Length / Channels, (int)SampleRate);

            for (int i = 0; i < Samples.Length; i++)
            {
                File.Samples[i / Channels, i % Channels] = Samples[i];
            }

            return File;
        }
    }
}
