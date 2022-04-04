namespace Etienne
{
    public static class Utils
    {
        public static float Normalize(ref this float x, float min, float max)
        {
            return x = (x - min) / (max - min); ;
        }
    }
}