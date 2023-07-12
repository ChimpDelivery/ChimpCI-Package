using UnityEngine;

namespace ChimpCI.Editor.BuildSystem.BuildSteps
{
    public abstract class BuildStep : ScriptableObject
    {
        public abstract void Execute();
    }
}