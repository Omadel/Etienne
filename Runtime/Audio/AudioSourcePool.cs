using System;
using UnityEngine;

namespace Etienne.Pools
{
    public class AudioSourcePool : ComponentPool<AudioSource>
    {
        public static AudioSourcePool Instance => instance;

        private static AudioSourcePool instance;
#if UNITY_WEBGL
        static WebGLAudioRoutiner routiner;
#endif

        protected override void CreatePool(int maxSize, params object[] additionnalParameters)
        {
            base.CreatePool(maxSize, additionnalParameters);
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

        public static AudioSource Play(Sound sound, Vector3 position)
        {
            if (instance == null) instance = CreateInstance<AudioSourcePool>(100);

            AudioSource source = instance.Dequeue();
            source.SetSoundToSource(sound);
            source.Play();
            source.transform.position = position;
            EnqueueSoundAfterClip(source);
            return source;
        }

        public static AudioSource Play(Sound sound, Transform transform = null)
        {
            if (instance == null) instance = CreateInstance<AudioSourcePool>(100);

            AudioSource source = instance.Dequeue();
            source.SetSoundToSource(sound);
            source.Play();
            if (transform != null)
            {
                source.transform.parent = transform;
                source.transform.localPosition = Vector3.zero;
            }
            EnqueueSoundAfterClip(source);
            return source;
        }
#if UNITY_WEBGL
        static void EnqueueSoundAfterClip(AudioSource source)
        {
            if (routiner == null) routiner = ((AudioSourcePool)instance)._Parent.AddComponent<WebGLAudioRoutiner>();
            routiner.EnqueueSoundAfterClip(instance, source);
        }
#else
        private static async void EnqueueSoundAfterClip(AudioSource source)
        {
            await System.Threading.Tasks.Task.Delay((int)(source.clip.length * 1010));
            if (!Application.isPlaying || source == null) return;

            instance.Enqueue(source);
        }
#endif
        public static AudioSource PlayLooped(Sound sound, Vector3 position)
        {
            if (instance == null) instance = CreateInstance<AudioSourcePool>(100);

            AudioSource source = instance.Dequeue();
            source.SetSoundToSource(sound);
            source.loop = true;
            source.Play();
            source.transform.position = position;
            return source;
        }

        public static AudioSource PlayLooped(Sound sound, Transform transform = null)
        {
            if (instance == null) instance = CreateInstance<AudioSourcePool>(100);

            AudioSource source = instance.Dequeue();
            source.SetSoundToSource(sound);
            source.loop = true;
            source.Play();
            if (transform != null)
            {
                source.transform.parent = transform;
                source.transform.localPosition = Vector3.zero;
            }
            return source;
        }

    }
}
