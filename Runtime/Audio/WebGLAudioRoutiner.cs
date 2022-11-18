#if UNITY_WEBGL
using System;
using System.Collections;
using UnityEngine;

namespace Etienne.Pools
{
    internal class WebGLAudioRoutiner : MonoBehaviour
    {
        internal void EnqueueSoundAfterClip(Pool<AudioSource> instance,AudioSource source)
        {
            StartCoroutine(AudioReturnRoutine(instance, source));
        }

        private IEnumerator AudioReturnRoutine(Pool<AudioSource> instance, AudioSource source)
        {
            yield return new WaitForSeconds(source.clip.length*1.1f);
            instance.Enqueue(source);
        }
    }
}
#endif