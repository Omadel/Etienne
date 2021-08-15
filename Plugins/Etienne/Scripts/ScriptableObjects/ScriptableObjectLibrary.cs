#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using UnityEngine;

namespace Etienne {
    public class ScriptableObjectLibrary<T> : InitializableScriptableObject where T : ScriptableObject {
        public static List<T> Library => staticLibrary;
        private static List<T> staticLibrary;
        [SerializeField] private List<T> library;
        public override void Initialize() {
#if UNITY_EDITOR
            library.Clear();
            string[] assets = AssetDatabase.FindAssets($"t:{typeof(T).FullName}");
            foreach(string asset in assets) {
                string path = AssetDatabase.GUIDToAssetPath(asset);
                T item = AssetDatabase.LoadAssetAtPath<T>(path);
                library.Add(item);
                Debug.Log($"{item} Initialized");
            }
            staticLibrary = library;
#endif
        }
    }
}