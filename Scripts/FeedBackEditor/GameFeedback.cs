using System.Collections;

namespace Etienne.Feedback {
    [System.Serializable]
    public abstract class GameFeedback {
        public virtual IEnumerator Execute(GameEvent gameEvent) {
            yield break;
        }
    }
}
