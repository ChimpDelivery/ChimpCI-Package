using UnityEditor;

namespace TalusCI.Editor
{
    public static class BuildActions
    {
        [MenuItem("TalusBackend/Manuel Build/iOS/Development", priority = 11000)]
        public static void IOSDevelopment()
        {
            BuildCreator buildInfo = new BuildCreator
            {
                IsDevBuild = true,
                TargetPlatform = BuildTarget.iOS,
                TargetGroup = BuildTargetGroup.iOS
            };
            buildInfo.PrepareBuild();
        }

        [MenuItem("TalusBackend/Manuel Build/iOS/Release", priority = 11001)]
        public static void IOSRelease()
        {
            BuildCreator buildInfo = new BuildCreator
            {
                TargetPlatform = BuildTarget.iOS,
                TargetGroup = BuildTargetGroup.iOS
            };
            buildInfo.PrepareBuild();
        }

        [MenuItem("TalusBackend/Manuel Build/Android/Development", priority = 11002)]
        public static void AndroidDevelopment()
        {
            EditorUserBuildSettings.buildAppBundle = true;

            BuildCreator buildInfo = new BuildCreator
            {
                TargetPlatform = BuildTarget.Android,
                TargetGroup = BuildTargetGroup.Android
            };
            buildInfo.PrepareBuild();
        }

        [MenuItem("TalusBackend/Manuel Build/Android/Release", priority = 11002)]
        public static void AndroidRelease()
        {
            EditorUserBuildSettings.buildAppBundle = true;

            BuildCreator buildInfo = new BuildCreator
            {
                IsDevBuild = false,
                TargetPlatform = BuildTarget.Android,
                TargetGroup = BuildTargetGroup.Android
            };
            buildInfo.PrepareBuild();
        }
    }
}
