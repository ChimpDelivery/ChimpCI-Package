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

        private const string iOSReleaseConfigs = "BuildGenerator_iOS_Release.asset";
        private const string AndroidReleaseAABConfigs = "BuildGenerator_Android_Release_AAB.asset";
        private const string AndroidReleaseAPKConfigs = "BuildGenerator_Android_Release_APK.asset";

        [MenuItem("TalusBackend/Manuel Build/iOS/Release")]
        public static void IOSRelease()
        {
            string configPath = $"{PackagePath}/{iOSReleaseConfigs}";
            LoadAndRun(configPath);
        }

        [MenuItem("TalusBackend/Manuel Build/Android/Release(aab)")]
        public static void AndroidRelease()
        {
            string configPath = $"{PackagePath}/{AndroidReleaseAABConfigs}";
            LoadAndRun(configPath);
        }

        [MenuItem("TalusBackend/Manuel Build/Android/Release(apk)")]
        public static void AndroidReleaseAPK()
        {
            string configPath = $"{PackagePath}/{AndroidReleaseAPKConfigs}";
            LoadAndRun(configPath);
        }

        public static void SetBuildVersion()
        {
            string appVersion = CommandLineParser.GetArgument("-buildVersion");
            string bundleVersion = CommandLineParser.GetArgument("-bundleVersion");
            Debug.Log($"[TalusCI-Package] App Version: {appVersion}, Bundle Version: {bundleVersion}");

            PlayerSettings.bundleVersion = appVersion;
            PlayerSettings.Android.bundleVersionCode = int.Parse(bundleVersion);
            PlayerSettings.iOS.buildNumber = bundleVersion;

            Debug.Log("[TalusCI-Package] Version settings initialized.");

            EditorApplication.Exit(0);
        }
        
        private static void LoadAndRun(string path)
        {
            var generator = AssetDatabase.LoadAssetAtPath<BuildGenerator>(path);
            generator.Run();
        }
    }
}
