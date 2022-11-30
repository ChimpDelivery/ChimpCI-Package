using TalusBackendData.Editor;
using TalusBackendData.Editor.Requests;

using UnityEditor;
using UnityEngine;

// ReSharper disable once InconsistentNaming

namespace TalusCI.Editor.BuildSystem.BuildSteps
{
    [CreateAssetMenu(menuName = "_OTHERS/Build/Build Steps/iOS Provision")]
    public class iOSProvision : BuildStep
    {
        private readonly BackendApiConfigs _ApiConfigs = new();
        
        public override void Execute()
        {
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.iOS) { return; }

            var request = new ProvisionProfileRequest();
            BackendApi.DownloadFile(request, onDownloadComplete: path =>
            {
                string provisionFileUuid = request.GetHeader(_ApiConfigs.ProvisionUuidKey);

                Debug.Log($"[TalusCI-Package] iOSProvision Step | Provision profile path: {path}");
                Debug.Log($"[TalusCI-Package] iOSProvision Step | Provision profile uuid: {provisionFileUuid}");
                
                PlayerSettings.iOS.iOSManualProvisioningProfileType = ProvisioningProfileType.Distribution;
                PlayerSettings.iOS.iOSManualProvisioningProfileID = provisionFileUuid;
            });
            
            Debug.Log("[TalusCI-Package] iOSProvision Step | Completed!");
        }
    }
}
