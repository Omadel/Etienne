using System.Threading.Tasks;
using UnityEngine;

namespace Etienne.Feedback
{
    [System.Serializable]
    [GameFeedback(51, 204, 51, "Instantiate")]
    public class Instantiate : GameFeedback
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private bool isLocal = true, isChild;
        [SerializeField] private Vector3 position;

        public override async Task Execute(GameObject gameObject)
        {
            if(prefab == null) return;
            Vector3 position = isLocal ? gameObject.transform.position + this.position : this.position;
            GameObject.Instantiate(prefab, position, Quaternion.identity, isChild ? gameObject.transform : null);
            await Task.CompletedTask;
        }

        public override string ToString()
        {
            return $"Instantiate {(prefab == null ? "" : prefab.name)}";
        }
    }
}
