using UnityEngine;

namespace EtienneEditor
{
    internal static class PrefsKeys
    {

        internal static Prefint DefaultSceneBuildIndex => _DefaultSceneBuildIndex;
        internal static Prefbool UseDefaultLoader => _UseDefaultLoader;
        internal static Prefstring CurrentSceneName => _CurrentSceneName;
        internal static Prefbool GoBackToCurrentScene => _GoBackToCurrentScene;
        internal static Prefbool AutoSaveCurrentScene => _AutoSaveCurrentScene;
        internal static Prefstring PackageCurrentVersion => _PackageCurrentVersion;
        internal static Prefstring PackageUrlVersion => _PackageUrlVersion;

        private static Prefint _DefaultSceneBuildIndex = new Prefint("EtienneEditor.DefaultSceneBuildIndex");
        private static Prefbool _UseDefaultLoader = new Prefbool("EtienneEditor.UseDefaultLoader");
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
