using Sirenix.OdinInspector;

using UnityEditor;

using UnityEngine;

namespace TalusCI.Editor.BuildSystem.BuildSteps
{
    [CreateAssetMenu(menuName = "_OTHERS/Build/Build Steps/Android Api Level Step")]
    public class AndroidApiLevelStep : BuildStep
    {
        public AndroidSdkVersions MinSDKVersion = (AndroidSdkVersions)31;
        
        [Button]
        public override void Execute()
        {
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
            {
                return;
            }

            PlayerSettings.Android.minSdkVersion = MinSDKVersion;
            
            Debug.Log($"[TalusCI-Package] Android Api Level step completed with {MinSDKVersion}!");
        }
    }
}
