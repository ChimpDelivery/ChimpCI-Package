using System.Threading;

using UnityEditor;

using TalusBackendData.Editor.Utility;

namespace TalusCI.Editor.SettingProviders.iOS
{
    public static class Runner
    {
        // Jenkins execute this function as a stage
        [MenuItem("TalusBackend/Project Settings/iOS")]
        public static void Run()
        {
            BatchMode.Log("[TalusBackendData-Package] CollectAssets() is running for iOS...");
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