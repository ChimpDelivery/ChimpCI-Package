using System.IO;
using System.Linq;

using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;

#if ENABLE_ADDRESSABLES
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
#endif

using TalusBackendData.Editor;
using TalusBackendData.Editor.Models;
using TalusBackendData.Editor.Utility;

namespace TalusCI.Editor
{
    public class BuildCreator
    {
        // included scenes
        public string[] Scenes => (from t in EditorBuildSettings.scenes select t.path).ToArray();

        // build properties
        public bool IsDevBuild;
        public BuildTarget TargetPlatform;
        public BuildTargetGroup TargetGroup;
        public BuildOptions Options;

        public BuildCreator(bool isDevBuild, BuildTarget targetPlatform, BuildTargetGroup targetGroup, BuildOptions options = BuildOptions.CompressWithLz4HC)
        {
            IsDevBuild = isDevBuild;
            TargetPlatform = targetPlatform;
            TargetGroup = targetGroup;
            Options = options;

            if (TargetPlatform == BuildTarget.Android)
            {
                PlayerSettings.keyaliasPass = CommandLineParser.GetArgument("-keyStorePass");
                PlayerSettings.keystorePass = CommandLineParser.GetArgument("-keyStorePass");
            }

            PreProcessProjectSettings.OnSyncComplete += CreateBuild;
        }

        public void PrepareBuild()
        {
            EditorUserBuildSettings.development = IsDevBuild;

            if (!Application.isBatchMode && !(EditorUserBuildSettings.activeBuildTarget != BuildTarget.iOS ||
                EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android))
            {
                Debug.LogError("[TalusCI-Package] Build Target must be iOS/Android! Switch platform (File/Build Settings)");
                return;
            }

            PreProcessProjectSettings.Sync();
        }

        private void CreateBuild(AppModel model)
        {
            Debug.Log($"[TalusCI-Package] Define Symbols: {PlayerSettings.GetScriptingDefineSymbolsForGroup(TargetGroup)}");
            Debug.Log($"[TalusCI-Package] Build path: {GetBuildPath()}");

#if ENABLE_ADDRESSABLES
            if (BuildAddressables())
            {
                Debug.Log($"[TalusCI-Package] Addressable content built succesfully!");
            }
#endif

            BuildReport report = BuildPipeline.BuildPlayer(Scenes, GetBuildPath(), TargetPlatform, Options);
            Debug.Log($"[TalusCI-Package] Build status: {report.summary.result}");
            Debug.Log($"[TalusCI-Package] Output path: {report.summary.outputPath}");

            // batch mode clean-up
            if (Application.isBatchMode)
            {
                EditorApplication.Exit(report.summary.result == BuildResult.Succeeded ? 0 : -1);
            }
        }

        // ios expects folder, android expect file
        private string GetBuildPath()
        {
            return TargetPlatform switch
            {
                BuildTarget.iOS => Path.Combine(iOSSettingsHolder.ProjectFolder, iOSSettingsHolder.instance.BuildFolder),
                BuildTarget.Android => Path.Combine(
                    Path.Combine(AndroidSettingsHolder.ProjectFolder, AndroidSettingsHolder.instance.BuildFolder),
                    Path.GetFileName(AndroidSettingsHolder.instance.BuildFileName)
                ),
                _ => "/Builds",
            };
        }

#if ENABLE_ADDRESSABLES
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
            int index = _settings.DataBuilders.IndexOf((ScriptableObject) builder);

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
#endif

    }
}