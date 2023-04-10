using UnityEditor;

namespace TalusCI.Editor.BuildSystem
{
    /// <summary>
    ///     Using by Jenkins
    /// </summary>
    public static class BuildActions
    {
        [MenuItem("TalusBackend/Manuel Build/iOS/Release")]
        public static void iOSRelease()
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
    }
}
