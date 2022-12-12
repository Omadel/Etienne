using System;
using System.Collections.Generic;
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
            Timer timer = Start(duration, enQueueWhenCompleted);
            Manager.Add(timer);
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


        private Timer() { }

        private void Update()
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
            onComplete?.Invoke();
            isComplete = true;
            Kill();
        }

        public void Kill()
        {
            isPlaying = false;
            if (enQueueWhenCompleted) Manager.Remove(this);
        }

        private void OnApplicationQuit()
        {
            manager = null;
        }

        private class TimerManager : MonoBehaviour
        {
            private List<Timer> timers = new List<Timer>();
            private Queue<Timer> timerQueue = new Queue<Timer>();

            protected void Awake()
            {
                enabled = false;
            }

            public Timer GetTimer()
            {
                if (timerQueue.TryDequeue(out Timer timer)) return timer;
                return new Timer();
            }

            public void Add(Timer timer)
            {
                enabled = true;
                if (timers.Contains(timer)) return;
                timers.Add(timer);
            }

            public void Remove(Timer timer)
            {
                timers.Remove(timer);
                timerQueue.Enqueue(timer);
                if (timers.Count <= 0) enabled = false;
            }

            private void Update()
            {
                foreach (Timer timer in new List<Timer>(timers))
                {
                    timer.Update();
                }
            }
        }
    }
}
