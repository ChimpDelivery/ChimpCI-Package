using System.Threading;

using UnityEditor;

using ChimpBackendData.Editor.Utility;

namespace ChimpCI.Editor.SettingProviders.Android
{
    public static class Runner
    {
        // Jenkins execute this function as a stage
        [MenuItem("ChimpDelivery/Product Settings/Android")]
        public static void Run()
        {
            BatchMode.Log("[ChimpCI-Package] Applying platform settings for Android...");
            if (!EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android))
            {
                BatchMode.Close(-1);
            }

            PlatformSetting platform = PlatformSettingsHolder.instance.Android;
            platform.ApplySettings();

            while (!platform.IsApplied)
            {
                Thread.Sleep(100);
            }

            BatchMode.Close(0);
        }
    }
}