using UnityEditor;
using UnityEngine;

using TalusBackendData.Editor.Utility;

namespace TalusCI.Editor
{
    /// <summary>
    ///     Using by Jenkins
    /// </summary>
    public static class BuildActions
    {
        public static void IOSRelease()
        {
            
        }

        public static void AndroidRelease()
        {
            
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
    }
}
