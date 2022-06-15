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

            Object systems = PrefsKeys.BootStrapperObjectID == 0 ? Resources.Load("Systems") :
                EditorUtility.InstanceIDToObject(PrefsKeys.BootStrapperObjectID);
            if(!systems)
            {
                Debug.LogWarning("There is no \"Systems\" prefab named in \"Assets/Resources\"" +
                   System.Environment.NewLine +
                    "Consider either disableling this option in \"Etienne Utility Panel\" or create one");
                return;
            }
            Object.DontDestroyOnLoad(Object.Instantiate(systems));
        }

        public static void DrawGUI()
        {
            PrefsKeys.UseBootStrapper = EditorGUILayout.Toggle(new GUIContent("Use Boot Strapper"), PrefsKeys.UseBootStrapper);
            Object obj = PrefsKeys.BootStrapperObjectID == 0 ? Resources.Load("Systems") : EditorUtility.InstanceIDToObject(PrefsKeys.BootStrapperObjectID);
            Object field = EditorGUILayout.ObjectField(new GUIContent("Systems"), obj, typeof(GameObject), false);
            PrefsKeys.BootStrapperObjectID = field ? field.GetInstanceID() : 0;
        }
    }
}