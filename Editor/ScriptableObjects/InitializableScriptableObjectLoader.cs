using Etienne;
using UnityEditor;

namespace EtienneEditor
{
    internal static class InitializableScriptableObjectLoader
    {

        [InitializeOnLoadMethod]
        internal static void Load()
        {
            string[] assets = AssetDatabase.FindAssets("t:InitializableScriptableObject");
            for(int i = 0; i < assets.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(assets[i]);
                InitializableScriptableObject staticSO = AssetDatabase.LoadAssetAtPath<InitializableScriptableObject>(path);
                staticSO.Initialize();
            }
        }
    }
}
