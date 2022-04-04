using UnityEngine;
using UnityEngine.Audio;

namespace Etienne
{
    [System.Serializable]
    public struct SoundParameters
    {
        public AudioMixerGroup AudioMixerGroup => audioMixerGroup;
        public int Priority => priority;
        public float Volume => volume;
        public float Pitch => pitch;
        public float StereoPan => stereoPan;
        public int SpacialBlend => spacialBlend;
        public bool Is2D => spacialBlend == 0;
        public bool Is3D => spacialBlend == 1;
        public float MinDistance => minDistance;
        public float MaxDistance => maxDistance;


        [SerializeField] private AudioMixerGroup audioMixerGroup;
        [RangeLabelled(0f, 256f, "High", "Low")]
        [SerializeField] private int priority;
        [UnityEngine.Range(0f, 1f)]
        [SerializeField] private float volume;
        [UnityEngine.Range(.1f, 3)]
        [SerializeField] private float pitch;
        [RangeLabelled(-1f, 1f, "Left", "Right")]
        [SerializeField] private float stereoPan;
        [RangeLabelled(0, 1, "2D", "3D")]
        [SerializeField] private int spacialBlend;
        [SerializeField, Min(0)] private float minDistance;
        [SerializeField, Min(0)] private float maxDistance;

        public SoundParameters(AudioMixerGroup audioMixerGroup = null)
        {
            this.audioMixerGroup = audioMixerGroup;
            priority = 128;
            volume = 1f;
            pitch = 1f;
            stereoPan = 0;
            spacialBlend = 0;
            minDistance = 1;
            maxDistance = 500;
        }
    }
}