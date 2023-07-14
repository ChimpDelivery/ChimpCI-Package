using System.IO;
using System.Linq;

using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;

using ChimpBackendData.Editor;
using ChimpBackendData.Editor.Utility;

namespace ChimpCI.Editor.BuildSystem.BuildSteps
{
    [CreateAssetMenu(menuName = "ChimpDelivery/Build Steps/Create Build Step")]
    public class CreateBuildStep : BuildStep
    {
        public BuildConfigs BuildConfigs;

        public SwitchBuildTargetStep SwitchStep;

        private static string[] _Scenes => (
            from scene in EditorBuildSettings.scenes
            where scene.enabled
            select scene.path
        ).ToArray();

        // ios expects folder, android expects file
        private string GetBuildPath()
        {
            return SwitchStep.TargetPlatform switch
            {
                BuildTarget.iOS => Path.Join(BackendSettingsHolder.instance.ArtifactFolder, "UnityBuild"),
                BuildTarget.Android => Path.Combine(
                    Path.Join(BackendSettingsHolder.instance.ArtifactFolder, "UnityBuild"),
                    (EditorUserBuildSettings.buildAppBundle) ? "Build.aab" : "Build.apk"
                ),
                _ => BackendSettingsHolder.instance.ArtifactFolder + "/UnityBuild",
            };
        }

        public override void Execute()
        {
            Debug.Log("[ChimpCI-Package] Create Build Step | Define Symbols");
            Debug.Log(PlayerSettings.GetScriptingDefineSymbolsForGroup(SwitchStep.TargetGroup));

            Debug.Log($"[ChimpCI-Package] Create Build Step | Build path: {GetBuildPath()}");

            BuildReport report = BuildPipeline.BuildPlayer(
                _Scenes,
                GetBuildPath(),
                SwitchStep.TargetPlatform,
                BuildConfigs.Options
            );

            Debug.Log(@$"[ChimpCI-Package] Create Build Step |
                Build status: {report.summary.result} |
                Output path: {report.summary.outputPath} |
                Total errors: {report.summary.totalErrors} |
                Total warnings: {report.summary.totalWarnings}"
            );

            BatchMode.Close(report.summary.result == BuildResult.Succeeded ? 0 : -1);
        }
    }
}