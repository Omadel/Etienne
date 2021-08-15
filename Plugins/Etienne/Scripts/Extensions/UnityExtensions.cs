using UnityEngine;

namespace Etienne {
    public partial class Extensions {

        public static Vector2 Multiply(this Vector2 v, Vector2 vector) {
            return new Vector3(v.x * vector.x, v.y * vector.y);
        }

        public static Vector3 Multiply(this Vector3 v, Vector3 vector) {
            return new Vector3(v.x * vector.x, v.y * vector.y, v.z * vector.z);
        }

        /// <summary>
        /// Get a direction from start to end
        /// </summary>
        /// <param name="start">The start of the direction</param>
        /// <param name="end">The end of the direction</param>
        /// <returns>end - start</returns>
        public static Vector3 Direction(this Vector3 start, Vector3 end) {
            return end - start;
        }

        /// <summary>
        /// Get a direction from start to end
        /// </summary>
        /// <param name="start">The start of the direction</param>
        /// <param name="end">The end of the direction</param>
        /// <returns>end - start</returns>
        public static Vector3 Direction(this Transform start, Transform end) {
            return start.position.Direction(end.position);
        }

        public static Transform FindInChildren(this Transform parent, string name) {
            foreach(Transform child in parent) {
                if(child.name == name) {
                    return child;
                }
            }
            return null;
        }
    }
}