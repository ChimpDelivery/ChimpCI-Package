using UnityEditor;

using UnityEngine;

namespace TalusCI.Editor.BuildSystem.BuildSteps
{
    [CreateAssetMenu(menuName = "_OTHERS/Build/Build Steps/Android Api Level Step")]
    public class AndroidApiLevelStep : BuildStep
    {
        // Google Play requirement: MinSdkVersion >= 31
        public AndroidSdkVersions MinSdk = (AndroidSdkVersions) 21;
        public AndroidSdkVersions TargetSdk = (AndroidSdkVersions) 31;

        public override void Execute()
        {
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android) { return; }

            PlayerSettings.Android.minSdkVersion = MinSdk;
            PlayerSettings.Android.targetSdkVersion = TargetSdk;

            Debug.Log($"[TalusCI-Package] Android Api Level step completed with min: {MinSdk} target: {TargetSdk}!");
        }
    }
}
