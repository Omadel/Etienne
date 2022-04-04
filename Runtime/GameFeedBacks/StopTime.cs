using System.Threading.Tasks;
using UnityEngine;

namespace Etienne.Feedback
{
    [System.Serializable]
    [GameFeedback(255, 80, 80, "Stop Time")]
    public class StopTime : GameFeedback
    {
        [SerializeField] private float timer;

        public override async Task Execute(GameObject gameObject)
        {
            float oldScale = Time.timeScale;
            Time.timeScale = 0;
            await Task.Delay((int)(timer * 1000));
            Time.timeScale = oldScale;
        }

        public override string ToString()
        {
            return $"Stop time for {timer}s";
        }
    }
}
