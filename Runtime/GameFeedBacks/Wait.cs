using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Etienne.Feedback
{
    [System.Serializable]
    [GameFeedback(255, 153, 51, "Wait")]
    public class Wait : GameFeedback
    {
        [SerializeField] private bool realtime;
        [SerializeField] private float timer;

        protected override void Execute(GameObject gameObject) { }

        public override async Task ExecuteAsync(GameObject gameObject)
        {
            await Task.Delay((int)(timer * 1000 * (realtime ? 1 : Time.timeScale)));
        }

        public override IEnumerator ExecuteCoroutine(GameObject gameObject)
        {
            yield return new WaitForSeconds(timer);
        }

        public override string ToString()
        {
            return $"Wait {timer}s{(realtime ? ", in realtime" : "")}";
        }
    }
}
