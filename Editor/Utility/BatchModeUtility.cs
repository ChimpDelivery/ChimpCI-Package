using UnityEditor.Build.Reporting;
using UnityEditor;

using UnityEngine;

namespace TalusCI.Editor.Utility
{
    public static class BatchModeUtility
    {
        public static void Exit(BuildResult result)
        {
            if (Application.isBatchMode)
            {
                EditorApplication.Exit(result == BuildResult.Succeeded ? 0 : -1);
            }
        }
    }
}
