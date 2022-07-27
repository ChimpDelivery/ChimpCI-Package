using System.IO;
using System.Linq;

using UnityEngine;

using UnityEditor;
using UnityEditor.Build.Reporting;

using TalusBackendData.Editor.Models;
using TalusBackendData.Editor;

using TalusCI.Editor.Utility;

namespace TalusCI.Editor
{
    public class BuildCreator
    {
        public string[] Scenes => (from t in EditorBuildSettings.scenes select t.path).ToArray();

        public bool IsDevBuild;

        public BuildTarget TargetPlatform;
        public BuildTargetGroup TargetGroup;
        public BuildOptions Options = BuildOptions.CompressWithLz4HC;

        public void PrepareBuild()
        {
            EditorUserBuildSettings.development = IsDevBuild;

            bool isBatchMode = Application.isBatchMode;

            if (!isBatchMode && !(EditorUserBuildSettings.activeBuildTarget != BuildTarget.iOS ||
                EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android))
            {
                Debug.LogError("[TalusCI-Package] Build Target must be iOS/Android! Switch platform (File/Build Settings)");
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

        private string GetBuildPath()
        {
            // ios expects folder
            // android expect file

            switch (TargetPlatform)
            {
                case BuildTarget.iOS:
                    return Path.Combine(CISettingsHolder.ProjectFolder, CISettingsHolder.instance.BuildFolder);
                case BuildTarget.Android:
                    return Path.Combine(CISettingsHolder.ProjectFolder, Path.GetFileName(CISettingsHolder.instance.BuildFolder));
            }

            return CISettingsHolder.ProjectFolder + "/Builds";
        }

        private void CreateBuild(AppModel app)
        {
            Debug.Log($"[TalusCI-Package] Define Symbols: {PlayerSettings.GetScriptingDefineSymbolsForGroup(TargetGroup)}");

            UpdateProductSettings(app);

            Debug.Log($"[TalusCI-Package] Build path: {GetBuildPath()}");

            BuildReport report = BuildPipeline.BuildPlayer(Scenes, GetBuildPath(), TargetPlatform, Options);

            Debug.Log($"[TalusCI-Package] Build status: {report.summary.result}");
            Debug.Log($"[TalusCI-Package] Output path: {report.summary.outputPath}");

            // batch mode clean-up
            if (!Application.isBatchMode)
            {
                return;
            }

            EditorApplication.Exit(report.summary.result == BuildResult.Succeeded ? 0 : -1);
        }

        private void UpdateProductSettings(AppModel app)
        {
            PlayerSettings.SplashScreen.showUnityLogo = false;
            PlayerSettings.SetScriptingBackend(TargetGroup, ScriptingImplementation.IL2CPP);

            if (app != null)
            {
                PlayerSettings.SetApplicationIdentifier(TargetGroup, app.app_bundle);
                PlayerSettings.productName = app.app_name;
            }
            else
            {
                Debug.LogError($"[TalusCI-Package] AppModel data is null! Product Settings couldn't updated...");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}