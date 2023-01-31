using System.Collections.Generic;
using UnityEngine;

namespace Etienne
{
    public class NavPath : MonoBehaviour
    {
        public Intersection[] Intersections;
    }

    [System.Serializable]
    public struct Intersection
    {
        public Passage[] passages;

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
    }

    [System.Serializable]
    public struct Passage
    {
        public PathIndex a;
        public PathIndex b;

        public Passage(PathIndex a, PathIndex b)
        {
            this.a = a;
            this.b = b;
        }

        public Vector3[] GetPoints()
        {
            return new Vector3[2] { a.GetPoint(), b.GetPoint() };
        }
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

        public static bool operator ==(PathIndex a, PathIndex b) => a.Equals(b);
        public static bool operator !=(PathIndex a, PathIndex b) => !a.Equals(b);
    }

}
