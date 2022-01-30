using System;
using System.Globalization;

namespace Etienne {
    /// <summary>
    /// A range with a min and a max value.
    /// </summary>
    [System.Serializable]
    public struct Range : System.IFormattable {
        public float Min, Max;

        public Range(Range range) {
            Min = range.Min;
            Max = range.Max;
        }
        public Range(float max) {
            Min = 0;
            Max = max;
        }
        public Range(float min, float max) {
            Min = min;
            Max = max;
        }

        /// <summary>
        /// Shorthand for writing Range(0, 1).
        /// </summary>
        public static Range One => new Range(1f);

        /// <summary>
        /// Shorthand for writing Range(0, 100).
        /// </summary>
        public static Range Hundred => new Range(100f);

        /// <summary>
        /// Check if the provided value is contained withing the range.
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <returns></returns>
        public bool Contains(float value) => value >= Min && value <= Max;

        /// <summary>
        /// Linerarly interpolates between range's min and max by the value provided.
        /// </summary>
        /// <param name="value">The interpolation value between min and max.</param>
        /// <returns>The interpolated float result between the min and max.</returns>
        public float Lerp(float value) => Min - (Min - Max) * value;


        /// <summary>
        /// Normalize the provided value with the range's min and max ratio.
        /// </summary>
        /// <param name="value">The value to normalize.</param>
        /// <returns>A float between 0 and 1.</returns>
        public float Normalize(float value) => (value - Min) / (Max - Min);

        /// <summary>
        /// Clamps the provided value between the range's min and max. Returns the provided value if it is witinh min and max.
        /// </summary>
        /// <param name="value">the float value to restrict inside the range.</param>
        /// <returns>The float result between range's min and max.</returns>
        public float Clamp(float value) {
            if(Contains(value)) return value;
            if(value < Min) return Min;
            if(value > Max) return Max;
            throw new System.Exception();
        }

        public override string ToString() => ToString(null, null);
        public string ToString(string format, IFormatProvider formatProvider = null) {
            if(string.IsNullOrEmpty(format)) format = "F2";
            formatProvider ??= CultureInfo.InvariantCulture.NumberFormat;
            return $"({Min.ToString(format, formatProvider)}, {Max.ToString(format, formatProvider)})";
        }
    }


    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class MinMaxRangeAttribute : UnityEngine.PropertyAttribute {
        public readonly Range Range;
        public MinMaxRangeAttribute(float min, float max) {
            Range.Min = min;
            Range.Max = max;
        }
    }
}