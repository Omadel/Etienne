using UnityEngine;

namespace Etienne.Feedback
{
    [System.Serializable]
    [GameFeedback(0, 153, 255, "Audio/PlaySound")]
    public class PlaySound : GameFeedback
    {

        [SerializeField] private bool _IsAttached;
        [SerializeField] private Cue _Cue = new Cue(null);

        protected override void Execute(GameObject gameObject)
        {
            _Cue.Play(_IsAttached ? gameObject.transform : null);
        }

        public override string ToString()
        {
            return $"Play {_Cue}{(_IsAttached ? ", attached to transform" : "")}";
        }
    }
}
