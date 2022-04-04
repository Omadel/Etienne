using System;
using UnityEngine;

namespace Etienne {
    [AttributeUsage(AttributeTargets.Field)]
    public class ShowIfAttribute : PropertyAttribute {
        public readonly string FieldName;
        public readonly int EnumValue;

        public ShowIfAttribute() {}

        public ShowIfAttribute(string boolName) {
            FieldName = boolName;
        }

        public ShowIfAttribute(string enumName, object enumValue) {
            FieldName = enumName;
            EnumValue = (int)enumValue;
        }
    }
}
