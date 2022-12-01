using System;
using System.Collections.Generic;
using System.IO;

using TalusBackendData.Editor;

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

        private void OnEnable()
        {
            hideFlags &= ~HideFlags.NotEditable;
        }

        public void Run()
        {
            Debug.Log($"[TalusCI-Package] Build Generator: {name} gonna work in {Steps.Count} step(s)!");

            try
            {
                string buildPath = BackendApiConfigs.GetInstance().ArtifactFolder + "/UnityBuild";
                if (!Directory.Exists(buildPath))
                {
                    Directory.CreateDirectory(buildPath);
                }

                Debug.Log($"[TalusCI-Package] Build Generator | Build Path: {buildPath}");

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