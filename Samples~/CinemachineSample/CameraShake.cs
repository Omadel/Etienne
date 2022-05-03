using Cinemachine;
using UnityEngine;
using static Cinemachine.CinemachineImpulseManager;

namespace Etienne.Feedback
{
    [GameFeedback(255, 51, 0, "Cinemachine/CameraShake")]
    public class CameraShake : GameFeedback
    {
        public float ShakeDuration => _TimeEnvelope.m_SustainTime + _TimeEnvelope.m_DecayTime + _TimeEnvelope.m_AttackTime;

        [SerializeField] private bool _IsAttached = false;
        [Header("Signal Shape")]
        [SerializeField] private SignalSourceAsset _RawSignal;
        [SerializeField] private float _AmplitudeGain = 1f;
        [SerializeField] private float _FrequencyGain = 1f;
        [SerializeField] private bool _Randomize = true;
        [SerializeField]
        private EnvelopeDefinition _TimeEnvelope = new EnvelopeDefinition() { m_SustainTime = .2f, m_DecayTime = .7f, m_ScaleWithImpact = true, };
        [Header("Spatial Range")]
        [SerializeField] private float _ImpactRadius = 100;
        [SerializeField] private ImpulseEvent.DirectionMode _DirectionMode;
        [SerializeField] private ImpulseEvent.DissipationMode _DissipationMode;
        [SerializeField] private float _DissipationDistance = 1000;
        [SerializeField] private float _PropagationSpeed = 345;

        protected override void Execute(GameObject gameObject)
        {
            if(ImpulseSourcePool.Instance == null) ImpulseSourcePool.CreateInstance<ImpulseSourcePool>(100);
            ImpulseSourcePool.GenerateImpulse(GetImpulseDefinition(), _IsAttached ? gameObject.transform : null);
        }

        private CinemachineImpulseDefinition GetImpulseDefinition()
        {
            return new CinemachineImpulseDefinition()
            {
                m_AmplitudeGain = _AmplitudeGain,
                m_DirectionMode = _DirectionMode,
                m_DissipationDistance = _DissipationDistance,
                m_DissipationMode = _DissipationMode,
                m_FrequencyGain = _FrequencyGain,
                m_ImpactRadius = _ImpactRadius,
                m_PropagationSpeed = _PropagationSpeed,
                m_Randomize = _Randomize,
                m_RawSignal = _RawSignal,
                m_TimeEnvelope = _TimeEnvelope
            };
        }

        public override string ToString()
        {
            return $"Shake camera for {ShakeDuration}s.";
        }
    }
}