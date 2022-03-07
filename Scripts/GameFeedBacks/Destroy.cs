using System.Collections;

namespace Etienne.Feedback
{
    [GameFeedback(204, 0, 0, "Destroy")]
    public class Destroy : GameFeedback
    {
        public override IEnumerator Execute(GameEvent gameEvent)
        {
            UnityEngine.GameObject.Destroy(gameEvent.GameObject);
            yield break;
        }
        public override string ToString() => "Destroy";
    }
}