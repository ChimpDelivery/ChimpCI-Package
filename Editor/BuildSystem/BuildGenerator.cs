using System.Collections.Generic;

using UnityEngine;

using TalusBackendData.Editor;

using TalusCI.Editor.BuildSystem.BuildSteps;

namespace TalusCI.Editor.BuildSystem
{
    [CreateAssetMenu(menuName = "_OTHERS/Build/Build Generator")]
    public sealed class BuildGenerator : ScriptableObject
    {
        [Header("Build Steps")]
        public List<BuildStep> Steps;
        
        public void Run()
        {
            Debug.Log($"[TalusCI-Package] Build Generator: {name} gonna work in {Steps.Count} step(s)!");
            
            // sync project keys before run
            var initializer = new PreProcessProjectSettings();
            initializer.UpdateSettings(() =>
            {
                foreach (BuildStep step in Steps)
                {
                    Debug.Log($"[TalusCI-Package] Build Generator: Step - {step.name} is executing!");
                    step.Execute();
                }
            });
        }
    }
}