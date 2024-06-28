using UnityEngine;

namespace Etienne
{
    [System.Serializable]
    public struct Cue
    {
        /// <summary>
        /// Random sound from cue's clips
        /// </summary>
        public readonly Sound Sound => new(Clips[Random.Range(0, Clips.Length)], Parameters);
        public readonly SoundParameters Parameters => _parameters ?? SoundParameters.DefaultParameters;

        [field: SerializeField] public AudioClip[] Clips { get; private set; }

        [SerializeField] private SoundParametersScriptableObject _parameters;

        public Cue(AudioClip[] clips = null)
        {
            Clips = clips;
            _parameters = null;
        }

        public Cue(AudioClip[] clips, SoundParametersScriptableObject parameters)
        {
            Clips = clips;
            _parameters = parameters;
        }

        public static implicit operator Sound(Cue cue) => cue.Sound;

        public override string ToString()
        {
            return (Clips == null || Clips.Length == 0) ? "Nothing" : $"Cue of {Clips.Length} clips";
        }

        public readonly AudioSource Play(int index) => GetSafeSoundAt(index)?.Play();
        public readonly AudioSource Play(int index, Transform transform) => GetSafeSoundAt(index)?.Play(transform);
        public readonly AudioSource Play(int index, Vector3 position) => GetSafeSoundAt(index)?.Play(position);

        public readonly AudioSource Play() => GetSafeSound()?.Play();
        public readonly AudioSource Play(Transform transform) => GetSafeSound()?.Play(transform);
        public readonly AudioSource Play(Vector3 position) => GetSafeSound()?.Play(position);

        private readonly Sound? GetSafeSoundAt(int index)
        {
            if (index < 0 || index >= Clips.Length)
            {
                Debug.LogWarning("Index out of range, played random sound instead.");
                return GetSafeSound();
            }
            return new Sound(Clips[index], Parameters);
        }

        private readonly Sound? GetSafeSound()
        {
            if (Clips == null || Clips.Length <= 0)
            {
                Debug.LogError("Cue is empty");
                return null;
            }
            var sound = Sound;
            if (sound.Clip == null)
            {
                Debug.LogError("Clip in cue is empty");
                return null;
            }
            return sound;
        }
    }
}