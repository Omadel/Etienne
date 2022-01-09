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
        private static List<Type> types;
        private static List<GameFeedbackAttribute> typesAttributes;

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
            typesAttributes = new List<GameFeedbackAttribute>();

            List<Type> typeToRemove = new List<Type>();

            foreach(Type type in types) {
                if(Attribute.GetCustomAttribute(type, typeof(GameFeedbackAttribute)) is GameFeedbackAttribute attribute) typesAttributes.Add(attribute);
                else typesAttributes.Add(new GameFeedbackAttribute(0, 0, 0, "Skip"));//typeToRemove.Add(type);
            }
            for(int i = 0; i < types.Count; i++) {
                Debug.Log($"type: {types[i]}, name: { typesAttributes[i].MenuName}.");
            }
        }

        private static List<Type> FetchTypes<T>() where T : class {
            return (from domainAssembly in System.AppDomain.CurrentDomain.GetAssemblies()
                    from assemblyType in domainAssembly.GetTypes()
                    where assemblyType.IsSubclassOf(typeof(T))
                    select assemblyType).ToList();
        }

        private void AddDropDown(Rect rect, ReorderableList list) {
            GenericMenu menu = new GenericMenu();
            for(int i = 0; i < types.Count; i++) {
                int o = i;
                if(typesAttributes[i] == null)
                    menu.AddItem(new GUIContent("No Attribute: " + types[i].Name), false, () => { Debug.Log("No Attribute"); });
                else if(typesAttributes[i].MenuName != "Skip")
                    menu.AddItem(new GUIContent(typesAttributes[i].MenuName), false, () => AddItem(types[o]));
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
            EditorGUI.DrawRect(line, typesAttributes[index] == null ? Color.white : typesAttributes[index].Color);
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
