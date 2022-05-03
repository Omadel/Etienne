using DG.Tweening;
using UnityEngine;

namespace Etienne.Feedback
{
    [GameFeedback(255, 153, 204, "DoTween/Punch/Scale")]
    public class DOPunchScale : DGTween
    {
        [Header("Parameters")]
        [SerializeField, Tooltip("The punch strength (added to the Transform's current scale)")]
        private Vector3 _Punch;
        [SerializeField, Tooltip("Indicates how much will the punch vibrate")]
        private int _Vibrato = 10;
        [SerializeField, Tooltip("Represents how much (0 to 1) the vector will go beyond the starting size when bouncing backwards." +
            " 1 creates a full oscillation between the punch scale and the opposite scale, while 0 oscillates only between the punch " +
            "scale and the start scale")]
        private float _Elasticity = 1;

        protected override void Execute(GameObject gameObject)
        {
            GameObject go = GetGameObject(gameObject);
            HandleOldTween(go.transform);
            Tween tween = go.transform.DOPunchScale(_Punch, duration, _Vibrato, _Elasticity);
            tween.Play();
        }

        public override string ToString()
        {
            return $"Punch scale to {_Punch}" + base.ToString();
        }
    }
}