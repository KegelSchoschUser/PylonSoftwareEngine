using PylonGameEngine.FileSystem.Filetypes.Pylon;
using System;
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

        private int SamplesPlayedLastStop = 0;
        public int SamplesPlayed
        {
            get
            {
                return (int)Voice.State.SamplesPlayed - SamplesPlayedLastStop;
            }
        }

        public int BuffersQueued
        {
            get
            {
                return Voice.State.BuffersQueued;
            }
        }

        public int SampleRate
        {
            get
            {
                return Voice.VoiceDetails.InputSampleRate;
            }
        }
        public int Length
        {
            get
            {
                return Buffer.Length / 4 / Voice.VoiceDetails.InputChannelCount;
            }
        }

        public delegate void OnBufferEnd();
        public event OnBufferEnd BufferEnd;

        IXAudio2SourceVoice Voice;
        public byte[] Buffer { get; private set; }

        public Audio(int SampleRate = 48000, int Channels = 2)
        {
            CreateVoice(SampleRate, Channels);
            BufferEnd += () => { };
        }

        public Audio(PylonAudioFile file)
        {
            CreateVoice(file.SampleRate, file.ChannelCount);
            AddBuffer(file.Samples, true);
            BufferEnd += () => { };
        }

        private void CreateVoice(int SampleRate = 48000, int Channels = 2)
        {
            Voice = AudioEngine.Engine.CreateSourceVoice(new WaveFormat(SampleRate, 16, Channels), true);
            Voice.BufferEnd += Voice_BufferEnd;
        }

        private void Voice_BufferEnd(IntPtr obj)
        {
            BufferEnd();
            if (Loop)
            {
                MyGame.RenderLoop.Invoke(() => { Play(); });
            }
        }

        public void AddBuffer(PylonAudioFile File, bool Final)
        {
            AddBuffer(File.Samples, Final);
        }

        public void AddBuffer(float[] SamplesLeft, float[] SamplesRight, bool Final)
        {
            if (SamplesLeft.Length != SamplesRight.Length)
                throw new ArgumentOutOfRangeException();
            else
            {
                float[] samples = new float[SamplesLeft.Length * 2];

                for (int x = 0; x < SamplesLeft.Length * 2; x++)
                {
                    if (x % 2 == 0)
                    {
                        samples[x] = SamplesLeft[x / 2];

                    }
                    else
                    {
                        samples[x] = SamplesRight[x / 2];
                    }
                }
                ConvertSamplesAndSubmit(samples, Final);
            }
        }

        public void AddBuffer(float[] Samples, bool Final)
        {
            ConvertSamplesAndSubmit(Samples, Final);
        }

        public void AddBuffer(float[,] Samples, bool Final)
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
            ConvertSamplesAndSubmit(samples, Final);

        }

        private void ConvertSamplesAndSubmit(float[] Samples, bool Final)
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
                SubmitBuffer(Final);
            }
        }

        private void SubmitBuffer(bool Final)
        {
            var Audiobuffer = new AudioBuffer(Buffer, Final ? BufferFlags.EndOfStream : BufferFlags.None);
      
            if (Voice.State.BuffersQueued < 64)
                Voice.SubmitSourceBuffer(Audiobuffer);
        }

        public bool Play()
        {
            Stop();
            SamplesPlayedLastStop = (int)Voice.State.SamplesPlayed;

            //if (Buffer == null || Buffer.Length == 0)
            //    return false;
            //SubmitBuffer(true);
            Voice.Start();

            return true;
        }

        public void Stop()
        {
            Voice.Stop();
        }

        public void FlushBuffers()
        {
            Stop();
            Voice.FlushSourceBuffers();
        }

        ~Audio()
        {
            Voice.DestroyVoice();
            Buffer = null;
        }
    }
}
