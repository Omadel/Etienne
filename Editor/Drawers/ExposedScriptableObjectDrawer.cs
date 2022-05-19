using UnityEditor;
using UnityEngine;
namespace EtienneEditor
{
    [CustomPropertyDrawer(typeof(Etienne.ExposedScriptableObjectAttribute))]
    public class ExposedScriptableObjectAttributeDrawer : PropertyDrawer
    {
        private Editor editor = null;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label, true);

            if (!property.objectReferenceValue) return;

            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, GUIContent.none);

            if (!property.isExpanded) return;

            EditorGUI.indentLevel++;
            if (!editor) Editor.CreateCachedEditor(property.objectReferenceValue, null, ref editor);
            editor.OnInspectorGUI();
            EditorGUI.indentLevel--;
        }
    }
}
