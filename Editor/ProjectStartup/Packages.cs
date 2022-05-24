using System.Net.Http;
using System.Threading.Tasks;
using static System.IO.Path;
using static UnityEngine.Application;

namespace EtienneEditor
{
    internal static class Packages
    {
        public static async Task ReplacePackageFromGist(string id, string user = "Omadel")
        {
            string url = GetGistUrl(id, user);
            string contents = await GetContents(url);
            ReplacePackageFile(contents);
        }

        private static string GetGistUrl(string id, string user = "Omadel")
        {
            return $"https://gist.github.com/{user}/{id}/raw";
        }

        private static async Task<string> GetContents(string url)
        {
            using HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            string content = await response.Content.ReadAsStringAsync();
            return content;
        }

        private static void ReplacePackageFile(string contents)
        {
            string existing = Combine(dataPath, "../Packages/manifest.json");
            System.IO.File.WriteAllText(existing, contents);
            ForceResolve();
        }

        public static void InstallUnityPackage(string packageName)
        {
            UnityEditor.PackageManager.Client.Add($"com.unity.{packageName}");
        }

        public static void ForceResolve()
        {
            UnityEditor.PackageManager.Client.Resolve();
        }
    }
}
