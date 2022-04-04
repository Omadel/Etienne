using UnityEngine;

namespace Etienne {
    public abstract class InitializableScriptableObject : ScriptableObject, IInitializable {
        public abstract void Initialize();
    }
}
