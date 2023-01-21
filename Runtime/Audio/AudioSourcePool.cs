using UnityEngine;

namespace Etienne.Pools
{
    public class AudioSourcePool : ComponentPool<AudioSource>
    {
        public static AudioSourcePool Instance => instance;
        private static AudioSourcePool instance;

        protected override void CreatePool(int maxSize, params object[] additionnalParameters)
        {
            base.CreatePool(maxSize, additionnalParameters);
            if (instance != null) return;
            instance = this;
#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged -= EditorDestroyInstance;
            UnityEditor.EditorApplication.playModeStateChanged += EditorDestroyInstance;
#endif
        }

#if UNITY_EDITOR
        private void EditorDestroyInstance(UnityEditor.PlayModeStateChange state)
        {
            if (state != UnityEditor.PlayModeStateChange.ExitingPlayMode) return;
            instance = null;
        }
#endif

        public static AudioSource Play(Cue cue, int index, Vector3 position)
        {
            if (index < 0 || index >= cue.Clips.Length)
            {
                Debug.LogWarning("Index out of range, played random sound instead.");
                return cue.Sound.Play(position);
            }
            return new Sound(cue.Clips[index], cue.Parameters).Play(position);
        }

        public static AudioSource Play(Cue cue, int index, Transform transform = null)
        {
            if (index < 0 || index >= cue.Clips.Length)
            {
                Debug.LogWarning("Index out of range, played random sound instead.");
                return cue.Sound.Play(transform);
            }
            return new Sound(cue.Clips[index], cue.Parameters).Play(transform);
        }

        public static AudioSource Play(Sound sound)
        {
            if (instance == null)
            {
                instance = CreateInstance<AudioSourcePool>(100);
                instance.inspector.SetOnDestroy(() => instance = null);
            }

            AudioSource source = instance.Dequeue();
            source.SetSoundToSource(sound);
            source.Play();
            instance.DelayedEnqueue(source, source.clip.length * 1.1f);
            return source;
        }

        public static AudioSource Play(Sound sound, Vector3 position)
        {
            AudioSource source = Play(sound);
            source.transform.position = position;
            return source;
        }

        public static AudioSource Play(Sound sound, Transform transform = null)
        {
            AudioSource source = Play(sound);
            if (transform != null)
            {
                source.transform.parent = transform;
                source.transform.localPosition = Vector3.zero;
            }
            return source;
        }

        public static AudioSource PlayLooped(Sound sound, Vector3 position)
        {
            AudioSource source = Play(sound);
            source.loop = true;
            source.transform.position = position;
            return source;
        }

        public static AudioSource PlayLooped(Sound sound, Transform transform = null)
        {
            AudioSource source = Play(sound);
            source.loop = true;
            if (transform != null)
            {
                source.transform.parent = transform;
                source.transform.localPosition = Vector3.zero;
            }
            return source;
        }
    }
}
