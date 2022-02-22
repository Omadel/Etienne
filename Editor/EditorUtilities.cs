using System;
using System.Reflection;
using UnityEditor;

namespace EtienneEditor
{
    internal sealed class EditorUtilities
    {
        private static EditorWindow targetWindow
        {
            get
            {
                if(EditorWindow.focusedWindow != null && EditorWindow.focusedWindow.GetType().Name == "InspectorWindow") return EditorWindow.focusedWindow;
                if(EditorWindow.mouseOverWindow != null && EditorWindow.mouseOverWindow.GetType().Name == "InspectorWindow") return EditorWindow.mouseOverWindow;
                return null;
            }
        }

        private static Type inspectorWindowType;

        [MenuItem("Tools/Etienne/Clear Console &c", priority = 1000)]
        private static void ClearConsole()
        {
            Etienne.Utils.ClearLog();
        }

        [MenuItem("Tools/Etienne/Toggle Inspector Lock &e", priority = 1000)]
        private static void ToggleInspectorLock()
        {
            inspectorWindowType ??= Assembly.GetAssembly(typeof(Editor)).GetType("UnityEditor.InspectorWindow");
            PropertyInfo propertyInfo = inspectorWindowType.GetProperty("isLocked");
            bool value = (bool)propertyInfo.GetValue(targetWindow, null);
            propertyInfo.SetValue(targetWindow, !value, null);

            targetWindow.Repaint();
        }

        [MenuItem("Tools/Etienne/Toggle Inspector Lock &e", true, priority = 1000)]
        private static bool ValidateToggleInspectorLock()
        {
            return targetWindow != null;
        }

        [MenuItem("Tools/Etienne/Toggle Inspector Mode &d", priority = 1000)]
        private static void ToggleInspectorDebug()
        {
            inspectorWindowType ??= Assembly.GetAssembly(typeof(Editor)).GetType("UnityEditor.InspectorWindow");
            FieldInfo field = inspectorWindowType.GetField("m_InspectorMode", BindingFlags.NonPublic | BindingFlags.Instance);

            InspectorMode mode = (InspectorMode)field.GetValue(targetWindow);
            mode = mode == InspectorMode.Normal ? InspectorMode.Debug : InspectorMode.Normal;

            MethodInfo method = inspectorWindowType.GetMethod("SetMode", BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(targetWindow, new object[] { mode });

            targetWindow.Repaint();
        }

        [MenuItem("Tools/Etienne/Toggle Inspector Mode &d", true, priority = 1000)]
        private static bool ValidateToggleInspectorDebug()
        {
            return targetWindow != null;
        }
    }
}
