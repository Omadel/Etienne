using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Networking;

namespace EtienneEditor {
    public static class VersionChecker {
        public static string CurrentVersion => PlayerPrefs.GetString(EditorPrefsKeys.PackageCurrentVersion, "0.0.0");
        public static string UrlVersion => PlayerPrefs.GetString(EditorPrefsKeys.PackageUrlVersion, "0.0.0");

        private const string url = "https://raw.githubusercontent.com/Omadel/Etienne/main/package.json",
            giturl = "https://github.com/Omadel/Etienne.git",
            packageName = "com.etienne";

        private const int timeout = 2000, step = 5;

        private static async Task FetchPackageInfosAsync(string packagename) {
            int time = 0;
            UnityEditor.PackageManager.Requests.ListRequest list = Client.List(true, false);
            while(list.Status == StatusCode.InProgress) {
                if(time > timeout) break;
                EditorUtility.DisplayProgressBar("CheckVersion", $"Searching current version", time / timeout);
                await Task.Delay(step);
                time += step;
            }

            PackageCollection packages = list.Result;
            UnityEditor.PackageManager.PackageInfo info = (from packageInfo in packages
                                                           where packageInfo.name == packagename
                                                           select packageInfo).FirstOrDefault();
            PlayerPrefs.SetString(EditorPrefsKeys.PackageCurrentVersion, info != null ? info.version : "0.0.0");
            EditorUtility.ClearProgressBar();
        }

        public static async Task FetchUrlVersion(string url) {
            int time = 0;
            UnityWebRequest web = UnityWebRequest.Get(url);
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
            } else {
                Package json = JsonUtility.FromJson<Package>(web.downloadHandler.text);
                string version = json.version;
                Debug.Log(version);
                PlayerPrefs.SetString(EditorPrefsKeys.PackageUrlVersion, version);
            }

            web.Dispose();
            EditorUtility.ClearProgressBar();
        }

        public static async void CheckVersion() {
            Debug.Log("Check Version");
            await FetchPackageInfosAsync(packageName);
            await FetchUrlVersion(url);
            CompareVersions();
        }

        private static void CompareVersions() {
            if(!IsUpToDate()) {
                if(EditorUtility.DisplayDialog("Your version is old",
                    "Do you want to update ?" + System.Environment.NewLine +
                    $"Your version: {CurrentVersion}{System.Environment.NewLine}" +
                    $"Newest version: {UrlVersion}", "Yes", "No")) {
                    UpdatePackage();
                }
            } else {
                if(EditorUtility.DisplayDialog("Your version is up to date",
                    "Do you want to re-import ?" + System.Environment.NewLine +
                    $"Your version: {CurrentVersion}{System.Environment.NewLine}" +
                    $"Newest version: {UrlVersion}", "Yes", "No")) {
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