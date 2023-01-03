using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Etienne.Pools
{
    public class ComponentPool<T> : Pool<T> where T : Component
    {
        internal ComponentPoolInspector inspector;
        private T prefab;

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
            inspector.QueueCount--;
            return item;
        }

        public T Dequeue(float enqueueDelay)
        {
            T item = Dequeue();
            inspector.EnqueueAfterDelay(this, item, enqueueDelay);
            return item;
        }

        public override void Enqueue(T item)
        {
            if (item == null) return;
            item.transform.parent = inspector.transform;
            item.gameObject.SetActive(false);
            queue.Enqueue(item);
            inspector.QueueCount++;
        }

        protected override void CreatePool(int maxSize, params object[] additionnalParameters)
        {
            string poolName = new Regex(@".[^`]$").Replace(GetType().Name, " ");

            if (additionnalParameters.Length >= 1)
            {
                string paramName = (string)additionnalParameters[0];
                poolName += paramName == "" ? typeof(T).Name : paramName;
            }
            inspector = new GameObject(poolName).AddComponent<ComponentPoolInspector>();
            HideFlags paramHideFlags = HideFlags.None;
            if (additionnalParameters.Length >= 2) paramHideFlags = (HideFlags)additionnalParameters[1];
            if (additionnalParameters.Length >= 3)
            {
                prefab = (T)additionnalParameters[2];
                inspector.Prefab = prefab;
            }
            if (additionnalParameters.Length >= 4 && (bool)additionnalParameters[3]) GameObject.DontDestroyOnLoad(inspector.gameObject);

            string itemName = poolName.Replace("Pool", "");
            for (int i = 0; i < maxSize; i++)
            {
                if (prefab == null)
                {
                    GameObject go = new GameObject($"{itemName} {i + 1:000}") { hideFlags = paramHideFlags };
                    Enqueue(go.AddComponent<T>());
                }
                else
                {
                    T instance = GameObject.Instantiate(prefab, inspector.transform);
                    instance.gameObject.name = $"{itemName} {i + 1:000}";
                    instance.gameObject.hideFlags = paramHideFlags;
                    Enqueue(instance);
                }
            }
            inspector.QueueCount = maxSize;
        }
    }
    internal class ComponentPoolInspector : MonoBehaviour
    {
        [ReadOnly] public int QueueCount;
        [ReadOnly] public Component Prefab;

#if UNITY_WEBGL
        internal void EnqueueAfterDelay<T>(ComponentPool<T> pool, T item, float delay) where T : Component => StartCoroutine(EnqueueDelayRoutine(pool, item, delay));

        private IEnumerator EnqueueDelayRoutine<T>(ComponentPool<T> pool, T item, float delay) where T : Component
        {
            yield return new WaitForSeconds(delay);
            pool.Enqueue(item);
        }
#else
        internal async void EnqueueAfterDelay<T>(ComponentPool<T> pool, T item, float delay) where T : Component
        {
            await System.Threading.Tasks.Task.Delay(Mathf.RoundToInt(delay * 1000));
            pool.Enqueue(item);
        }
#endif
    }

}