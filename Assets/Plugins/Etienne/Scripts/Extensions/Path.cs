using System;
using UnityEngine;

namespace Etienne {
    public class Path : MonoBehaviour {
        public Vector3[] WorldWaypoints {
            get {
                Vector3[] path = LocalWaypoints.Clone() as Vector3[];
                for(int i = 0; i < path.Length; i++) {
                    path[i] = transform.localToWorldMatrix.MultiplyPoint(path[i]);
                }
                return path;
            }
        }
        public Vector3[] LocalWaypoints { get => isLooped ? LoopedLocalWaypoints : waypoints; }
        public float PathLenght {
            get {
                float lenght = 0f;
                for(int i = 1; i < LocalWaypoints.Length; i++) {
                    lenght += Vector3.Distance(LocalWaypoints[i - 1], LocalWaypoints[i]);
                }
                return lenght;
            }
        }


        [SerializeField] private bool isLooped = false;
        [SerializeField] private Vector3[] waypoints = new Vector3[2] { Vector3.forward, Vector3.forward * 2 };

        private Vector3[] LoopedLocalWaypoints {
            get {
                Vector3[] path = waypoints;
                Array.Resize(ref path, path.Length + 1);
                path[path.Length - 1] = waypoints[0];
                return path;
            }

        }
    }
}