using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Networking;

namespace EtienneEditor
{
    internal static class VersionChecker
    {
        private static string CurrentVersion => PrefsKeys.PackageCurrentVersion;
        private static string UrlVersion => PrefsKeys.PackageUrlVersion;

        private const string _Url = "https://raw.githubusercontent.com/Omadel/Etienne/main/package.json";
        private const string _Giturl = "https://github.com/Omadel/Etienne.git";
        private const string _PackageName = "com.etienne";

        private const int _Timeout = 2000, _Step = 5;

        private static async Task FetchPackageInfosAsync(string packagename)
        {
            int time = 0;
            UnityEditor.PackageManager.Requests.ListRequest list = Client.List(true, false);
            while(list.Status == StatusCode.InProgress)
            {
                if(time > _Timeout) break;
                EditorUtility.DisplayProgressBar("CheckVersion", $"Searching current version", time / _Timeout);
                await Task.Delay(_Step);
                time += _Step;
            }

            PackageCollection packages = list.Result;
            UnityEditor.PackageManager.PackageInfo info = (from packageInfo in packages
                                                           where packageInfo.name == packagename
                                                           select packageInfo).FirstOrDefault();
            PrefsKeys.PackageCurrentVersion.SetValue(info != null ? info.version : "0.0.0");
            EditorUtility.ClearProgressBar();
        }

        private static async Task FetchUrlVersion(string url)
        {
            int time = 0;
            UnityWebRequest web = UnityWebRequest.Get(url);
            UnityWebRequestAsyncOperation opertation = web.SendWebRequest();
            while(!opertation.isDone)
            {
                if(web.timeout > 5) break;
                if(time > _Timeout) break;
                EditorUtility.DisplayProgressBar("CheckVersion", $"Searching newest version", opertation.progress + .75f);
                await Task.Delay(_Step);
                time += _Step;
            }

            if(web.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(web.error);
                web.Dispose();
                EditorUtility.ClearProgressBar();
                PrefsKeys.PackageUrlVersion.SetValue("0.0.0");
            } else
            {
                Package json = JsonUtility.FromJson<Package>(web.downloadHandler.text);
                string version = json.version;
                PrefsKeys.PackageUrlVersion.SetValue(version);
            }

            web.Dispose();
            EditorUtility.ClearProgressBar();
        }

        [InitializeOnLoadMethod]
        private static async void UpdateCheckVersion()
        {
            await FetchUrlVersion(_Url);
            if(!IsUpToDate()) Debug.LogWarning("Etienne is not up to date, consider updating.");
        }

        internal static async void CheckVersion()
        {
            await FetchPackageInfosAsync(_PackageName);
            await FetchUrlVersion(_Url);
            CompareVersions();
        }

        private static void CompareVersions()
        {
            bool isUpToDate = IsUpToDate();
            if(EditorUtility.DisplayDialog($"Your version is {(isUpToDate ? "up to date" : "old")}",
                $"Do you want to {(isUpToDate ? "update" : "re-import")} ?"
                + System.Environment.NewLine +
                $"Your version: {CurrentVersion}"
                + System.Environment.NewLine +
                $"Newest version: {UrlVersion}", "Yes", "No"))
            {
                UpdatePackage();
            }
        }

        internal static bool IsUpToDate()
        {
            string[] cVer = CurrentVersion.Split('.');
            string[] urlVer = UrlVersion.Split('.');
            for(int i = 0; i < 3; i++)
            {
                if(int.Parse(cVer[i]) < int.Parse(urlVer[i])) return false;
                if(int.Parse(cVer[i]) > int.Parse(urlVer[i])) return true;
            }
            return true;
        }

        private static async void UpdatePackage()
        {
            int time = 0, timeout = 20000, step = 5;
            UnityEditor.PackageManager.Requests.AddRequest update = Client.Add(_Giturl);
            while(!update.IsCompleted)
            {
                if(time > timeout) break;
                await Task.Delay(step);
                time += step;
            }
            if(update.Status == StatusCode.Success) Debug.Log("Installed: " + update.Result.packageId);
            else if(update.Status >= StatusCode.Failure) Debug.Log(update.Error.message);
        }


        [System.Serializable]
        private class Package
        {
            public string name;
            public string displayName;
            public string version;
            public string description;
            public string unity;
            public string author;
            public bool enableLockFile;
            public string resolutionStrategy;
            public Sample[] samples;
            [System.Serializable]
            public class Sample
            {
                public string displayName;
                public string description;
                public string path;
            }
        }

    }
}