using UnityEngine;

namespace Etienne
{
    public partial class Extensions
    {
        public static AudioSource SetSoundToSource(this AudioSource source, Sound sound)
        {
            source.outputAudioMixerGroup = sound.Parameters.AudioMixerGroup;
            source.clip = sound.Clip;
            source.loop = sound.Parameters.Loop;
            source.pitch = sound.Parameters.Pitch;
            source.volume = sound.Parameters.Volume;
            source.spatialBlend = sound.Parameters.SpacialBlend;
            source.minDistance = sound.Parameters.MinDistance;
            source.maxDistance = sound.Parameters.MaxDistance;
            return source;
        }
    }
}