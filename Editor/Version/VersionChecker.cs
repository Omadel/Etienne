using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

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
                await Task.Delay(_Step);
                time += _Step;
            }

            PackageCollection packages = list.Result;
            UnityEditor.PackageManager.PackageInfo info = (from packageInfo in packages
                                                           where packageInfo.name == packagename
                                                           select packageInfo).FirstOrDefault();
            PrefsKeys.PackageCurrentVersion = info != null ? info.version : "0.0.0";
        }

        private static async Task FetchUrlVersion(string url)
        {
            using HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            string content = await response.Content.ReadAsStringAsync();
            string[] lines = content.Split('"');
            for(int i = 0; i < lines.Length; i++)
            {
                if(lines[i] != "version") continue;
                string version = lines[i + 2];
                PrefsKeys.PackageUrlVersion = version;
                return;
            }
        }

        [InitializeOnLoadMethod]
        private static void SetListeners()
        {
            AssemblyReloadEvents.afterAssemblyReload -= UpdateCheckVersion;
            AssemblyReloadEvents.afterAssemblyReload += UpdateCheckVersion;
        }

        private static async void UpdateCheckVersion()
        {
            if(string.IsNullOrEmpty(CurrentVersion)) await FetchPackageInfosAsync(_PackageName);
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
            if(UnityEditor.EditorUtility.DisplayDialog($"Your version is {(isUpToDate ? "up to date" : "old")}",
                $"Do you want to {(isUpToDate ? "re-import" : "update")} ?"
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
            /////
        }

        private static void UpdatePackage()
        {
            Client.Add(_Giturl);
            Packages.ForceResolve();
        }
    }
}