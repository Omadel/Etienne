#if ENABLE_INPUT_SYSTEM
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EtienneEditor
{
    [InitializeOnLoad, ExecuteAlways]
    public static class InputsEditor
    {
        private static InputActionAsset inputActions;
        static InputsEditor()
        {
            AssemblyReloadEvents.beforeAssemblyReload -= GenerateInputs;
            AssemblyReloadEvents.beforeAssemblyReload += GenerateInputs;
        }

        private static void GenerateInputs()
        {
            string guid = AssetDatabase.FindAssets($"t:{typeof(InputActionAsset)}").FirstOrDefault();
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            inputActions = AssetDatabase.LoadAssetAtPath<InputActionAsset>(assetPath);
            if (inputActions == null) return;

            string path = $"{Application.dataPath}/Plugins/Etienne/Inputs";
            CreateInputPostProcessor($"{path}/Editor/InputsEditorPostprocessor.cs");
            CreateInputSender($"{path}/InputSender.cs");
            CreateInputReader($"{path}/InputReader.cs");
            CreateInputReaderEditor($"{path}/Editor/InputReaderEditor.cs");
        }

        private static void CreateInputReaderEditor(string path)
        {
            StringBuilder contents = new StringBuilder();
            contents.AppendLine("using UnityEditor;");
            contents.AppendLine("using UnityEngine;");
            contents.AppendLine("");
            string className = Path.GetFileNameWithoutExtension(path);
            contents.AppendLine($"[CustomEditor(typeof({className.Replace("Editor", "")}))]");
            contents.AppendLine($"public class {className} : Editor");
            contents.AppendLine("{");
            contents.AppendLine("   public override void OnInspectorGUI()");
            contents.AppendLine("   {");
            contents.AppendLine($"       if(GUILayout.Button(\"Refresh InputSender\")) EditorUtility.RequestScriptReload();");
            contents.AppendLine("   }");
            contents.AppendLine("}");
            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            File.WriteAllText(path, contents.ToString());
        }

        private static void CreateInputPostProcessor(string path)
        {
            StringBuilder contents = new StringBuilder();
            contents.AppendLine("using UnityEditor;");
            contents.AppendLine("using UnityEngine;");
            contents.AppendLine("using UnityEngine.InputSystem;");
            contents.AppendLine("");
            contents.AppendLine("[InitializeOnLoad, ExecuteAlways]");
            contents.AppendLine("internal static class InputsEditorPostprocessor");
            contents.AppendLine("{");
            contents.AppendLine("   static InputsEditorPostprocessor()");
            contents.AppendLine("   {");
            contents.AppendLine("       Selection.selectionChanged -= AddAdditionalData;");
            contents.AppendLine("       Selection.selectionChanged += AddAdditionalData;");
            contents.AppendLine("   }");
            contents.AppendLine("   private static void AddAdditionalData()");
            contents.AppendLine("   {");
            contents.AppendLine("       GameObject go = Selection.activeGameObject;");
            contents.AppendLine("       if (go == null) return;");
            contents.AppendLine("       EtienneEditor.InputsEditor.AddAdditionalData<PlayerInput, InputReader>(go);");
            contents.AppendLine("   }");
            contents.AppendLine("}");
            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            File.WriteAllText(path, contents.ToString());
        }

        private static void CreateInputSender(string path)
        {
            string className = Path.GetFileNameWithoutExtension(path);
            StringBuilder contents = new StringBuilder();
            contents.AppendLine("using System;");
            contents.AppendLine("using UnityEngine;");
            contents.AppendLine("using UnityEngine.InputSystem;");
            contents.AppendLine($"public static class {className}");
            contents.AppendLine("{");
            contents.AppendLine($"   static {className}()");
            contents.AppendLine("   {");
            contents.AppendLine("       Application.quitting += Reset;");
            contents.AppendLine("   }");
            List<string> fieldsToReset = new List<string>();
            foreach (InputActionMap actionMap in inputActions.actionMaps)
            {
                string actionMapclassName = actionMap.name.Replace(" ", "");
                string actionMapclassFieldName = $"{char.ToLower(actionMapclassName[0])}{actionMapclassName.Substring(1)}Events";
                fieldsToReset.Add(actionMapclassFieldName);
                contents.AppendLine("");
                contents.AppendLine($"   public static {actionMapclassName} {actionMapclassName}Events");
                contents.AppendLine("   {");
                contents.AppendLine("       get");
                contents.AppendLine("       {");
                contents.AppendLine($"           {actionMapclassFieldName} ??= new {actionMapclassName}();");
                contents.AppendLine($"           return {actionMapclassFieldName};");
                contents.AppendLine("       }");
                contents.AppendLine("   }");
                contents.AppendLine($"   private static {actionMapclassName} {actionMapclassFieldName};");
                contents.AppendLine($"   public class {actionMapclassName}");
                contents.AppendLine("   {");
                foreach (InputAction action in actionMap)
                {
                    string actionFieldName = $"On{action.name.Replace(" ", "")}";
                    contents.AppendLine($"      public event Action<InputValue> {actionFieldName};");
                    contents.AppendLine($"      public void Invoke{actionFieldName}(InputValue value = null) => {actionFieldName}?.Invoke(value);");
                }
                contents.AppendLine("   }");
            }
            contents.AppendLine("   private static void Reset()");
            contents.AppendLine("   {");
            foreach (string field in fieldsToReset)
            {
                contents.AppendLine($"      {field} = null;");
            }
            contents.AppendLine("   }");
            contents.AppendLine("}");
            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            File.WriteAllText(path, contents.ToString());
        }

        private static void CreateInputReader(string path)
        {
            string className = Path.GetFileNameWithoutExtension(path);
            StringBuilder contents = new StringBuilder();
            contents.AppendLine("using UnityEngine;");
            contents.AppendLine("using UnityEngine.InputSystem;");
            contents.AppendLine($"public class {className} : MonoBehaviour");
            contents.AppendLine("{");
            foreach (InputActionMap actionMap in inputActions.actionMaps)
            {
                string actionMapName = actionMap.name.Replace(" ", "");
                foreach (InputAction action in actionMap)
                {
                    string actionName = action.name.Replace(" ", "");
                    contents.AppendLine($"   private void On{actionName}(InputValue value) => InputSender.{actionMapName}Events.InvokeOn{actionName}(value);");
                }
            }
            contents.AppendLine("}");

            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            File.WriteAllText(path, contents.ToString());
        }

        /// <summary>Add the appropriate AdditionalData to the given GameObject and its children containing the original component</summary>
        /// <typeparam name="T">The type of the original component</typeparam>
        /// <typeparam name="AdditionalT">The type of the AdditionalData component</typeparam>
        /// <param name="go">The root object to update</param>
        public static void AddAdditionalData<T, AdditionalT>(GameObject go)
            where T : Component
            where AdditionalT : Component
        {
            Component[] components = go.GetComponentsInChildren<T>(true);
            foreach (Component c in components)
            {
                if (!c.TryGetComponent<AdditionalT>(out _))
                {
                    c.gameObject.AddComponent<AdditionalT>();
                }
            }
        }
    }
}
#endif
