using UnityEngine;

namespace Etienne
{
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class RangeLabelledAttribute : PropertyAttribute
    {

        public readonly float Min, Max;
        public readonly string LeftLabel, RightLabel;

        public RangeLabelledAttribute(float min, float max, string leftLabel, string rightlabel)
        {
            Min = min;
            Max = max;
            LeftLabel = leftLabel;
            RightLabel = rightlabel;
        }
    }
}
