using UnityEngine;

namespace Etienne
{
    [System.Serializable]
    public struct Cue
    {
        /// <summary>
        /// Random sound from cue's clips
        /// </summary>
        public Sound Sound => new Sound(_Clips[Random.Range(0, _Clips.Length)], _Parameters);
        public AudioClip[] Clips => _Clips;
        public SoundParameters Parameters => _ParametersSO == null ? _Parameters : _ParametersSO;

        [SerializeField] private AudioClip[] _Clips;
        [Header("Parameters")]
        [SerializeField, Tooltip("Has priority over the self parameters")] private SoundParametersScriptableObject _ParametersSO;
        [SerializeField] private SoundParameters _Parameters;

        public Cue(AudioClip[] clips = null)
        {
            _Clips = clips;
            _ParametersSO = null;
            _Parameters = new SoundParameters(null);
        }

        public Cue(AudioClip[] clips, SoundParametersScriptableObject parameters)
        {
            _Clips = clips;
            _ParametersSO = parameters;
            _Parameters = parameters;
        }

        public static implicit operator Sound(Cue cue)
        {
            return cue.Sound;
        }

        public override string ToString()
        {
            return (_Clips == null || _Clips.Length == 0) ? "Nothing" : $"Cue of {_Clips.Length} clips";
        }

        public void Play(int index, Transform transform = null)
        {
            if (index < 0 || index >= _Clips.Length)
            {
                Sound.Play(transform);
                Debug.LogWarning("Index out of range, played random sound instead.");
                return;
            }
            new Sound(_Clips[index], _Parameters).Play(transform);
        }

        public void Play(Transform transform = null)
        {
            Sound.Play(transform);
        }
    }
}