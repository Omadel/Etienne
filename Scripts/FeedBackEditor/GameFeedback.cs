using System.Collections;
using UnityEngine;

namespace Etienne.Feedback {
    [System.Serializable]
    public abstract class GameFeedback {
        [SerializeField] protected Color color = Color.white;
        public virtual IEnumerator Execute(GameEvent gameEvent) {
            yield break;
        }
    }
}
