using UnityEditor;

using UnityEngine;

namespace TalusCI.Editor.BuildSystem.BuildSteps
{
    [CreateAssetMenu(menuName = "_OTHERS/Build/Build Steps/Android Api Level Step")]
    public class AndroidApiLevelStep : BuildStep
    {
        public AndroidSdkVersions MinSdkVersion = (AndroidSdkVersions)31;
        public AndroidSdkVersions TargetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;

        public override void Execute()
        {
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
            {
                return;
            }

            PlayerSettings.Android.minSdkVersion = MinSdkVersion;
            PlayerSettings.Android.targetSdkVersion = TargetSdkVersion;
            
            Debug.Log($"[TalusCI-Package] Android Api Level step completed with min: {MinSdkVersion} target: {TargetSdkVersion}!");
        }
    }
}
