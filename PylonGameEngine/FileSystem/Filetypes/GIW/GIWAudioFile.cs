using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PylonGameEngine.FileSystem.Filetypes.GIW
{
    public class GIWAudioFile
    {
        public float[,] Samples;
        public int SampleRate;

        public float[] GetChannel(int index)
        {
            return Enumerable.Range(0, Samples.GetLength(0)).Select(x => Samples[x, index]).ToArray();
        }

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

        public GIWAudioFile(int channels, int Length, int sampleRate)
        {
            Samples = new float[Length, channels];
            SampleRate = sampleRate;
        }
    }
}
