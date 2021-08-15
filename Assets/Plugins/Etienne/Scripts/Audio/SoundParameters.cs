using UnityEngine;

namespace Etienne {
    [System.Serializable]
    public struct SoundParameters {
        public float Volume => volume;
        public float Pitch => pitch;
        public bool Loop => loop;
        public int SpacialBlend => spacialBlend;
        public bool Is2D => spacialBlend == 0;
        public bool Is3D => spacialBlend == 1;

        [Range(0f, 1f)]
        [SerializeField] private float volume;
        [Range(.1f, 3)]
        [SerializeField] private float pitch;
        [SerializeField] private bool loop;
        [Range(0, 1)]
        [SerializeField] private int spacialBlend;

        public SoundParameters(float volume = 1f, float pitch = 1f, bool loop = false, int spacialBlend = 0) {
            this.volume = volume;
            this.pitch = pitch;
            this.loop = loop;
            this.spacialBlend = spacialBlend;
        }
    }
}