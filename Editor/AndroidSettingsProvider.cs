using System.Collections.Generic;

using UnityEditor;
using UnityEngine.UIElements;

using TalusBackendData.Editor.Interfaces;

namespace TalusCI.Editor
{
    internal class AndroidSettingsProvider : BaseSettingsProvider<AndroidSettingsProvider>
    {
        public override string Title => $"{AndroidSettingsHolder.ProviderPath} (Do not leave any input fields blank!)";
        public override string Description => "To automate App Signing and Distribution on Google Play.";

        public override SerializedObject SerializedObject => _SerializedObject;
        private SerializedObject _SerializedObject;

        [SettingsProvider]
        public static SettingsProvider CreateAndroidSettingsProvider()
        {
            return new AndroidSettingsProvider(AndroidSettingsHolder.ProviderPath, SettingsScope.Project);
        }

        public AndroidSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null)
            : base(path, scopes, keywords)
        { }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            base.OnActivate(searchContext, rootElement);

            AndroidSettingsHolder.instance.SaveSettings();

            _SerializedObject = new SerializedObject(AndroidSettingsHolder.instance);
        }

        public override void OnGUI(string searchContext)
        {
            _SerializedObject.Update();

            base.OnGUI(searchContext);

            if (EditorGUI.EndChangeCheck())
            {
                _SerializedObject.ApplyModifiedProperties();
                AndroidSettingsHolder.instance.SaveSettings();
            }
        }
    }
}