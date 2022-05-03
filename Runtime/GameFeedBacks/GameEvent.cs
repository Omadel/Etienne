using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Etienne.Feedback
{
    [CreateAssetMenu(menuName = "Etienne/GameEvent", order = 100)]
    public class GameEvent : ScriptableObject
    {
        public List<GameFeedback> Feedbacks => feedbacks;
        [SerializeReference] private List<GameFeedback> feedbacks;

        public async Task ExecuteAsync(GameObject gameObject)
        {
#if UNITY_WEBGL
            Debug.LogWarning("WebGL doesn't support multi threaded behaviours, consider using ExecuteCoroutine in a Coroutine instead.");
#endif
            foreach(GameFeedback feedback in Feedbacks)
            {
                if(!Application.isPlaying || gameObject == null) return;
                await feedback.ExecuteAsync(gameObject);
            }
        }

        /// <summary>
        /// Use With StartCoroutine()
        /// </summary>
        public IEnumerator ExecuteCoroutine(GameObject gameObject)
        {
            foreach(GameFeedback feedback in feedbacks)
            {
                yield return feedback.ExecuteCoroutine(gameObject);
            }
            yield break;
        }
    }
}
