using UnityEngine;

namespace TalusCI.Editor
{
    public class JenkinsAppInfo
    {
        public static readonly string AppFolder = System.IO.Directory.GetCurrentDirectory();

        public readonly string Identifier = Application.identifier;
        
        // iOS build path.
        public string IOSFolder = $"{AppFolder}/Builds/iOS/";
        
        // ExportOptions.plist path, required by jenkins for app-provisioning.
        public string ExportOptionsPath = $"{AppFolder}/Certificates/exportOptions.plist";

        // Provisioning profile.
        public string ProvisioningProfileName = "EmreMac-Profile";

        // Signing certificate.
        public string SigningCertificateName = "iPhone Distribution";

        // AppStoreConnect team id.
        public string TeamID = "R6857T9FN6";
    }
}
