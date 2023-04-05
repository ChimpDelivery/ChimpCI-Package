using UnityEditor;

using UnityEngine;

namespace TalusCI.Editor.BuildSystem.BuildSteps
{
    [CreateAssetMenu(menuName = "Talus/Build/Build Steps/Scripting Backend Step")]
    public class ScriptingBackendStep : BuildStep
    {
        public SwitchBuildTargetStep SwitchStep;
        public ScriptingImplementation ScriptingBackend = ScriptingImplementation.IL2CPP;

        public override void Execute()
        {
            PlayerSettings.SetScriptingBackend(SwitchStep.TargetGroup, ScriptingBackend);
        }
    }
}
