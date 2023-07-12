using System.IO;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

using ChimpBackendData.Editor;
using ChimpBackendData.Editor.Requests;
using ChimpBackendData.Editor.Interfaces;

namespace ChimpCI.Editor.SettingProviders.iOS
{
    [CreateAssetMenu(menuName = "ChimpDelivery/Providers/iOS Provision")]
    public class iOSProvisionProvider : BaseProvider
    {
        public override void Provide()
        {
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.iOS &&
                EditorUserBuildSettings.activeBuildTarget != BuildTarget.tvOS)
            {
                IsCompleted = true;

                Debug.LogWarning($"[ChimpCI-Package] Current build target is not iOS! iOSProvision Step skipped...");
                return;
            }

            var request = new ProvisionProfileRequest();

            BackendApi.RequestRoutine(
                request,
                new DownloadHandlerFile(BackendSettingsHolder.instance.TempProvisionProfile),
                onSuccess: () =>
                {
                    string profileUuid = request.Request.GetResponseHeader("Dashboard-Provision-Profile-UUID");
                    Debug.Log($"[ChimpCI-Package] iOSProvision Step | Provision File exits: {File.Exists(BackendSettingsHolder.instance.TempProvisionProfile)}");
                    Debug.Log($"[ChimpCI-Package] iOSProvision Step | Provision profile uuid: {profileUuid}");

                    PlayerSettings.iOS.iOSManualProvisioningProfileType = ProvisioningProfileType.Distribution;
                    PlayerSettings.iOS.iOSManualProvisioningProfileID = profileUuid;

                    GenerateExportOptions(profileUuid);
                }
            );

            IsCompleted = true;
        }

        private void GenerateExportOptions(string profileUuid)
        {
            var fileContents = new List<string>
            {
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?>",
                "<!DOCTYPE plist PUBLIC \"-//Apple//DTD PLIST 1.0//EN\" \"http://www.apple.com/DTDs/PropertyList-1.0.dtd\">",
                "<plist version=\"1.0\">",
                "<dict>",
                "    <key>compileBitcode</key>",
                "    <false/>",
                "    <key>provisioningProfiles</key>",
                "    <dict>",
               $"        <key>{PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.iOS)}</key>",
               $"        <string>{profileUuid}</string>",
                "    </dict>",
                "    <key>method</key>",
                "    <string>app-store</string>",
                "    <key>signingStyle</key>",
                "    <string>automatic</string>",
                "    <key>stripSwiftSymbols</key>",
                "    <true/>",
                "    <key>uploadSymbols</key>",
                "    <false/>",
                "</dict>",
                "</plist>"
            };

            string exportOptionsPath = Path.Combine(BackendSettingsHolder.instance.ArtifactFolder, "exportOptions.plist");
            File.WriteAllLines(exportOptionsPath, fileContents.ToArray());
            Debug.Log($"[ChimpCI-Package] exportOptions.plist created at {exportOptionsPath}");
        }
    }
}