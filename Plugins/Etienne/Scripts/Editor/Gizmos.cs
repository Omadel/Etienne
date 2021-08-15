using UnityEditor;
#if UNITY_EDITOR
using UnityEngine;
#endif

namespace EtienneEditor {
    public class Gizmos {
#if UNITY_EDITOR
        private static Texture2D gizmoLineAaTexture;
        public const float AlphaFactor = 0.125f;
        private static Color colorAlphaFactor = new Color(1f, 1f, 1f, AlphaFactor);
        private static Texture2D GizmoLineAaTexture {
            get {
                if(gizmoLineAaTexture == null) {
                    gizmoLineAaTexture = new Texture2D(1, 2);
                    gizmoLineAaTexture.SetPixels(new Color[] { new Color(1.0f, 1.0f, 1.0f, 0.0f), new Color(1.0f, 1.0f, 1.0f, 1.0f) });
                    gizmoLineAaTexture.Apply();
                }

                return gizmoLineAaTexture;
            }
        }

        #region Cone
        /// <summary>
        /// Draws a cone at a position and a rotation
        /// </summary>
        public static void DrawCone(Vector3 position, Quaternion rotation, float angle, float range, Color color, float thickness = 4f) {
            float angleToWidth = Mathf.Tan(angle * Mathf.Deg2Rad * 0.5f) * 2.0f * range;
            DrawCone(Matrix4x4.TRS(position, rotation, new Vector3(angleToWidth, angleToWidth, range)), color, thickness);
        }
        private static void DrawCone(Matrix4x4 transform, Color color, float thickness) {
            DrawLine(Vector3.zero, new Vector3(0, 0.5f, 1.0f), transform, color, thickness);
            DrawLine(Vector3.zero, new Vector3(0, -0.5f, 1.0f), transform, color, thickness);
            DrawLine(Vector3.zero, new Vector3(0.5f, 0.0f, 1.0f), transform, color, thickness);
            DrawLine(Vector3.zero, new Vector3(-0.5f, 0.0f, 1.0f), transform, color, thickness);
            DrawCircle(transform, Matrix4x4.TRS(Vector3.forward, Quaternion.AngleAxis(90, Vector3.right), Vector3.one), color, thickness);
        }
        #endregion

        #region Line
        /// <summary>
        /// Draws a line segment between two points
        /// </summary>
        public static void DrawLine(Vector3 startPosition, Vector3 endPosition, Color color, float thickness = 4f, bool dotted = false) {
            if(startPosition != endPosition) {
                Vector3 position = (startPosition + endPosition) / 2;
                Quaternion rotation = Quaternion.LookRotation((endPosition - startPosition).normalized);
                Vector3 scale = Vector3.one * Vector3.Distance(startPosition, endPosition) / 2;

                DrawLine(Vector3.back, Vector3.forward, Matrix4x4.TRS(position, rotation, scale), color, thickness, dotted);
            }
        }
        private static void DrawLine(Vector3 normalizedStartPosition, Vector3 normalizedEndPosition, Matrix4x4 transform, Color color, float thickness = 4f, bool dotted = false) {

            Vector3[] points = new Vector3[2];
            points[0] = transform.MultiplyPoint(normalizedStartPosition);
            points[1] = transform.MultiplyPoint(normalizedEndPosition);

            Color tmp = Handles.color;
            Handles.color = color * colorAlphaFactor;
            // Draws the gizmo only if depth > pixel's
            Handles.zTest = UnityEngine.Rendering.CompareFunction.Greater;
            DrawLine(points, thickness, dotted);
            Handles.color = color;
            // Then draws the gizmo only if depth <= pixel's
            Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
            DrawLine(points, thickness, dotted);
            Handles.color = tmp;
        }
        public static void DrawLine(Vector3[] points, float thickness = 4f, bool dotted = false) {
            if(dotted) {
                Handles.DrawDottedLine(points[0], points[1], thickness);
            } else {
                Handles.DrawAAPolyLine(GizmoLineAaTexture, thickness, points);
            }
        }
        #endregion

        #region Circle
        private const float CircleTangentFactor = 0.551915024494f;
        public static void DrawCircle(Vector3 position, Quaternion rotation, float radius, Color color, float thickness = 4f) {
            DrawCircle(Matrix4x4.TRS(position, rotation, Vector3.one * radius), color, thickness);
        }
        private static void DrawCircle(Matrix4x4 transform, Matrix4x4 offset, Color color, float thickness) {
            Matrix4x4 offsetTransform = transform * offset;
            DrawCircle(offsetTransform, color, thickness);
        }
        private static void DrawCircle(Matrix4x4 transform, Color color, float thickness) {
            DrawBezier(Vector3.right, Vector3.right + Vector3.forward * CircleTangentFactor, Vector3.forward, Vector3.forward + Vector3.right * CircleTangentFactor, transform, Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one * 0.5f), color, thickness);
            DrawBezier(Vector3.forward, Vector3.forward - Vector3.right * CircleTangentFactor, Vector3.left, Vector3.left + Vector3.forward * CircleTangentFactor, transform, Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one * 0.5f), color, thickness);
            DrawBezier(Vector3.left, Vector3.left + Vector3.back * CircleTangentFactor, Vector3.back, Vector3.back - Vector3.right * CircleTangentFactor, transform, Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one * 0.5f), color, thickness);
            DrawBezier(Vector3.back, Vector3.back + Vector3.right * CircleTangentFactor, Vector3.right, Vector3.right + Vector3.back * CircleTangentFactor, transform, Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one * 0.5f), color, thickness);
        }
        #endregion

        #region Bezier
        public static void DrawBezier(Vector3 startPosition, Vector3 endPosition, Vector3 startTangent, Vector3 endTangent, Color color, float thickness) {
            Handles.DrawBezier(startPosition, endPosition, startTangent, endTangent, color, GizmoLineAaTexture, thickness);
        }
        private static void DrawBezier(Vector3 startPosition, Vector3 startTangent, Vector3 endPosition, Vector3 endTangent, Matrix4x4 transform, Matrix4x4 offset, Color color, float thickness) {
            Matrix4x4 offsetTransform = transform * offset;
            DrawBezier(startPosition, startTangent, endPosition, endTangent, offsetTransform, color, thickness);
        }
        private static void DrawBezier(Vector3 startPosition, Vector3 startTangent, Vector3 endPosition, Vector3 endTangent, Matrix4x4 transform, Color color, float thickness) {
            startPosition = transform.MultiplyPoint(startPosition);
            startTangent = transform.MultiplyPoint(startTangent);
            endPosition = transform.MultiplyPoint(endPosition);
            endTangent = transform.MultiplyPoint(endTangent);

            // Draws the gizmo only if depth > pixel's
            Handles.zTest = UnityEngine.Rendering.CompareFunction.Greater;
            Handles.DrawBezier(startPosition, endPosition, startTangent, endTangent, color * colorAlphaFactor, GizmoLineAaTexture, thickness);
            // Then draws the gizmo only if depth <= pixel's
            Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
            Handles.DrawBezier(startPosition, endPosition, startTangent, endTangent, color, GizmoLineAaTexture, thickness);
        }
        #endregion

        #region Sphere
        public static void DrawSphere(Vector3 position, float radius, Color color, float thickness = 4f) {
            DrawCircle(position, Quaternion.identity, radius, color, thickness);
            DrawCircle(position, Quaternion.AngleAxis(90, Vector3.right), radius, color, thickness);
            DrawCircle(position, Quaternion.AngleAxis(90, Vector3.forward), radius, color, thickness);
        }
        #endregion
#endif
    }
}
