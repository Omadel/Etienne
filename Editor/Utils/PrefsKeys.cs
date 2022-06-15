using UnityEngine;

namespace EtienneEditor
{
    internal static class PrefsKeys
    {

        internal static int DefaultSceneBuildIndex
        {
            get => _DefaultSceneBuildIndex.GetValue();
            set => _DefaultSceneBuildIndex.SetValue(value);
        }
        internal static bool UseDefaultLoader
        {
            get => _UseDefaultLoader.GetValue();
            set => _UseDefaultLoader.SetValue(value);
        }
        internal static bool UseBootStrapper
        {
            get => _UseBootStrapper.GetValue();
            set => _UseBootStrapper.SetValue(value);
        }
        internal static int BootStrapperObjectID
        {
            get => _BootStrapperObjectID.GetValue();
            set => _BootStrapperObjectID.SetValue(value);
        }
        internal static string CurrentSceneName
        {
            get => _CurrentSceneName.GetValue();
            set => _CurrentSceneName.SetValue(value);
        }
        internal static bool GoBackToCurrentScene
        {
            get => _GoBackToCurrentScene.GetValue();
            set => _GoBackToCurrentScene.SetValue(value);
        }
        internal static bool AutoSaveCurrentScene
        {
            get => _AutoSaveCurrentScene.GetValue();
            set => _AutoSaveCurrentScene.SetValue(value);
        }
        internal static string PackageCurrentVersion
        {
            get => _PackageCurrentVersion.GetValue();
            set => _PackageCurrentVersion.SetValue(value);
        }
        internal static string PackageUrlVersion
        {
            get => _PackageUrlVersion.GetValue();
            set => _PackageUrlVersion.SetValue(value);
        }

        private static Prefint _DefaultSceneBuildIndex = new Prefint("EtienneEditor.DefaultSceneBuildIndex");
        private static Prefbool _UseDefaultLoader = new Prefbool("EtienneEditor.UseDefaultLoader");
        private static Prefbool _UseBootStrapper = new Prefbool("EtienneEditor.UseBootStrapper");
        private static Prefint _BootStrapperObjectID = new Prefint("EtienneEditor._BootStrapperObjectID");
        private static Prefstring _CurrentSceneName = new Prefstring("EtienneEditor.CurrentSceneName");
        private static Prefbool _GoBackToCurrentScene = new Prefbool("EtienneEditor.GoBackToCurrentScene");
        private static Prefbool _AutoSaveCurrentScene = new Prefbool("EtienneEditor.SaveCurrentScene");
        private static Prefstring _PackageCurrentVersion = new Prefstring("EtienneEditor.PackageCurrentVersion");
        private static Prefstring _PackageUrlVersion = new Prefstring("EtienneEditor.PackageUrlVersion");

        internal abstract class PrefBase<T>
        {
            protected readonly string key;

            internal PrefBase(string key)
            {
                this.key = key;
            }

            internal abstract T GetValue();
            internal abstract void SetValue(T value);

            public static implicit operator T(PrefBase<T> pref) => pref.GetValue();
        }

        internal class Prefbool : PrefBase<bool>
        {
            internal Prefbool(string key) : base(key) { }

            internal override bool GetValue()
            {
                return bool.Parse(PlayerPrefs.GetString(key, bool.FalseString));
            }

            internal override void SetValue(bool value)
            {
                PlayerPrefs.SetString(key, value.ToString());
            }
        }
        internal class Prefstring : PrefBase<string>
        {
            internal Prefstring(string key) : base(key) { }

            internal override string GetValue()
            {
                return PlayerPrefs.GetString(key, string.Empty);
            }

            internal override void SetValue(string value)
            {
                PlayerPrefs.SetString(key, value.ToString());
            }
        }
        internal class Prefint : PrefBase<int>
        {
            internal Prefint(string key) : base(key) { }

            internal override int GetValue()
            {
                return PlayerPrefs.GetInt(key, 0);
            }

            internal override void SetValue(int value)
            {
                PlayerPrefs.SetInt(key, value);
            }
        }
        internal class Preffloat : PrefBase<float>
        {
            internal Preffloat(string key) : base(key) { }

            internal override float GetValue()
            {
                return PlayerPrefs.GetFloat(key, 0f);
            }

            internal override void SetValue(float value)
            {
                PlayerPrefs.GetFloat(key, value);
            }
        }
    }
}
