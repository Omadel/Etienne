using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace EtienneEditor
{

    internal sealed class EditorShortcut
    {
        private const string MenuName = " Tools/Etienne/";
        private const int MenuOrder = 1000;

        private static bool IsSelectingAnyGameObjects => Selection.gameObjects.Length > 0;
        private static EditorWindow CurrentInspectorWindow
        {
            get
            {
                if(EditorWindow.focusedWindow != null && EditorWindow.focusedWindow.GetType().Name == "InspectorWindow") return EditorWindow.focusedWindow;
                if(EditorWindow.mouseOverWindow != null && EditorWindow.mouseOverWindow.GetType().Name == "InspectorWindow") return EditorWindow.mouseOverWindow;
                return null;
            }
        }

        private static Type _inspectorWindowType;

        [MenuItem(MenuName + "Clear Console &c", false, MenuOrder)]
        private static void ClearConsole()
        {
            EditorUtils.ClearConsole();
        }

        [MenuItem(MenuName + "Toggle Inspector Lock &e", true, MenuOrder + 1)]
        private static bool ValidateToggleInspectorLock()
        {
            return CurrentInspectorWindow != null;
        }
        [MenuItem(MenuName + "Toggle Inspector Lock &e", false, MenuOrder + 1)]
        private static void ToggleInspectorLock()
        {
            _inspectorWindowType ??= Assembly.GetAssembly(typeof(Editor)).GetType("UnityEditor.InspectorWindow");
            PropertyInfo propertyInfo = _inspectorWindowType.GetProperty("isLocked");
            bool value = (bool)propertyInfo.GetValue(CurrentInspectorWindow, null);
            propertyInfo.SetValue(CurrentInspectorWindow, !value, null);

            CurrentInspectorWindow.Repaint();
        }

        [MenuItem(MenuName + "Toggle Inspector Mode &d", true, MenuOrder + 1)]
        private static bool ValidateToggleInspectorDebug()
        {
            return CurrentInspectorWindow != null;
        }
        [MenuItem(MenuName + "Toggle Inspector Mode &d", false, MenuOrder + 1)]
        private static void ToggleInspectorDebug()
        {
            _inspectorWindowType ??= Assembly.GetAssembly(typeof(Editor)).GetType("UnityEditor.InspectorWindow");
            FieldInfo field = _inspectorWindowType.GetField("m_InspectorMode", BindingFlags.NonPublic | BindingFlags.Instance);

            InspectorMode mode = (InspectorMode)field.GetValue(CurrentInspectorWindow);
            mode = mode == InspectorMode.Normal ? InspectorMode.Debug : InspectorMode.Normal;

            MethodInfo method = _inspectorWindowType.GetMethod("SetMode", BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(CurrentInspectorWindow, new object[] { mode });

            CurrentInspectorWindow.Repaint();
        }

        [MenuItem(MenuName + "Reset Transform &r", true, MenuOrder + 2)]
        private static bool ValidateResetTransform()
        {
            return Selection.gameObjects.Length > 0;
        }
        [MenuItem(MenuName + "Reset Transform &r", false, MenuOrder + 2)]
        private static void ResetTransform()
        {
            Transform[] selection = Selection.transforms;
            Undo.RecordObjects(selection, "Reset Transform");
            foreach(Transform transform in selection)
            {
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                transform.localScale = Vector3.one;
            }
        }

        [MenuItem(MenuName + "Reset Name &n", true, MenuOrder + 2)]
        private static bool ValidateResetName()
        {
            return IsSelectingAnyGameObjects;
        }
        [MenuItem(MenuName + "Reset Name &n", false, MenuOrder + 2)]
        private static void ResetName()
        {
            GameObject[] selection = Selection.gameObjects;
            Undo.RecordObjects(selection, "Reset Name");
            foreach(GameObject go in selection)
            {
                Rename(go);
            }
        }
        private static void Rename(GameObject go)
        {
            int start = go.name.LastIndexOf("(");
            int end = go.name.LastIndexOf(")");
            if(start == -1 || end == -1 || start >= end) return;
            go.name = go.name.Substring(0, start);
        }

        [MenuItem(MenuName + "Revert Prefab &%a", true, MenuOrder + 3)]
        private static bool ValidateRevertPrefab()
        {
            return IsSelectingAnyGameObjects;
        }
        [MenuItem(MenuName + "Revert Prefab &%a", false, MenuOrder + 3)]
        private static void RevertPrefab()
        {
            GameObject[] selection = Selection.gameObjects;
            Undo.RegisterCompleteObjectUndo(selection, "Revert Prefab");
            foreach(GameObject go in selection)
            {
                if(PrefabUtility.GetPrefabAssetType(go) == PrefabAssetType.NotAPrefab) continue;
                PrefabUtility.RevertPrefabInstance(go, InteractionMode.UserAction);
            }
        }

        [MenuItem(MenuName + "Apply Prefab &a", true, MenuOrder + 3)]
        private static bool ValidateSaveChangesToPrefab()
        {
            return IsSelectingAnyGameObjects;
        }
        [MenuItem(MenuName + "Apply Prefab &a", false, MenuOrder + 3)]
        private static void SaveChangesToPrefab()
        {
            GameObject[] selection = Selection.gameObjects;
            Undo.RegisterCompleteObjectUndo(selection, "Apply Prefab");
            foreach(GameObject go in selection)
            {
                if(PrefabUtility.GetPrefabAssetType(go) == PrefabAssetType.NotAPrefab) continue;
                PrefabUtility.ApplyPrefabInstance(go, InteractionMode.UserAction);
            }
        }
    }
}
