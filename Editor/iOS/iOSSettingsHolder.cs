using UnityEditor;
using UnityEngine;

namespace TalusCI.Editor.iOS
{
    /// <summary>
    ///     iOSSettingsHolder provides information about iOS building & signing.
    /// </summary>
    [FilePath("ProjectSettings/TalusIOS.asset", FilePathAttribute.Location.ProjectFolder)]
    public class iOSSettingsHolder : ScriptableSingleton<iOSSettingsHolder>
    {
        // TalusCI.asset path
        public string Path => GetFilePath();

        // Unity3D - CI Layout Panel Path
        private const string _ProviderPath = "Talus Studio/2. iOS Layout";
        public static string ProviderPath => _ProviderPath;

        // Unity3D project absolute path.
        private static readonly string _ProjectFolder = System.IO.Directory.GetCurrentDirectory();
        public static string ProjectFolder => _ProjectFolder;

        // {ExportOptions.plist} path, required by XCode for app-provisioning
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
        private string _ProvisioningProfileName = "EmreMac-Profile";
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
        private string _SigningCertificateName = "iPhone Distribution";
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

        public void SaveSettings()
        {
            Save(true);
        }

        private void OnEnable()
        {
            hideFlags &= ~HideFlags.NotEditable;
        }
    }
}