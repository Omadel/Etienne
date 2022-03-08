using Etienne;
using UnityEditor;
using UnityEngine;

namespace EtienneEditor
{
    [CustomPropertyDrawer(typeof(Etienne.RangeLabelledAttribute))]
    public class RangeLabelledDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight + 8f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PrefixLabel(position, label);
            float prefixWidth = EditorGUIUtility.labelWidth - 28f;
            position.x += prefixWidth;
            position.width -= prefixWidth;

            RangeLabelledAttribute attribute = (RangeLabelledAttribute)base.attribute;
            if (property.propertyType == SerializedPropertyType.Integer)
            {
                int value = EditorGUI.IntSlider(position, property.intValue, Mathf.RoundToInt(attribute.Min), Mathf.RoundToInt(attribute.Max));
                property.intValue = value;
            }
            else if (property.propertyType == SerializedPropertyType.Float)
            {
                float value = EditorGUI.Slider(position, property.floatValue, attribute.Min, attribute.Max);
                property.floatValue = value;
            }

            position.y += 8;
            position.width -= 55;
            GUIStyle style = new GUIStyle(EditorStyles.miniLabel)
            {
                alignment = TextAnchor.LowerLeft
            };
            Color color = style.normal.textColor;
            color.a = .4f;
            style.normal.textColor = color;
            GUIContent leftLabel = new GUIContent(attribute.LeftLabel);
            EditorGUI.LabelField(position, leftLabel, style);

            GUIContent rightLabel = new GUIContent(attribute.RightLabel);
            style.alignment = TextAnchor.LowerRight;
            EditorGUI.LabelField(position, rightLabel, style);
        }
    }
}
