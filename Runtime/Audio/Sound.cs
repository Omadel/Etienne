using Etienne.Pools;
using UnityEngine;

namespace Etienne
{
    [System.Serializable]
    public struct Sound
    {
        [field: SerializeField] public AudioClip Clip { get; private set; }
        public readonly SoundParameters Parameters => _parameters ?? _parametersStruct;

        [SerializeField] private SoundParametersScriptableObject _parameters;

        private SoundParameters _parametersStruct;

        public Sound(AudioClip clip = null)
        {
            Clip = clip;
            _parameters = null;
            _parametersStruct = SoundParameters.DefaultParameters;
        }

        public Sound(AudioClip clip, SoundParametersScriptableObject parameters)
        {
            Clip = clip;
            _parameters = parameters;
            _parametersStruct = parameters;
        }

        public Sound(AudioClip clip, SoundParameters parameters)
        {
            Clip = clip;
            _parameters = null;
            _parametersStruct = parameters;
        }

        public readonly AudioSource Play() => AudioSourcePool.Play(this);
        public readonly AudioSource Play(Transform transform) => AudioSourcePool.Play(this, transform);
        public readonly AudioSource Play(Vector3 position) => AudioSourcePool.Play(this, position);

        public readonly AudioSource PlayWithDelay(float delay) => AudioSourcePool.PlayWithDelay(this, delay);
        public readonly AudioSource PlayWithDelay(Transform transform, float delay) => AudioSourcePool.PlayWithDelay(this, transform, delay);
        public readonly AudioSource PlayWithDelay(Vector3 position, float delay) => AudioSourcePool.PlayWithDelay(this, position, delay);

        public readonly AudioSource PlayLooped() => AudioSourcePool.PlayLooped(this);
        public readonly AudioSource PlayLooped(Transform transform) => AudioSourcePool.PlayLooped(this, transform);
        public readonly AudioSource PlayLooped(Vector3 position) => AudioSourcePool.PlayLooped(this, position);

        public readonly AudioSource PlayLoopedWithDelay(float delay) => AudioSourcePool.PlayLoopedWithDelay(this, delay);
        public readonly AudioSource PlayLoopedWithDelay(Transform transform, float delay) => AudioSourcePool.PlayLoopedWithDelay(this, transform, delay);
        public readonly AudioSource PlayLoopedWithDelay(Vector3 position, float delay) => AudioSourcePool.PlayLoopedWithDelay(this, position, delay);
    }
}