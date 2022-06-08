using UnityEditor;
using UnityEngine;

namespace TalusCI.Editor
{
    /// <summary>
    ///     CISettingsHolder provides information about iOS building.
    /// </summary>
    [FilePath("ProjectSettings/TalusCI.asset", FilePathAttribute.Location.ProjectFolder)]
    public class CISettingsHolder : ScriptableSingleton<CISettingsHolder>
    {
        // TalusCI.asset path
        public string Path => GetFilePath();

        // Unity3D - CI Layout Panel Path
        private const string _ProviderPath = "Talus Studio/XCode Layout";
        public static string ProviderPath => _ProviderPath;

        // Unity3D project absolute path.
        private static readonly string _ProjectFolder = System.IO.Directory.GetCurrentDirectory();
        public static string ProjectFolder
        {
            get { return _ProjectFolder; }
        }

        // iOS build path.
        [SerializeField]
        private string _BuildFolder = $"/Builds/iOS/";
        public string BuildFolder
        {
            get { return _BuildFolder; }
            set
            {
                _BuildFolder = value;
                SaveSettings();
            }
        }

        // ExportOptions.plist path, required by XCode for app-provisioning.
        [SerializeField]
        private string _ExportOptionsPath = $"/Builds/";
        public string ExportOptionsPath
        {
            get { return _ExportOptionsPath; }
            set
            {
                _ExportOptionsPath = value;
                SaveSettings();
            }
        }

        // AppStoreConnect - Provisioning Profile.
        [SerializeField]
        private string _ProvisioningProfileName = "EmreMac-Profile";
        public string ProvisioningProfileName
        {
            get { return _ProvisioningProfileName; }
            set
            {
                _ProvisioningProfileName = value;
                SaveSettings();
            }
        }

        // AppStoreConnect - Signing Certificate.
        [SerializeField]
        private string _SigningCertificateName = "iPhone Distribution";
        public string SigningCertificateName
        {
            get { return _SigningCertificateName; }
            set
            {
                _SigningCertificateName = value;
                SaveSettings();
            }
        }

        // AppStoreConnect - Team ID.
        [SerializeField]
        private string _TeamID = "R6857T9FN6";
        public string TeamID
        {
            get { return _TeamID; }
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
    }
}