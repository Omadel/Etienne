namespace EtienneEditor {
    public class Editor<T> : UnityEditor.Editor where T : class {
        protected T Target => target as T;

        protected void ForceSceneUpdate() {
            UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
        }
    }
}