using UnityEngine;
namespace Etienne {
    [System.Serializable]
    public struct Sound {
        public AudioClip Clip => clip;
        public SoundParameters Parameters => parameters;

        [SerializeField] private AudioClip clip;
        [SerializeField] private SoundParameters parameters;

        public Sound(AudioClip clip = null, float volume = 1, float pitch = 1, bool loop = false) {
            this.clip = clip;
            parameters = new SoundParameters(volume, pitch, loop);
        }
        public Sound(AudioClip clip, SoundParameters parameters) {
            this.clip = clip;
            this.parameters = parameters;
        }
    }
}