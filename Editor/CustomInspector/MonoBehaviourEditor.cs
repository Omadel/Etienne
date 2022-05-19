using UnityEditor;
using UnityEngine;

namespace EtienneEditor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class MonoBehaviourEditor : Editor { }
}
