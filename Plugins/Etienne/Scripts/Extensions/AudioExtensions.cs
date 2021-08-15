using UnityEngine;

namespace Etienne {
    public partial class Extensions {

        public static AudioSource SetSoundToSource(this AudioSource source, Sound sound) {
            source.clip = sound.Clip;
            source.loop = sound.Parameters.Loop;
            source.pitch = sound.Parameters.Pitch;
            source.volume = sound.Parameters.Volume;
            source.spatialBlend = sound.Parameters.SpacialBlend;
            return source;
        }
    }
}