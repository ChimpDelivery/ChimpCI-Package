using UnityEngine;

using UnityEditor;

using ChimpBackendData.Editor.Utility;

namespace ChimpCI.Editor.BuildSystem.BuildSteps
{
    [CreateAssetMenu(menuName = "ChimpDelivery/Build Steps/Switch Build Target Step")]
    public class SwitchBuildTargetStep : BuildStep
    {
        public BuildTargetGroup TargetGroup;
        public BuildTarget TargetPlatform;

        public override void Execute()
        {
            Debug.Log($"[ChimpCI-Package] Switching to Group: {TargetGroup} / Platform: {TargetPlatform}");

            if (!EditorUserBuildSettings.SwitchActiveBuildTarget(TargetGroup, TargetPlatform))
            {
                BatchMode.Close(-1);
            }
        }
    }
}
