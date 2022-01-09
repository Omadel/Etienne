using System.Collections;
using UnityEngine;

namespace Etienne.Feedback {
    [System.Serializable]
    public abstract class GameFeedback {
        [SerializeField] protected Color color = Color.white;

        protected void SetColor(int r, int g, int b) => color = new Color(r / 255f, g / 255f, b / 255f);

        public virtual IEnumerator Execute(GameEvent gameEvent) {
            yield break;
        }
    }
}
