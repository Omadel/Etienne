using Etienne;
using UnityEditor;
using UnityEngine;

namespace EtienneEditor
{
    [CustomPropertyDrawer(typeof(HideIfAttribute))]
    internal class HideIfDrawer : PropertyDrawer
    {
        private bool hidden;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            HideIfAttribute attr = attribute as HideIfAttribute;
            hidden = true;

            string varName = attr.FieldName;
            if (!string.IsNullOrEmpty(varName))
            {
                SerializedProperty serializedProperty = property.serializedObject.FindProperty(attr.FieldName);
                hidden = serializedProperty.propertyType switch
                {
                    SerializedPropertyType.Boolean => serializedProperty.boolValue,
                    SerializedPropertyType.Enum => serializedProperty.enumValueIndex == attr.EnumValue,
                    SerializedPropertyType.ObjectReference => serializedProperty.objectReferenceValue != null,
                    _ => true,
                };
            }

            if (!hidden)
            {
                return base.GetPropertyHeight(property, label);
            }

            return -EditorGUIUtility.standardVerticalSpacing;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!hidden)
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }
    }
}
