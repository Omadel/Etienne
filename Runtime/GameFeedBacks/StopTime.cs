using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Etienne.Feedback
{
    [System.Serializable]
    [GameFeedback(255, 80, 80, "Stop Time")]
    public class StopTime : GameFeedback
    {
        [SerializeField] private float timer;

        protected override void Execute(GameObject gameObject)
	    {
            Time.timeScale = 0;
        }

        public override async Task ExecuteAsync(GameObject gameObject)
        {
	        float oldScale = Time.timeScale;
	        Execute(gameObject);
	        await Task.Delay((int)(timer * 1000));
            Time.timeScale = oldScale;
        }

        public override IEnumerator ExecuteCoroutine(GameObject gameObject)
        {
	        float oldScale = Time.timeScale;
	        Execute(gameObject);
	        yield return new WaitForSecondsRealtime(timer);
            Time.timeScale = oldScale;
        }

        public override string ToString()
        {
            return $"Stop time for {timer}s";
        }
    }
}
