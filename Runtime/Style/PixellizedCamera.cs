using UnityEngine;
using UnityEngine.UI;

namespace Etienne
{
    [AddComponentMenu("Rendering/Pixellized Camera")]
    [RequireComponent(typeof(Camera))]
    [ExecuteAlways]
    public class PixellizedCamera : MonoBehaviour
    {
        [SerializeField] private Vector2Int referenceResolution = new Vector2Int(640, 360);
        [SerializeField, ReadOnly] private Vector2Int gameResolution;
        [SerializeField, ReadOnly] private Vector2Int targetResolution = new Vector2Int(768, 432);
        [SerializeField, RangeLabelled(0f, 1f, "Width", "Height")] private float match = 1;
        private Camera camera;
        private RenderTexture mainTexture;

        private void Reset()
        {
            Canvas canvas = gameObject.GetComponentInChildren<Canvas>();
            camera = GetComponent<Camera>();
            mainTexture = camera.targetTexture;
            if (canvas == null)
            {

                GameObject go = new GameObject("RenderCamera");
                go.transform.SetParent(transform);
                canvas = go.AddComponent<Canvas>();
                go.AddComponent<RawImage>();
            }
#if UNITY_EDITOR
            UnityEditor.SceneVisibilityManager.instance.Hide(canvas.gameObject, true);
#endif
            canvas.gameObject.hideFlags = HideFlags.HideInHierarchy;
            canvas.GetComponent<RawImage>().texture = mainTexture;
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.pixelPerfect = true;
            canvas.sortingOrder = -999;
            OnValidate();
        }

        void OnDestroy()
        {
#if UNITY_EDITOR
            GameObject.DestroyImmediate(transform.GetChild(0).gameObject);
#else
            GameObject.Destroy(transform.GetChild(0).gameObject);
#endif
        }

        private void Update()
        {
            Vector2Int newResolution = GetGameResolution();
            if (newResolution != gameResolution)
            {
                mainTexture.Release();
                SetTargetResolution();
                mainTexture.Create();
                camera.ResetAspect();
            }
            gameResolution = newResolution;
        }

        private void SetTargetResolution()
        {
            targetResolution.x = Mathf.Max(4,
                Mathf.RoundToInt(
                    Mathf.Lerp(
                        referenceResolution.x,
                        gameResolution.x / (float)gameResolution.y * targetResolution.y,
                        match)));
            targetResolution.y = Mathf.Max(4,
                Mathf.RoundToInt(
                    Mathf.Lerp(
                        gameResolution.y / (float)gameResolution.x * targetResolution.x,
                        referenceResolution.y,
                        match)));
            targetResolution.x = Mathf.Max(4,
                Mathf.RoundToInt(
                    Mathf.Lerp(
                        referenceResolution.x,
                        gameResolution.x / (float)gameResolution.y * targetResolution.y,
                        match)));

            mainTexture.width = targetResolution.x;
            mainTexture.height = targetResolution.y;
        }

        private void OnValidate()
        {
            camera = GetComponent<Camera>();
            mainTexture = camera.targetTexture;
            mainTexture.Release();

            mainTexture.anisoLevel = 0;
            mainTexture.antiAliasing = 1;
            mainTexture.autoGenerateMips = false;
            mainTexture.dimension = UnityEngine.Rendering.TextureDimension.Tex2D;
            mainTexture.filterMode = FilterMode.Point;

            gameResolution = GetGameResolution();



            SetTargetResolution();

            mainTexture.Create();
            camera.forceIntoRenderTexture = true;
            camera.ResetAspect();
        }
        public static Vector2Int GetGameResolution()
        {
#if UNITY_EDITOR
            // Get the System.Type for the UnityEditor.GameView class using reflection
            System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
            // Get the GetSizeOfMainGameView method from the UnityEditor.GameView class using reflection
            // This method is not publicly exposed, so we need to use binding flags to get access to it
            System.Reflection.MethodInfo GetSizeOfMainGameView = T.GetMethod("GetSizeOfMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            // Invoke the GetSizeOfMainGameView method to get the size of the main GameView window
            System.Object Res = GetSizeOfMainGameView.Invoke(null, null);
            // Cast the returned object to a Vector2 and return it as a Vector2Int rounded
            return Vector2Int.RoundToInt((Vector2)Res);
#else
            // If this code is not running in the Unity editor, return the screen resolution
            return new Vector2Int(Screen.width, Screen.height);
#endif
        }
    }
}
