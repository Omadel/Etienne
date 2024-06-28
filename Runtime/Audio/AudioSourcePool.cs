using UnityEngine;

namespace Etienne.Pools
{
    public class AudioSourcePool : ComponentPool<AudioSource>
    {
        public static AudioSourcePool Instance { get; private set; }

        protected override void CreatePool(int maxSize, params object[] additionnalParameters)
        {
            if (Instance != null) return;
            base.CreatePool(maxSize, additionnalParameters);
            Instance = this;
#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged -= EditorDestroyInstance;
            UnityEditor.EditorApplication.playModeStateChanged += EditorDestroyInstance;
#endif
        }

#if UNITY_EDITOR
        private void EditorDestroyInstance(UnityEditor.PlayModeStateChange state)
        {
            if (state != UnityEditor.PlayModeStateChange.ExitingPlayMode) return;
            Instance = null;
        }
#endif

        internal static AudioSource PrepareAudioSource(Sound sound)
        {
            if (Instance == null)
            {
                Instance = CreateInstance<AudioSourcePool>(100);
                Instance.Inspector.SetOnDestroy(() => Instance = null);
            }

            AudioSource source = Instance.Dequeue();
            source.SetSoundToSource(sound);
            return source;
        }

        internal static AudioSource InternalPlay(Sound sound)
        {
            AudioSource source = PrepareAudioSource(sound);
            source.Play();
            return source;
        }

        public static AudioSource Play(Sound sound)
        {
            AudioSource source = InternalPlay(sound);
            Instance.DelayedEnqueue(source, source.clip.length + .01f);
            return source;
        }
        public static AudioSource Play(Sound sound, Vector3 position)
        {
            AudioSource source = Play(sound);
            source.transform.position = position;
            return source;
        }
        public static AudioSource Play(Sound sound, Transform transform)
        {
            AudioSource source = Play(sound);
            source.transform.SetParent(transform, false);
            return source;
        }

        internal static AudioSource InternalPlayWithDelay(Sound sound, float delay)
        {
            var source = PrepareAudioSource(sound);
            source.PlayDelayed(delay);
            return source;
        }

        public static AudioSource PlayWithDelay(Sound sound, float delay)
        {
            var source = InternalPlayWithDelay(sound, delay);
            Instance.DelayedEnqueue(source, source.clip.length + .01f + delay);
            return source;
        }
        public static AudioSource PlayWithDelay(Sound sound, Transform transform, float delay)
        {
            AudioSource source = Play(sound);
            source.transform.SetParent(transform, false);
            return source;
        }
        public static AudioSource PlayWithDelay(Sound sound, Vector3 position, float delay)
        {
            AudioSource source = Play(sound);
            source.transform.position = position;
            return source;
        }


        public static AudioSource PlayLooped(Sound sound)
        {
            AudioSource source = InternalPlay(sound);
            source.loop = true;
            return source;
        }
        public static AudioSource PlayLooped(Sound sound, Vector3 position)
        {
            AudioSource source = PlayLooped(sound);
            source.transform.position = position;
            return source;
        }
        public static AudioSource PlayLooped(Sound sound, Transform transform)
        {
            AudioSource source = PlayLooped(sound);
            source.transform.SetParent(transform, false);
            return source;
        }

        public static AudioSource PlayLoopedWithDelay(Sound sound, float delay)
        {
            AudioSource source = InternalPlayWithDelay(sound, delay);
            source.loop = true;
            return source;
        }
        public static AudioSource PlayLoopedWithDelay(Sound sound, Vector3 position, float delay)
        {
            AudioSource source = PlayLoopedWithDelay(sound, delay);
            source.transform.position = position;
            return source;
        }
        public static AudioSource PlayLoopedWithDelay(Sound sound, Transform transform, float delay)
        {
            AudioSource source = PlayLoopedWithDelay(sound, delay);
            source.transform.SetParent(transform, false);
            return source;
        }

        public static void ResetInstance()
        {
            Instance = null;
        }
    }
}
