using System.IO;
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

        private string GetBuildPath()
        {
            // ios expects folder
            // android expect file

            switch (SwitchStep.TargetPlatform)
            {
                case BuildTarget.iOS:
                return Path.Join(BackendSettingsHolder.instance.ArtifactFolder, "UnityBuild");

                case BuildTarget.Android:
                return Path.Combine(
                    Path.Join(BackendSettingsHolder.instance.ArtifactFolder, "UnityBuild"),
                    (EditorUserBuildSettings.buildAppBundle) ? "Build.aab" : "Build.apk"
               );
            }

            return BackendSettingsHolder.instance.ArtifactFolder + "/UnityBuild";
        }

        public override void Execute()
        {
            Debug.Log("[TalusCI-Package] Create Build Step | Define Symbols");
            Debug.Log(PlayerSettings.GetScriptingDefineSymbolsForGroup(SwitchStep.TargetGroup));

            Debug.Log($"[TalusCI-Package] Create Build Step | Build path: {GetBuildPath()}");

            BuildReport report = BuildPipeline.BuildPlayer(
                _Scenes,
                GetBuildPath(),
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