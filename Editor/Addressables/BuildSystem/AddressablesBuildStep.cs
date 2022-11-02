using UnityEditor;

using UnityEngine;

using TalusCI.Editor.BuildSystem.BuildSteps;

namespace TalusCI.Editor.Addressables.BuildSystem
{
    [CreateAssetMenu(menuName = "_OTHERS/Build/Build Steps/Addressable Step")]
    public class AddressablesBuildStep : BuildStep
    {
        [SerializeField]
        private string _BuildScript = "Assets/AddressableAssetsData/DataBuilders/BuildScriptPackedMode.asset";

        [SerializeField]
        private string _SettingsAsset = "Assets/AddressableAssetsData/AddressableAssetSettings.asset";

        [SerializeField]
        private string _ProfileName = "Default";
        
        public override void Execute()
        {
            var builder = new AddressablesContentBuilder(_BuildScript, _SettingsAsset, _ProfileName);

            if (builder.BuildAddressables())
            {
                Debug.Log("[TalusCI-Package] Addressable content built successfully!");
            }
            else
            {
                if (Application.isBatchMode)
                {
                    EditorApplication.Exit(-1);
                }
            }
        }
    }
}