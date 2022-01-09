using UnityEditor;
using UnityEngine;

namespace EtienneEditor {
    public class UtilityPanel : EditorWindow {

        private const string windowName = "Etienne Utility Panel";

        private bool useDefaultLoader,
            goBackToCurrentScene,
            saveCurrentScene;
        private int defaultSceneBuildIndex;
        private string[] buildSceneNames;

        [MenuItem("Tools/Etienne/" + windowName)]
        private static void ShowWindow() {
            UtilityPanel window = GetWindow<UtilityPanel>(true, windowName);
            window.minSize = new UnityEngine.Vector2(369, 400);
            window.maxSize = window.minSize;
        }

        private void UpdateFields() {
            useDefaultLoader = EditorPrefs.GetBool(EditorPrefsKeys.UseDefaultLoader, false);
            defaultSceneBuildIndex = EditorPrefs.GetInt(EditorPrefsKeys.DefaultSceneBuildIndex, 0);
            goBackToCurrentScene = EditorPrefs.GetBool(EditorPrefsKeys.GoBackToCurrentScene, true);
            saveCurrentScene = EditorPrefs.GetBool(EditorPrefsKeys.AutoSaveCurrentScene, false);

            buildSceneNames = new string[EditorBuildSettings.scenes.Length];
            for(int i = 0; i < buildSceneNames.Length; i++) {
                string name = System.IO.Path.GetFileNameWithoutExtension(EditorBuildSettings.scenes[i].path);
                buildSceneNames[i] = name;
            }
            if(defaultSceneBuildIndex >= buildSceneNames.Length) {
                defaultSceneBuildIndex = 0;
                EditorPrefs.SetInt(EditorPrefsKeys.DefaultSceneBuildIndex, 0);
            }
        }
        private void OnEnable() => UpdateFields();
        private void OnFocus() => UpdateFields();
        private void OnValidate() => UpdateFields();

        private void OnGUI() {
            UpdatePackageGUI();

            EditorGUI.BeginChangeCheck();
            useDefaultLoader = EditorGUILayout.BeginToggleGroup("Use default loader", useDefaultLoader);
            if(EditorGUI.EndChangeCheck()) { EditorPrefs.SetBool(EditorPrefsKeys.UseDefaultLoader, useDefaultLoader); }

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Default scene", EditorStyles.boldLabel);
            UnityEngine.GUIStyle rightAlignedStyle = new UnityEngine.GUIStyle(EditorStyles.label) { alignment = UnityEngine.TextAnchor.MiddleRight, padding = new UnityEngine.RectOffset(0, 5, 0, 0) };
            for(int i = 0; i < buildSceneNames.Length; i++) {
                EditorGUILayout.BeginHorizontal();
                UnityEngine.Rect rect = EditorGUILayout.GetControlRect();

                EditorGUI.LabelField(rect, new UnityEngine.GUIContent(i.ToString()), rightAlignedStyle);

                EditorGUI.BeginChangeCheck();
                EditorGUI.Toggle(rect, buildSceneNames[i], defaultSceneBuildIndex == i);
                if(EditorGUI.EndChangeCheck()) { EditorPrefs.SetInt(EditorPrefsKeys.DefaultSceneBuildIndex, i); defaultSceneBuildIndex = i; }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();

            EditorGUI.BeginChangeCheck();
            saveCurrentScene = EditorGUILayout.Toggle("Auto save current scene", saveCurrentScene);
            if(EditorGUI.EndChangeCheck()) { EditorPrefs.SetBool(EditorPrefsKeys.AutoSaveCurrentScene, saveCurrentScene); }

            EditorGUI.BeginChangeCheck();
            goBackToCurrentScene = EditorGUILayout.Toggle("Go back to current scene", goBackToCurrentScene);
            if(EditorGUI.EndChangeCheck()) { EditorPrefs.SetBool(EditorPrefsKeys.GoBackToCurrentScene, goBackToCurrentScene); }

            EditorGUILayout.EndToggleGroup();
            EditorGUILayout.EndVertical();

        }

        private static void UpdatePackageGUI() {
            GUIStyle style = new GUIStyle(GUI.skin.label) {
                richText = true
            };
            string currentVersion = VersionChecker.CurrentVersion;
            string urlVersion = VersionChecker.UrlVersion;
            bool hasCurrent = currentVersion != "0.0.0";
            bool hasUrl = urlVersion != "0.0.0";
            if(hasCurrent && hasUrl) {
                EditorGUILayout.BeginHorizontal();
                string color = VersionChecker.IsUpToDate() ? "green" : "red";
                GUIContent label = new GUIContent((hasCurrent ? "" : $"Current version: <color={color}>" + currentVersion + "</color>")
                    + (!hasCurrent && !hasUrl ? ", " : "")
                    + (hasUrl ? "" : "Newest version: " + urlVersion));
                EditorGUILayout.LabelField(label, style);
                EditorGUILayout.EndHorizontal();
            }
            if(UnityEngine.GUILayout.Button("Check Package")) VersionChecker.CheckVersion();
        }
    }
}