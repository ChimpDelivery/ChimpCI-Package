using UnityEditor;
using UnityEngine;

using TalusBackendData.Editor.Utility;

using TalusCI.Editor.Utility;

namespace TalusCI.Editor
{
    public static class BuildActions
    {
        [MenuItem("TalusBackend/Manuel Build/iOS/Development", priority = 11000)]
        public static void IOSDevelopment()
        {
            new BuildCreator(true, BuildTarget.iOS, BuildTargetGroup.iOS);
        }

        [MenuItem("TalusBackend/Manuel Build/iOS/Release", priority = 11001)]
        public static void IOSRelease()
        {
            new BuildCreator(false, BuildTarget.iOS, BuildTargetGroup.iOS);
        }

        [MenuItem("TalusBackend/Manuel Build/Android/Development", priority = 11002)]
        public static void AndroidDevelopment()
        {
            EditorUserBuildSettings.buildAppBundle = true;

            new BuildCreator(true, BuildTarget.Android, BuildTargetGroup.Android);
        }

        [MenuItem("TalusBackend/Manuel Build/Android/Release", priority = 11002)]
        public static void AndroidRelease()
        {
            EditorUserBuildSettings.buildAppBundle = true;

            new BuildCreator(false, BuildTarget.Android, BuildTargetGroup.Android);
        }

        // using by Jenkins
        public static void SetBuildVersion()
        {
            string appVersion = CommandLineParser.GetArgument("-buildVersion");
            string bundleVersion = CommandLineParser.GetArgument("-bundleVersion");
            Debug.Log($"[TalusCI-Package] App Version: {appVersion}, Bundle Version: {bundleVersion}");

            PlayerSettings.bundleVersion = appVersion;
            PlayerSettings.Android.bundleVersionCode = int.Parse(bundleVersion);
            PlayerSettings.iOS.buildNumber = bundleVersion;

            Debug.Log("[TalusCI-Package] Version settings initialized.");

            BatchModeUtility.Exit(UnityEditor.Build.Reporting.BuildResult.Succeeded);
        }
    }
}
