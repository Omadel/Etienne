namespace Etienne
{
    public static class Utils
    {
        public static float Normalize(this float x, float min, float max)
        {
            return (x - min) / (max - min); ;
        }
        public static float Normalize(this int x, int min, int max)
        {
            return (x - (float)min) / (max - (float)min); ;
        }
    }
}