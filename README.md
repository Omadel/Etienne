# Etienne
Bundle of all my utilities

## Use the Etienne namespace :
using Etienne;

## Audio handeling:
- Audio sound
- Audio Cue (Random sound between a list of clips)
- Audio Pool

## Timers:
- "Timer.Start(<float>);" to start a timer
- "timer.OnComplete(<delegate>);" to do something when the timer is completed
- "timer.OnUpdate(<delegate(float)>);" to do something when the timer updates, the parameter is the time spent of the timer
- "timer.Complete();" to prematurely complete the timer
- "timer.Kill();" to prematurely kill (Complete without OnComplete behaviour) the timer
- "timer.Pause();" to pause the timer
- "timer.Play();" to play the timer (automatic, use "Play()" uniquely when paused beforehand)
- "timer.Restart();" to restart the timer (set the time to 0 again)

## Singletons
- "class ClassName : Singleton<ClassName>" to declare a singleton
- "Awake()" is used, override it if needed
