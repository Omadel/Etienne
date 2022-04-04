using Cinemachine;
using UnityEngine;

namespace Etienne
{
    using Pools;
    public class ImpulseSourcePool : ComponentPool<CinemachineImpulseSource>
    {
        public static async void GenerateImpulse(CinemachineImpulseDefinition definition, Transform transform = null)
        {
            CinemachineImpulseSource source = instance.Dequeue();
            source.m_ImpulseDefinition.m_RawSignal = definition.m_RawSignal;
            source.m_ImpulseDefinition.m_AmplitudeGain = definition.m_AmplitudeGain;
            source.m_ImpulseDefinition.m_FrequencyGain = definition.m_FrequencyGain;
            source.m_ImpulseDefinition.m_TimeEnvelope = definition.m_TimeEnvelope;
            source.m_ImpulseDefinition.m_ImpactRadius = definition.m_ImpactRadius;
            source.m_ImpulseDefinition.m_DirectionMode = definition.m_DirectionMode;
            source.m_ImpulseDefinition.m_DissipationMode = definition.m_DissipationMode;
            source.m_ImpulseDefinition.m_DissipationDistance = definition.m_DissipationDistance;
            source.m_ImpulseDefinition.m_PropagationSpeed = definition.m_PropagationSpeed;
            if(transform != null)
            {
                source.transform.parent = transform;
                source.transform.localPosition = Vector3.zero;
            }
            source.GenerateImpulse();
            float totalTime = definition.m_TimeEnvelope.m_DecayTime + definition.m_TimeEnvelope.m_AttackTime + definition.m_TimeEnvelope.m_SustainTime;
            await System.Threading.Tasks.Task.Delay((int)(totalTime * 1000));
            instance.Enqueue(source);
        }
    }
}