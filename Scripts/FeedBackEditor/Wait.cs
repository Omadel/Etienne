using System.Collections;
using UnityEngine;

namespace Etienne.Feedback {
    [System.Serializable]
    [GameFeedback(255, 153, 51, "Wait")]
    public class Wait : GameFeedback {
        [SerializeField] private bool realtime;
        [SerializeField] private float timer;

        public override IEnumerator Execute(GameEvent gameEvent) {
            if(realtime) yield return new WaitForSecondsRealtime(timer);
            else yield return new WaitForSeconds(timer);
        }

        public override string ToString() => $"Wait {timer}s{(realtime ? ", in realtime" : "")}";
    }
}
