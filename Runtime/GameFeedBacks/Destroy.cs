using System.Threading.Tasks;
using UnityEngine;

namespace Etienne.Feedback
{
    [GameFeedback(204, 0, 0, "Destroy")]
    public class Destroy : GameFeedback
    {
        public override async Task Execute(GameObject gameObject)
        {
            UnityEngine.GameObject.Destroy(gameObject);
            await Task.CompletedTask;
        }
        public override string ToString()
        {
            return "Destroy";
        }
    }
}