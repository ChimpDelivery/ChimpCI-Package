using UnityEngine;

using TalusBackendData.Editor;

namespace TalusCI.Editor.BuildSystem
{
    [CreateAssetMenu(menuName = "_OTHERS/Build/Build Steps/Sync Project Keys Step")]
    public class SyncProjectKeysStep : BuildStep
    {
        public override void Execute()
        {
            PreProcessProjectSettings.Sync();
        }
    }
}
