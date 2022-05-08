using UnityEditor;

namespace TalusCI.Editor
{
    public static class iOSAppBuildInfo
    {
        public static readonly string AppFolder = System.IO.Directory.GetCurrentDirectory();

        // iOS bundle identifier.
        public static readonly string Identifier = PlayerSettings.applicationIdentifier;

        // iOS build path.
        public static readonly string IOSFolder = $"{AppFolder}/Builds/iOS/";

        // ExportOptions.plist path, required by jenkins for app-provisioning.
        public static readonly string ExportOptionsPath = $"{AppFolder}/Builds/";

        // Provisioning profile.
        public static readonly string ProvisioningProfileName = "EmreMac-Profile";

        // Signing certificate.
        public static readonly string SigningCertificateName = "iPhone Distribution";

        // AppStoreConnect team id.
        public static readonly string TeamID = "R6857T9FN6";
    }
}
