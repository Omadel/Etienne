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
        private List<PathIndex> selectedPathIndexes = new List<PathIndex>();

        private PathIndex debugFound;
        private PathIndex debugTargetFound;
        private Vector3 debugPosition = new Vector3(-3, 0, 0);
        private Vector3 debugTargetPosition = new Vector3(3, 0, 0);
        private List<Vector3> debugPath;

        private void OnEnable()
        {
            DebugFindPath();
        }

        private void DebugFindPath()
        {
            DebugFindTarget();
            DebugFindStart();

            DebugFindPathPositions();

            ForceSceneUpdate();
        }

        private void DebugFindPathPositions()
        {
            debugPath = new List<Vector3>() { debugFound.GetPoint() };
            if (debugFound.path == debugTargetFound.path)
            {
                debugPath.AddRange(Target.GetPathToPointInSamePath(debugFound, debugTargetFound));
                return;
            }

            Intersection[] connectedIntersections = Target.GetPathIntersections(debugFound.path);
            Intersection intersection = Target.GetNextIntersection(connectedIntersections,debugFound, debugTargetFound, out PathIndex pathIndex);
            debugPath.AddRange(Target.GetPathToClosestIntersection(debugFound, intersection));
            debugPath.Add(pathIndex.GetPoint());

            if (intersection.IsConnectedToPath(debugTargetFound.path, out _))
            {
                debugPath.AddRange(Target.GetPathToPointInSamePath(pathIndex, debugTargetFound));
            }
        }

        private void DebugFindStart() => debugFound = Target.FindClosestWaypoint(debugPosition);
        private void DebugFindTarget() => debugTargetFound = Target.FindClosestWaypoint(debugTargetPosition);

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Create Intersection")) { CreateIntersection(selectedPathIndexes); }


            Vector3 targetold = debugTargetPosition;
            debugTargetPosition = EditorGUILayout.Vector3Field(nameof(debugTargetPosition), debugTargetPosition);
            Vector3 old = debugPosition;
            debugPosition = EditorGUILayout.Vector3Field(nameof(debugPosition), debugPosition);
            if (targetold != debugTargetPosition || old != debugPosition) DebugFindPath();

            base.OnInspectorGUI();
        }


        private void CreateIntersection(List<PathIndex> selectedPathIndexes)
        {
            List<Intersection> intersectionsList = Target.Intersections.ToList();
            Intersection intersection = new Intersection(selectedPathIndexes.ToArray());
            if (intersectionsList.Contains(intersection)) return;

            intersectionsList.Add(intersection);
            Target.Intersections = intersectionsList.ToArray();
            ForceSceneUpdate();
        }

        private void OnSceneGUI()
        {
            Color color = Handles.color;

            DrawWaypoints();
            DrawPassages();

            DrawDebug();

            Handles.color = color;
        }

        private void DrawPassages()
        {
            Handles.color = Color.green;
            foreach (Intersection intersection in Target.Intersections)
            {
                foreach (Passage passage in intersection.passages)
                {
                    Handles.DrawAAPolyLine(passage.GetPoints());
                }
            }
        }

        private void DrawWaypoints()
        {
            foreach (Path path in Target.paths)
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
        }

        private void DrawDebug()
        {

            Vector3 targetold = debugTargetPosition;
            debugTargetPosition = Handles.PositionHandle(debugTargetPosition, Quaternion.identity);
            Vector3 old = debugPosition;
            debugPosition = Handles.PositionHandle(debugPosition, Quaternion.identity);
            if (targetold != debugTargetPosition || old != debugPosition) DebugFindPath();


            Handles.color = Color.red;
            Handles.DrawWireArc(Target.transform.position + debugTargetPosition, Vector3.up, Vector3.forward, 360f, .5f);
            if (debugTargetFound.path != null) Handles.DrawWireArc(debugTargetFound.GetPoint(), Vector3.up, Vector3.forward, 360f, .5f);
            Handles.color = Color.blue;
            Handles.DrawWireArc(Target.transform.position + debugPosition, Vector3.up, Vector3.forward, 360f, .5f);
            if (debugFound.path != null) Handles.DrawWireArc(debugFound.GetPoint(), Vector3.up, Vector3.forward, 360f, .5f);
            if (debugPath.Count > 1)
            {
                Handles.color = EtienneEditor.EditorColor.Orange;
                Handles.DrawAAPolyLine(debugPath.ToArray());
            }

            Handles.color = EditorColor.Red;

            Intersection[] connectedIntersections = Target.GetPathIntersections(debugFound.path);
            foreach (Intersection item in connectedIntersections)
            {
                Handles.DrawAAPolyLine(new Vector3[] { item.Center, item.Center + Vector3.up * 5 });
            }
            Handles.color = Color.white;
        }
    }
}
