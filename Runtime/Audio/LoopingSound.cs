using Etienne.Pools;
using UnityEngine;

namespace Etienne
{
    [System.Serializable]
    public struct LoopingSound
    {
        [field: SerializeField] public AudioClip StartClip { get; private set; }
        [field: SerializeField] public AudioClip LoopClip { get; private set; }
        [field: SerializeField] public AudioClip EndClip { get; private set; }
        public readonly SoundParameters Parameters => _parameters ?? SoundParameters.DefaultParameters;

        [SerializeField] private SoundParametersScriptableObject _parameters;
        private AudioSource _startSource;
        private AudioSource _loopSource;
        private AudioSource _endSource;

        public LoopingSound(AudioClip startClip, AudioClip loopClip, AudioClip endClip, SoundParametersScriptableObject parameters = null)
        {
            StartClip = startClip;
            LoopClip = loopClip;
            EndClip = endClip;
            _parameters = parameters;
            _startSource = null;
            _loopSource = null;
            _endSource = null;
        }

        private readonly void InternalStart(out Sound startSound, out Sound loopSound)
        {
            StopSources();
            startSound = new(StartClip, Parameters);
            loopSound = new(LoopClip, Parameters);
        }

        public void Start()
        {
            if (StartClip == null && LoopClip == null) return;
            InternalStart(out var startSound, out var loopSound);
            if (StartClip != null) _startSource = startSound.Play();
            if (LoopClip != null) _loopSource = loopSound.PlayLoopedWithDelay(StartClip.length);
        }
        public void Start(Vector3 position)
        {
            if (StartClip == null && LoopClip == null) return;
            InternalStart(out var startSound, out var loopSound);
            if (StartClip != null) _startSource = startSound.Play(position);
            if (LoopClip != null) _loopSource = loopSound.PlayLoopedWithDelay(position, StartClip.length);
        }
        public void Start(Transform transform)
        {
            if (StartClip == null && LoopClip == null) return;
            InternalStart(out var startSound, out var loopSound);
            if (StartClip != null) _startSource = startSound.Play(transform);
            if (LoopClip != null) _loopSource = loopSound.PlayLoopedWithDelay(transform, StartClip.length);
        }

        internal readonly void InternalEnd(out Sound endSound)
        {
            StopSources();
            endSound = new Sound(EndClip, Parameters);
        }

        public void End()
        {
            if (EndClip == null) return;
            InternalEnd(out var endSound);
            _endSource = endSound.Play();
        }
        public void End(Vector3 position)
        {
            if (EndClip == null) return;
            InternalEnd(out var endSound);
            _endSource = endSound.Play(position);
        }
        public void End(Transform transform)
        {
            if (EndClip == null) return;
            InternalEnd(out var endSound);
            _endSource = endSound.Play(transform);
        }

        public readonly void StopSources()
        {
            if (_startSource != null)
            {
                _startSource.Stop();
            }
            if (_loopSource != null)
            {
                AudioSourcePool.Instance?.Enqueue(_loopSource);
                _loopSource.Stop();
            }
            if (_endSource != null)
            {
                _endSource.Stop();
            }
        }
    }
}
