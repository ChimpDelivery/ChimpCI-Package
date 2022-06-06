namespace TalusCI.Editor
{
    public static class CIDefinitions
    {
        // Unity3D project absolute path.
        public static readonly string ProjectFolder = System.IO.Directory.GetCurrentDirectory();

        // iOS build path.
        public static readonly string BuildFolder = $"{ProjectFolder}/Builds/iOS/";

        // ExportOptions.plist path, required by jenkins for app-provisioning.
        public static readonly string ExportOptionsPath = $"{ProjectFolder}/Builds/";

        // Provisioning profile.
        public static readonly string ProvisioningProfileName = "EmreMac-Profile";

        // Signing certificate.
        public static readonly string SigningCertificateName = "iPhone Distribution";

        // AppStoreConnect team id.
        public static readonly string TeamID = "R6857T9FN6";
    }
}
