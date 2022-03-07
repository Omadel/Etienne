using System.Collections;
using UnityEngine;

namespace Etienne.Feedback
{
    using Pools;
    [System.Serializable]
    [GameFeedback(0, 153, 255, "Audio/PlaySound")]
    public class PlaySound : GameFeedback
    {

        [SerializeField] private bool _IsAttached;
        [SerializeField] private Cue _Cue = new Cue(null);

        public override IEnumerator Execute(GameEvent gameEvent)
        {
            if(AudioSourcePool.Instance == null) AudioSourcePool.CreateInstance<AudioSourcePool>(100);
            AudioSourcePool.Play(_Cue, _IsAttached ? gameEvent.GameObject.transform : null);
            yield break;
        }

        public override string ToString()
        {
            return $"Play {_Cue}{(_IsAttached ? ", attached to transform" : "")}";
        }
    }
}
