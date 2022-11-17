using UnityEditor;

using UnityEngine;

namespace TalusCI.Editor.BuildSystem.BuildSteps
{
    [CreateAssetMenu(menuName = "_OTHERS/Build/Build Steps/iOS Provision Profile ID Step")]
    public class iOSProvisionProfileIdStep : BuildStep
    {
        public ProvisioningProfileType ProfileType;
        public string ProfileUUID;
        
        public override void Execute()
        {
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.iOS)
            {
                return;
            }

            PlayerSettings.iOS.iOSManualProvisioningProfileType = ProfileType;
            PlayerSettings.iOS.iOSManualProvisioningProfileID = ProfileUUID;
            
            Debug.Log("[TalusCI-Package] iOS Provisioning Profile ID step completed!");
        }
    }
}
