using System.Threading;

using UnityEditor;

using ChimpBackendData.Editor.Utility;

namespace ChimpCI.Editor.SettingProviders.iOS
{
    public static class Runner
    {
        // Jenkins execute this function as a stage
        [MenuItem("ChimpDelivery/Product Settings/iOS")]
        public static void Run()
        {
            BatchMode.Log("[ChimpCI-Package] CollectAssets() is running for iOS...");
            if (!EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS))
            {
                BatchMode.Close(-1);
            }

            PlatformSetting platform = PlatformSettingsHolder.instance.iOS;
            platform.ApplySettings();

            while (!platform.IsApplied)
            {
                Thread.Sleep(100);
            }

            BatchMode.Close(0);
        }
    }
}