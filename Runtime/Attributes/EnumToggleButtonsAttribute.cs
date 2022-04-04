using System;
using UnityEngine;

namespace Etienne {
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumToggleButtonsAttribute : PropertyAttribute {
        public readonly bool HideLabel;
        public EnumToggleButtonsAttribute(bool hideLabel = false) {
            HideLabel = hideLabel;
        }
    }
}
