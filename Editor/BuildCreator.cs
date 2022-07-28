using System.IO;
using System.Linq;

using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;

using TalusBackendData.Editor.Models;
using TalusBackendData.Editor;
using TalusCI.Editor.Utility;

namespace TalusCI.Editor
{
    public class BuildCreator
    {
        // addressables
        public static string BuildScript = "Assets/AddressableAssetsData/DataBuilders/BuildScriptPackedMode.asset";
        public static string SettingsAsset = "Assets/AddressableAssetsData/AddressableAssetSettings.asset";
        public static string ProfileName = "Default";
        private static AddressableAssetSettings _settings;

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
        }

        public void PrepareBuild()
        {
            EditorUserBuildSettings.development = IsDevBuild;

            bool isBatchMode = Application.isBatchMode;

            if (!isBatchMode && !(EditorUserBuildSettings.activeBuildTarget != BuildTarget.iOS ||
                EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android))
            {
                Debug.LogError("[TalusCI-Package] Build Target must be iOS/Android! Switch platform (File/Build Settings)");
                return;
            }

            string apiUrl = (isBatchMode) ? CommandLineParser.GetArgument("-apiUrl")
                : BackendSettingsHolder.instance.ApiUrl;

            string apiToken = (isBatchMode) ? CommandLineParser.GetArgument("-apiKey")
                : BackendSettingsHolder.instance.ApiToken;

            string appId = (isBatchMode) ? CommandLineParser.GetArgument("-appId")
                : BackendSettingsHolder.instance.AppId;

            // create build when backend data fetched
            BackendApi api = new(apiUrl, apiToken);
            api.GetAppInfo(appId, CreateBuild);
        }

        private void CreateBuild(AppModel app)
        {
            BuildAddressables();

            Debug.Log($"[TalusCI-Package] Addressable content built succesfully!");
            Debug.Log($"[TalusCI-Package] Define Symbols: {PlayerSettings.GetScriptingDefineSymbolsForGroup(TargetGroup)}");
            Debug.Log($"[TalusCI-Package] Build path: {GetBuildPath()}");

            UpdateProductSettings(app);

            BuildReport report = BuildPipeline.BuildPlayer(Scenes, GetBuildPath(), TargetPlatform, Options);
            Debug.Log($"[TalusCI-Package] Build status: {report.summary.result}");
            Debug.Log($"[TalusCI-Package] Output path: {report.summary.outputPath}");

            // batch mode clean-up
            if (!Application.isBatchMode)
            {
                return;
            }

            EditorApplication.Exit(report.summary.result == BuildResult.Succeeded ? 0 : -1);
        }

        private void UpdateProductSettings(AppModel app)
        {
            PlayerSettings.SplashScreen.showUnityLogo = false;
            PlayerSettings.SetScriptingBackend(TargetGroup, ScriptingImplementation.IL2CPP);

            if (app != null)
            {
                PlayerSettings.SetApplicationIdentifier(TargetGroup, app.app_bundle);
                PlayerSettings.productName = app.app_name;
            }
            else
            {
                Debug.LogError($"[TalusCI-Package] AppModel data is null! Product Settings couldn't updated...");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        // ios expects folder
        // android expect file
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

#region ADDRESSABLES_CONTENT_BUILD
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

        public bool BuildAddressables()
        {
            GetSettingsObject(SettingsAsset);
            SetProfile(ProfileName);

            if (AssetDatabase.LoadAssetAtPath<ScriptableObject>(BuildScript) is not IDataBuilder builderScript)
            {
                Debug.LogError($"[TalusCI-Package] {BuildScript} couldn't be found or isn't a build script.");
                return false;
            }

            SetBuilder(builderScript);

            return BuildAddressableContent();
        }
#endregion

    }
}