using UnityEditor;
using UnityEngine;

namespace EtienneEditor
{
    public class UtilityPanel : EditorWindow
    {

        private const string _WindowName = "Etienne Utility Panel";

        private bool _UseDefaultLoader;
        private bool _GoBackToCurrentScene;
        private bool _SaveCurrentScene;
        private int _DefaultSceneBuildIndex;
        private string[] _BuildSceneNames;

        [MenuItem("Tools/Etienne/" + _WindowName, priority = -100)]
        private static void ShowWindow()
        {
            UtilityPanel window = GetWindow<UtilityPanel>(true, _WindowName);
            window.minSize = new Vector2(369, 400);
            window.maxSize = window.minSize;
        }

        private void UpdateFields()
        {
            _UseDefaultLoader = PrefsKeys.UseDefaultLoader;
            _DefaultSceneBuildIndex = PrefsKeys.DefaultSceneBuildIndex;
            _GoBackToCurrentScene = PrefsKeys.GoBackToCurrentScene;
            _SaveCurrentScene = PrefsKeys.AutoSaveCurrentScene;

            _BuildSceneNames = new string[EditorBuildSettings.scenes.Length];
            for(int i = 0; i < _BuildSceneNames.Length; i++)
            {
                string name = System.IO.Path.GetFileNameWithoutExtension(EditorBuildSettings.scenes[i].path);
                _BuildSceneNames[i] = name;
            }
            if(_DefaultSceneBuildIndex >= _BuildSceneNames.Length)
            {
                _DefaultSceneBuildIndex = 0;
                PrefsKeys.DefaultSceneBuildIndex.SetValue(_DefaultSceneBuildIndex);
            }
        }
        private void OnEnable()
        {
            UpdateFields();
        }

        private void OnFocus()
        {
            UpdateFields();
        }

        private void OnValidate()
        {
            UpdateFields();
        }

        private void OnGUI()
        {
            DrawVersion();

            EditorGUI.BeginChangeCheck();
            _UseDefaultLoader = EditorGUILayout.BeginToggleGroup("Use default loader", _UseDefaultLoader);
            if(EditorGUI.EndChangeCheck())
            {
                PrefsKeys.UseDefaultLoader.SetValue(_UseDefaultLoader);
            }

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            DrawScenesBox();
            DrawSettings();

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndToggleGroup();
        }

        private void DrawSettings()
        {
            EditorGUI.BeginChangeCheck();
            _SaveCurrentScene = EditorGUILayout.Toggle("Auto save current scene", _SaveCurrentScene);
            if(EditorGUI.EndChangeCheck())
            {
                PrefsKeys.AutoSaveCurrentScene.SetValue(_SaveCurrentScene);
            }

            EditorGUI.BeginChangeCheck();
            _GoBackToCurrentScene = EditorGUILayout.Toggle("Go back to current scene", _GoBackToCurrentScene);
            if(EditorGUI.EndChangeCheck())
            {
                PrefsKeys.GoBackToCurrentScene.SetValue(_GoBackToCurrentScene);
            }
        }

        private void DrawScenesBox()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUILayout.LabelField("Default scene", EditorStyles.boldLabel);
            for(int i = 0; i < _BuildSceneNames.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();

                Rect rect = EditorGUILayout.GetControlRect();

                GUIStyle rightAlignedStyle = new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleRight, padding = new RectOffset(0, 5, 0, 0) };
                EditorGUI.LabelField(rect, new GUIContent(i.ToString()), rightAlignedStyle);

                EditorGUI.BeginChangeCheck();
                EditorGUI.Toggle(rect, _BuildSceneNames[i], _DefaultSceneBuildIndex == i);
                if(EditorGUI.EndChangeCheck())
                {
                    _DefaultSceneBuildIndex = i;
                    PrefsKeys.DefaultSceneBuildIndex.SetValue(_DefaultSceneBuildIndex);
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
        }

        private static void DrawVersion()
        {
            GUIStyle style = new GUIStyle(GUI.skin.label)
            {
                richText = true
            };

            EditorGUILayout.BeginHorizontal();

            string color = VersionChecker.IsUpToDate() ? "green" : "red";
            GUIContent label =
                new GUIContent($"Current version: <color={color}> {PrefsKeys.PackageCurrentVersion.GetValue()} </color>, Newest version: {PrefsKeys.PackageUrlVersion.GetValue()}");
            EditorGUILayout.LabelField(label, style);

            EditorGUILayout.EndHorizontal();

            if(GUILayout.Button("Check Package")) VersionChecker.CheckVersion();
        }
    }
}