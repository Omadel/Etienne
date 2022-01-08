using System.Collections;
using UnityEngine;

namespace Etienne.Feedback {
    [System.Serializable]
    public class StopTime : GameFeedback {
        public StopTime() => color = new Color(255 / 255f, 80 / 255f, 80 / 255f);

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
