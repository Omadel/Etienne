using UnityEngine;
using UnityEngine.Audio;

namespace Etienne
{
    [CreateAssetMenu(fileName = "SoundParameters", menuName = "Etienne/Audio/Sound Parameters")]
    public class SoundParametersScriptableObject : ScriptableObject
    {
        public SoundParameters Parameters => _Parameters;

        [SerializeField] private SoundParameters _Parameters = new SoundParameters(null);

        public void SetAudioMixerGroup(AudioMixerGroup audioMixerGroup) => _Parameters.SetAudioMixerGroup(audioMixerGroup);
        public void SetPriority(int priority) => _Parameters.SetPriority(priority);
        public void SetVolume(float volume) => _Parameters.SetVolume(volume);
        public void SetPitch(float pitch) => _Parameters.SetPitch(pitch);
        public void SetStereoPan(float stereoPan) => _Parameters.SetStereoPan(stereoPan);
        public void SetSpacialBlend(int spacialBlend) => _Parameters.SetSpacialBlend(spacialBlend);
        public void SetMinDistance(float minDistance) => _Parameters.SetMinDistance(minDistance);
        public void SetMaxDistance(float maxDistance) => _Parameters.SetMaxDistance(maxDistance);

        public static implicit operator SoundParameters(SoundParametersScriptableObject parameters)
        {
            return parameters._Parameters;
        }
    }
}
