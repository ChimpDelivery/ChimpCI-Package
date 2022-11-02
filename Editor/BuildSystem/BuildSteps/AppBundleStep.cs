using UnityEditor;

using UnityEngine;

namespace TalusCI.Editor.BuildSystem.BuildSteps
{
    [CreateAssetMenu(menuName = "_OTHERS/Build/Build Steps/App Bundle Step")]
    public class AppBundleStep : BuildStep
    {
        [Header("Google Play")]
        public bool BuildAppBundle;
        
        public override void Execute()
        {
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android) { return;}

            EditorUserBuildSettings.buildAppBundle = BuildAppBundle;

            Debug.Log($"[TalusCI-Package] Android Build App Bundle step completed with {BuildAppBundle}!");
        }
    }
}
