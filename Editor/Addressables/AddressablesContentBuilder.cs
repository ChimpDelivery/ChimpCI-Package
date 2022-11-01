using UnityEngine;

using UnityEditor;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;

namespace TalusCI.Editor.Addressables
{
    public class AddressablesContentBuilder
    {
        private static AddressableAssetSettings _settings;

        public bool BuildAddressables()
        {
            GetSettingsObject(AddressableSettingsHolder.instance.SettingsAsset);
            SetProfile(AddressableSettingsHolder.instance.ProfileName);

            if (AssetDatabase.LoadAssetAtPath<ScriptableObject>(AddressableSettingsHolder.instance.BuildScript) is not IDataBuilder builderScript)
            {
                Debug.LogError($"[TalusCI-Package] {AddressableSettingsHolder.instance.BuildScript} couldn't be found or isn't a build script.");
                return false;
            }

            SetBuilder(builderScript);

            return BuildAddressableContent();
        }

        private void GetSettingsObject(string settingsAsset)
        {
            _settings = AssetDatabase.LoadAssetAtPath<ScriptableObject>(settingsAsset) as AddressableAssetSettings;

            if (_settings == null)
            {
                Debug.LogError($"[TalusCI-Package] {settingsAsset} couldn't be found or isn't a settings object.");
            }
        }

        private void SetProfile(string profile)
        {
            string profileId = _settings.profileSettings.GetProfileId(profile);

            if (string.IsNullOrEmpty(profileId))
            {
                Debug.LogError($"[TalusCI-Package] Couldn't find a profile named, {profile}, using current profile instead.");
                return;
            }

            _settings.activeProfileId = profileId;
            Debug.Log($"[TalusCI-Package] Active profile id: {profileId}");
        }

        private void SetBuilder(IDataBuilder builder)
        {
            int index = _settings.DataBuilders.IndexOf((ScriptableObject)builder);

            if (index > 0)
            {
                Debug.Log($"[TalusCI-Package] Addressables builder index: {index}");
                _settings.ActivePlayerDataBuilderIndex = index;

                return;
            }

            Debug.LogError($"[TalusCI-Package] {builder} must be added to the " +
                           $"DataBuilders list before it can be made " +
                           $"active. Using last run builder instead.");
        }

        private bool BuildAddressableContent()
        {
            AddressableAssetSettings.BuildPlayerContent(out AddressablesPlayerBuildResult result);
            bool success = string.IsNullOrEmpty(result.Error);

            if (!success)
            {
                Debug.LogError($"[TalusCI-Package] Addressables build error encountered: {result.Error}");
            }

            return success;
        }
    }
}