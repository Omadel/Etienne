using UnityEngine;

namespace Etienne {
    [CreateAssetMenu(fileName = "Library", menuName = "Samplable Library", order = 5)]
    internal class SamplableLibrary : ScriptableObjectLibrary<Object> {
        [SerializeField] private Object sampleType;
        protected override System.Type Type => sampleType.GetType();
    }
}