using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PylonGameEngine.FileSystem.Filetypes.Pylon
{
    public class PylonAudioFile : IPylonSerializable
    {
        public float[,] Samples;
        public int SampleRate;
        public int ChannelCount
        {
            get
            {
                return Samples.GetLength(1);
            }
        }
        public int Length
        {
            get { return Samples.GetLength(0); }
        }
        

        public PylonAudioFile(int channels, int Length, int sampleRate)
        {
            Samples = new float[Length, channels];
            SampleRate = sampleRate;
        }
        public float[] GetChannel(int index)
        {
            return Enumerable.Range(0, Samples.GetLength(0)).Select(x => Samples[x, index]).ToArray();
        }

        public bool Serialize(DataWriter writer)
        {
            writer.WriteInt(SampleRate);
            writer.WriteInt(Length);

            writer.WriteInt(ChannelCount);
            for (int i = 0; i < ChannelCount; i++)
            {
                writer.WriteFloatArray(GetChannel(i));
            }

            return true;
        }

        public bool DeSerialize(DataReader reader)
        {
            try
            {
                var sampleRate = reader.ReadInt();
                var length = reader.ReadInt();
                var channelcount = reader.ReadInt();

                Samples = new float[Length, channelcount];
                SampleRate = sampleRate;



                for (int c = 0; c < channelcount; c++)
                {
                    var channel = reader.ReadFloatArray();
                    for (int l = 0; l < length; l++)
                    {
                        Samples[l, c] = channel[l];
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }


            return true;
        }

        public float GetLengthSeconds()
        {
            return Length / SampleRate;
        }
    }
}
