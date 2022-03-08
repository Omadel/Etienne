using UnityEngine;

namespace Etienne
{
    [System.Serializable]
    public struct Cue
    {
        /// <summary>
        /// Random sound from cue's clips
        /// </summary>
        public Sound Sound => new Sound(clips[Random.Range(0, clips.Length)], parameters);
        public AudioClip[] Clips => clips;
        public SoundParameters Parameters => parameters;

        [SerializeField] private AudioClip[] clips;
        [SerializeField] private SoundParameters parameters;

        public Cue(AudioClip[] clips = null)
        {
            this.clips = clips;
            parameters = new SoundParameters(null);
        }

        public Cue(AudioClip[] clips, SoundParameters parameters)
        {
            this.clips = clips;
            this.parameters = parameters;
        }

        public static implicit operator Sound(Cue cue)
        {
            return cue.Sound;
        }

        public override string ToString()
        {
            return (clips == null || clips.Length == 0) ? "Nothing" : $"Cue of {clips.Length} clips";
        }

        public void Play(int index, Transform transform = null)
        {
            if (index < 0 || index >= clips.Length)
            {
                Sound.Play(transform);
                Debug.LogWarning("Index out of range, played random sound instead.");
                return;
            }
            new Sound(clips[index], parameters).Play(transform);
        }

        public void Play(Transform transform = null)
        {
            Sound.Play(transform);
        }
    }
}