using System;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

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

            try
            {
                foreach (BuildStep step in Steps)
                {
                    Debug.Log($"[TalusCI-Package] Build Generator: Step - {step.name} is executing!");
                    step.Execute();
                }
            }
            catch (Exception exception)
            {
                Debug.Log($"[TalusCI-Package] Build Generator exception!: {exception.Message}");

                if (Application.isBatchMode)
                {
                    EditorApplication.Exit(-1);
                }
            }
        }
    }
}