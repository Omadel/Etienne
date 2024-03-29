﻿using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace EtienneEditor
{
    [InitializeOnLoad, ExecuteAlways]
    public static class SceneEnumGenerator
    {
        private const string PATH = "/Plugins/Etienne/BuildScenes.cs";
        private static bool hasChanged = true;
        static SceneEnumGenerator()
        {
            EditorBuildSettings.sceneListChanged += UpdateEnum;
            AssemblyReloadEvents.beforeAssemblyReload += UpdateEnumIfChanged;
        }

        private static void UpdateEnumIfChanged()
        {
            if (hasChanged) UpdateEnum(true);
        }

        private static void UpdateEnum() => UpdateEnum(false);
        private static void UpdateEnum(bool force)
        {
            hasChanged = true;

            string[] sceneNames = GetBuildSceneNames();
            if (!PrefsKeys.AutoGenerateBuildScenesEnum)
            {
                if (!force && !EditorUtility.DisplayDialog("Scenes In Build Settings Changed",
                    "Scenes in build settings have been change, do you want to update the BuildScene Enum ?",
                    "Yes", "Not now")) return;
            }

            StringBuilder sb = new StringBuilder("public enum BuildScenes {\n");
            for (int i = 0; i < sceneNames.Length; i++)
            {
                sb.AppendLine($"\t{sceneNames[i].Replace(" ", string.Empty)} = {i}{((i + 1) >= sceneNames.Length ? "" : ",")}");
            }
            sb.AppendLine("}");
            SaveEnumAsset(sb.ToString());
            hasChanged = false;
        }

        private static void SaveEnumAsset(string contents)
        {
            string path = Application.dataPath + PATH;
            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            File.WriteAllText(path, contents.ToString());
        }

        private static string[] GetBuildSceneNames()
        {
            List<string> names = new List<string>();
            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                if (!EditorBuildSettings.scenes[i].enabled) continue;
                names.Add(System.IO.Path.GetFileNameWithoutExtension(EditorBuildSettings.scenes[i].path));
            }
            return names.ToArray();
        }
    }
}