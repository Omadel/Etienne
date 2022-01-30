using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Etienne {
    [CustomPropertyDrawer(typeof(Range))]
    public class RangeDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty min = property.FindPropertyRelative("Min");
            SerializedProperty max = property.FindPropertyRelative("Max");

            position = EditorGUI.PrefixLabel(position, label);
            float fieldWidth = position.width;
            Rect floatFieldPosition = position;
            floatFieldPosition.width = 50f;
            min.floatValue = EditorGUI.FloatField(floatFieldPosition, min.floatValue);
            floatFieldPosition.x += fieldWidth - floatFieldPosition.width;
            max.floatValue = EditorGUI.FloatField(floatFieldPosition, max.floatValue);

            position.x += floatFieldPosition.width + EditorGUIUtility.standardVerticalSpacing;
            position.width = fieldWidth - floatFieldPosition.width * 2 - EditorGUIUtility.standardVerticalSpacing * 2;
            DrawMinMaxSlider(position, min, max);

            EditorGUI.EndProperty();
        }

        private void DrawMinMaxSlider(Rect position, SerializedProperty min, SerializedProperty max) {
            float minValue = min.floatValue;
            float maxValue = max.floatValue;
            Range range = GetRange();
            EditorGUI.MinMaxSlider(position, ref minValue, ref maxValue, range.Min, range.Max);
            min.floatValue = minValue;
            max.floatValue = maxValue;
        }

        private Range GetRange() {
            Range range = new Range(0, 1);
            MinMaxRangeAttribute attribute = fieldInfo.GetCustomAttributes<MinMaxRangeAttribute>().FirstOrDefault();
            if(attribute != null) range = attribute.Range;
            return range;
        }
    }
}
