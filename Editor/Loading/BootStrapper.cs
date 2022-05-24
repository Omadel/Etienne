using UnityEditor;
using UnityEngine;

namespace EtienneEditor
{
    internal static class BootStrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Execute()
        {
            if(!PrefsKeys.UseBootStrapper) return;

            Object systems = Resources.Load("Systems");
            if(!systems)
            {
                Debug.LogWarning("There is no \"Systems\" named prefab in \"Assets/Resources\"" +
                   System.Environment.NewLine +
                    "Consider either disableling this option in \"Etienne Utility Panel\" or create one");
                return;
            }
            Object.DontDestroyOnLoad(Object.Instantiate(systems));
        }

        public static void DrawGUI()
        {
            PrefsKeys.UseBootStrapper = EditorGUILayout.Toggle(new GUIContent("Use Boot Strapper"), PrefsKeys.UseBootStrapper);
        }
    }
}