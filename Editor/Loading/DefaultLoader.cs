using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace EtienneEditor {

    [InitializeOnLoad, ExecuteAlways]
    public static class DefaultLoader {

        static DefaultLoader() => EditorApplication.playModeStateChanged += LoadDefaultScene;

        private static void LoadDefaultScene(PlayModeStateChange state) {
            if(!EditorPrefs.GetBool(EditorPrefsKeys.UseDefaultLoader, false)) return;

            switch(state) {
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

        private static void LoadDefaultScene() {
            int sceneBuildIndex = EditorPrefs.GetInt(EditorPrefsKeys.DefaultSceneBuildIndex, 0);
            if(EditorSceneManager.GetActiveScene() == EditorSceneManager.GetSceneByBuildIndex(sceneBuildIndex)) return;

            Debug.LogWarning($"Default loader used, loading default scene{System.Environment.NewLine}To change the settings go to Tools>Etienne>Etienne Utility Panel");

            EditorSceneManager.LoadScene(sceneBuildIndex);
        }

        private static void GoBackToCurrentScene() {
            if(!EditorPrefs.GetBool(EditorPrefsKeys.GoBackToCurrentScene, true)) return;

            string name = EditorPrefs.GetString(EditorPrefsKeys.CurrentSceneName);
            if(string.IsNullOrEmpty(name)) return;
            EditorSceneManager.LoadScene(name);
        }

        private static void SaveCurrentScene() {
            if(!EditorPrefs.GetBool(EditorPrefsKeys.AutoSaveCurrentScene, false)) {
                if(!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) EditorApplication.ExitPlaymode();
            } else {
                EditorSceneManager.SaveOpenScenes();
            }

            if(!EditorPrefs.GetBool(EditorPrefsKeys.GoBackToCurrentScene, true)) return;

            EditorPrefs.SetString(EditorPrefsKeys.CurrentSceneName, System.IO.Path.GetFileNameWithoutExtension(EditorSceneManager.GetActiveScene().path));
        }
    }
}