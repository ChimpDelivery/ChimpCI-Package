using System.Linq;

using UnityEditor;
using UnityEngine;

using TalusBackendData.Editor;
using TalusBackendData.Editor.Models;

namespace TalusCI.Editor
{
    public static class BuildActions
    {
        [MenuItem("TalusKit/Manuel Build/iOS Development", priority = 11000)]
        public static void IOSDevelopment()
        {
            PrepareIOSBuild(true);
        }

        [MenuItem("TalusKit/Manuel Build/iOS Release", priority = 11001)]
        public static void IOSRelease()
        {
            PrepareIOSBuild(false);
        }

        private static void PrepareIOSBuild(bool isDevelopment)
        {
            EditorUserBuildSettings.development = isDevelopment;

            if (!Application.isBatchMode && EditorUserBuildSettings.activeBuildTarget != BuildTarget.iOS)
            {
                Debug.LogError("[TalusCI-Package] Build Target must be iOS!");
                return;
            }

            bool isBatchMode = Application.isBatchMode;

            string apiUrl = (isBatchMode)
                ? CommandLineParser.GetArgument("-apiUrl")
                : EditorPrefs.GetString(BackendDefinitions.BackendApiUrlPref);

            string apiToken = (isBatchMode)
                ? CommandLineParser.GetArgument("-apiKey")
                : EditorPrefs.GetString(BackendDefinitions.BackendApiTokenPref);

            string appId = (isBatchMode)
                ? CommandLineParser.GetArgument("-appId")
                : EditorPrefs.GetString(BackendDefinitions.BackendAppIdPref);

            // create build when backend data fetched
            BackendApi api = new BackendApi(apiUrl, apiToken);
            api.GetAppInfo(appId, CreateBuild);
        }

        private static void CreateBuild(AppModel app)
        {
            Debug.Log("[TalusCI-Package] Define Symbols: " + PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS));

            UpdateProductSettings(app);

            BuildPipeline.BuildPlayer(GetScenes(), iOSAppBuildInfo.IOSFolder, BuildTarget.iOS, BuildOptions.CompressWithLz4HC);

            Debug.Log($"[TalusCI-Package] Build succceed! Path: {iOSAppBuildInfo.IOSFolder}");

            if (Application.isBatchMode)
            {
                EditorApplication.Exit(0);
            }
        }

        private static void UpdateProductSettings(AppModel app)
        {
            PlayerSettings.SplashScreen.showUnityLogo = false;
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, app.app_bundle);
            PlayerSettings.productName = app.app_name;
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static string[] GetScenes()
        {
            return (from t in EditorBuildSettings.scenes where t.enabled select t.path).ToArray();
        }
    }
}
