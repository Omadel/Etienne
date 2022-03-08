using UnityEngine;

namespace Etienne.Pools
{
    public class AudioSourcePool : ComponentPool<AudioSource>
    {
        public static async void Play(Etienne.Sound sound, Transform transform = null)
        {
            AudioSource source = instance.Dequeue();
            source.SetSoundToSource(sound);
            source.Play();
            if (transform != null)
            {
                source.transform.parent = transform;
                source.transform.localPosition = Vector3.zero;
            }
            while (source != null && source.isPlaying)
            {
                if (!Application.isPlaying) return;
                await System.Threading.Tasks.Task.Delay(100);
            }

            instance.Enqueue(source);
        }
    }
}