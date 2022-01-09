using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Networking;

namespace EtienneEditor {
    public static class VersionChecker {
        public static string CurrentVersion => EditorPrefs.GetString(EditorPrefsKeys.PackageCurrentVersion, null);
        public static string UrlVersion => EditorPrefs.GetString(EditorPrefsKeys.PackageUrlVersion, null);
        private static string currentVersion, urlVersion;

        private const string url = "https://raw.githubusercontent.com/Omadel/Etienne/main/package.json",
            giturl = "https://github.com/Omadel/Etienne.git";


        public static async void CheckVersion() {
            int time = 0, timeout = 2000, step = 5;
            UnityEditor.PackageManager.Requests.SearchRequest search = Client.Search("com.etienne");
            while(!search.IsCompleted) {
                if(time > timeout) {
                    Error("Search timeout");
                    return;
                }

                EditorUtility.DisplayProgressBar("CheckVersion", $"Searching current version {search.Status}", time / (float)timeout);
                await Task.Delay(step);
                time += step;
            }
            if(search.Error != null) {
                Error(search.Error.message);
                return;
            }
            foreach(UnityEditor.PackageManager.PackageInfo r in search.Result) {
                currentVersion = r.version;
                EditorPrefs.SetString(EditorPrefsKeys.PackageCurrentVersion, currentVersion);
            }

            UnityWebRequest web = UnityWebRequest.Get(url);
            time = 0;
            UnityWebRequestAsyncOperation opertation = web.SendWebRequest();
            while(!opertation.isDone) {
                if(web.timeout > 5) break;
                if(time > timeout) break;
                EditorUtility.DisplayProgressBar("CheckVersion", $"Searching newest version", opertation.progress + .75f);
                await Task.Delay(step);
                time += step;
            }
            if(web.result != UnityWebRequest.Result.Success) {
                Debug.LogError(web.error);
                web.Dispose();
                EditorUtility.ClearProgressBar();
                return;
            }

            Package json = JsonUtility.FromJson<Package>(web.downloadHandler.text);
            string version = json.version;
            urlVersion = version;
            EditorPrefs.SetString(EditorPrefsKeys.PackageUrlVersion, urlVersion);
            web.Dispose();

            EditorUtility.ClearProgressBar();
            CompareVersions();
        }

        private static void Error(object message) {
            Debug.LogError(message);
            EditorUtility.ClearProgressBar();
            return;
        }

        private static void CompareVersions() {
            if(!IsUpToDate()) {
                if(EditorUtility.DisplayDialog("Your version is old",
                    "Do you want to update ?" + System.Environment.NewLine +
                    $"Your version: {currentVersion}{System.Environment.NewLine}" +
                    $"Newest version: {urlVersion}", "Yes", "No")) {
                    UpdatePackage();
                }
            }
        }

        public static bool IsUpToDate() {
            string[] cVer = CurrentVersion.Split('.');
            string[] urlVer = UrlVersion.Split('.');
            for(int i = 0; i < 3; i++) {
                if(int.Parse(cVer[i]) < int.Parse(urlVer[i])) return false;
                if(int.Parse(cVer[i]) > int.Parse(urlVer[i])) return true;
            }
            return true;
        }

        public static async void UpdatePackage() {
            int time = 0, timeout = 20000, step = 5;
            UnityEditor.PackageManager.Requests.AddRequest update = Client.Add(giturl);
            while(!update.IsCompleted) {
                if(time > timeout) break;
                await Task.Delay(step);
                time += step;
            }
            if(update.Status == StatusCode.Success) Debug.Log("Installed: " + update.Result.packageId);
            else if(update.Status >= StatusCode.Failure) Debug.Log(update.Error.message);
        }


        [System.Serializable]
        private class Package {
            public string name;
            public string displayName;
            public string version;
            public string description;
            public string unity;
            public string author;
            public bool enableLockFile;
            public string resolutionStrategy;
            public Sample[] samples;
        }

        [System.Serializable]
        private class Sample {
            public string displayName;
            public string description;
            public string path;
        }
    }
}