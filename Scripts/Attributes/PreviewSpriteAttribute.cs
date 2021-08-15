using UnityEngine;

namespace Etienne {
    public class PreviewSpriteAttribute : PropertyAttribute {
        public int Height;
        public PreviewSpriteAttribute(int height = 100) => Height = height;
    }
}