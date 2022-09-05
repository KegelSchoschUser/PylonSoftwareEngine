/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

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
