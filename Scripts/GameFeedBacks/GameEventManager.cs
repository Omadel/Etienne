using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Etienne.Feedback {
    public class GameEventManager : Singleton<GameEventManager> {
        [SerializeField] private List<GameEvent> gameEvents;
        private Dictionary<string, GameEvent> events;

        protected override void Awake() {
            base.Awake();

            events = new Dictionary<string, GameEvent>(gameEvents.Count);

            foreach(GameEvent e in gameEvents) events.Add(e.name, e);
        }

        public static void PlayEvent(GameEvent e, GameObject gameObject) => Instance.StartCoroutine(Instance.PlayEventCoroutine(e, gameObject));

        public static void PlayEvent(string name, GameObject gameObject) => Instance.StartCoroutine(Instance.PlayEventCoroutine(name, gameObject));


        public IEnumerator PlayEventCoroutine(GameEvent e, GameObject gameObject) {
            yield return e.Execute(gameObject);
        }

        public IEnumerator PlayEventCoroutine(string name, GameObject gameObject) {
            yield return events[name].Execute(gameObject);
        }

    }
}
