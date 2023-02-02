using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Etienne
{
    public class NavPath : MonoBehaviour
    {

        public Intersection[] Intersections;
        public Path[] paths;

        private void Reset()
        {
            paths = GetComponentsInChildren<Path>();
        }

        public PathIndex FindClosestWaypoint(Vector3 position)
        {
            float minDistance = float.MaxValue;
            PathIndex pathIndex = new PathIndex();
            foreach (Path path in paths)
            {
                for (int i = 0; i < path.WaypointCount; i++)
                {
                    float distance = Vector3.Distance(position, path.WorldWaypoints[i]);
                    if (distance >= minDistance) continue;
                    minDistance = distance;
                    pathIndex.path = path;
                    pathIndex.index = i;
                }
            }
            return pathIndex;
        }

        public List<Vector3> GetPathToPointInSamePath(PathIndex from, PathIndex to)
        {
            if (from.path != to.path) { Debug.LogError($"{from} and {to} are not in the same path, ensure the two are in the same path."); return null; }
            List<Vector3> list = new List<Vector3>();
            Vector3[] points = from.path.WorldWaypoints;
            if (from.index < to.index) { for (int i = from.index; i <= to.index; i++) { list.Add(points[i]); } }
            else { for (int i = from.index; i >= to.index; i--) { list.Add(points[i]); } }
            return list;
        }

        public Intersection GetNextIntersection(Intersection[] intersections, PathIndex start, PathIndex target, out PathIndex pathIndex)
        {
            float minDistance = float.MaxValue;
            Intersection nextIntersection = new Intersection();
            PathIndex nextPathIndex = new PathIndex();
            pathIndex = nextPathIndex;
            foreach (Intersection intersection in intersections)
            {
                float distance = Vector3.Distance(start.GetPoint(), intersection.Center);
                if (intersection.IsConnectedToPath(target.path, out nextPathIndex))
                {
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        pathIndex = nextPathIndex;
                        nextIntersection = intersection;
                    }
                }
                else
                {
                }
            }
            if (minDistance == float.MaxValue) Debug.LogError("Implement recursion");
            return nextIntersection;
        }

        public Intersection[] GetPathIntersections(Path path)
        {
            return Intersections.Where(i => i.IsConnectedToPath(path, out _)).ToArray();
        }

        public List<Vector3> GetPathToClosestIntersection(PathIndex start, Intersection intersection)
        {
            foreach (Passage passage in intersection.passages)
            {
                if (!passage.IsConnectedToPath(start.path, out PathIndex to)) continue;
                return GetPathToPointInSamePath(start, to);
            }
            return null;
        }
    }

    [System.Serializable]
    public struct Intersection
    {
        public Passage[] passages;
        public Vector3 Center
        {
            get
            {
                Vector3 center = new Vector3();
                foreach (Passage passage in passages)
                {
                    center += passage.Center;
                }
                center /= passages.Length;
                return center;
            }
        }

        public Intersection(params PathIndex[] indices)
        {
            List<Passage> passList = new List<Passage>();
            for (int i = 0; i < indices.Length; i++)
            {
                for (int u = i + 1; u < indices.Length; u++)
                {
                    passList.Add(new Passage(indices[i], indices[u]));
                }
            }
            passages = passList.ToArray();
        }

        public bool IsConnectedToPath(Path path, out PathIndex pathIndex)
        {
            foreach (Passage passage in passages)
            {
                if (passage.IsConnectedToPath(path, out pathIndex)) return true;
            }
            pathIndex = new PathIndex();
            return false;
        }
    }

    [System.Serializable]
    public struct Passage
    {
        public PathIndex a;
        public PathIndex b;
        public Vector3 Center => (a.GetPoint() + b.GetPoint()) / 2;

        public Passage(PathIndex a, PathIndex b)
        {
            this.a = a;
            this.b = b;
        }

        public Vector3[] GetPoints()
        {
            return new Vector3[2] { a.GetPoint(), b.GetPoint() };
        }

        public bool IsConnecting(Path a, Path b) => IsConnectingOnWay(a, b) || IsConnectingOnWay(b, a);

        internal bool IsConnectedToPath(Path path, out PathIndex to)
        {
            if (a.path == path)
            {
                to = a;
                return true;
            }
            if (b.path == path)
            {
                to = b;
                return true;
            }
            to = new PathIndex();
            return false;
        }

        private bool IsConnectingOnWay(Path a, Path b) => (this.a.path == a && this.b.path == b);
    }

    [System.Serializable]
    public struct PathIndex
    {
        public Path path;
        public int index;

        public PathIndex(Path path, int index)
        {
            this.path = path;
            this.index = index;
        }

        public Vector3 GetPoint() => path.WorldWaypoints[index];

        public override bool Equals(object obj)
        {
            if (!(obj is PathIndex pathIndex)) return false;
            return path == pathIndex.path && index == pathIndex.index;
        }

        public override int GetHashCode() => path.GetHashCode() + index.GetHashCode();

        public static bool operator ==(PathIndex a, PathIndex b) => a.Equals(b);
        public static bool operator !=(PathIndex a, PathIndex b) => !a.Equals(b);
    }

}
