using System.Collections;
using UnityEngine;

namespace Etienne.Feedback {
    [System.Serializable]
    public class PlaySound : GameFeedback {
        public PlaySound() => SetColor(0, 153, 255);

        [SerializeField] private bool isAttached;
        [SerializeField] private Sound sound = new Sound(null);

        public override IEnumerator Execute(GameEvent gameEvent) {
            AudioManager.Play(sound, isAttached ? gameEvent.GameObject.transform : null);
            yield break;
        }

        public override string ToString() => $"Play {(sound.Clip == null ? "nothing" : sound.Clip.name)}{(isAttached ? ", attached to transform" : "")}";
    }
}
