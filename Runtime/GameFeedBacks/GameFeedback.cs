using System.Threading.Tasks;

namespace Etienne.Feedback
{
    [System.Serializable]
    public abstract class GameFeedback
    {
        public abstract Task Execute(UnityEngine.GameObject gameObject);
    }
}
