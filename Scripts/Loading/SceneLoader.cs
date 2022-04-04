using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Etienne {
    public class SceneLoader : Singleton<SceneLoader> {
        public System.Action<int[]> OnSceneLoaded;

        [Header("Scene Loader")]
#if TMP
        [SerializeField] private TMPro.TextMeshProUGUI percent;
        [SerializeField] private TMPro.TextMeshProUGUI tip;
#else
        [SerializeField] private Text percent;
        [SerializeField] private Text tip;
#endif
        [SerializeField] private float tipDelay = 3f;
        [SerializeField] [TextArea(0, 2)] private string[] tips;

        protected CanvasGroup canvasGroup = null;

        private Slider slider;
        private int[] loadIndexes = null, unloadIndexes = null;
        private new Camera camera;

        private void Start() {
            SetupUICamera();
            slider = GetComponentInChildren<Slider>();
            canvasGroup = GetComponent<CanvasGroup>();
            LoadLevels(1).StartLoading();
        }

        private void SetupUICamera() {
            camera = GameObject.FindObjectOfType<Camera>();
            camera.tag = "Untagged";
            camera.name = "Loading Camera";
            camera.clearFlags = CameraClearFlags.Nothing;
            camera.gameObject.layer = gameObject.layer;
            camera.cullingMask = 1 << gameObject.layer;
            if(camera.TryGetComponent(out AudioListener audioListener)) Destroy(audioListener);
        }

        /// <summary>
        /// Use with <see cref="StartLoading()"/>
        /// </summary>
        public SceneLoader LoadLevels(params int[] loadIndexes) {
            this.loadIndexes = loadIndexes;
            return this;
        }

        /// <summary>
        /// Use with <see cref="StartLoading()"/>
        /// </summary>
        public SceneLoader LoadLevels<T>(params T[] loadScenes) where T : System.Enum {
            int[] loadIndexes = new int[loadScenes.Length];
            for(int i = 0; i < loadIndexes.Length; i++) {
                loadIndexes[i] = (int)(object)loadScenes[i];
            }
            return LoadLevels(loadIndexes);
        }

        /// <summary>
        /// Use with <see cref="StartLoading()"/>
        /// </summary>
        public SceneLoader UnloadLevels(params int[] unloadAdditionnalIndexes) {
            unloadIndexes = unloadAdditionnalIndexes;
            return this;
        }

        /// <summary>
        /// Use with <see cref="StartLoading()"/>
        /// </summary>
        public SceneLoader UnloadLevels<T>(params T[] unloadAdditionnalScenes) where T : System.Enum {
            int[] loadIndexes = new int[unloadAdditionnalScenes.Length];
            for(int i = 0; i < loadIndexes.Length; i++) {
                loadIndexes[i] = (int)(object)unloadAdditionnalScenes[i];
            }
            return UnloadLevels(loadIndexes);
        }

        public void StartLoading(bool loadPaused = true, bool showLoadingScreen = true) {
            if(showLoadingScreen) {
                ShowLoadingScreen();
                StartCoroutine(GenerateTips());
            }
            if(loadPaused) Time.timeScale = 0f;
            StartCoroutine(LoadAsync(loadIndexes, unloadIndexes));
            loadIndexes = null;
            unloadIndexes = null;
        }

        private IEnumerator GenerateTips() {
            int randomTip = UnityEngine.Random.Range(0, tips.Length);
            tip.text = tips[randomTip];

            while(canvasGroup.alpha != 0f) {
                yield return new WaitForSecondsRealtime(tipDelay);
                randomTip = (randomTip + 1) % tips.Length;
                tip.text = tips[randomTip];
            }
        }

        private IEnumerator LoadAsync(int[] loadIndexes, int[] unloadIndexes) {
            slider.value = 0f;
            percent.text = "0%";
            camera.gameObject.SetActive(true);
            yield return new WaitForEndOfFrame();

            List<AsyncOperation> operations = new List<AsyncOperation>();
            if(unloadIndexes != null) {
                foreach(int unloadIndex in unloadIndexes) {
                    operations.Add(SceneManager.UnloadSceneAsync(unloadIndex));
                }
            }

            if(loadIndexes != null) {
                foreach(int loadIndex in loadIndexes) {
                    operations.Add(SceneManager.LoadSceneAsync(loadIndex, LoadSceneMode.Additive));
                }
            }

            for(int i = 0; i < operations.Count; i++) {
                while(!operations[i].isDone) {
                    float globalProgress = 0f;
                    foreach(AsyncOperation operation in operations) {
                        globalProgress += Mathf.Clamp01(operation.progress / 0.9f);
                    }
                    globalProgress = globalProgress / operations.Count * 100f;
                    int value = Mathf.RoundToInt(globalProgress);
                    slider.value = value;
                    percent.text = $"{value}%";
                    yield return null;
                }
            }

            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(loadIndexes[loadIndexes.Length - 1]));

            HideLoadingScreen();
            camera.gameObject.SetActive(false);

            Time.timeScale = 1f;

            OnSceneLoaded?.Invoke(loadIndexes);
        }

        public void ResetScenes() {
            List<int> scenesToUnload = new List<int>();
            for(int i = 1; i < SceneManager.sceneCount; i++) {
                scenesToUnload.Add(SceneManager.GetSceneAt(i).buildIndex);
            }
            LoadLevels(1).UnloadLevels(scenesToUnload.ToArray()).StartLoading();
        }

        protected virtual void ShowLoadingScreen() => canvasGroup.alpha = 1;

        protected virtual void HideLoadingScreen() => canvasGroup.alpha = 0;
    }
}