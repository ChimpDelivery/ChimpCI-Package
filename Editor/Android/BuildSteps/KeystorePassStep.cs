using UnityEditor;

using UnityEngine;

using TalusCI.Editor.BuildSystem;
using TalusBackendData.Editor.Utility;

namespace TalusCI.Editor.Android
{
    [CreateAssetMenu]
    public class KeystorePassStep : BuildStep
    {
        public override void Execute()
        {
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android || !Application.isBatchMode) return;

            PlayerSettings.keyaliasPass = CommandLineParser.GetArgument("-keyStorePass");
            PlayerSettings.keystorePass = CommandLineParser.GetArgument("-keyStorePass");
        }
    }
}