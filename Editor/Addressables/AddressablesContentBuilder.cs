using UnityEngine;

using UnityEditor;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;

namespace ChimpCI.Editor.Addressables
{
    public class AddressablesContentBuilder
    {
        private readonly string _BuildScript;
        private readonly string _SettingsAsset;
        private readonly string _ProfileName;

        private AddressableAssetSettings _Settings;

        public AddressablesContentBuilder(string buildScript, string settingsAsset, string profileName)
        {
            _BuildScript = buildScript;
            _SettingsAsset = settingsAsset;
            _ProfileName = profileName;
        }

        public bool BuildAddressables()
        {
            GetSettingsObject(_SettingsAsset);
            SetProfile(_ProfileName);

            if (AssetDatabase.LoadAssetAtPath<ScriptableObject>(_BuildScript) is not IDataBuilder builderScript)
            {
                Debug.LogError($"[ChimpCI-Package] {_BuildScript} couldn't be found or isn't a build script.");
                return false;
            }

            SetBuilder(builderScript);

            return BuildAddressableContent();
        }

        private void GetSettingsObject(string settingsAsset)
        {
            _Settings = AssetDatabase.LoadAssetAtPath<ScriptableObject>(settingsAsset) as AddressableAssetSettings;

            if (_Settings == null)
            {
                Debug.LogError($"[ChimpCI-Package] {settingsAsset} couldn't be found or isn't a settings object.");
            }
        }

        private void SetProfile(string profile)
        {
            string profileId = _Settings.profileSettings.GetProfileId(profile);

            if (string.IsNullOrEmpty(profileId))
            {
                Debug.LogError($"[ChimpCI-Package] Couldn't find a profile named, {profile}, using current profile instead.");
                return;
            }

            _Settings.activeProfileId = profileId;
            Debug.Log($"[ChimpCI-Package] Active profile id: {profileId}");
        }

        private void SetBuilder(IDataBuilder builder)
        {
            int index = _Settings.DataBuilders.IndexOf((ScriptableObject) builder);

            if (index > 0)
            {
                Debug.Log($"[ChimpCI-Package] Addressables builder index: {index}");
                _Settings.ActivePlayerDataBuilderIndex = index;

                return;
            }

            Debug.LogError($"[ChimpCI-Package] {builder} must be added to the " +
                            "DataBuilders list before it can be made " +
                            "active. Using last run builder instead.");
        }

        private bool BuildAddressableContent()
        {
            AddressableAssetSettings.CleanPlayerContent();
            AddressableAssetSettings.BuildPlayerContent(out AddressablesPlayerBuildResult result);
            bool success = string.IsNullOrEmpty(result.Error);

            if (!success)
            {
                Debug.LogError($"[ChimpCI-Package] Addressables build error encountered: {result.Error}");
            }

            return success;
        }
    }
}