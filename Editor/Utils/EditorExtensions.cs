using UnityEditor;
using UnityEngine;

namespace EtienneEditor
{
    public static class EditorExtensions
    {
        public static Transform FindInChildren(this Transform transform, string name)
        {
            Transform[] children = transform.GetComponentsInChildren<Transform>();
            foreach(Transform child in children)
            {
                if(child.name == name) return child;
            }
            return null;
        }
        public static void SetExpandedRecursive(this GameObject go, bool expand = true)
        {
            System.Type type = typeof(EditorWindow).Assembly.GetType("UnityEditor.SceneHierarchyWindow");
            EditorWindow window = EditorWindow.GetWindow(type);
            System.Reflection.MethodInfo exprec = type.GetMethod("SetExpandedRecursive");
            exprec!.Invoke(window, new object[] { go.GetInstanceID(), true });
        }
    }
}