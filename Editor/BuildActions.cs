using UnityEditor;
using UnityEngine;

using TalusBackendData.Editor;
using TalusBackendData.Editor.Utility;

using TalusCI.Editor.BuildSystem;

namespace TalusCI.Editor
{
    /// <summary>
    ///     Using by Jenkins
    /// </summary>
    public static class BuildActions
    {
        [MenuItem("TalusBackend/Manuel Build/iOS/Release")]
        public static void IOSRelease()
        {
            BuildGenerator generator = BuildSettingsHolder.instance.IOSRelease;
            
#if TALUS_ADDRESSABLES
            generator = BuildSettingsHolder.instance.IOSReleaseAddressable;
#endif
            generator.Run();
        }

        [MenuItem("TalusBackend/Manuel Build/Android/Release(aab)")]
        public static void AndroidRelease()
        {
            BuildGenerator generator = BuildSettingsHolder.instance.AndroidReleaseAAB;
            
#if TALUS_ADDRESSABLES
            generator = BuildSettingsHolder.instance.AndroidReleaseAABAddressable;
#endif
            generator.Run();
        }

        [MenuItem("TalusBackend/Manuel Build/Android/Release(apk)")]
        public static void AndroidReleaseAPK()
        {
            BuildGenerator generator = BuildSettingsHolder.instance.AndroidReleaseAPK;

#if TALUS_ADDRESSABLES
            generator = BuildSettingsHolder.instance.AndroidReleaseAPKAddressable;
#endif
            generator.Run();
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
    }
}
