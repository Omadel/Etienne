using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Etienne {
    [AttributeUsage(AttributeTargets.Field)]
    public class HideIfAttribute : PropertyAttribute {
        public readonly string FieldName;
        public readonly int EnumValue;

        public HideIfAttribute() { }

        public HideIfAttribute(string boolName) {
            FieldName = boolName;
        }

        public HideIfAttribute(string enumName, object enumValue) {
            FieldName = enumName;
            EnumValue = (int)enumValue;
        }
    }
}
