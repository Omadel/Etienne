using Etienne;
using UnityEditor;
using UnityEngine;

namespace EtienneEditor {
    [CustomPropertyDrawer(typeof(EnumToggleButtonsAttribute))]
    public class EnumToggleButtonsDrawer : PropertyDrawer {
        private Color normalColor = GUI.backgroundColor;
        private Color selectedColor = new Color32(102, 102, 102, 255);
        private string[] names;
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            names = property.enumDisplayNames;
            return EditorGUIUtility.wideMode ?
                base.GetPropertyHeight(property, label) :
                base.GetPropertyHeight(property, label) * Mathf.Ceil(names.Length / 5f);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            int value = property.enumValueIndex;
            Rect buttonRect = position;
            buttonRect.height = EditorGUIUtility.singleLineHeight;

            EnumToggleButtonsAttribute attr = attribute as EnumToggleButtonsAttribute;
            if(!attr.HideLabel) {
                Rect labelRect = position;
                Vector2 labelSize = EditorStyles.label.CalcSize(label);
                labelRect.height = EditorGUIUtility.singleLineHeight;
                labelRect.width = labelSize.x + 15;
                GUI.Label(labelRect, label);
                buttonRect.width -= labelRect.width;
                buttonRect.x += labelRect.width;
            }

            if(EditorGUIUtility.wideMode) {
                buttonRect.width /= names.Length;
                foreach(string name in names) {
                    DrawButton(property, name, name == names[value], buttonRect, System.Array.IndexOf(names, name));
                    buttonRect.x += buttonRect.width;
                }
            } else {
                buttonRect.width /= 5;
                for(int i = 0; i < names.Length; i += 5) {
                    for(int u = 0; u < 5; u++) {
                        if(i + u >= names.Length) {
                            return;
                        }

                        string name = names[i + u];
                        DrawButton(property, name, name == names[value], buttonRect, System.Array.IndexOf(names, name));
                        buttonRect.x += buttonRect.width;
                    }
                    buttonRect.x = position.x;
                    buttonRect.width = position.width / 5;
                    buttonRect.y += EditorGUIUtility.singleLineHeight;
                }
            }
        }

        private void DrawButton(SerializedProperty property, string name, bool selected, Rect buttonRect, int newIndex) {
            if(selected) {
                GUI.backgroundColor = selectedColor;
            }

            if(GUI.Button(buttonRect, name)) {
                property.enumValueIndex = newIndex;
            }

            if(selected) {
                GUI.backgroundColor = normalColor;
            }
        }
    }
}
