using System.Collections.Generic;

using UnityEditor;

using UnityEngine;
using UnityEngine.UIElements;

namespace TalusCI.Editor
{
    internal class CISettingsProvider : SettingsProvider
    {
        private SerializedObject _SerializedObject;

        public CISettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null)
            : base(path, scopes, keywords)
        { }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            CISettingsHolder.instance.SaveSettings();

            _SerializedObject = new SerializedObject(CISettingsHolder.instance);
        }

        public override void OnGUI(string searchContext)
        {
            EditorGUILayout.BeginVertical();
            {
                _SerializedObject.Update();

                Color defaultColor = GUI.color;
                GUI.backgroundColor = Color.yellow;
                EditorGUILayout.HelpBox(
                    string.Join(
                        "\n\n",
                        "Talus Prototype - CI Layout",
                        "To work with CI/CD automation."),
                    MessageType.Info,
                    true
                );
                GUI.backgroundColor = defaultColor;

                {
                    EditorGUI.BeginChangeCheck();

                    SerializedProperty serializedProperty = _SerializedObject.GetIterator();
                    while (serializedProperty.NextVisible(true))
                    {
                        if (serializedProperty.name == "m_Script") { continue; }

                        EditorGUILayout.Separator();

                        EditorGUILayout.LabelField($"{serializedProperty.displayName}:");
                        serializedProperty.stringValue = EditorGUILayout.TextField(serializedProperty.stringValue);
                    }

                    if (EditorGUI.EndChangeCheck())
                    {
                        _SerializedObject.ApplyModifiedProperties();
                        CISettingsHolder.instance.SaveSettings();
                    }
                }
            }
            EditorGUILayout.EndVertical();
        }

        [SettingsProvider]
        public static SettingsProvider CreateCISettingsProvider()
        {
            return new CISettingsProvider(
                CISettingsHolder.ProviderPath,
                SettingsScope.Project
            );

        }
    }
}