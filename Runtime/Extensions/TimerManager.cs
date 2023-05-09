using System.Collections.Generic;
using UnityEngine;

namespace Etienne
{

    public class TimerManager : MonoBehaviour
    {
        public List<Timer> Timers => _timers;
        public Queue<Timer> Queue => _timerQueue;
        [SerializeField] private List<Timer> _timers = new List<Timer>();
        private Queue<Timer> _timerQueue = new Queue<Timer>();

        protected void Awake()
        {
            enabled = false;
        }

        public Timer GetTimer()
        {
            if (_timerQueue.TryDequeue(out Timer timer)) return timer;
            return new Timer();
        }

        public void Add(Timer timer)
        {
            enabled = true;
            if (_timers.Contains(timer)) return;
            _timers.Add(timer);
        }

        public void Remove(Timer timer, bool enQueueWhenCompleted)
        {
            _timers.Remove(timer);
            if (enQueueWhenCompleted) _timerQueue.Enqueue(timer);
            if (_timers.Count <= 0) enabled = false;
        }

        private void Update()
        {
            foreach (Timer timer in new List<Timer>(_timers))
            {
                timer.Update();
            }
        }
    }
}
