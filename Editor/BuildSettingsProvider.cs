using System.Collections.Generic;

using UnityEditor;

using UnityEngine.UIElements;

using TalusBackendData.Editor.Interfaces;

namespace TalusCI.Editor
{
    internal class BuildSettingsProvider : BaseSettingsProvider
    {
        public override string Title => "Talus Studio/2. Build Layout (Do not leave any input fields blank!)";
        public override string Description => "To automate App Signing and Distribution on App Store Connect.";

        public override SerializedObject SerializedObject => _SerializedObject;
        private SerializedObject _SerializedObject;

        [SettingsProvider]
        public static SettingsProvider CreateBuildSettingsProvider()
        {
            return new BuildSettingsProvider("Talus Studio/2. Build Layout", SettingsScope.Project);
        }

        public BuildSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null)
                : base(path, scopes, keywords)
        { }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            base.OnActivate(searchContext, rootElement);

            BuildSettingsHolder.instance.SaveSettings();

            _SerializedObject = new SerializedObject(BuildSettingsHolder.instance);
        }

        public override void OnGUI(string searchContext)
        {
            _SerializedObject.Update();

            base.OnGUI(searchContext);

            if (EditorGUI.EndChangeCheck())
            {
                _SerializedObject.ApplyModifiedProperties();
                BuildSettingsHolder.instance.SaveSettings();
            }
        }
    }
}
