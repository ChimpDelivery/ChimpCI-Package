using System.Linq;
using System.Threading;
using System.Collections.Generic;

using UnityEditor;

using TalusBackendData.Editor.Utility;

namespace TalusCI.Editor.iOS
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