using UnityEngine;

namespace Etienne.Feedback
{
    [GameFeedback(204, 0, 0, "Destroy")]
    public class Destroy : GameFeedback
    {
        protected override void Execute(GameObject gameObject)
        {
            UnityEngine.GameObject.Destroy(gameObject);
        }

        public override string ToString()
        {
            return "Destroy";
        }
    }
}