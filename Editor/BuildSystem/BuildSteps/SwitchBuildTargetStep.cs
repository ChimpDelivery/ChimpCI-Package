using UnityEngine;

using UnityEditor;

namespace TalusCI.Editor.BuildSystem.BuildSteps
{
    [CreateAssetMenu(menuName = "_OTHERS/Build/Build Steps/Switch Build Target Step")]
    public class SwitchBuildTargetStep : BuildStep
    {
        public BuildTargetGroup TargetGroup;
        public BuildTarget TargetPlatform;

        public override void Execute()
        {
            Debug.Log($"[TalusCI-Package] Switching to Group: {TargetGroup} / Platform: {TargetPlatform}");

            if (!EditorUserBuildSettings.SwitchActiveBuildTarget(TargetGroup, TargetPlatform) && Application.isBatchMode)
            {
                EditorApplication.Exit(-1);
            }
        }
    }
}
