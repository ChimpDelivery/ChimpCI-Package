using System.IO;

using UnityEditor.Build;
using UnityEditor.Build.Reporting;

using UnityEngine;

namespace TalusCI.Editor.Android
{
    public class AndroidManifestPreProcessor : IPreprocessBuildWithReport
    {
        public int callbackOrder => 1;

        public void OnPreprocessBuild(BuildReport report)
        {
            bool isManifestExist = File.Exists(BuildSettingsHolder.instance.ManifestFilePath);
            Debug.Log($"[TalusCI-Package] AndroidManifest file exists: {isManifestExist}");
            if (!isManifestExist) { return; }

            var androidManifest = new AndroidManifest(BuildSettingsHolder.instance.ManifestFilePath);
            androidManifest.SetApplicationAttribute(
                "debuggable", 
                UnityEditor.EditorUserBuildSettings.development 
                    ? "true" 
                    : "false"
            );
            androidManifest.Save();
        }
    }
}

