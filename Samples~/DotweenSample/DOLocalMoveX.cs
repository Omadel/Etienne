using DG.Tweening;
using UnityEngine;

namespace Etienne.Feedback
{
    [GameFeedback(255, 0, 102, "DoTween/Local Move X")]
    public class DOLocalMoveX : DGTweenEased
    {
        [Header("Parameters")]
        [SerializeField] private float _EndValue;

        protected override void Execute(GameObject gameObject)
        {
            GameObject go = GetGameObject(gameObject);
            float endValue = _EndValue;
            HandleOldTween(go.transform);
            Tween tween = go.transform.DOLocalMoveX(endValue, duration).SetLoops(loops, loopType);
            if(ease == Ease.Unset) tween.SetEase(curveEase);
            else tween.SetEase(ease);
            tween.Play();
        }

        public override string ToString()
        {
            return $"Move X locally to {_EndValue}" + base.ToString();
        }
    }
}