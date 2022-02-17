using System;
using UnityEngine;

namespace Etienne
{
    [AttributeUsage(AttributeTargets.Field)]
    public class CurveCursorAttribute : PropertyAttribute
    {
        public readonly string FieldName, MinField, MaxField;
        public readonly float MinValue, MaxValue;

        public CurveCursorAttribute() { }

        public CurveCursorAttribute(string fieldName, float min = 0, float max = 1)
        {
            FieldName = fieldName;
            MinValue = min;
            MaxValue = max;
        }
        public CurveCursorAttribute(string fieldName, string minField, string maxField)
        {
            FieldName = fieldName;
            MinField = minField;
            MaxField = maxField;
            MinValue = 0;
            MaxValue = 1;
        }
        public CurveCursorAttribute(string fieldName, float min, string maxField)
        {
            FieldName = fieldName;
            MinValue = min;
            MaxValue = 1;
            MaxField = maxField;
        }
        public CurveCursorAttribute(string fieldName, string minField, float max = 1)
        {
            FieldName = fieldName;
            MinField = minField;
            MaxValue = max;
            MinValue = 0;
        }
    }
}
