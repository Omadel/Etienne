﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Etienne
{
    public class Timer
	{
		static TimerManager Manager  
		{
			get
			{
				if(manager != null) return manager;
				var go = new GameObject(nameof(TimerManager));
				manager = go.AddComponent<TimerManager>();
				go.hideFlags =	HideFlags.NotEditable;
				GameObject.DontDestroyOnLoad(go);
				return manager;	
			}
		}
		static TimerManager manager;
		
		public static Timer Start(float duration)
		{
			Timer timer = Manager.GetTimer();
			timer.SetDuration(duration);
			Manager.Add(timer);
			return timer;
		}
		
		public float Duration => duration;
		public float Time => time;
		
		event Action onComplete;
		event Action<float> onUpdate;
		
		float duration;
		float time;
		bool isComplete, isPaused;
		
		private Timer(){}
		
		void Update()
		{
			if(isPaused) return;
			time += UnityEngine.Time.deltaTime;
			onUpdate?.Invoke(time);
			if(time < duration) return;
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
		}
		
		public void Pause()
		{
			isPaused = true;
		}
		
		public void Play()
		{
			isPaused = false;
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
			if(isComplete)
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
			Manager.Remove(this);
		}
		
		void OnApplicationQuit()
		{
			manager = null;	
		}
		
		class TimerManager : MonoBehaviour
		{
			List<Timer> timers = new List<Timer>();
			Queue<Timer> timerQueue = new Queue<Timer>();
			
			protected void Awake()
			{
				enabled = false;
			}
			
			public Timer GetTimer()
			{
				if(timerQueue.TryDequeue(out Timer timer)) return timer;
				return new Timer();
			}
			
			public void Add(Timer timer)
			{
				enabled = true;
				if(timers.Contains(timer)) return;
				timers.Add(timer);
			}
			
			public void Remove(Timer timer)
			{
				timers.Remove(timer);
				timerQueue.Enqueue(timer);
				if(timers.Count <=0 ) enabled = false;
			}
			
			void Update()
			{
				foreach (Timer timer in new List<Timer>(timers))
				{
					timer.Update();
				}
			}
		}
    }
}
