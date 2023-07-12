using UnityEditor;

namespace ChimpCI.Editor.BuildSystem
{
    /// <summary>
    ///     Using by Jenkins
    /// </summary>
    public static class BuildActions
    {
        [MenuItem("ChimpDelivery/Manuel Build/iOS/Release")]
        public static void iOSRelease()
        {
            BuildGenerator generator = BuildSettingsHolder.instance.IOSRelease;

#if SUPPORT_ADDRESSABLES
            generator = BuildSettingsHolder.instance.IOSReleaseAddressable;
#endif
            generator.Run();
        }

        [MenuItem("ChimpDelivery/Manuel Build/Android/Release(aab)")]
        public static void AndroidRelease()
        {
            BuildGenerator generator = BuildSettingsHolder.instance.AndroidReleaseAAB;

#if SUPPORT_ADDRESSABLES
            generator = BuildSettingsHolder.instance.AndroidReleaseAABAddressable;
#endif
            generator.Run();
        }

        [MenuItem("ChimpDelivery/Manuel Build/Android/Release(apk)")]
        public static void AndroidReleaseAPK()
        {
            BuildGenerator generator = BuildSettingsHolder.instance.AndroidReleaseAPK;

#if SUPPORT_ADDRESSABLES
            generator = BuildSettingsHolder.instance.AndroidReleaseAPKAddressable;
#endif
            generator.Run();
        }
    }
}
