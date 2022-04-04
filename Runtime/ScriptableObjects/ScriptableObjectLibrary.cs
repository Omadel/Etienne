#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace Etienne {
    public class ScriptableObjectLibrary<T> : InitializableScriptableObject where T : Object {
        public static List<T> Library => staticLibrary;
        protected static List<T> staticLibrary;
        [SerializeField] protected List<T> library;
        protected virtual System.Type Type => typeof(T);

        public override void Initialize() {
#if UNITY_EDITOR
            library.Clear();
            StringBuilder message = new StringBuilder();
            message.Append($"{GetType().FullName} Initialize {Type.FullName} \r\n");
            string[] assets = AssetDatabase.FindAssets($"t:{Type.FullName}");
            foreach(string asset in assets) {
                string path = AssetDatabase.GUIDToAssetPath(asset);
                Object item = AssetDatabase.LoadAssetAtPath(path, Type);
                library.Add(item as T);
                message.Append($"{item}, ");
            }
            staticLibrary = library;
#endif
        }
    }
}