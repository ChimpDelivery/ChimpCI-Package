using UnityEditor;
using UnityEngine;

using TalusBackendData.Editor.Utility;

using TalusCI.Editor.Utility;

namespace TalusCI.Editor
{
    public static class BuildActions
    {
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
