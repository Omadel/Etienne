using Etienne;
using UnityEditor;
using UnityEngine;

namespace EtienneEditor {
    [CustomPropertyDrawer(typeof(ForceDebugModeAttribute))]
    public class ForceDebugModeDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.PropertyField(position, property, label);
        }
    }
}