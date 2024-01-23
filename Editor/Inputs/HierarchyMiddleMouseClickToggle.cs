using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class HierarchyMiddleMouseClickToggle
{
    static HierarchyMiddleMouseClickToggle()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyItemGUI;
    }

    private static void HandleHierarchyItemGUI(int instanceID, Rect selectionRect)
    {
        Event e = Event.current;
        if (e == null) return;
        if (e.rawType != EventType.MouseDown) return;
        if (e.isMouse && e.button == 2 && selectionRect.Contains(e.mousePosition))
        {
            GameObject go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

            if (go != null)
            {
                Undo.RecordObject(go, "Toggle Is Active");
                // Toggle the GameObject's active state.
                go.SetActive(!go.activeSelf);
                e.Use();
            }
        }
    }
}
