using System.Text.RegularExpressions;
using UnityEngine;

namespace Etienne.Pools
{
    public class ComponentPool<T> : Pool<T> where T : Component
    {
        protected GameObject _Parent;
        private T _Prefab;
        protected override void CreatePool(int maxSize, params object[] additionnalParameters)
        {
            string poolName = new Regex(@".[^`]$").Replace(GetType().Name, " ") + typeof(T).Name;
            _Parent = new GameObject(poolName);
            if (additionnalParameters.Length >= 1) _Parent.hideFlags = (HideFlags)additionnalParameters[0];
            if (additionnalParameters.Length >= 2) _Prefab = (T)additionnalParameters[1];
            GameObject.DontDestroyOnLoad(_Parent);
            string itemName = poolName.Replace("Pool", "");
            for (int i = 0; i < maxSize; i++)
            {
                if (_Prefab == null)
                {
                    GameObject go = new GameObject($"{itemName} {i + 1:000}") { hideFlags = _Parent.hideFlags };
                    Enqueue(go.AddComponent<T>());
                }
                else
                {
                    T instance = GameObject.Instantiate(_Prefab, _Parent.transform);
                    instance.gameObject.name = $"{itemName} {i + 1:000}";
                    instance.gameObject.hideFlags = _Parent.hideFlags;
                    Enqueue(instance);
                }
            }
        }
        /// <summary>
        /// Create a pool with a prefab as a reference.
        /// </summary>
        /// <param name="size">The desired count of GameObject·s to instantiate.</param>
        /// <param name="prefab">The desired prefab to instantiate.</param>
        /// <param name="hideFlags">Optionnal, the pool's HideFlags to avoid bloat.</param>
        /// <returns>The created pool</returns>
        public static ComponentPool<T> CreatePoolFromPrefab(int size, T prefab, HideFlags hideFlags = HideFlags.None)
        {
            ComponentPool<T> pool = new ComponentPool<T>() { queue = new System.Collections.Generic.Queue<T>(size) };
            pool.CreatePool(size, hideFlags, prefab);
            return pool;
        }

        public override T Dequeue()
        {
            T item = queue.Dequeue();
            item.gameObject.SetActive(true);
            return item;
        }

        public override void Enqueue(T item)
        {
            if (item == null) return;
            item.transform.parent = _Parent.transform;
            item.gameObject.SetActive(false);
            queue.Enqueue(item);
        }
    }
}