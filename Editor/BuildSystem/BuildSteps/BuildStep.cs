using UnityEngine;

namespace TalusCI.Editor.BuildSystem.BuildSteps
{
    public abstract class BuildStep : ScriptableObject
    {
        public abstract void Execute();
    }
}