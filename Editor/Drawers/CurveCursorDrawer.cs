using Etienne;
using System;
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
            if(serializedProperty.propertyType == SerializedPropertyType.Float ||
                serializedProperty.propertyType == SerializedPropertyType.Integer)
            {
                float min = GetMinValue(property), max = GetMaxValue(property);
                float x = serializedProperty.propertyType == SerializedPropertyType.Float ? serializedProperty.floatValue : serializedProperty.intValue;
                float normalizedX = x.Normalize(min, max);
                line.x = (maxX - minX) * normalizedX + minX;
            }
            EditorGUI.DrawRect(line, Color.red);
        }

        private float GetMinValue(SerializedProperty property)
        {
            CurveCursorAttribute attr = attribute as CurveCursorAttribute;
            if(string.IsNullOrEmpty(attr.MinField)) return attr.MinValue;
            SerializedProperty field = property.serializedObject.FindProperty(attr.MinField);
            if(field.propertyType == SerializedPropertyType.Float)
            {
                return field.floatValue;
            } else if(field.propertyType == SerializedPropertyType.Integer)
            {
                return field.intValue;
            } else
            {
                Debug.LogWarning("The min field parameter is not a float nor an integer", property.serializedObject.targetObject);
                return attr.MinValue;
            }
        }

        private float GetMaxValue(SerializedProperty property)
        {
            CurveCursorAttribute attr = attribute as CurveCursorAttribute;
            if(string.IsNullOrEmpty(attr.MaxField)) return attr.MaxValue;
            SerializedProperty field = property.serializedObject.FindProperty(attr.MaxField);
            if(field.propertyType == SerializedPropertyType.Float)
            {
                return field.floatValue;
            } else if(field.propertyType == SerializedPropertyType.Integer)
            {
                return field.intValue;
            } else
            {
                Debug.LogWarning("The min field parameter is not a float nor an integer", property.serializedObject.targetObject);
                return attr.MaxValue;
            }
        }
    }
}
