using System;
using UnityEngine;

namespace Etienne {
    public class Path : MonoBehaviour {
        public int WaypointCount => waypoints.Length;
        public Vector3[] CatmullWaypoints => GenerateCatmullRom(waypoints, resolution);
        public Vector3[] WorldWaypoints
        {
            get
            {
                Vector3[] path = LocalWaypoints.Clone() as Vector3[];
                for (int i = 0; i < path.Length; i++)
                {
                    path[i] = transform.localToWorldMatrix.MultiplyPoint(path[i]);
                }
                return path;
            }
        }
        public Vector3[] LocalWaypoints => waypoints;

        [SerializeField, Range(1, 20)] private int resolution = 10;
        [SerializeField] private Vector3[] waypoints = new Vector3[] {Vector3.up, Vector3.up*2};


        private static Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            float t2 = t * t;
            float t3 = t2 * t;
            return 0.5f * ((2 * p1) + ((-p0 + p2) * t) + (((2 * p0) - (5 * p1) + (4 * p2) - p3) * t2) + ((-p0 + (3 * p1) - (3 * p2) + p3) * t3));
        }

        // Generate a Catmull-Rom interpolated Vector3 array based on the control points and resolution
        public static Vector3[] GenerateCatmullRom(Vector3[] controlPoints, int resolution)
        {
            // Calculate the number of segments between each pair of control points
            int segmentCount = controlPoints.Length - 1;
            int pointCount = (segmentCount * resolution) + 1;

            // Create a new array to hold the interpolated points
            Vector3[] points = new Vector3[pointCount];

            // Interpolate between each pair of control points
            for (int i = 0; i < segmentCount; i++)
            {
                // Determine the control points for this segment
                Vector3 p0 = i == 0 ? controlPoints[0] : controlPoints[i - 1];
                Vector3 p1 = controlPoints[i];
                Vector3 p2 = controlPoints[i + 1];
                Vector3 p3 = i == segmentCount - 1 ? controlPoints[segmentCount] : controlPoints[i + 2];

                // Interpolate along the segment
                for (int j = 0; j < resolution; j++)
                {
                    float t = j / (float)resolution;
                    int index = (i * resolution) + j;
                    points[index] = CatmullRom(p0, p1, p2, p3, t);
                }
            }

            // Add the final control point
            points[pointCount - 1] = controlPoints[controlPoints.Length - 1];

            return points;
        }
    }
}