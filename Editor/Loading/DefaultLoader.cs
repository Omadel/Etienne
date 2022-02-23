using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace EtienneEditor
{

    [InitializeOnLoad, ExecuteAlways]
    public static class DefaultLoader
    {

        static DefaultLoader()
        {
            EditorApplication.playModeStateChanged += LoadDefaultScene;
        }

        private static void LoadDefaultScene(PlayModeStateChange state)
        {
            if(!PrefsKeys.UseDefaultLoader) return;

            switch(state)
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
            int sceneBuildIndex = PrefsKeys.DefaultSceneBuildIndex;
            if(EditorSceneManager.GetActiveScene() == EditorSceneManager.GetSceneByBuildIndex(sceneBuildIndex)) return;

            Debug.Log($"<b>Etienne Default Loader</b> <color=green>used</color>, loading default scene{System.Environment.NewLine}To change the settings go to <b>Tools>Etienne>Etienne Utility Panel</b>");

            EditorSceneManager.LoadScene(sceneBuildIndex);
        }

        private static void GoBackToCurrentScene()
        {
            if(!PrefsKeys.GoBackToCurrentScene) return;

            string name = PrefsKeys.CurrentSceneName.GetValue();
            if(string.IsNullOrEmpty(name)) return;
            EditorSceneManager.LoadScene(name);
        }

        private static void SaveCurrentScene()
        {
            if(!PrefsKeys.AutoSaveCurrentScene)
            {
                if(!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) EditorApplication.ExitPlaymode();
            } else
            {
                EditorSceneManager.SaveOpenScenes();
            }

            if(!PrefsKeys.GoBackToCurrentScene) return;

            PrefsKeys.CurrentSceneName.SetValue(System.IO.Path.GetFileNameWithoutExtension(EditorSceneManager.GetActiveScene().path));
        }
    }
}