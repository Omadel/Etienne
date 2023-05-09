using System;
using UnityEngine;

namespace Etienne
{
    public class Timer
    {
        private static TimerManager Manager
        {
            get
            {
                if (manager != null) return manager;
                GameObject go = new GameObject(nameof(TimerManager));
                manager = go.AddComponent<TimerManager>();
                go.hideFlags = HideFlags.NotEditable;
                GameObject.DontDestroyOnLoad(go);
                return manager;
            }
        }

        private static TimerManager manager;

        public static Timer Create(float duration, bool enQueueWhenCompleted = true)
        {
            Timer timer = Manager.GetTimer();
            timer.SetDuration(duration);
            timer.enQueueWhenCompleted = enQueueWhenCompleted;
            return timer;
        }

        public static Timer Start(float duration, bool enQueueWhenCompleted = true)
        {
            Timer timer = Create(duration, enQueueWhenCompleted);
            timer.Restart();
            return timer;
        }

        public bool IsPlaying => isPlaying;
        public float Duration => duration;
        public float Time => time;

        private event Action onComplete;
        private event Action<float> onUpdate;

        private float duration;
        private float time;
        private bool isComplete, isPaused, isPlaying;
        private bool enQueueWhenCompleted;


        internal Timer() { }

        internal void Update()
        {
            if (isPaused) return;
            time += UnityEngine.Time.deltaTime;
            onUpdate?.Invoke(time);
            if (time < duration) return;
            Complete();
        }

        public Timer OnUpdate(Action<float> onUpdate)
        {
            this.onUpdate += onUpdate;
            return this;
        }

        public void Restart()
        {
            time = 0f;
            Manager.Add(this);
            isComplete = false;
            isPaused = false;
            isPlaying = true;
        }

        public void Pause()
        {
            isPaused = true;
            isPlaying = false;
        }

        public void Play()
        {
            isPaused = false;
            isPlaying = true;
        }

        public void SetDuration(float duration)
        {
            this.duration = duration;
        }

        public Timer OnComplete(Action onComplete)
        {
            this.onComplete += onComplete;
            return this;
        }

        public void Complete()
        {
            if (isComplete)
            {
                Debug.LogWarning("Timer is already completed");
                return;
            }
            isComplete = true;
            Kill();
            onComplete?.Invoke();
        }

        public void Kill() => Kill(enQueueWhenCompleted);
        public void Kill(bool enqueue)
        {
            isPlaying = false;
            if (enqueue)
            {
                onComplete = null;
                onUpdate = null;
            }
            Manager.Remove(this, enqueue);
        }

        private void OnApplicationQuit()
        {
            manager = null;
        }
    }
}
