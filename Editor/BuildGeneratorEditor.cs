using UnityEditor;
using UnityEngine;

using TalusCI.Editor.BuildSystem;

namespace TalusCI.Editor
{
    [CustomEditor(typeof(BuildGenerator))]
    public class BuildGeneratorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var generator = target as BuildGenerator;

            if (GUILayout.Button("Run", GUILayout.Height(40)))
            {
                generator.Run();
            }
        }
    }
}
