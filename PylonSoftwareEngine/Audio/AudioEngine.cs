using Vortice.XAudio2;

namespace PylonSoftwareEngine.Audio
{
    public static class AudioEngine
    {
        public static IXAudio2 Engine;
        private static IXAudio2MasteringVoice MasterVoice;

        public static void Initialize()
        {
            Engine = XAudio2.XAudio2Create(ProcessorSpecifier.UseDefaultProcessor, true);
            MasterVoice = Engine.CreateMasteringVoice(0, 0, Vortice.Multimedia.AudioStreamCategory.GameEffects);
        }
    }
}
