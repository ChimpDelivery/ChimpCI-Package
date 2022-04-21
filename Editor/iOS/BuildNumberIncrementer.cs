using System;

using UnityEditor;
using UnityEditor.Callbacks;

namespace TalusCI.Editor.iOS
{
    /// <summary>
    /// Actually there is no need to this class, there is a step on Jenkins Build that sets the version of the app.
    /// Maybe convenient for manuel building?
    /// </summary>
    public class IncrementBuildNumber : UnityEditor.Editor
    {
        [PostProcessBuild]
        public static void OnPostProcessBuild(BuildTarget buildTarget, string pathToBuild)
        {
            if (buildTarget != BuildTarget.iOS && buildTarget != BuildTarget.tvOS) { return; }

            if (int.TryParse(PlayerSettings.iOS.buildNumber, out int currentBuildNumber))
            {
                string nextBuildNumber = (currentBuildNumber + 1).ToString();
                PlayerSettings.iOS.buildNumber = nextBuildNumber;

                Console.WriteLine("Setting new iOS build number to " + nextBuildNumber);
            }
            else
            {
                Console.WriteLine("Failed to parse build number " + PlayerSettings.iOS.buildNumber + " as int.");
            }
        }
    }
}