using TalusBackendData.Editor;

using UnityEditor;
using UnityEngine;

using TalusBackendData.Editor.Utility;

using TalusCI.Editor.BuildSystem;

namespace TalusCI.Editor
{
    /// <summary>
    ///     Using by Jenkins
    /// </summary>
    public static class BuildActions
    {
        private const string PackagePath = "Packages/com.talus.talusci/Editor/BuildSystem/BuildGenerators";
        
#if TALUS_ADDRESSABLES
        private const string AddressablePackagePath = "Packages/com.talus.talusci/Editor/Addressables/BuildSystem/BuildGenerators";

        private const string iOSReleaseAddressableConfigs = "BuildGenerator_iOS_Addressables_Release.asset";
        private const string AndroidReleaseAABAddressableConfigs = "BuildGenerator_Android_Addressables_Release_AAB.asset";
        private const string AndroidReleaseAPKAddressableConfigs = "BuildGenerator_Android_Addressables_Release_APK.asset";
#endif
        
        private const string iOSReleaseConfigs = "BuildGenerator_iOS_Release.asset";
        private const string AndroidReleaseAABConfigs = "BuildGenerator_Android_Release_AAB.asset";
        private const string AndroidReleaseAPKConfigs = "BuildGenerator_Android_Release_APK.asset";

        [MenuItem("TalusBackend/Manuel Build/iOS/Release")]
        public static void IOSRelease()
        {
            string configPath = $"{PackagePath}/{iOSReleaseConfigs}";
            #if TALUS_ADDRESSABLES
                configPath = $"{AddressablePackagePath}/{iOSReleaseAddressableConfigs}";
            #endif
            LoadAndRun(configPath);
        }

        [MenuItem("TalusBackend/Manuel Build/Android/Release(aab)")]
        public static void AndroidRelease()
        {
            string configPath = $"{PackagePath}/{AndroidReleaseAABConfigs}";
            #if TALUS_ADDRESSABLES
                configPath = $"{AddressablePackagePath}/{AndroidReleaseAABAddressableConfigs}";
            #endif
            LoadAndRun(configPath);
        }

        [MenuItem("TalusBackend/Manuel Build/Android/Release(apk)")]
        public static void AndroidReleaseAPK()
        {
            string configPath = $"{PackagePath}/{AndroidReleaseAPKConfigs}";
            #if TALUS_ADDRESSABLES
                configPath = $"{AddressablePackagePath}/{AndroidReleaseAPKAddressableConfigs}";
            #endif
            LoadAndRun(configPath);
        }

        // sync project keys and set build version
        public static void SetBuildVersion()
        {
            var initializer = new PreProcessProjectSettings();
            initializer.UpdateSettings(() =>
            {
                string appVersion = CommandLineParser.GetArgument("-buildVersion");
                string bundleVersion = CommandLineParser.GetArgument("-bundleVersion");
                Debug.Log($"[TalusCI-Package] App Version: {appVersion}, Bundle Version: {bundleVersion}");

                PlayerSettings.bundleVersion = appVersion;
                PlayerSettings.Android.bundleVersionCode = int.Parse(bundleVersion);
                PlayerSettings.iOS.buildNumber = bundleVersion;

                Debug.Log("[TalusCI-Package] Version settings initialized.");

                EditorApplication.Exit(0);
            });
        }
        
        private static void LoadAndRun(string path)
        {
            var generator = AssetDatabase.LoadAssetAtPath<BuildGenerator>(path);
            generator.Run();
        }
    }
}
