using UnityEditor;
using UnityEngine;

using ChimpBackendData.Editor.Utility;
using ChimpBackendData.Editor.Interfaces;

namespace ChimpCI.Editor.SettingProviders
{
    [CreateAssetMenu(menuName = "ChimpDelivery/Providers/Version Settings")]
    public class VersionSettingsProvider : BaseProvider
    {
        public override void Provide()
        {
            if (!Application.isBatchMode)
            {
                Debug.Log("[ChimpCI-Package] Version Provider could not run! Only CI/CD pipeline supported!");
                IsCompleted = true;
                return;
            }

            string appVersion = CommandLineParser.GetArgument("-buildVersion");
            string bundleVersion = CommandLineParser.GetArgument("-bundleVersion");
            Debug.Log($"[ChimpCI-Package] App Version: {appVersion}, Bundle Version: {bundleVersion}");

            PlayerSettings.bundleVersion = appVersion;
            PlayerSettings.Android.bundleVersionCode = int.Parse(bundleVersion);
            PlayerSettings.iOS.buildNumber = bundleVersion;

            Debug.Log("[ChimpCI-Package] Version settings initialized.");

            IsCompleted = true;
        }
    }
}