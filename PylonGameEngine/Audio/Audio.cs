using PylonGameEngine.FileSystem.Filetypes.Pylon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Multimedia;
using Vortice.XAudio2;

namespace PylonGameEngine.Audio
{
    public class Audio
    {
        public bool Loop = false;
        private float _Volume = 1f;
        public float Volume
        {
            get
            {
                return _Volume;
            }

            set
            {
                _Volume = value;
                Voice.SetVolume(value);
            }
        }

        private ulong SamplesPlayedLastStop = 0;
        public ulong SamplesPlayed
        {
            get
            {
                return Voice.State.SamplesPlayed - SamplesPlayedLastStop;
            }
        }

        public int SampleRate
        {
            get
            {
                return Voice.VoiceDetails.InputSampleRate;
            }
        }
        public ulong Length
        {
            get
            {
                return (ulong)Buffer.Length;
            }
        }


        IXAudio2SourceVoice Voice;
        private byte[] Buffer;

        public Audio(int SampleRate = 48000, int Channels = 2)
        {
            CreateVoice(SampleRate, Channels);
        }

        public Audio(PylonAudioFile file)
        {
            CreateVoice(file.SampleRate, file.ChannelCount);
            SubmitBuffer(file.Samples);
        }

        private void CreateVoice(int SampleRate = 48000, int Channels = 2)
        {
            Voice = AudioEngine.Engine.CreateSourceVoice(new WaveFormat(SampleRate, 16, Channels), true);
            Voice.BufferEnd += Voice_BufferEnd;
        }

        private void Voice_BufferEnd(IntPtr obj)
        {
            if (Loop)
            {
                MyGame.RenderLoop.Invoke(() => { Play(); });
            }
        }

        public void SubmitBuffer(PylonAudioFile File)
        {
            SubmitBuffer(File.Samples);
        }

        public void SubmitBuffer(float[] SamplesLeft, float[] SamplesRight)
        {
            if (SamplesLeft.Length != SamplesRight.Length)
                throw new ArgumentOutOfRangeException();
            else
            {
                float[] samples = new float[SamplesLeft.Length * 2];

                for (int x = 0; x < SamplesLeft.Length * 2; x++)
                {
                    if(x % 2 == 0)
                    {
                        samples[x] = SamplesLeft[x /2];

                    }
                    else
                    {
                        samples[x] = SamplesRight[x / 2];
                    }
                }
                ConvertSamples(samples);
            }
        }

        public void SubmitBuffer(float[] Samples)
        {
            ConvertSamples(Samples);
        }

        public void SubmitBuffer(float[,] Samples)
        {
            float[] samples = new float[Samples.Rank * Samples.Length];
            int i = 0;
            for (int x = 0; x < Samples.GetLength(0); x++)
            {
                for (int y = 0; y < Samples.GetLength(1); y++)
                {
                    samples[i] = Samples[x, y];
                    i++;
                }
            }
            ConvertSamples(samples);

        }

        private void ConvertSamples(float[] Samples)
        {
            var Bytes = new byte[Samples.Length * 2];

            for (int i = 0; i < Samples.Length; i++)
            {
                var bytes = BitConverter.GetBytes((short)(Samples[i] * short.MaxValue));

                int offset = i * 2;
                Bytes[offset] = bytes[0];
                Bytes[offset + 1] = bytes[1];
            }

            if (Bytes.Length != 0)
            {
                Buffer = Bytes;
            }
        }

        private void SubmitBuffer()
        {
            Voice.FlushSourceBuffers();

            var Audiobuffer = new AudioBuffer(Buffer, BufferFlags.EndOfStream);
            Voice.SubmitSourceBuffer(Audiobuffer);
        }

        public bool Play()
        {
            Stop();
            if (Buffer == null || Buffer.Length == 0)
                return false;
            SubmitBuffer();
            Voice.Start();

            return true;
        }

        public void Stop()
        {
            Voice.Stop();
            SamplesPlayedLastStop = Voice.State.SamplesPlayed;
        }

        ~Audio()
        {
            Voice.DestroyVoice();
            Buffer = null;
        }
    }
}
