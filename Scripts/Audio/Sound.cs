using UnityEngine;
namespace Etienne
{
    [System.Serializable]
    public struct Sound
    {
        public AudioClip Clip => _Clip;
        public SoundParameters Parameters => _ParametersSO == null ? _Parameters : _ParametersSO;

        [SerializeField] private AudioClip _Clip;
        [Header("Parameters")]
        [SerializeField, Tooltip("Has priority over the self parameters")] private SoundParametersScriptableObject _ParametersSO;
        [SerializeField] private SoundParameters _Parameters;

        public Sound(AudioClip clip = null)
        {
            _Clip = clip;
            _ParametersSO = null;
            _Parameters = new SoundParameters(null);
        }

        public Sound(AudioClip clip, SoundParameters parameters)
        {
            _Clip = clip;
            _ParametersSO = null;
            _Parameters = parameters;
        }

        public Sound(AudioClip clip, SoundParametersScriptableObject parameters)
        {
            _Clip = clip;
            _ParametersSO = parameters;
            _Parameters = parameters;
        }

        public void Play(Transform transform = null)
        {
            if (Pools.AudioSourcePool.Instance == null) Pools.AudioSourcePool.CreateInstance<Pools.AudioSourcePool>(100);
            Pools.AudioSourcePool.Play(this, transform);
        }
    }
}