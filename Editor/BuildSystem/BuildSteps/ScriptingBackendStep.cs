using UnityEditor;

using UnityEngine;

namespace ChimpCI.Editor.BuildSystem.BuildSteps
{
    [CreateAssetMenu(menuName = "ChimpDelivery/Build Steps/Scripting Backend Step")]
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
