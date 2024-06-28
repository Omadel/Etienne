using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Etienne.Pools
{
    public class ComponentPool<T> : Pool<T> where T : Component
    {
        public ComponentPoolInspector Inspector { get; protected set; }
        private T prefab;
        private int maxSize;
        private string itemsName;
        private HideFlags hideFlags;

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
            if (!queue.TryDequeue(out T item))
            {
                Debug.LogWarning($"Trying to dequeue <b>{Inspector.gameObject.name}</b>'s empty queue. Increasing max size from <b>{maxSize}</b> to <b>{maxSize * 2}</b>." +
                    System.Environment.NewLine +
                    $"This may cause performance issues, if this warning happens often try manually increasing the pool's max size.", Inspector);
                EnqueueNewItems(maxSize, maxSize * 2);
                item = queue.Dequeue();
            }
            item.gameObject.hideFlags = HideFlags.None;
            item.gameObject.SetActive(true);
            Inspector.QueueCount--;
            return item;
        }

        public T Dequeue(float delay)
        {
            T item = Dequeue();
            DelayedEnqueue(item, delay);
            Inspector.EnqueueAfterDelay(this, item, delay);
            return item;
        }

        public T DelayedEnqueue(T item, float delay)
        {
            Inspector.EnqueueAfterDelay(this, item, delay);
            return item;
        }

        public override void Enqueue(T item)
        {
            if (item == null || queue.Contains(item)) return;
            item.transform.parent = Inspector.transform;
            item.gameObject.SetActive(false);
            item.gameObject.hideFlags = hideFlags;
            Inspector.QueueCount++;
            base.Enqueue(item);
        }

        protected override void CreatePool(int maxSize, params object[] additionnalParameters)
        {
            this.maxSize = maxSize;
            string poolName = new Regex(@"[`].").Replace(GetType().Name, " ");

            if (additionnalParameters.Length >= 1)
            {
                string paramName = (string)additionnalParameters[0];
                poolName += paramName == "" ? typeof(T).Name : paramName;
            }
            Inspector = new GameObject(poolName).AddComponent<ComponentPoolInspector>();
            Inspector.MaxSize = this.maxSize;

            if (additionnalParameters.Length >= 2) hideFlags = (HideFlags)additionnalParameters[1];
            if (additionnalParameters.Length >= 3)
            {
                prefab = (T)additionnalParameters[2];
                Inspector.Prefab = prefab;
            }
            if (additionnalParameters.Length >= 4 && (bool)additionnalParameters[3]) GameObject.DontDestroyOnLoad(Inspector.gameObject);

            itemsName = poolName.Replace("Pool", "");
            EnqueueNewItems(0, maxSize);
        }

        private void EnqueueNewItems(int oldMaxSize, int maxSize)
        {
            this.maxSize = maxSize;
            Inspector.MaxSize = maxSize;
            for (int i = oldMaxSize; i < maxSize; i++) EnqueueNewItem(i);
        }

        private void EnqueueNewItem(int index)
        {
            if (prefab == null)
            {
                GameObject go = new($"{itemsName} {index + 1:000}") { hideFlags = hideFlags };
                Enqueue(go.AddComponent<T>());
            }
            else
            {
                T instance = GameObject.Instantiate(prefab, Inspector.transform);
                instance.gameObject.name = $"{itemsName} {index + 1:000}";
                instance.gameObject.hideFlags = hideFlags;
                Enqueue(instance);
            }
        }
    }
    public class ComponentPoolInspector : MonoBehaviour
    {
        [ReadOnly] public int MaxSize;
        [ReadOnly] public int QueueCount;
        [ReadOnly] public Component Prefab;
        private Action onDestroy;

#if !UNITY_WEBGL
        internal void EnqueueAfterDelay<T>(ComponentPool<T> pool, T item, float delay) where T : Component => StartCoroutine(EnqueueDelayRoutine(pool, item, delay));

        private System.Collections.IEnumerator EnqueueDelayRoutine<T>(ComponentPool<T> pool, T item, float delay) where T : Component
        {
            yield return new WaitForSeconds(delay);
            pool.Enqueue(item);
        }
#else
        internal async void EnqueueAfterDelay<T>(ComponentPool<T> pool, T item, float delay) where T : Component
        {
            await System.Threading.Tasks.Task.Delay(Mathf.RoundToInt(delay * 1000));
            pool?.Enqueue(item);
        }
#endif
        public void SetOnDestroy(Action onDestroy)
        {
            this.onDestroy += onDestroy;
        }

        private void OnDestroy()
        {
            onDestroy?.Invoke();
        }
    }

}