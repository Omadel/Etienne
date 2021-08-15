using System;
using UnityEngine;
namespace Etienne {

    /// <summary>
    /// Derive from this class to show an error message when the specified type with <see cref="RequirementAttribute"/> is not present in scene.
    /// </summary>
    public abstract class MonoBehaviourWithRequirement : MonoBehaviour {
        public virtual Type GetAttibute() {
            Type type = GetType();
            if(type == null) {
                Debug.LogError("Cant Get Type", this);
                return null;
            }
            RequirementAttribute attribute = Attribute.GetCustomAttribute(type, typeof(RequirementAttribute), true) as RequirementAttribute;
            if(attribute == null) {
                Debug.LogError("The attribute was not found.");
                return null;
            } else {
                //Debug.Log(attribute.InspectedType);
                return attribute.InspectedType;
            }
        }
    }
}