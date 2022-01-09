using System.Collections;
using UnityEngine;

namespace Etienne.Feedback {
    [System.Serializable]
    public class Instantiate : GameFeedback {
        public Instantiate() => SetColor(51, 204, 51);

        [SerializeField] private GameObject prefab;
        [SerializeField] private bool isLocal = true, isChild;
        [SerializeField] private Vector3 position;

        public override IEnumerator Execute(GameEvent gameEvent) {
            if(prefab == null) yield break;
            Vector3 position = isLocal ? gameEvent.GameObject.transform.position + this.position : this.position;
            GameObject.Instantiate(prefab, position, Quaternion.identity, isChild ? gameEvent.GameObject.transform : null);
            yield break;
        }

        public override string ToString() => $"Instantiate {(prefab == null ? "" : prefab.name)}";
    }
}
