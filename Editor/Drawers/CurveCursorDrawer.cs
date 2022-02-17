using Etienne;
using UnityEditor;
using UnityEngine;

namespace EtienneEditor
{
    [CustomPropertyDrawer(typeof(CurveCursorAttribute))]
    internal class CurveCursorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label);
            float maxX = position.x + position.width - 2f;
            float minX = EditorGUIUtility.labelWidth + 2 + position.x;
            Rect line = position;
            line.width = 1f;
            CurveCursorAttribute attr = attribute as CurveCursorAttribute;
            SerializedProperty serializedProperty = property.serializedObject.FindProperty(attr.FieldName);
            if(serializedProperty.propertyType == SerializedPropertyType.Float)
            {
                float min, max;
                if(string.IsNullOrEmpty(attr.MinField) || string.IsNullOrEmpty(attr.MaxField))
                {
                    min = attr.MinValue;
                    max = attr.MaxValue;
                } else
                {
                    SerializedProperty minField = property.serializedObject.FindProperty(attr.MinField);
                    SerializedProperty maxField = property.serializedObject.FindProperty(attr.MaxField);
                    if(minField.propertyType != SerializedPropertyType.Float &&
                        minField.propertyType != SerializedPropertyType.Integer)
                    {
                        Debug.LogWarning("The min field parameter is not a float nor an integer", property.serializedObject.targetObject);
                        min = attr.MinValue;
                    } else
                    {
                        min = minField.floatValue;
                    }
                    if(maxField.propertyType != SerializedPropertyType.Float &&
                        maxField.propertyType != SerializedPropertyType.Integer)
                    {
                        Debug.LogWarning("The max field parameter is not a float nor an integer", property.serializedObject.targetObject);
                        max = attr.MaxValue;
                    } else
                    {
                        max = maxField.floatValue;
                    }
                }

                float x = serializedProperty.floatValue;
                float normalizedX = x.Normalize(min, max);
                line.x = (maxX - minX) * normalizedX + minX;
            }
            EditorGUI.DrawRect(line, Color.red);
        }
    }
}
