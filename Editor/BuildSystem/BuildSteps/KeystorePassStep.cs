using UnityEditor;

using UnityEngine;

using ChimpBackendData.Editor.Utility;

namespace ChimpCI.Editor.BuildSystem.BuildSteps
{
    [CreateAssetMenu(menuName = "ChimpDelivery/Build Steps/Key Store Pass Step")]
    public class KeystorePassStep : BuildStep
    {
        public override void Execute()
        {
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android || !Application.isBatchMode) { return; }

            PlayerSettings.keyaliasPass = CommandLineParser.GetArgument("-keyStorePass");
            PlayerSettings.keystorePass = CommandLineParser.GetArgument("-keyStorePass");

            Debug.Log("[ChimpCI-Package] Android Key Store Pass step completed!");
        }
    }
}