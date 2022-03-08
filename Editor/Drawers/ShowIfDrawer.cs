using Etienne;
using UnityEditor;
using UnityEngine;

namespace EtienneEditor
{
    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    internal class ShowIfDrawer : PropertyDrawer
    {
        private bool showed;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            ShowIfAttribute attr = attribute as ShowIfAttribute;
            showed = true;

            string varName = attr.FieldName;
            if (!string.IsNullOrEmpty(varName))
            {
                SerializedProperty serializedProperty = property.serializedObject.FindProperty(attr.FieldName);
                showed = serializedProperty.propertyType switch
                {
                    SerializedPropertyType.Boolean => serializedProperty.boolValue,
                    SerializedPropertyType.Enum => serializedProperty.enumValueIndex == attr.EnumValue,
                    SerializedPropertyType.ObjectReference => serializedProperty.objectReferenceValue != null,
                    _ => true,
                };
            }

            if (showed)
            {
                return base.GetPropertyHeight(property, label);
            }

            return -EditorGUIUtility.standardVerticalSpacing;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (showed)
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }
    }
}
