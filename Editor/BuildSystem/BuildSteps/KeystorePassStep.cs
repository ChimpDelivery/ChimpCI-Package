using UnityEditor;

using UnityEngine;

using TalusBackendData.Editor.Utility;

namespace TalusCI.Editor.BuildSystem
{
    [CreateAssetMenu(menuName = "_OTHERS/Build/Build Steps/Key Store Pass Step")]
    public class KeystorePassStep : BuildStep
    {
        public override void Execute()
        {
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android || !Application.isBatchMode) { return; }

            PlayerSettings.keyaliasPass = CommandLineParser.GetArgument("-keyStorePass");
            PlayerSettings.keystorePass = CommandLineParser.GetArgument("-keyStorePass");
        }
    }
}