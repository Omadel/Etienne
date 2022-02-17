using System.Reflection;

namespace Etienne
{
    public static class Utils
    {
        public static void ClearLog()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
            System.Type type = assembly.GetType("UnityEditor.LogEntries");
            MethodInfo method = type.GetMethod("Clear");
            method.Invoke(new object(), null);
        }


        public static float Normalize(this float x, float min, float max)
        {
            return (x - min) / (max - min); ;
        }
    }
}