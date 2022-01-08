using Etienne.Feedback;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EtienneEditor {
    [CustomEditor(typeof(GameEvent))]
    public class GameEventEditor : Editor<GameEvent> {
        private SerializedProperty feedbacksProperty;
        private ReorderableList feedbacksList;
        private static Type[] types;
        private static List<string> typesName;

        private void OnEnable() {
            feedbacksProperty = serializedObject.FindProperty("feedbacks");
            feedbacksList = new ReorderableList(serializedObject, feedbacksProperty, true, false, true, true) {
                drawElementCallback = DrawListItems,
                onAddDropdownCallback = AddDropDown,
            };
        }

        [InitializeOnLoadMethod]
        private static void UpdateTypes() {
            types = FetchTypes<GameFeedback>();
            typesName = new List<string>();
            foreach(Type type in types) typesName.Add(type.Name);
        }

        private static Type[] FetchTypes<T>() where T : class {
            return (from domainAssembly in System.AppDomain.CurrentDomain.GetAssemblies()
                    from assemblyType in domainAssembly.GetTypes()
                    where assemblyType.IsSubclassOf(typeof(T))
                    select assemblyType).ToArray();
        }

        private void AddDropDown(Rect rect, ReorderableList list) {

            GenericMenu menu = new GenericMenu();
            for(int i = 0; i < types.Length; i++) {
                int o = i;
                menu.AddItem(new GUIContent(typesName[i]), false, () => AddItem(types[o]));
            }
            menu.ShowAsContext();

        }

        private void AddItem(Type type) {
            serializedObject.Update();
            Undo.RecordObject(Target, "Add feedback");
            Target.Feedbacks.Add(System.Activator.CreateInstance(type) as GameFeedback);
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawListItems(Rect rect, int index, bool isActive, bool isFocused) {
            SerializedProperty element = feedbacksProperty.GetArrayElementAtIndex(index);
            EditorGUI.LabelField(rect, Target.Feedbacks[index].ToString());
            Rect line = rect;
            line.x += line.width - 5;
            line.width = 5;
            line.height -= 2;
            line.y += 1;
            EditorGUI.DrawRect(line, element.FindPropertyRelative("color").colorValue);
            if(!isFocused && !isActive) return;

            foreach(SerializedProperty child in GetChildren(element)) {
                if(child.name == "color") continue;
                EditorGUILayout.PropertyField(child);
            }

        }

        private IEnumerable<SerializedProperty> GetChildren(SerializedProperty property) {
            SerializedProperty currentProperty = property.Copy();
            SerializedProperty nextProperty = property.Copy();
            nextProperty.Next(false);

            if(currentProperty.Next(true)) {
                do {
                    if(SerializedProperty.EqualContents(currentProperty, nextProperty)) break;
                    yield return currentProperty;
                } while(currentProperty.Next(false));
            }
        }

        public override void OnInspectorGUI() {

            serializedObject.Update();

            feedbacksList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
