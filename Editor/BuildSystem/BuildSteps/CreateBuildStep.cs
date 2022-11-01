using System.Linq;

using UnityEditor;
using UnityEditor.Build.Reporting;

using UnityEngine;

namespace TalusCI.Editor.BuildSystem
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
            Debug.Log("[TalusCI-Package] Define Symbols");
            Debug.Log(PlayerSettings.GetScriptingDefineSymbolsForGroup(SwitchStep.TargetGroup));
            
            Debug.Log($"[TalusCI-Package] Build path: {BuildConfigs.BuildPath}");

            BuildReport report = BuildPipeline.BuildPlayer(_Scenes, BuildConfigs.BuildPath, SwitchStep.TargetPlatform, BuildConfigs.Options);
            Debug.Log($"[TalusCI-Package] Build status: {report.summary.result}");
            Debug.Log($"[TalusCI-Package] Output path: {report.summary.outputPath}");

            if (Application.isBatchMode)
            {
                EditorApplication.Exit(report.summary.result == BuildResult.Succeeded ? 0 : -1);
            }
        }
    }
}