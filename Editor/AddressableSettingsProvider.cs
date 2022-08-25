using System.Collections.Generic;

using UnityEditor;
using UnityEngine.UIElements;

using TalusBackendData.Editor.Interfaces;

namespace TalusCI.Editor
{
    internal class AddressableSettingsProvider : BaseSettingsProvider<AddressableSettingsProvider>
    {
        public override string Title => $"{AddressableSettingsHolder.ProviderPath} (Do not leave any input fields blank!)";
        public override string Description => "To automate Addressable Content build.";

        public override SerializedObject SerializedObject => _SerializedObject;
        private SerializedObject _SerializedObject;

        [SettingsProvider]
        public static SettingsProvider CreateAddressableSettingsProvider()
        {
            return new AddressableSettingsProvider(AddressableSettingsHolder.ProviderPath, SettingsScope.Project);
        }

        public AddressableSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null)
            : base(path, scopes, keywords)
        { }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            base.OnActivate(searchContext, rootElement);

            AddressableSettingsHolder.instance.SaveSettings();

            _SerializedObject = new SerializedObject(AddressableSettingsHolder.instance);
        }

        public override void OnGUI(string searchContext)
        {
            _SerializedObject.Update();

            base.OnGUI(searchContext);

            if (EditorGUI.EndChangeCheck())
            {
                _SerializedObject.ApplyModifiedProperties();
                AddressableSettingsHolder.instance.SaveSettings();
            }
        }
    }
}