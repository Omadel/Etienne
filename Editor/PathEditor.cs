using Etienne;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EtienneEditor {
    [CustomEditor(typeof(Path))]
    public class PathEditor : Editor<Path> {

        private void OnEnable() {
            SetupPathList();
        }

        private void SetupPathList() {
            waypointsList = new ReorderableList(serializedObject, serializedObject.FindProperty("waypoints")) {
                drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => DrawElement(rect, index),
                drawHeaderCallback = (Rect rect) => EditorGUI.LabelField(rect, "Path"),
                onCanRemoveCallback = (ReorderableList list) => waypointsList.count > 2,
                onAddCallback = (ReorderableList list) => AddElement(list)
            };
        }

        private void AddElement(ReorderableList list) {
            int index = list.serializedProperty.arraySize;
            list.serializedProperty.arraySize++;
            list.index = index;
            SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
            Vector3[] lasts = new Vector3[2] {
                list.serializedProperty.GetArrayElementAtIndex(waypointsList.index - 1).vector3Value,
                list.serializedProperty.GetArrayElementAtIndex(waypointsList.index - 2).vector3Value };
            Vector3 pos = (lasts[0] - lasts[1]) + lasts[0];
            element.vector3Value = pos;
        }

        private void DrawElement(Rect rect, int index) {
            SerializedProperty element = waypointsList.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                element, GUIContent.none);
        }

        public override void OnInspectorGUI() {
            if(waypointsList == null) {
                SetupPathList();
            }

            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("isLooped"));
            EditorGUILayout.LabelField("Path Lenght", Target.PathLenght.ToString("0.00"));
            waypointsList.DoLayoutList();
            if(EditorGUI.EndChangeCheck()) {
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void OnSceneGUI() {
            Color oldColor = Handles.color;
            if(Tools.current == Tool.Move) {
                Matrix4x4 localToWorld = Target.transform.localToWorldMatrix;
                for(int i = 0; i < waypointsList.count; i++) {
                    DrawSelectionHandle(i, localToWorld);
                    if(waypointsList.index == i) {
                        DrawPositionControl(i, localToWorld, Target.transform.rotation);
                    }
                }
            }
            DrawPath();
            Handles.color = oldColor;
        }

        private void DrawPath() {
            Handles.DrawAAPolyLine(Target.WorldWaypoints);
        }

        private void DrawPositionControl(int i, Matrix4x4 localToWorld, Quaternion localRotation) {
            Vector3 waypoint = Target.LocalWaypoints[i];
            EditorGUI.BeginChangeCheck();
            Vector3 pos = localToWorld.MultiplyPoint(waypoint);
            Quaternion rotation = (Tools.pivotRotation == PivotRotation.Local) ? localRotation : Quaternion.identity;
            float size = HandleUtility.GetHandleSize(pos) * .1f;
            Handles.SphereHandleCap(0, pos, rotation, size, EventType.Repaint);
            pos = Handles.PositionHandle(pos, rotation);
            if(EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(target, "Move Waypoint");
                waypoint = Matrix4x4.Inverse(localToWorld).MultiplyPoint(pos);
                serializedObject.FindProperty("waypoints").GetArrayElementAtIndex(i).vector3Value = waypoint;
                serializedObject.ApplyModifiedProperties();
                ForceSceneUpdate();
            }
        }

        private void DrawSelectionHandle(int i, Matrix4x4 localToWorld) {
            Handles.color = Color.white;
            Vector3 pos = localToWorld.MultiplyPoint(Target.LocalWaypoints[i]);

            float size = HandleUtility.GetHandleSize(pos) * 0.2f;

            if(Handles.Button(pos, Quaternion.identity, size, size, Handles.SphereHandleCap) && waypointsList.index != i) {
                waypointsList.index = i;
                ForceSceneUpdate();
            }

            Handles.BeginGUI();

            Vector2 labelSize = Vector2.one * EditorGUIUtility.singleLineHeight;
            Vector2 labelPos = HandleUtility.WorldToGUIPoint(pos);
            labelPos.x -= labelSize.x / 2;
            labelPos.y -= labelSize.y / 2;

            GUILayout.BeginArea(new Rect(labelPos, labelSize));

            GUIStyle style = new GUIStyle() {
                alignment = TextAnchor.MiddleCenter
            };
            style.normal.textColor = Color.black;
            GUILayout.Label(new GUIContent(i.ToString(), "Waypoint " + i), style);

            GUILayout.EndArea();

            Handles.EndGUI();
        }

        private ReorderableList waypointsList;
    }
}