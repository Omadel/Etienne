using System.Collections;
using UnityEngine;

namespace Etienne.Feedback {
    [System.Serializable]
    public class StopTime : GameFeedback {
        public StopTime() => SetColor(255, 80, 80);

        [SerializeField] private float timer;

        public override IEnumerator Execute(GameEvent gameEvent) {
            float oldScale = Time.timeScale;
            Time.timeScale = 0;
            yield return new WaitForSecondsRealtime(timer);
            Time.timeScale = oldScale;
        }

        public override string ToString() => $"Stop time for {timer}s";
    }
}
