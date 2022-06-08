using System.Collections.Generic;

using UnityEditor;

using UnityEngine;
using UnityEngine.UIElements;

namespace TalusCI.Editor
{
    internal class CISettingsProvider : SettingsProvider
    {
        private bool _UnlockPanel = true;

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
                        $"{CISettingsHolder.ProviderPath} (Do not leave any input fields blank!)",
                        "To automate App Signing and Distribution on App Store Connect."
                    ),
                    MessageType.Info,
                    true
                );
                GUI.backgroundColor = defaultColor;

                GUILayout.Space(8);
                EditorGUI.BeginChangeCheck();

                GUI.enabled = !_UnlockPanel;
                {

                    SerializedProperty serializedProperty = _SerializedObject.GetIterator();
                    while (serializedProperty.NextVisible(true))
                    {
                        if (serializedProperty.name == "m_Script") { continue; }

                        serializedProperty.stringValue = EditorGUILayout.TextField(
                            GetLabel(serializedProperty.displayName),
                            serializedProperty.stringValue
                        );
                    }

                    GUILayout.FlexibleSpace();

                    GUI.enabled = true;
                    GUI.backgroundColor = Color.yellow;

                    string lockButtonName = (_UnlockPanel) ? "Unlock Settings" : "Lock Settings";
                    if (GUILayout.Button(lockButtonName, GUILayout.MinHeight(50)))
                    {
                        _UnlockPanel = !_UnlockPanel;
                    }

                    GUI.enabled = !_UnlockPanel;
                    GUI.backgroundColor = Color.green;
                    if (GUILayout.Button("Reset to defaults", GUILayout.MinHeight(50)))
                    {

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

        public static GUIContent GetLabel(string name) => EditorGUIUtility.TrTextContent(name);

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