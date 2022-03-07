using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Etienne.Feedback
{
    [GameFeedback(255, 102, 255, "DoTween/2D/Fade")]
    public class DOFade : DGTweenEased
    {

        [SerializeField] private float _EndValue;

        public override IEnumerator Execute(GameEvent gameEvent)
        {
            base.Execute(gameEvent);
            GameObject go = GetGameObject(gameEvent);
            SpriteRenderer spriteRenderer = go.GetComponentInChildren<SpriteRenderer>();
            HandleOldTween(spriteRenderer);
            Tween tween = spriteRenderer.DOFade(_EndValue, duration).SetLoops(loops, loopType);
            if(ease == Ease.Unset) tween.SetEase(curveEase);
            else tween.SetEase(ease);
            yield break;
        }

        public override string ToString()
        {
            return $"Fade to {_EndValue}" + base.ToString();
        }
    }
}