using UnityEngine;

namespace Etienne
{
    [System.Serializable]
    public struct Cue
    {
        /// <summary>
        /// Random sound from cue's clips
        /// </summary>
        public Sound Sound => new Sound(_Clips[Random.Range(0, _Clips.Length)], Parameters);
        public AudioClip[] Clips => _Clips;
        public SoundParameters Parameters => _Parameters ?? new SoundParameters(null);

        [SerializeField] private AudioClip[] _Clips;
        [SerializeField] private SoundParametersScriptableObject _Parameters;

        public Cue(AudioClip[] clips = null)
        {
            _Clips = clips;
            _Parameters = null;
        }

        public Cue(AudioClip[] clips, SoundParametersScriptableObject parameters)
        {
            _Clips = clips;
            _Parameters = parameters;
        }

        public static implicit operator Sound(Cue cue) => cue.Sound;

        public override string ToString()
        {
            return (_Clips == null || _Clips.Length == 0) ? "Nothing" : $"Cue of {_Clips.Length} clips";
        }

        public AudioSource Play(int index, Transform transform = null)
        {
            return Pools.AudioSourcePool.Play(this, index, transform);
        }

        public AudioSource Play(Transform transform = null)
        {
            if(_Clips == null || _Clips.Length <= 0)
            {
                Debug.LogError("Cue is empty");
                return null;
            }
            Sound sound = Sound;
            if(sound.Clip == null)
            {
                Debug.LogError("Clip in cue is empty");
                return null;
            }

            return sound.Play(transform);
        }
    }
}