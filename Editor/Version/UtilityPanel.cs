using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;

namespace EtienneEditor
{
    public class UtilityPanel : EditorWindow
    {

        private const string _WindowName = "Etienne Utility Panel";

        [MenuItem("Tools/Etienne/" + _WindowName, priority = -100)]
        private static void ShowWindow()
        {
            UtilityPanel window = GetWindow<UtilityPanel>(true, _WindowName);
            window.minSize = new Vector2(369, 400);
            window.maxSize = window.minSize;
        }

        private void OnGUI()
        {
            DrawVersion();
            DrawTabs();

        }

        private int selectIndex = 0;
        private string[] tabs = new string[] { "Project Initialization", "Default Loader", "Boot Strapper" };
        private void DrawTabs()
        {
            GUILayout.Space(10);
            BeginHorizontal();
            for(int i = 0; i < tabs.Length; i++)
            {
                EditorGUI.BeginDisabledGroup(i == selectIndex);
                if(GUILayout.Button(tabs[i], GUILayout.Height(35)))
                {
                    selectIndex = i;
                    break;
                }
                EditorGUI.EndDisabledGroup();
            }
            EndHorizontal();
            switch(selectIndex)
            {
                case 0:
                    ProjectInitialization.DrawGUI();
                    break;
                case 1:
                    DefaultLoader.DrawGUI();
                    break;
                case 2:
                    BootStrapper.DrawGUI();
                    break;
                default:
                    break;
            }
        }

        private static void DrawVersion()
        {
            GUIStyle style = new GUIStyle(GUI.skin.label) { richText = true };

            BeginHorizontal();

            string color = VersionChecker.IsUpToDate() ? EditorHexColor.Green : EditorHexColor.Red;
            GUIContent label = new GUIContent($"Current version: <color={color}> {PrefsKeys.PackageCurrentVersion} </color>, Newest version: {PrefsKeys.PackageUrlVersion}");
            LabelField(label, style);

            EndHorizontal();

            if(GUILayout.Button("Check Package")) VersionChecker.CheckVersion();
        }
    }
}