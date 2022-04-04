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

        public override async Task Execute(GameObject gameObject)
        {
            await Task.Delay((int)(timer * 1000 * (realtime ? 1 : Time.timeScale)));
        }

        public override string ToString()
        {
            return $"Wait {timer}s{(realtime ? ", in realtime" : "")}";
        }
    }
}
