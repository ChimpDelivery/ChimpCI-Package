using System.Linq;

using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;

using TalusBackendData.Editor;
using TalusBackendData.Editor.Utility;

namespace TalusCI.Editor.BuildSystem.BuildSteps
{
    [CreateAssetMenu(menuName = "_OTHERS/Build/Build Steps/Create Build Step")]
    public class CreateBuildStep : BuildStep
    {
        public BuildConfigs BuildConfigs;

        public SwitchBuildTargetStep SwitchStep;

        private static string[] _Scenes => (from scene in EditorBuildSettings.scenes
                                            where scene.enabled
                                            select scene.path).ToArray();

        public override void Execute()
        {
            Debug.Log("[TalusCI-Package] Create Build Step | Define Symbols");
            Debug.Log(PlayerSettings.GetScriptingDefineSymbolsForGroup(SwitchStep.TargetGroup));

            string buildPath = BackendSettingsHolder.instance.ArtifactFolder + "/UnityBuild";
            Debug.Log($"[TalusCI-Package] Create Build Step | Build path: {buildPath}");

            BuildReport report = BuildPipeline.BuildPlayer(
                _Scenes,
                buildPath,
                SwitchStep.TargetPlatform,
                BuildConfigs.Options
            );

            Debug.Log(@$"[TalusCI-Package] Create Build Step | 
                Build status: {report.summary.result} | 
                Output path: {report.summary.outputPath} | 
                Total errors: {report.summary.totalErrors} | 
                Total warnings: {report.summary.totalWarnings}");

            BatchMode.Close(report.summary.result == BuildResult.Succeeded ? 0 : -1);
        }
    }
}