using System.Collections;

namespace Etienne.Feedback
{
    [System.Serializable]
    public abstract class GameFeedback
    {
        protected abstract void Execute(UnityEngine.GameObject gameObject);

        public virtual async System.Threading.Tasks.Task ExecuteAsync(UnityEngine.GameObject gameObject)
        {
            Execute(gameObject);
            await System.Threading.Tasks.Task.CompletedTask;
        }

        public virtual IEnumerator ExecuteCoroutine(UnityEngine.GameObject gameObject)
        {
            Execute(gameObject);
            yield break;
        }
    }
}
