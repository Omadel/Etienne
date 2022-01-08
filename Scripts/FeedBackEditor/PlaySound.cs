using System.Collections;
using UnityEngine;

namespace Etienne.Feedback {
    [System.Serializable]
    public class PlaySound : GameFeedback {
        public PlaySound() => color = new UnityEngine.Color(0 / 255f, 153 / 255f, 255 / 255f);

        [SerializeField] private bool isAttached;
        [SerializeField] private Sound sound = new Sound(null);

        public override IEnumerator Execute(GameEvent gameEvent) {
            AudioManager.Play(sound, isAttached ? gameEvent.GameObject.transform : null);
            yield break;
        }

        public override string ToString() => $"Play {(sound.Clip == null ? "nothing" : sound.Clip.name)}{(isAttached ? ", attached to transform" : "")}";
    }
}
