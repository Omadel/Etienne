using UnityEditor;

namespace Etienne {
    public static class InitializableScriptableObjectLoader {

        [InitializeOnLoadMethod]
        private static void Load() {
            string[] assets = AssetDatabase.FindAssets("t:InitializableScriptableObject");
            foreach(string asset in assets) {
                string path = AssetDatabase.GUIDToAssetPath(asset);
                InitializableScriptableObject staticSO = AssetDatabase.LoadAssetAtPath<InitializableScriptableObject>(path);
                staticSO.Initialize();
            }
        }
    }
}
