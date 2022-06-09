namespace Etienne
{
    public static class Utils
    {
        public static float Normalize(this float x, float min, float max)
        {
            return (x - min) / (max - min); ;
        }
    }
}