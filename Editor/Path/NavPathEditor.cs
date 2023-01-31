using Etienne;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EtienneEditor
{
    [CustomEditor(typeof(NavPath))]
    public class NavPathEditor : Editor<NavPath>
    {
        private Path[] paths;
        private List<PathIndex> selectedPathIndexes = new List<PathIndex>();

        private void OnEnable()
        {
            paths = Target.GetComponentsInChildren<Path>();
        }

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Create Intersection"))
            {
                CreateIntersection(selectedPathIndexes);
            }
            Vector3 old = debugTargetPosition;
            debugTargetPosition = EditorGUILayout.Vector3Field("DebugPosition", debugTargetPosition);
            if (old != debugTargetPosition)
            {
                ForceSceneUpdate();
            }

            old = debugPosition;
            debugPosition = EditorGUILayout.Vector3Field("DebugPosition", debugPosition);
            if (old != debugPosition)
            {
                DebugFindClosestPoint(debugPosition, debugTargetPosition);
                ForceSceneUpdate();
            }
            if (GUILayout.Button("Find Closest point"))
            {
                DebugFindClosestPoint(debugPosition, debugTargetPosition);
                ForceSceneUpdate();
            }

            base.OnInspectorGUI();
        }


        private void CreateIntersection(List<PathIndex> selectedPathIndexes)
        {
            List<Intersection> intersectionsList = Target.Intersections.ToList();
            intersectionsList.Add(new Intersection(selectedPathIndexes.ToArray()));
            Target.Intersections = intersectionsList.ToArray();
            ForceSceneUpdate();
        }

        private void OnSceneGUI()
        {
            Color color = Handles.color;

            foreach (Path path in paths)
            {
                for (int i = 0; i < path.WorldWaypoints.Length; i++)
                {
                    Vector3 point = path.WorldWaypoints[i];
                    float size = HandleUtility.GetHandleSize(point) * 0.2f;
                    if (selectedPathIndexes.Contains(new PathIndex(path, i)))
                    {
                        Handles.color = Color.red;
                    }
                    else
                    {
                        Handles.color = Color.white;
                    }
                    if (Handles.Button(point, Quaternion.identity, size, size, Handles.SphereHandleCap))
                    {
                        if (Event.current.control)
                        {
                            selectedPathIndexes.Add(new PathIndex(path, i));
                        }
                        else
                        {
                            selectedPathIndexes.Clear();
                            selectedPathIndexes.Add(new PathIndex(path, i));
                        }

                        EditorGUIUtility.PingObject(path);
                        ForceSceneUpdate();
                    }
                }
                Handles.color = Color.white;
                Handles.DrawAAPolyLine(path.WorldWaypoints);
            }
            Handles.color = Color.green;
            foreach (Intersection intersection in Target.Intersections)
            {
                foreach (Passage passage in intersection.passages)
                {
                    //Handles.DrawAAPolyLine(passage.GetPoints());
                }
            }

            DrawDebug();

            Handles.color = color;
        }

        private PathIndex debugFound;
        private PathIndex debugTargetFound;
        private Vector3 debugPosition;
        private Vector3 debugTargetPosition;
        private void DebugFindClosestPoint(Vector3 debugPosition, Vector3 debugTargetPosition)
        {
            float minDistance = float.MaxValue;
            foreach (Path path in paths)
            {
                for (int i = 0; i < path.WorldWaypoints.Length; i++)
                {
                    float distance = Vector3.Distance(debugPosition, path.WorldWaypoints[i]);
                    if (distance >= minDistance) continue;
                    minDistance = distance;
                    debugFound.path = path;
                    debugFound.index = i;
                }
            }
            minDistance = float.MaxValue;
            foreach (Path path in paths)
            {
                for (int i = 0; i < path.WorldWaypoints.Length; i++)
                {
                    float distance = Vector3.Distance(debugTargetPosition, path.WorldWaypoints[i]);
                    if (distance >= minDistance) continue;
                    minDistance = distance;
                    debugTargetFound.path = path;
                    debugTargetFound.index = i;
                }
            }
            DebugFindClosestIntersection(debugFound);
        }

        private Passage debugpassageFound;
        private void DebugFindClosestIntersection(PathIndex debugFound)
        {
            float minDistance = float.MaxValue;
            foreach (Intersection intersection in Target.Intersections)
            {
                foreach (Passage passage in intersection.passages)
                {
                    if (passage.a.path != debugFound.path && passage.b.path != debugFound.path) continue;
                    PathIndex path = passage.a.path == debugFound.path ? passage.a : passage.b;
                    float distance = Vector3.Distance(debugFound.GetPoint(), path.GetPoint());
                    if (distance >= minDistance) continue;
                    debugpassageFound = passage;
                }
            }
        }

        private void DrawDebug()
        {
            Handles.color = Color.red;
            Handles.DrawWireArc(Target.transform.position + debugTargetPosition, Vector3.up, Vector3.forward, 360f, .5f);
            if (debugTargetFound.path != null) Handles.DrawWireArc(debugTargetFound.GetPoint(), Vector3.up, Vector3.forward, 360f, .5f);
            Handles.color = Color.blue;
            Handles.DrawWireArc(Target.transform.position + debugPosition, Vector3.up, Vector3.forward, 360f, .5f);
            if (debugFound.path != null) Handles.DrawWireArc(debugFound.GetPoint(), Vector3.up, Vector3.forward, 360f, .5f);
            if (debugpassageFound.a.path != null) Handles.DrawAAPolyLine(debugpassageFound.GetPoints());
        }
    }
}
