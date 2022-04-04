using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Etienne.Feedback
{
    [CreateAssetMenu(menuName = "Etienne/GameEvent", order = 100)]
    public class GameEvent : ScriptableObject
    {
        public List<GameFeedback> Feedbacks => feedbacks;
        [SerializeReference] private List<GameFeedback> feedbacks;

        public async Task Execute(GameObject gameObject)
        {
            foreach(GameFeedback feedback in Feedbacks)
            {
                if(!Application.isPlaying || gameObject == null) return;
                await feedback.Execute(gameObject);
            }
        }
    }
}
