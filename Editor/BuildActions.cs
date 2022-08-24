using UnityEditor;
using UnityEngine;

using TalusBackendData.Editor.Utility;

namespace TalusCI.Editor
{
    public static class BuildActions
    {
        [MenuItem("TalusBackend/Manuel Build/iOS/Development", priority = 11000)]
        public static void IOSDevelopment()
        {
            BuildCreator buildInfo = new(true, BuildTarget.iOS, BuildTargetGroup.iOS);
            buildInfo.PrepareBuild();
        }

        [MenuItem("TalusBackend/Manuel Build/iOS/Release", priority = 11001)]
        public static void IOSRelease()
        {
            BuildCreator buildInfo = new(false, BuildTarget.iOS, BuildTargetGroup.iOS);
            buildInfo.PrepareBuild();
        }

        [MenuItem("TalusBackend/Manuel Build/Android/Development", priority = 11002)]
        public static void AndroidDevelopment()
        {
            EditorUserBuildSettings.buildAppBundle = true;

            BuildCreator buildInfo = new(true, BuildTarget.Android, BuildTargetGroup.Android);
            buildInfo.PrepareBuild();
        }

        [MenuItem("TalusBackend/Manuel Build/Android/Release", priority = 11002)]
        public static void AndroidRelease()
        {
            EditorUserBuildSettings.buildAppBundle = true;

            BuildCreator buildInfo = new(false, BuildTarget.Android, BuildTargetGroup.Android);
            buildInfo.PrepareBuild();
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

            EditorApplication.Exit(0);

            Debug.Log("[TalusCI-Package] Version settings initialized.");
        }
    }
}
