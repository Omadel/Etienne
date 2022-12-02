using System.Collections.Generic;

namespace Etienne.Pools
{
    public class Pool<T>
    {
        protected Queue<T> queue;

        public static Y CreateInstance<Y>(int maxSize, params object[] additionnalParameters) where Y : Pool<T>, new()
        {
            Y instance = new Y { queue = new Queue<T>() };
            instance.CreatePool(maxSize, additionnalParameters);
            return instance;
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
    }
}