using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Etienne.Feedback
{
    [GameFeedback(255, 102, 153, "DoTween/Scale")]
    public class DOScale : DGTweenEased
    {
        [Header("Parameters")]
        [SerializeField] private Vector3 _EndValue;

        public override IEnumerator Execute(GameEvent gameEvent)
        {
            GameObject go = GetGameObject(gameEvent);
            HandleOldTween(go.transform);
            Tween tween = go.transform.DOScale(_EndValue, duration).SetLoops(loops, loopType);
            if(ease == Ease.Unset) tween.SetEase(curveEase);
            else tween.SetEase(ease);
            tween.Play();
            yield break;
        }
        public override string ToString()
        {
            return $"Scale to {_EndValue}" + base.ToString();
        }
    }
}