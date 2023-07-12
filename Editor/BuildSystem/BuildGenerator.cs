using System;
using System.Collections.Generic;

using UnityEngine;

using ChimpBackendData.Editor.Utility;

using ChimpCI.Editor.BuildSystem.BuildSteps;

namespace ChimpCI.Editor.BuildSystem
{
    [CreateAssetMenu(menuName = "ChimpDelivery/Build Generator")]
    public sealed class BuildGenerator : ScriptableObject
    {
        [Header("Build Steps")]
        public List<BuildStep> Steps;

        public void Run()
        {
            Debug.Log($"[ChimpCI-Package] Build Generator: {name} running...");

            try
            {
                foreach (BuildStep step in Steps)
                {
                    Debug.Log($"[ChimpCI-Package] Build Generator: Step - {step.name} is executing!");
                    step.Execute();
                }
            }
            catch (Exception exception)
            {
                Debug.LogError($"[ChimpCI-Package] Build Generator: {name} Exception!: {exception.Message}");
                BatchMode.Close(-1);
            }
        }
    }
}