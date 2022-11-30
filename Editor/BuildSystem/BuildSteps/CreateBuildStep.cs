using System.Linq;

using UnityEditor;
using UnityEditor.Build.Reporting;

using UnityEngine;

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
            
            string buildPath = System.IO.Directory.GetCurrentDirectory() + "/" + BuildConfigs.BuildPath;
            Debug.Log($"[TalusCI-Package] Create Build Step | Build path: {buildPath}");

            BuildReport report = BuildPipeline.BuildPlayer(_Scenes, buildPath, SwitchStep.TargetPlatform, BuildConfigs.Options);
            Debug.Log($"[TalusCI-Package] Create Build Step | Build status: {report.summary.result}");
            Debug.Log($"[TalusCI-Package] Create Build Step | Output path: {report.summary.outputPath}");
            Debug.Log($"[TalusCI-Package] Create Build Step | Total Errors: {report.summary.totalErrors}");
            Debug.Log($"[TalusCI-Package] Create Build Step | Total Warnings: {report.summary.totalWarnings}");

            if (Application.isBatchMode)
            {
                EditorApplication.Exit(report.summary.result == BuildResult.Succeeded ? 0 : -1);
            }
        }
    }
}