using UnityEngine;
namespace Etienne
{
    [System.Serializable]
    public struct Sound
    {
        public AudioClip Clip => _Clip;
        public SoundParameters Parameters => _Parameters;

        [SerializeField] private AudioClip _Clip;
        [SerializeField] private SoundParameters _Parameters;

        public Sound(AudioClip clip = null)
        {
            _Clip = clip;
            _Parameters = new SoundParameters(null);
        }

        public Sound(AudioClip clip, SoundParameters parameters)
        {
            _Clip = clip;
            _Parameters = parameters;
        }

        public void Play(Transform transform = null)
        {
            if (Pools.AudioSourcePool.Instance == null) Pools.AudioSourcePool.CreateInstance<Pools.AudioSourcePool>(100);
            Pools.AudioSourcePool.Play(this, transform);
        }
    }
}