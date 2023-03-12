using Etienne;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EtienneEditor {
    [CustomEditor(typeof(Path), true)]
    public class PathEditor : Editor<Path> {

        private SerializedProperty waypointsProperty;
        private List<int> selectedIndex = new List<int>();

        private void OnEnable()
        {
            waypointsProperty = serializedObject.FindProperty("waypoints");
        }

        private void OnSceneGUI()
        {
            HandleOutOfRange();
            HandleShortcuts();
            using (new Handles.DrawingScope(Color.white, Target.transform.localToWorldMatrix))
            {
                DrawCurve();
                DrawPositionHandles();
                DrawButtons();
            }
            serializedObject.ApplyModifiedProperties();
        }

        private void HandleShortcuts()
        {
            Event e = Event.current;
            if (e == null) return;
            if (e.isKey && e.type == EventType.KeyDown && selectedIndex.Count > 0)
            {
                if (e.control && e.keyCode == KeyCode.D)
                {
                    DuplicatePoints();
                    e.Use();
                }
                if (e.keyCode == KeyCode.Delete)
                {
                    DeletePoints();
                    e.Use();
                }
            }
        }

        private void DeletePoints()
        {
            selectedIndex.Sort();
            selectedIndex = selectedIndex.Distinct().ToList();
            if (waypointsProperty.arraySize <= 2) return;
            for (int i = selectedIndex.Count - 1; i >= 0; i--)
            {
                waypointsProperty.DeleteArrayElementAtIndex(selectedIndex[i]);
            }
            for (int i = selectedIndex.Count - 1; i >= 0; i--)
            {
                int newValue = selectedIndex[i] - (i + 1);
                if (newValue < 0) continue;
                else selectedIndex[i] = newValue;
            }
            selectedIndex.Sort();
            selectedIndex = selectedIndex.Distinct().ToList();
        }

        private void DuplicatePoints()
        {
            selectedIndex.Sort();
            selectedIndex = selectedIndex.Distinct().ToList();
            for (int i = selectedIndex.Count - 1; i >= 0; i--)
            {
                waypointsProperty.InsertArrayElementAtIndex(selectedIndex[i]);
            }
            for (int i = selectedIndex.Count - 1; i >= 0; i--)
            {
                selectedIndex[i] += i + 1;
            }
            selectedIndex.Sort();
            selectedIndex = selectedIndex.Distinct().ToList();
        }

        private void HandleOutOfRange()
        {
            for (int i = selectedIndex.Count - 1; i >= 0; i--)
            {
                if (selectedIndex[i] >= waypointsProperty.arraySize)
                {
                    selectedIndex.Remove(selectedIndex[i]);
                    Debug.Log($"Removing index {i}");
                }
            }
        }

        private void DrawPositionHandles()
        {
            SerializedProperty position;
            Vector3 delta = Vector3.zero;
            Quaternion rotation = Quaternion.identity;
            for (int i = 0; i < selectedIndex.Count; i++)
            {
                position = waypointsProperty.GetArrayElementAtIndex(selectedIndex[i]);
                delta += Handles.PositionHandle(position.vector3Value, rotation) - position.vector3Value;
            }
            for (int i = 0; i < selectedIndex.Count; i++)
            {
                position = waypointsProperty.GetArrayElementAtIndex(selectedIndex[i]);
                position.vector3Value += delta;
            }
        }

        private void DrawButtons()
        {
            SerializedProperty position;
            Quaternion rotation = Quaternion.identity;
            for (int i = 0; i < waypointsProperty.arraySize; i++)
            {
                position = waypointsProperty.GetArrayElementAtIndex(i);
                float size = HandleUtility.GetHandleSize(position.vector3Value) * 0.2f;
                Color color = selectedIndex.Contains(i) ? (Color)EtienneEditor.EditorColor.Orange : Color.white;

                using (new Handles.DrawingScope(color))
                {
                    if (Handles.Button(position.vector3Value, rotation, size, size*.5f, Handles.SphereHandleCap))
                    {
                        if (!Event.current.control) selectedIndex.Clear();
                        if (selectedIndex.Contains(i)) selectedIndex.Remove(i);
                        else selectedIndex.Add(i);
                    }
                }

                Handles.BeginGUI();

                Vector2 labelSize = Vector2.one * EditorGUIUtility.singleLineHeight;
                Vector2 labelPos = HandleUtility.WorldToGUIPoint(position.vector3Value);
                labelPos.x -= labelSize.x / 2;
                labelPos.y -= labelSize.y / 2;

                GUILayout.BeginArea(new Rect(labelPos, labelSize));

                GUIStyle style = new GUIStyle()
                {
                    alignment = TextAnchor.MiddleCenter
                };
                style.normal.textColor = Color.black;
                GUILayout.Label(new GUIContent(i.ToString(), "Waypoint " + i), style);

                GUILayout.EndArea();

                Handles.EndGUI();
            }
        }

        private void DrawCurve()
        {
            Handles.DrawAAPolyLine(Target.CatmullWaypoints);
            Color color = Handles.color;
            color.a = .2f;
            using (new Handles.DrawingScope(color, Handles.matrix))
            {
                Handles.DrawAAPolyLine(Target.LocalWaypoints);
            }
        }
    }
}