using UnityEditor;
using UnityEngine;

using TalusBackendData.Editor.Interfaces;

namespace TalusCI.Editor.iOS
{
    /// <summary>
    ///     iOSSettingsHolder provides information about iOS building & signing.
    /// </summary>
    [FilePath("ProjectSettings/TalusIOS.asset", FilePathAttribute.Location.ProjectFolder)]
    public class iOSSettingsHolder : BaseSettingsHolder<iOSSettingsHolder>
    {
        // {ExportOptions.plist} path, required by XCode for app-provisioning
        [Header("App Signing & Distribution")]
        [SerializeField]
        private string _ExportOptionsPath = "Builds/";
        public string ExportOptionsPath
        {
            get => _ExportOptionsPath;
            set
            {
                _ExportOptionsPath = value;
                SaveSettings();
            }
        }

        // {App Store Connect} - Provisioning Profile
        [SerializeField]
        private string _ProvisioningProfileName = "iOSProfile";
        public string ProvisioningProfileName
        {
            get => _ProvisioningProfileName;
            set
            {
                _ProvisioningProfileName = value;
                SaveSettings();
            }
        }

        // {App Store Connect} - Signing Certificate
        [SerializeField]
        private string _SigningCertificateName = "iOSCertificate";
        public string SigningCertificateName
        {
            get => _SigningCertificateName;
            set
            {
                _SigningCertificateName = value;
                SaveSettings();
            }
        }

        // {App Store Connect} - Team ID
        [SerializeField]
        private string _TeamID = "R6857T9FN6";
        public string TeamID
        {
            get => _TeamID;
            set
            {
                _TeamID = value;
                SaveSettings();
            }
        }

        [Header("Plist Configurations")]
        [SerializeField]
        private string _TrackingUsageText = "Your data will be used for analytical purposes.";
        public string TrackingUsageText
        {
            get => _TrackingUsageText;
            set
            {
                _TrackingUsageText = value;
                SaveSettings();
            }
        }
    }
}
