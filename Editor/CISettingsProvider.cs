using System.Collections.Generic;

using UnityEditor;

using UnityEngine;
using UnityEngine.UIElements;

using TalusBackendData.Editor.Interfaces;

namespace TalusCI.Editor
{
    internal class CISettingsProvider : BaseSettingsProvider<CISettingsProvider>
    {
        private SerializedObject _SerializedObject;
        public override SerializedObject SerializedObject => _SerializedObject;

        [SettingsProvider]
        public static SettingsProvider CreateCISettingsProvider()
        {
            return new CISettingsProvider(CISettingsHolder.ProviderPath, SettingsScope.Project);
        }

        public CISettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null)
            : base(path, scopes, keywords)
        { }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            base.OnActivate(searchContext, rootElement);

            CISettingsHolder.instance.SaveSettings();

            _SerializedObject = new SerializedObject(CISettingsHolder.instance);
        }

        public override void OnGUI(string searchContext)
        {
            _SerializedObject.Update();

            EditorGUILayout.BeginVertical();
            {
                Color defaultColor = GUI.color;
                GUI.backgroundColor = Color.yellow;
                EditorGUILayout.HelpBox(
                    string.Join(
                        "\n\n",
                        $"{CISettingsHolder.ProviderPath} (Do not leave any input fields blank!)",
                        "To automate App Signing and Distribution on App Store Connect."
                    ),
                    MessageType.Info,
                    true
                );
                GUI.backgroundColor = defaultColor;

                // unlock button
                base.OnGUI(searchContext);

                if (EditorGUI.EndChangeCheck())
                {
                    _SerializedObject.ApplyModifiedProperties();
                    CISettingsHolder.instance.SaveSettings();
                }
            }
            EditorGUILayout.EndVertical();
        }
    }
}