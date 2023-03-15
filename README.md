
# Etienne - Bundle of Utilities

Etienne is a collection of utilities to simplify common tasks in game development.

## Input Handling

To handle input, use Unity's PlayerInput. Once you have a InputReader instance, you can subscribe to C# events using the generated InputSender static class.

## Using the Etienne Namespace

To use Etienne, include the following namespace:

```using Etienne;```

## Audio Handling
 
Etienne provides the following audio utilities:

- Audio sound
- Audio Cue (random sound from a list of clips)
- Audio Pool

## Timers
<details><summary>Timers</summary>
 
Etienne includes a timer utility with the following methods:

- timer.Start(<float>): starts a timer with the given duration
- timer.OnComplete(<delegate>): sets a delegate to be called when the timer completes
- timer.OnUpdate(<delegate(float)>): sets a delegate to be called when the timer updates, passing the time spent as a parameter
- timer.Complete(): completes the timer prematurely
- timer.Kill(): completes the timer without triggering the OnComplete delegate
- timer.Pause(): pauses the timer
- timer.Play(): resumes the timer (automatic, use Play() only when the timer has been paused beforehand)
- timer.Restart(): restarts the timer (sets the time to 0)
 </details>
 
## Singletons

To create a singleton in Etienne, use the following syntax:

```class ClassName : Singleton<ClassName>```

If needed, you can override the Awake() method.

## Attributes
<details><summary>Attributes</summary>
 
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
  
## Range Class

<details><summary>Attributes</summary>
 
 
 The **Range** class represents a range with a minimum and a maximum value. It is a struct, which means it is a value type and is copied when passed around rather than being referenced.

### Constructors
- **Range(Range range)** - Creates a new **Range** object with the same minimum and maximum values as the provided Range.
- **Range(float max)** - Creates a new **Range** object with a minimum value of 0 and the provided maximum value.
- **Range(float min, float max)** - Creates a new **Range** object with the provided minimum and maximum values.
### Fields
- **public float Min** - The minimum value of the range.
- **public float Max** - The maximum value of the range.
### Properties
- **public static Range One** - A shorthand property for creating a **Range** object with minimum 0 and maximum 1.
- **public static Range Hundred** - A shorthand property for creating a **Range** object with minimum 0 and maximum 100.
### Methods
- **public bool Contains(float value)** - Returns true if the provided value is contained within the range.
- **public float Lerp(float value)** - Linearly interpolates between the minimum and maximum values of the range by the provided interpolation value.
- **public float Normalize(float value)** - Normalizes the provided value using the ratio between the minimum and maximum values of the range, resulting in a float between 0 and 1.
- **public float Clamp(float value)** - Clamps the provided value between the minimum and maximum values of the range. If the value is within the range, it is returned as is. Otherwise, if it is below the range, the minimum value is returned. If it is above the range, the maximum value is returned. If the provided value is not within the range, an exception is thrown.
- **public override string ToString()** - Returns a string representation of the range, with the minimum and maximum values formatted to two decimal places.
- **public string ToString(string format, IFormatProvider formatProvider = null)** - Returns a string representation of the range, with the minimum and maximum values formatted using the provided format and format provider.
### IFormattable Implementation
- **public string ToString(string format, IFormatProvider formatProvider = null)** - Returns a string representation of the range, with the minimum and maximum values formatted using the provided format and format provider.
 
</details>
  
