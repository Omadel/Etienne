using System.Collections.Generic;
using UnityEngine;

namespace Etienne.Pools
{
    public class Pool<T>
    {
        public static Pool<T> Instance => instance;

        protected static Pool<T> instance;
        protected Queue<T> queue;

        public static void CreateInstance<Y>(int maxSize, params object[] additionnalParameters) where Y : Pool<T>, new()
        {
            if (instance != null)
            {
                Debug.LogError("There is another Pool existing");
                return;
            }
            instance = new Y();
            instance.queue = new Queue<T>();
            instance.CreatePool(maxSize, additionnalParameters);
#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged -= instance.EditorDestroyInstance;
            UnityEditor.EditorApplication.playModeStateChanged += instance.EditorDestroyInstance;
#endif
        }

        protected Pool() { }

        protected virtual void CreatePool(int maxSize, params object[] additionnalParameters) { }

        public virtual T Peek()
        {
            return queue.Peek();
        }

        public virtual T Dequeue()
        {
            T item = queue.Dequeue();
            return item;
        }
        public virtual void Enqueue(T item)
        {
            queue.Enqueue(item);
        }

        public void ResetInstance()
        {
            instance = null;
        }

        public virtual void DestroyInstance()
        {
            ResetInstance();
        }

#if UNITY_EDITOR
        private void EditorDestroyInstance(UnityEditor.PlayModeStateChange state)
        {
            if (state != UnityEditor.PlayModeStateChange.ExitingPlayMode) return;
            ResetInstance();
        }
#endif
    }
}