using UnityEngine;

namespace Etienne.Pools
{
    public class ComponentPool<T> : Pool<T> where T : Component
    {
        private GameObject _Parent;
        protected override void CreatePool(int maxSize, params object[] additionnalParameters)
        {
            _Parent = new GameObject(GetType().Name);
            GameObject.DontDestroyOnLoad(_Parent);
            for(int i = 0; i < maxSize; i++)
            {
                GameObject go = new GameObject(GetType().Name.Replace("Pool", ""));
                go.transform.parent = _Parent.transform;
                go.SetActive(false);
                queue.Enqueue(go.AddComponent<T>());
            }
        }

        public override T Dequeue()
        {
            T item = queue.Dequeue();
            item.gameObject.SetActive(true);
            return item;
        }

        public override void Enqueue(T item)
        {
            if(item == null) return;
            item.transform.parent = _Parent.transform;
            item.gameObject.SetActive(false);
            queue.Enqueue(item);
        }
    }
}