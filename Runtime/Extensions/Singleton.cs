using UnityEngine;

namespace Etienne
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance
        {
            get
            {
                if (instance != null) return instance;
                instance = FindObjectOfType<T>();
                if (instance != null) return instance;
                Debug.LogError($"No instance of {typeof(T)} found in the scene.");
                return instance;
            }
        }

        [Header("Singleton")]
        [SerializeField] protected bool isPersistant = false;

        private static T instance;

        protected Singleton() { }

        protected virtual void OnEnable()
        {
            if (isPersistant)
            {
                if (transform.parent != null)
                {
                    transform.parent = null;
                    Debug.LogWarning($"Singleton <b>\"{typeof(T).Name}\"</b> is a child, it has been removed from its parent.", gameObject);
                }
                DontDestroyOnLoad(this);
            }
        }

        protected virtual void OnDestroy()
        {
            if (!isPersistant) ResetInstance();
        }

        public static void ResetInstance() => instance = null;
    }
}