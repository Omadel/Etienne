using UnityEngine;

namespace EtienneEditor
{
    public static class EditorColor
    {
        public static Color32 Clear => new Color32(0, 0, 0, 0);
        public static Color32 White => new Color32(31, 31, 31, 255);
        public static Color32 Black => new Color32(1, 1, 1, 255);
        public static Color32 Red => new Color32(204, 0, 0, 255);
        public static Color32 Green => new Color32(0, 204, 0, 255);
        public static Color32 Blue => new Color32(0, 0, 204, 255);
        public static Color32 Orange => new Color32(255, 119, 0, 255);
        public static Color32 Violet => new Color32(204, 51, 255, 255);
        public static Color32 Yellow => new Color32(238, 238, 0, 255);
    }
    public static class EditorHexColor
    {
        public static string White => "#f1f1f1";
        public static string Black => "#1f1f1f";
        public static string Red => "#cc0000";
        public static string Green => "#00cc00";
        public static string Blue => "#0000cc";
        public static string Orange => "#ff7700";
        public static string Violet => "#cc33ff";
        public static string Yellow => "#eeee00";
    }
}
