using System;
using System.Linq;

using UnityEditor;

using TalusBackendData.Editor;
using TalusBackendData.Editor.Models;

namespace TalusCI.Editor
{
    public static class BuildActions
    {
        public static void IOSDevelopment()
        {
            PrepareIOSBuild(true);
        }

        public static void IOSRelease()
        {
            PrepareIOSBuild(false);
        }

        private static void PrepareIOSBuild(bool isDevelopment)
        {
            EditorUserBuildSettings.development = isDevelopment;

            new FetchAppInfo(
                CommandLineParser.GetArgument("-apiUrl"),
                CommandLineParser.GetArgument("-apiKey"),
                CommandLineParser.GetArgument("-appId")
            ).GetInfo(CreateBuild);
        }

        private static void CreateBuild(AppModel app)
        {
            // splash screen
            PlayerSettings.SplashScreen.showUnityLogo = false;

            // app name & bundle id
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, app.app_bundle);
            PlayerSettings.productName = app.app_name;
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Console.WriteLine("[Unity-CI-Package] Define Symbols: " + PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS));

            BuildPipeline.BuildPlayer(GetScenes(), iOSAppBuildInfo.IOSFolder, BuildTarget.iOS, BuildOptions.CompressWithLz4HC);
            EditorApplication.Exit(0);
        }

        private static string[] GetScenes()
        {
            return (from t in EditorBuildSettings.scenes where t.enabled select t.path).ToArray();
        }
    }
}
