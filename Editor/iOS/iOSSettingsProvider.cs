using System.Collections.Generic;

using UnityEditor;
using UnityEngine.UIElements;

using TalusBackendData.Editor.Interfaces;

namespace TalusCI.Editor.iOS
{
    internal class iOSSettingsProvider : BaseSettingsProvider
    {
        public override string Title => "Talus Studio/2. iOS Layout (Do not leave any input fields blank!)";
        public override string Description => "To automate App Signing and Distribution on App Store Connect.";

        public override SerializedObject SerializedObject => _SerializedObject;
        private SerializedObject _SerializedObject;

        [SettingsProvider]
        public static SettingsProvider CreateIOSSettingsProvider()
        {
            return new iOSSettingsProvider("Talus Studio/2. iOS Layout", SettingsScope.Project);
        }

        public iOSSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null)
            : base(path, scopes, keywords)
        { }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            base.OnActivate(searchContext, rootElement);

            iOSSettingsHolder.instance.SaveSettings();

            _SerializedObject = new SerializedObject(iOSSettingsHolder.instance);
        }

        public override void OnGUI(string searchContext)
        {
            _SerializedObject.Update();

            base.OnGUI(searchContext);

            if (EditorGUI.EndChangeCheck())
            {
                _SerializedObject.ApplyModifiedProperties();
                iOSSettingsHolder.instance.SaveSettings();
            }
        }
    }
}