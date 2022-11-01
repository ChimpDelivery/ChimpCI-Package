using UnityEditor;

using UnityEngine;

using TalusCI.Editor.BuildSystem;

namespace TalusCI.Editor.Android
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
        }
    }
}
