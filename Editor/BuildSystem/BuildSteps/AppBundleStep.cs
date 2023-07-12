using UnityEditor;

using UnityEngine;

namespace ChimpCI.Editor.BuildSystem.BuildSteps
{
    [CreateAssetMenu(menuName = "ChimpDelivery/Build Steps/App Bundle Step")]
    public class AppBundleStep : BuildStep
    {
        [Header("Google Play")]
        public bool BuildAppBundle;

        public override void Execute()
        {
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android) { return; }

            EditorUserBuildSettings.buildAppBundle = BuildAppBundle;

            Debug.Log($"[ChimpCI-Package] Android Build App Bundle step completed with {BuildAppBundle}!");
        }
    }
}
