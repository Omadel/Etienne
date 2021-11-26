using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace Etienne {
    public static class RenderPipeline {

        private const bool LOG_NEW_DEFINE_SYMBOLS = true;

        private const string HDRP_PACKAGE = "render-pipelines.high-definition";
        private const string URP_PACKAGE = "render-pipelines.universal";

        private const string TAG_HDRP = "USING_HDRP";
        private const string TAG_URP = "USING_URP";

        private const string CS_CLASSNAME = "DefinedRenderPipeline";
        private const string CS_FILENAME = CS_CLASSNAME + ".cs";





        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded() {

            ListRequest packagesRequest = Client.List(true);

            LoadPackages(packagesRequest);

        }
        private static void LoadPackages(ListRequest request) {

            if(request == null)
                return;


            // Wait for request to complete
            for(int i = 0; i < 1000; i++) {
                if(request.Result != null)
                    break;
                Task.Delay(1).Wait();
            }
            if(request.Result == null)
                throw new TimeoutException();

            // Find out what packages are installed
            List<UnityEditor.PackageManager.PackageInfo> packagesList = request.Result.ToList();

            ///Debug.Log("List of offline Unity packages:\n\n" + String.Join("\n", packagesList.Select(x => x.name)) + "\n\n");

            bool hasHDRP = packagesList.Find(x => x.name.Contains(HDRP_PACKAGE)) != null;
            bool hasURP = packagesList.Find(x => x.name.Contains(URP_PACKAGE)) != null;

            if(hasHDRP && hasURP)
                Debug.LogError("<b>RenderPipeline Packages:</b> Both the HDRP and URP seem to be installed, this may cause problems!");


            DefinePreProcessors(hasHDRP, hasURP);
            SaveToFile(CSharpFileCode(hasHDRP, hasURP));

        }



        private static void DefinePreProcessors(bool defineHDRP, bool defineURP) {

            string originalDefineSymbols;
            string newDefineSymbols;

            List<string> defined;
            //List<BuildTargetGroup> avaliablePlatforms = Enum.GetValues(typeof(BuildTargetGroup)).Cast<BuildTargetGroup>().ToList();
            BuildTargetGroup platform = EditorUserBuildSettings.selectedBuildTargetGroup;

            string log = string.Empty;

            // foreach(var platform in avaliablePlatforms)
            originalDefineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(platform);
            defined = originalDefineSymbols.Split(';').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();


            Action<bool, string> AppendRemoveTag = (stat, tag) => {
                if(stat && !defined.Contains(tag))
                    defined.Add(tag);
                else if(!stat && defined.Contains(tag))
                    defined.Remove(tag);
            };

            AppendRemoveTag(defineHDRP, TAG_HDRP);
            AppendRemoveTag(defineURP, TAG_URP);

            newDefineSymbols = string.Join(";", defined);
            if(originalDefineSymbols != newDefineSymbols) {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(platform, newDefineSymbols);
                log += $"<color=yellow>{platform.ToString()}</color> Old Define Symbols:\n - <color=red>{originalDefineSymbols}</color>\n";
                log += $"<color=yellow>{platform.ToString()}</color> New Define Symbols:\n - <color=green>{newDefineSymbols}</color>\n";
            }
            // }

            if(LOG_NEW_DEFINE_SYMBOLS && !string.IsNullOrEmpty(log))
                Debug.Log($"<b>{nameof(RenderPipeline)}:</b> PlayerSetting Define Symbols have been updated! Check log for further details.\n{log}");

        }

        private static void SaveToFile(string Code) {

            // Get working directory to save the file to
            DirectoryInfo directory = Directory.GetParent(new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName());
            if(directory != null && directory.Parent != null)
                directory = directory.Parent;

            ///Debug.Log(directory.FullName);
            File.WriteAllText(directory.FullName + "\\" + CS_FILENAME, Code);

        }
        private static string CSharpFileCode(bool defineHDRP, bool defineURP) {

            Func<bool, string> ToString = (b) => b ? "true" : "false";

            return "namespace Etienne\n" +
            "{\n" +
                $"\tpublic static class {CS_CLASSNAME}\n" +
                "\t{\n\n" +

                    $"\t\tpublic const bool USING_HDRP = {ToString(defineHDRP)};\n\n" +

                    $"\t\tpublic const bool USING_URP = {ToString(defineURP)};\n\n" +

                "\t}\n" +
            "}";

        }


    }
}