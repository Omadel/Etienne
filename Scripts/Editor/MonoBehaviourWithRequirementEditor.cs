using System;
using Etienne;
using UnityEditor;
using UnityEngine;

namespace EtienneEditor {
    [CustomEditor(typeof(MonoBehaviourWithRequirement), true), CanEditMultipleObjects]
    public class MonoBehaviourWithRequirementEditor : Editor<MonoBehaviourWithRequirement> {
        private Type Type => type ??= Target?.GetAttibute();

        private Type type;
        private bool doesRequierementExists;

        private void OnEnable() {
            FindRequirement(Type);
        }

        public override void OnInspectorGUI() {
            if(!doesRequierementExists) {
                EditorGUILayout.HelpBox($"There is no {Type}, this component require {Type} and therefore does not work without. Click the button below to create a {type}. Or click the second button to fetch for an inactive {type}.", MessageType.Error);
                if(GUILayout.Button($"Create {Type}")) {
                    CreateGameObjectOfType(Type);
                }
                if(GUILayout.Button($"Fetch for an inactive {Type} and enable it")) {
                    Behaviour requierementObject = FindRequirement(Type, true) as Behaviour;
                    if(requierementObject != null) {
                        requierementObject.enabled = true;
                        requierementObject.gameObject.SetActive(true);
                        EditorGUIUtility.PingObject(requierementObject);
                    } else {
                        Debug.LogWarning($"Did not find any {Type} but created one instead");
                        CreateGameObjectOfType(Type);
                    }
                }
            }
            base.OnInspectorGUI();
        }

        private UnityEngine.Object FindRequirement(Type type, bool includeInactive = false) {
            if(type == null) {
                return null;
            }
            UnityEngine.Object requierementObject = GameObject.FindObjectOfType(type, includeInactive);
            doesRequierementExists = requierementObject != null;
            return requierementObject;
        }

        private void CreateGameObjectOfType(Type type) {
            GameObject go = new GameObject($"{type}");
            go.AddComponent(type);
            EditorGUIUtility.PingObject(go);
            doesRequierementExists = true;
        }
    }
}
