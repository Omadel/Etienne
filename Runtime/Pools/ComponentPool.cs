using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Etienne.Pools
{
    public class ComponentPool<T> : Pool<T> where T : Component
    {
        protected GameObject _Parent;
        private T _Prefab;

        protected ComponentPool() { }
        /// <summary>
        /// Create a pool with a prefab as a reference.
        /// </summary>
        /// <param name="size">The desired count of GameObject·s to instantiate.</param>
        /// <param name="prefab">The desired prefab to instantiate.</param>
        /// <param name="hideFlags">Optionnal, the pool's HideFlags to avoid bloat.</param>
        /// <returns>The created pool</returns>
        public ComponentPool(int size, T prefab, string name = "", HideFlags hideFlags = HideFlags.None, bool dontDestroyOnLoad = false)
        {
            queue = new Queue<T>(size);
            CreatePool(size, name, hideFlags, prefab, dontDestroyOnLoad);
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

        protected override void CreatePool(int maxSize, params object[] additionnalParameters)
        {
            string poolName = new Regex(@".[^`]$").Replace(GetType().Name, " ");

            if (additionnalParameters.Length >= 1)
            {
                string paramName = (string)additionnalParameters[0];
                poolName += paramName == "" ? typeof(T).Name : paramName;
            }
            _Parent = new GameObject(poolName);
            if (additionnalParameters.Length >= 2) _Parent.hideFlags = (HideFlags)additionnalParameters[1];
            if (additionnalParameters.Length >= 3) _Prefab = (T)additionnalParameters[2];
            if (additionnalParameters.Length >= 4 && (bool)additionnalParameters[3]) GameObject.DontDestroyOnLoad(_Parent);

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
    }
}