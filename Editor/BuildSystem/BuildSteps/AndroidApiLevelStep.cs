using UnityEditor;

using UnityEngine;

namespace ChimpCI.Editor.BuildSystem.BuildSteps
{
    [CreateAssetMenu(menuName = "ChimpDelivery/Build Steps/Android Api Level Step")]
    public class AndroidApiLevelStep : BuildStep
    {
        public AndroidSdkVersions MinSdk = (AndroidSdkVersions) 21;
        public AndroidSdkVersions TargetSdk = (AndroidSdkVersions) 31;

        public override void Execute()
        {
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android) { return; }

            PlayerSettings.Android.minSdkVersion = MinSdk;
            PlayerSettings.Android.targetSdkVersion = TargetSdk;

            Debug.Log($"[ChimpCI-Package] Android Api Level step completed with min: {MinSdk} target: {TargetSdk}!");
        }
    }
}
