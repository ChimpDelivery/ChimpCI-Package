using UnityEngine;

namespace TalusCI.Editor.BuildSystem.BuildSteps
{
    public abstract class BuildStep : ScriptableObject
    {
#if ODIN_INSPECTOR_3_1
        [Sirenix.OdinInspector.Button]
#endif
        public abstract void Execute();
    }
}