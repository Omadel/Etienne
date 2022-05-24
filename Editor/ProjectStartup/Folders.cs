using static System.IO.Directory;
using static System.IO.Path;
using static UnityEditor.AssetDatabase;
using static UnityEngine.Application;

namespace EtienneEditor
{
    internal static class Folders
    {
        public static void CreateDefaultFolders()
        {
            CreateDirectories("", "Plugins", "Resources");
            CreateDirectories("_Project", "Prefabs", "Scenes");
            CreateDirectories("_Project/Scripts", "Editor", "Runtime");
            CreateDirectories("_Project/Art", "Meshes", "Textures", "Shaders", "Materials");
            Refresh();
        }

        public static void CreateDirectories(string root, params string[] dirs)
        {
            string fullpath = Combine(dataPath, root);
            foreach(string dir in dirs)
            {
                CreateDirectory(Combine(fullpath, dir));
            }
        }
    }
}
