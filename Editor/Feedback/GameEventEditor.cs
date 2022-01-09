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
        //private static List<GameFeedbackAttribute> typesAttributes;
        private static Dictionary<string, GameFeedbackAttribute> typesAttributes;

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
            typesAttributes = new Dictionary<string, GameFeedbackAttribute>();

            foreach(Type type in types) {
                if(Attribute.GetCustomAttribute(type, typeof(GameFeedbackAttribute)) is GameFeedbackAttribute attribute)
                    typesAttributes.Add(type.Name, attribute);
                else typesAttributes.Add(type.Name, new GameFeedbackAttribute(0, 0, 0, "Skip"));
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
                if(typesAttributes.ElementAt(i).Value == null)
                    menu.AddItem(new GUIContent("No Attribute: " + types[i].Name), false, () => { Debug.Log("No Attribute"); });
                else if(typesAttributes.ElementAt(i).Value.MenuName != "Skip")
                    menu.AddItem(new GUIContent(typesAttributes.ElementAt(i).Value.MenuName), false, () => AddItem(types[o]));
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
            string type = element.type.Split('<', '>')[1];
            EditorGUI.DrawRect(line, typesAttributes[type] == null ? Color.white : typesAttributes[type].Color);
            if(!isFocused && !isActive) return;

            foreach(SerializedProperty child in GetChildren(element)) EditorGUILayout.PropertyField(child);

        }
        public static System.Type GetType(SerializedProperty property) {
            System.Type parentType = property.serializedObject.targetObject.GetType();
            System.Reflection.FieldInfo fi = parentType.GetField(property.propertyPath);
            return fi.FieldType;
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
