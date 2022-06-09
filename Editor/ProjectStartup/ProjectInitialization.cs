using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorSettings;

namespace EtienneEditor
{
    public static class ProjectInitialization
    {
        private static async void LoadNewManifest()
        {
            await Packages.ReplacePackageFromGist("7d3a2b810075ec2900dfd31261b96880");
        }

        public static void DrawGUI()
        {
            if(GUILayout.Button("Create Default Folders")) Folders.CreateDefaultFolders();
            if(GUILayout.Button("Load Default Packages")) LoadNewManifest();
            EditorGUILayout.LabelField(new GUIContent("Install Unity Packages"), EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            if(GUILayout.Button("New Input System")) Packages.InstallUnityPackage("inputsystem");
            if(GUILayout.Button("Post Processing")) Packages.InstallUnityPackage("postprocessing");
            if(GUILayout.Button("Cinemachine")) Packages.InstallUnityPackage("cinemachine");
            EditorGUILayout.EndHorizontal();

            PlayerSettings.colorSpace = (ColorSpace)EditorGUILayout.EnumPopup(new GUIContent("Color Space"), PlayerSettings.colorSpace);

            projectGenerationRootNamespace = EditorGUILayout.DelayedTextField(new GUIContent("Root namespace"), projectGenerationRootNamespace);
            EditorGUILayout.LabelField(new GUIContent("Enter Play Mode Settings"), EditorStyles.boldLabel);
            enterPlayModeOptionsEnabled = EditorGUILayout.Toggle(new GUIContent("Enter Play Mode Options", "Enables options when entering Play Mode"), enterPlayModeOptionsEnabled);
            EditorGUI.BeginDisabledGroup(!enterPlayModeOptionsEnabled);
            EditorGUI.indentLevel++;
            EditorGUI.BeginChangeCheck();
            bool reloadDomain = EditorGUILayout.Toggle(new GUIContent("Reload Domain",
                "Enables Domain Reload when Entering Play Mode. Domain reload reinitializes game completely making loading behavior very close to the Player"),
                !enterPlayModeOptions.HasFlag(EnterPlayModeOptions.DisableDomainReload));
            if(EditorGUI.EndChangeCheck())
            {
                if(reloadDomain) enterPlayModeOptions &= ~EnterPlayModeOptions.DisableDomainReload;
                else enterPlayModeOptions |= EnterPlayModeOptions.DisableDomainReload;
            }
            EditorGUI.BeginChangeCheck();
            bool reloadScene = EditorGUILayout.Toggle(new GUIContent("Reload Scene",
                "Enables Scene Reload when Entering Play Mode. Scene reload makes loading behavior and performance characteristics very close to the Player"),
                !enterPlayModeOptions.HasFlag(EnterPlayModeOptions.DisableSceneReload));
            if(EditorGUI.EndChangeCheck())
            {
                if(reloadScene) enterPlayModeOptions &= ~EnterPlayModeOptions.DisableSceneReload;
                else enterPlayModeOptions |= EnterPlayModeOptions.DisableSceneReload;
            }
            EditorGUI.indentLevel--;
            EditorGUI.EndDisabledGroup();
        }
    }
}
