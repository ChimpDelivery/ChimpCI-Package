using System.IO;
using System.Linq;

using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

using TalusBackendData.Editor;
using TalusBackendData.Editor.Models;
using TalusCI.Editor.Utility;

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

            if (!isBatchMode)
            {
                CreateBuild();
                return;
            }

            // create build when backend data fetched
            BackendApi api = new BackendApi(
                CommandLineParser.GetArgument("-apiUrl"),
                CommandLineParser.GetArgument("-apiKey")
            );
            api.GetAppInfo(CommandLineParser.GetArgument("-appId"), CreateBuild);
        }

        private static void CreateBuild(AppModel app = null)
        {
            Debug.Log($"[TalusCI-Package] Define Symbols: {PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS)}");

            UpdateProductSettings(app);

            string buildPath = Path.Combine(CISettingsHolder.ProjectFolder, CISettingsHolder.instance.BuildFolder);
            Debug.Log($"[TalusCI-Package] Build path: {buildPath}");
            BuildReport report = BuildPipeline.BuildPlayer(
                GetActiveScenes(),
                buildPath,
                BuildTarget.iOS,
                BuildOptions.CompressWithLz4HC
            );

            Debug.Log($"[TalusCI-Package] Build status: {report.summary.result}");
            Debug.Log($"[TalusCI-Package] Output path: {report.summary.outputPath}");

            if (Application.isBatchMode)
            {
                EditorApplication.Exit(report.summary.result == BuildResult.Succeeded ? 0 : -1);
            }
        }

        private static void UpdateProductSettings(AppModel app = null)
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

        private static string[] GetActiveScenes()
        {
            return (from t in EditorBuildSettings.scenes where t.enabled select t.path).ToArray();
        }
    }
}
