using DG.Tweening;
using UnityEngine;

namespace Etienne.Feedback
{
    [GameFeedback(255, 102, 255, "DoTween/2D/Fade")]
    public class DOFade : DGTweenEased
    {

        [SerializeField] private float _EndValue;

        protected override void Execute(GameObject gameObject)
        {
            GameObject go = GetGameObject(gameObject);
            SpriteRenderer spriteRenderer = go.GetComponentInChildren<SpriteRenderer>();
            HandleOldTween(spriteRenderer);
            Tween tween = spriteRenderer.DOFade(_EndValue, duration).SetLoops(loops, loopType);
            if(ease == Ease.Unset) tween.SetEase(curveEase);
            else tween.SetEase(ease);
        }

        public override string ToString()
        {
            return $"Fade to {_EndValue}" + base.ToString();
        }
    }
}