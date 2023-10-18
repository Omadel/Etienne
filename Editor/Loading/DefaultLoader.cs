using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EtienneEditor
{

    [InitializeOnLoad, ExecuteAlways]
    public static class DefaultLoader
    {
        private static bool useDefaultLoader
        {
            get => PrefsKeys.UseDefaultLoader;
            set => PrefsKeys.UseDefaultLoader = value;
        }
        private static bool goBackToCurrentScene
        {
            get => PrefsKeys.GoBackToCurrentScene;
            set => PrefsKeys.GoBackToCurrentScene = value;
        }
        private static bool saveCurrentScene
        {
            get => PrefsKeys.AutoSaveCurrentScene;
            set => PrefsKeys.AutoSaveCurrentScene = value;
        }
        private static int defaultSceneBuildIndex
        {
            get => PrefsKeys.DefaultSceneBuildIndex;
            set => PrefsKeys.DefaultSceneBuildIndex = value;
        }
        private static string[] buildSceneNames;


        static DefaultLoader()
        {
            EditorApplication.playModeStateChanged += LoadDefaultScene;
        }

        private static void LoadDefaultScene(PlayModeStateChange state)
        {
            if (!useDefaultLoader) return;

            switch (state)
            {
                case PlayModeStateChange.EnteredEditMode:
                    GoBackToCurrentScene();
                    break;
                case PlayModeStateChange.ExitingEditMode:
                    SaveCurrentScene();
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    LoadDefaultScene();
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    break;
            }
        }

        private static void LoadDefaultScene()
        {
            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(defaultSceneBuildIndex)) return;

            Debug.Log($"<b>Etienne Default Loader</b> <color=green>used</color>, loading default scene{System.Environment.NewLine}To change the settings go to <b>Tools>Etienne>Etienne Utility Panel</b>");

            SceneManager.LoadScene(defaultSceneBuildIndex);
        }

        private static void GoBackToCurrentScene()
        {
            if (goBackToCurrentScene) return;

            string name = PrefsKeys.CurrentSceneName;
            if (string.IsNullOrEmpty(name)) return;
            EditorSceneManager.OpenScene(name, OpenSceneMode.Single);
        }

        private static void SaveCurrentScene()
        {
            if (saveCurrentScene)
            {
                if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) EditorApplication.ExitPlaymode();
            }
            else
            {
                EditorSceneManager.SaveOpenScenes();
            }

            if (goBackToCurrentScene) return;

            PrefsKeys.CurrentSceneName = SceneManager.GetActiveScene().path;
        }

        private static string[] FetchSceneNames()
        {
            string[] names = new string[EditorBuildSettings.scenes.Length];
            for (int i = 0; i < names.Length; i++)
            {
                string name = System.IO.Path.GetFileNameWithoutExtension(EditorBuildSettings.scenes[i].path);
                names[i] = name;
            }
            if (defaultSceneBuildIndex >= names.Length)
            {
                defaultSceneBuildIndex = 0;
            }
            return names;
        }

        public static void DrawGUI()
        {
            useDefaultLoader = EditorGUILayout.BeginToggleGroup("Use default loader", useDefaultLoader);

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            DrawScenesBox();
            DrawSettings();

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndToggleGroup();
        }
        private static void DrawSettings()
        {
            saveCurrentScene = EditorGUILayout.Toggle("Auto save current scene", saveCurrentScene);

            goBackToCurrentScene = EditorGUILayout.Toggle("Go back to current scene", goBackToCurrentScene);
        }

        private static void DrawScenesBox()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUILayout.LabelField("Default scene", EditorStyles.boldLabel);

            if (buildSceneNames == null || buildSceneNames.Length != SceneManager.sceneCount) buildSceneNames = FetchSceneNames();
            for (int i = 0; i < buildSceneNames.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();

                Rect rect = EditorGUILayout.GetControlRect();

                GUIStyle rightAlignedStyle = new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleRight, padding = new RectOffset(0, 5, 0, 0) };
                EditorGUI.LabelField(rect, new GUIContent(i.ToString()), rightAlignedStyle);

                EditorGUI.BeginChangeCheck();
                EditorGUI.Toggle(rect, buildSceneNames[i], defaultSceneBuildIndex == i);
                if (EditorGUI.EndChangeCheck())
                {
                    defaultSceneBuildIndex = i;
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
        }

    }
}