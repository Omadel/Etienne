using Etienne;
using UnityEditor;
using UnityEngine;

namespace EtienneEditor {
    [CustomEditor(typeof(InitializableScriptableObject), true)]
    public class InitializableScriptableObjectEditor : Editor<Etienne.InitializableScriptableObject> {

        public override void OnInspectorGUI() {
            EditorGUILayout.BeginHorizontal();
            if(GUILayout.Button($"Refresh {Target.name}")) Target.Initialize();
            if(GUILayout.Button($"Refresh All Libraries")) InitializableScriptableObjectLoader.Load();
            EditorGUILayout.EndHorizontal();
            base.OnInspectorGUI();
        }
    }
}