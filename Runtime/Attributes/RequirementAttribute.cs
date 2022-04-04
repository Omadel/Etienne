using System;
using UnityEngine;
namespace Etienne {
    /// <Summary>
    /// Tells a <see cref="MonoBehaviourWithRequirement"/> class which run-time type it requires.
    /// </Summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class RequirementAttribute : Attribute {
        public Type InspectedType => inspectedType;
        private Type inspectedType;

        public RequirementAttribute(Type inspectedType) {
            if((object)inspectedType == null) {
                Debug.LogError("Failed to load Requirement inspected type");
            }

            this.inspectedType = inspectedType;
        }
    }
}