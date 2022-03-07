using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Etienne.Feedback {
    [CreateAssetMenu(menuName = "Etienne/GameEvent",order = 100)]
    public class GameEvent : ScriptableObject {
        public List<GameFeedback> Feedbacks => feedbacks;
        [SerializeReference] private List<GameFeedback> feedbacks;
        public GameObject GameObject => gameObject;
        private GameObject gameObject;

        public IEnumerator Execute(GameObject gameObject) {
            this.gameObject = gameObject;
            foreach(GameFeedback feedback in Feedbacks) yield return feedback.Execute(this);
        }
    }
}
