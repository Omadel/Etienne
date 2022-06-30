using UnityEngine;
namespace Etienne
{
    [System.Serializable]
    public struct Sound
    {
        public AudioClip Clip => _Clip;
        public SoundParameters Parameters => _Parameters ?? _ParametersStruct;

        [SerializeField] private AudioClip _Clip;
        [SerializeField] private SoundParametersScriptableObject _Parameters;

        private SoundParameters _ParametersStruct;

        public Sound(AudioClip clip = null)
        {
            _Clip = clip;
            _Parameters = null;
            _ParametersStruct = SoundParameters.DefaultParameters;
        }

        public Sound(AudioClip clip, SoundParametersScriptableObject parameters)
        {
            _Clip = clip;
            _Parameters = parameters;
            _ParametersStruct = parameters;
        }

        public Sound(AudioClip clip, SoundParameters parameters)
        {
            _Clip = clip;
            _Parameters = null;
            _ParametersStruct = parameters;
        }

        public AudioSource Play(Transform transform = null)
        {
            return Pools.AudioSourcePool.Play(this, transform);
        }
        public AudioSource PlayLooped(Transform transform = null)
        {
            return Pools.AudioSourcePool.PlayLooped(this, transform);
        }
    }
}