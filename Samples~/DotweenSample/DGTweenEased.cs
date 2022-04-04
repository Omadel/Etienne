using DG.Tweening;
using UnityEngine;

namespace Etienne.Feedback
{
    public abstract class DGTweenEased : DGTween
    {
        [SerializeField] protected Ease ease = Ease.Unset;
        [SerializeField, Tooltip("If ease is unset the curve will take the priority")]
        protected AnimationCurve curveEase = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0), new Keyframe(1, 1) });
    }
}