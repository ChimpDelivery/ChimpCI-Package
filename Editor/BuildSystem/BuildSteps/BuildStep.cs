using UnityEngine;

namespace TalusCI.Editor.BuildSystem
{
    public abstract class BuildStep : ScriptableObject
    {
        public abstract void Execute();
    }
}