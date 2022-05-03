using DG.Tweening;
using UnityEngine;

namespace Etienne.Feedback
{
    public abstract class DGTween : GameFeedback
    {
        private enum TweenType { Complete, Kill }

        [Header("Target")]
        [SerializeField] protected bool isChild;
        [SerializeField, Min(0)] protected int childIndex;
        [SerializeField] protected bool isGrandChild;
        [SerializeField, Min(0)] protected int grandChildIndex;

        [Header("Tween")]
        [SerializeField] private TweenType _OldTween;
        [SerializeField, Min(0), Tooltip("The duration of the tween")] protected float duration;
        [SerializeField, Tooltip("Number of cycles to play (-1 for infinite)"), Min(-1)]
        protected int loops;
        [SerializeField] protected LoopType loopType;

        protected void HandleOldTween(Component component)
        {
            if(_OldTween == TweenType.Complete)
            {
                component.DOComplete();
            } else
            {
                component.DOKill();
            }
        }
        protected GameObject GetGameObject(GameObject gameObject)
        {
            GameObject go = gameObject;
            if(isChild || isGrandChild) go = go.transform.GetChild(childIndex).gameObject;
            if(isGrandChild) go = go.transform.GetChild(grandChildIndex).gameObject;
            return go;
        }

        public override string ToString()
        {
            return $" in {duration}s" +
(loops > 0 ? $", for {loops} time, {loopType}" : (loops == -1 ? $", endlessly, {loopType}" : " once"));
        }
    }
}