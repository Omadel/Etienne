using DG.Tweening;
using UnityEngine;

namespace Etienne.Feedback
{
    [GameFeedback(255, 102, 153, "DoTween/Scale")]
    public class DOScale : DGTweenEased
    {
        [Header("Parameters")]
        [SerializeField] private Vector3 _EndValue;

        protected override void Execute(GameObject gameObject)
        {
            GameObject go = GetGameObject(gameObject);
            HandleOldTween(go.transform);
            Tween tween = go.transform.DOScale(_EndValue, duration).SetLoops(loops, loopType);
            if(ease == Ease.Unset) tween.SetEase(curveEase);
            else tween.SetEase(ease);
            tween.Play();
        }

        public override string ToString()
        {
            return $"Scale to {_EndValue}" + base.ToString();
        }
    }
}