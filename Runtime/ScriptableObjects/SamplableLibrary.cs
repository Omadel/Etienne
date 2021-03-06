using UnityEngine;

namespace Etienne {
    [CreateAssetMenu(fileName = "Library", menuName = "Etienne/Samplable Library")]
    internal class SamplableLibrary : ScriptableObjectLibrary<Object> {
        [SerializeField] private Object sampleType;
        protected override System.Type Type => sampleType.GetType();
    }
}