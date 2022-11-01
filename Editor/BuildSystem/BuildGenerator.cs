using System.Collections.Generic;

using UnityEngine;

namespace TalusCI.Editor.BuildSystem
{
    [CreateAssetMenu(menuName = "_OTHERS/Build/Build Generator")]
    public sealed class BuildGenerator : ScriptableObject
    {
        [Header("Build Steps")]
        public List<BuildStep> Steps;

        public void Run()
        {
            foreach (BuildStep step in Steps)
            {
                step.Execute();
            }
        }
    }
}