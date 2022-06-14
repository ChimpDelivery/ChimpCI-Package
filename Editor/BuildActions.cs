using System.IO;
using System.Linq;

using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

using TalusCI.Editor.Utility;

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

            bool isBatchMode = Application.isBatchMode;

            if (!isBatchMode && EditorUserBuildSettings.activeBuildTarget != BuildTarget.iOS)
            {
                Debug.LogError("[TalusCI-Package] Build Target must be iOS! Switch platform to iOS (File/Build Settings)");
                return;
            }

            string apiUrl = (isBatchMode)
                ? CommandLineParser.GetArgument("-apiUrl")
                : BackendSettingsHolder.instance.ApiUrl;

            string apiToken = (isBatchMode)
                ? CommandLineParser.GetArgument("-apiKey")
                : BackendSettingsHolder.instance.ApiToken;

            string appId = (isBatchMode)
                ? CommandLineParser.GetArgument("-appId")
                : BackendSettingsHolder.instance.AppId;

            // create build when backend data fetched
            BackendApi api = new BackendApi(apiUrl, apiToken);
            api.GetAppInfo(appId, CreateBuild);
        }

        private static void CreateBuild(AppModel app)
        {
            Debug.Log($"[TalusCI-Package] Define Symbols: {PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS)}");

            UpdateProductSettings(app);

            string buildPath = Path.Combine(CISettingsHolder.ProjectFolder, CISettingsHolder.instance.BuildFolder);
            Debug.Log($"[TalusCI-Package] Build path: {buildPath}");

            BuildReport report = BuildPipeline.BuildPlayer(
                GetScenes(),
                buildPath,
                BuildTarget.iOS,
                BuildOptions.CompressWithLz4HC
            );

            Debug.Log($"[TalusCI-Package] Build status: {report.summary.result}");
            Debug.Log($"[TalusCI-Package] Output path: {report.summary.outputPath}");

            // batch mode clean-up
            if (!Application.isBatchMode)
            {
                return;
            }

            EditorApplication.Exit(report.summary.result == BuildResult.Succeeded ? 0 : -1);
        }

        private static void UpdateProductSettings(AppModel app)
        {
            PlayerSettings.SplashScreen.showUnityLogo = false;
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);

            if (app != null)
            {
                PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, app.app_bundle);
                PlayerSettings.productName = app.app_name;
            }
            else
            {
                Debug.LogError($"[TalusCI-Package] AppModel data is null! Product Settings couldn't updated...");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static string[] GetScenes() => (from t in EditorBuildSettings.scenes select t.path).ToArray();
    }
}
