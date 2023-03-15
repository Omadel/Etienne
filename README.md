
# Etienne - Bundle of Utilities

Etienne is a collection of utilities to simplify common tasks in game development.

## Input Handling

To handle input, use Unity's PlayerInput. Once you have a InputReader instance, you can subscribe to C# events using the generated InputSender static class.

## Using the Etienne Namespace

To use Etienne, include the following namespace:

```cs 
using Etienne;
```

## Audio Handling
 
Etienne provides the following audio utilities:

- Audio sound
- Audio Cue (random sound from a list of clips)
- Audio Pool

<details>
	<summary><h2>Timer Class</h2></summary>
	<ul>
The Timer class provides a simple way to create a timer in Unity. It allows you to specify a duration and listen for updates and completion events. The Timer class uses a TimerManager to manage all active timers in the scene.

<li><details>
	<summary><h3>Constructors</h3></summary>
  
```cs
private Timer()
```
The constructor for the Timer class. It is private to ensure that only the TimerManager can create timers.
</br></br>
  
</details> </li>
<li><details>
	<summary><h3>Fields</h3></summary>
  
```cs   
public bool IsPlaying
```  
A read-only boolean that indicates whether the timer is currently playing.
</br></br>
```cs   
public float Duration
```  
A read-only float that indicates the duration of the timer.
</br></br>
```cs   
public float Time
```  
A read-only float that indicates the current time of the timer.
</br></br>
</details></li>
<li><details>
	<summary><h3>Methods</h3></summary>
  
```cs   
public Timer OnUpdate(Action<float> onUpdate)
```  
Adds a listener for the update event of the timer. The listener is called with the current time of the timer as a float parameter.
</br></br>

```cs   
public void Restart()
```   
Restarts the timer from the beginning.
</br></br>

```cs   
public void Pause()
```   
Pauses the timer.
</br></br>

```cs   
public void Play()
```   
Resumes the timer if it was paused.
</br></br>

```cs   
public void SetDuration(float duration)
```   
Sets the duration of the timer.
</br></br>

```cs   
public Timer OnComplete(Action onComplete)
```   
Adds a listener for the completion event of the timer. The listener is called when the timer reaches its duration.
</br></br>

```cs   
public void Complete()
```   
Completes the timer and invokes the completion event.
</br></br>

```cs   
public void Kill()
```   
Stops the timer and removes it from the TimerManager. If the timer was set to enqueue when completed, it is added to the timer queue.
</br></br>

```cs   
public static Timer Create(float duration, bool enQueueWhenCompleted = true)
```   
Creates a new timer and sets its duration. If enQueueWhenCompleted is true, the timer is added to the timer queue when completed.
</br></br>

```cs   
public static Timer Start(float duration, bool enQueueWhenCompleted = true)
```   
Creates a new timer and starts it. If enQueueWhenCompleted is true, the timer is added to the timer queue when completed.
</br></br>

</details> </li>
<li><details>
	<summary><h3>Usage</h3></summary>
To create a new Timer object, use the Create or Start methods:

```cs
// create a timer and set its duration to 5 seconds
Timer myTimer = Timer.Create(5f);

// start a timer and set its duration to 3 seconds
Timer.Start(3f);
```
You can add listeners to the update and completion events:

```cs
myTimer.OnUpdate((time) => {
    // do something with the current time of the timer
});

myTimer.OnComplete(() => {
    // do something when the timer completes
});
```
You can control the timer with the Pause, Play, Restart, and Kill methods:

```cs
myTimer.Pause();
myTimer.Play();
myTimer.Restart();
myTimer.Kill();
```
You can also get information about the timer using the IsPlaying, Duration, and Time properties:

```cs
bool isPlaying = myTimer.IsPlaying;
float duration = myTimer.Duration;
float time = myTimer.Time;
```
</details> </li>
</ul>
</details>
 
 
 
 
<details>
	<summary><h2>Singleton Class</h2></summary>
	<ul>
	
The Singleton class is an abstract class that provides a base implementation for creating singleton objects in Unity. A singleton is a design pattern that ensures that only one instance of a class can be created and accessed from anywhere in the code.

<li><details>
	<summary><h3>Constructors</h3></summary>

- **protected Singleton()** - The constructor for the Singleton class. It is protected to ensure that only derived classes can be instantiated.

</details> </li>
	
<li><details>
	<summary><h3>Fields</h3></summary>

- **public static T Instance** - The public getter for the singleton instance. It returns the instance of the derived class that is created.
- **protected bool isPersistant = false;** - A serialized bool that determines whether the singleton object should persist between scene loads.

</details></li>
	
<li><details>
	<summary><h3>Methods</h3></summary>

- **protected virtual void Awake()** - A virtual method that is called when the singleton object is initialized. It sets the singleton instance variable to the current object and destroys any other instances that exist. If the isPersistant flag is set to true, the object is marked as DontDestroyOnLoad.
protected virtual void OnDestroy() - A virtual method that is called when the singleton object is destroyed. If the isPersistant flag is not set to true, the singleton instance variable is set to null.
- **public static void ResetInstance()** - A public method that sets the singleton instance variable to null.
- **public void DestroyInstance()** - A public method that destroys the singleton object and sets the singleton instance variable to null.

</details> </li>
	
<li><details>
	<summary><h3>Usage</h3></summary>
 
To create a singleton object, derive a class from the Singleton class and provide the derived class as the generic type parameter, like this:
```cs
public class MySingletonClass : Singleton<MySingletonClass> {
    // ...
}
```
Access the singleton instance from anywhere in the code using the Instance property, like this:
`MySingletonClass.Instance.DoSomething();`
To make the singleton object persist between scene loads, set the isPersistant flag to true in the inspector or in code.
	
</details></li>
</ul></details>






<details>
	<summary><h2>Attributes</h2></summary>
 
### Requirement
Use the **[Requirement(typeof(Type))]** attribute to enforce a requirement for a specific **Component**. This attribute can be used on classes that inherit from MonoBehaviourWithRequirement.
If the requirement is not met, a warning message will be displayed in the inspector.
### CurveCursor
Use the **[CurveCursor(nameof(property))]** attribute to display a red cursor for an **AnimationCurve** property, the parameter property is the float controlling the cursor.
### EnumToggleButtons
Use the **[EnumToggleButtons]** attribute to display an enum as a set of toggle buttons. By default, this attribute will show the label, use **[EnumToggleButtons(true)]** to hide it.
### HideIf
Use the **[HideIf(nameof(property), value)]** attribute to hide a field if the specified property equals the specified value. This attribute can be used with enum and bool properties.
### ShowIf
Use the **[ShowIf(nameof(property), value)]** attribute to show a field if the specified property equals the specified value. This attribute can be used with enum and bool properties.
### PreviewSprite
Use the **[PreviewSprite]** attribute to display a sprite preview for a Sprite property.
### MinMaxRange
Use the **[MinMaxRange(min, max)]** attribute to limit a Range property to a specified minimum and maximum value.
### RangeLabelled
Use the **[RangeLabelled(min, max, labelMin, labelMax)]** attribute to display a labelled range slider for a float property.
### ReadOnly
Use the **[ReadOnly]** attribute to make a property read-only in the inspector.
</details>
  
<details>
	<summary><h2>Range Class</h2></summary>
	<ul>
 
The **Range** class represents a range with a minimum and a maximum value. It is a struct, which means it is a value type and is copied when passed around rather than being referenced.

<li><details>
	<summary><h3>Constructors</h3></summary>
  
- **Range(Range range)** - Creates a new **Range** object with the same minimum and maximum values as the provided Range.
- **Range(float max)** - Creates a new **Range** object with a minimum value of 0 and the provided maximum value.
- **Range(float min, float max)** - Creates a new **Range** object with the provided minimum and maximum values.
  
</details></li>

<li><details>
	<summary><h3>Fields</h3></summary>
   
- **public float Min** - The minimum value of the range.
- **public float Max** - The maximum value of the range.
</details></li>

<li><details>
	<summary><h3>Properties</h3></summary>
    
- **public static Range One** - A shorthand property for creating a **Range** object with minimum 0 and maximum 1.
- **public static Range Hundred** - A shorthand property for creating a **Range** object with minimum 0 and maximum 100.
</details></li>

<li><details>
	<summary><h3>Methods</h3></summary>
     
- **public bool Contains(float value)** - Returns true if the provided value is contained within the range.
- **public float Lerp(float value)** - Linearly interpolates between the minimum and maximum values of the range by the provided interpolation value.
- **public float Normalize(float value)** - Normalizes the provided value using the ratio between the minimum and maximum values of the range, resulting in a float between 0 and 1.
- **public float Clamp(float value)** - Clamps the provided value between the minimum and maximum values of the range. If the value is within the range, it is returned as is. Otherwise, if it is below the range, the minimum value is returned. If it is above the range, the maximum value is returned. If the provided value is not within the range, an exception is thrown.
- **public override string ToString()** - Returns a string representation of the range, with the minimum and maximum values formatted to two decimal places.
- **public string ToString(string format, IFormatProvider formatProvider = null)** - Returns a string representation of the range, with the minimum and maximum values formatted using the provided format and format provider.
</details></li>

<li><details>
	<summary><h3>IFormattable Implementation</h3></summary>
      
- **public string ToString(string format, IFormatProvider formatProvider = null)** - Returns a string representation of the range, with the minimum and maximum values formatted using the provided format and format provider.
</details></li>
 
<li><details>
	<summary><h2>MinMaxRangeAttribute Class</h2></summary><ul>
 
The **MinMaxRangeAttribute** class is an attribute that can be applied to fields in Unity scripts to indicate that they should be displayed as a range slider in the inspector. The range slider will have a minimum and maximum value based on the values provided to the attribute.

<li><details>
	<summary><h3>Constructors</h3></summary>
 
- **MinMaxRangeAttribute(float min, float max)** - Creates a new **MinMaxRangeAttribute** object with the provided minimum and maximum values.
</details></li>

<li><details>
	<summary><h3>Fields</h3></summary>
 
- **public readonly Range Range** - The range object that represents the minimum and maximum values of the range slider. This is initialized with the values provided to the constructor.
</details></li>

</li>

</ul></details>

<details>
	<summary><h3>Usage</h3></summary>
 
- To use the **MinMaxRangeAttribute**, apply it to a field in a Unity script with two float values, such as:
 
```cs
public class ExampleScript : MonoBehaviour {
	[MinMaxRange(0f, 10f)] public Vector2 speedRange;
}
```
 
This will display the **speedRange** field in the inspector as a range slider with a minimum value of 0 and a maximum value of 10.
</details>

  
