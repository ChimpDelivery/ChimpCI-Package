using System.IO;

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
        public override void Execute()
        {
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.iOS) { return; }

            var request = new ProvisionProfileRequest();
            BackendApi.DownloadFile(request, onDownloadComplete: path =>
            {
                string fileUuid = request.GetHeader(BackendApiConfigs.GetInstance().ProvisionUuidKey);
                string fileName = Path.GetFileName(path);
                
                Debug.Log($"[TalusCI-Package] iOSProvision Step | Provision profile path: {path}");
                Debug.Log($"[TalusCI-Package] iOSProvision Step | Provision profile name: {fileName}");
                Debug.Log($"[TalusCI-Package] iOSProvision Step | Provision profile uuid: {fileUuid}");
                
                PlayerSettings.iOS.iOSManualProvisioningProfileType = ProvisioningProfileType.Distribution;
                PlayerSettings.iOS.iOSManualProvisioningProfileID = fileUuid;
            });
            
            Debug.Log("[TalusCI-Package] iOSProvision Step | Completed!");
        }
    }
}
