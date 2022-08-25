using UnityEditor;
using UnityEngine;

namespace TalusCI.Editor
{
    /// <summary>
    ///     AddressablesSettingsHolder provides information about integration with Adressables Package.
    /// </summary>
    [FilePath("ProjectSettings/TalusAddressables.asset", FilePathAttribute.Location.ProjectFolder)]
    public class AddressableSettingsHolder : ScriptableSingleton<AddressableSettingsHolder>
    {
        // TalusCI.asset path
        public string Path => GetFilePath();

        // Unity3D - Addressable Layout Panel Path
        private const string _ProviderPath = "Talus Studio/5. Addressables Layout";
        public static string ProviderPath => _ProviderPath;

        [SerializeField]
        private string _BuildScript = "Assets/AddressableAssetsData/DataBuilders/BuildScriptPackedMode.asset";
        public string BuildScript
        {
            get => _BuildScript;
            set
            {
                _BuildScript = value;
                SaveSettings();
            }
        }

        [SerializeField]
        private string _SettingsAsset = "Assets/AddressableAssetsData/AddressableAssetSettings.asset";
        public string SettingsAsset
        {
            get => _SettingsAsset;
            set
            {
                _SettingsAsset = value;
                SaveSettings();
            }
        }

        [SerializeField]
        private string _ProfileName = "Default";
        public string ProfileName
        {
            get => _ProfileName;
            set
            {
                _ProfileName = value;
                SaveSettings();
            }
        }

        public void SaveSettings() => Save(true);
    }
}