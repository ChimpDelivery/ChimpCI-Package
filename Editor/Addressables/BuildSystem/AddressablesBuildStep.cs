using UnityEditor;

using UnityEngine;

using TalusCI.Editor.BuildSystem;

namespace TalusCI.Editor.Addressables.BuildSystem
{
    [CreateAssetMenu(menuName = "_OTHERS/Build/Build Steps/Addressable Step")]
    public class AddressablesBuildStep : BuildStep
    {
        public override void Execute()
        {
            var builder = new AddressablesContentBuilder();

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