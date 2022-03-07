using System.Reflection;

namespace Etienne
{
    public static class Utils
    {
        [System.Obsolete("Use EtienneEditor.EditorUtility.ClearConsole instead",false)]
        public static void ClearLog()
        {
#if UNITY_EDITOR
            Assembly assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
            System.Type type = assembly.GetType("UnityEditor.LogEntries");
            MethodInfo method = type.GetMethod("Clear");
            method.Invoke(new object(), null);
#endif
        }


        public static float Normalize(ref this float x, float min, float max)
        {
            return x = (x - min) / (max - min); ;
        }
    }
}