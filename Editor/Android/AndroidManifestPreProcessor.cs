using System.IO;

using UnityEditor.Build;
using UnityEditor.Build.Reporting;

using UnityEngine;

namespace ChimpCI.Editor.Android
{
    public class AndroidManifestPreProcessor : IPreprocessBuildWithReport
    {
        public string ManifestFilePath => "Assets/Plugins/Android/AndroidManifest.xml";

        public int callbackOrder => 1;

        public void OnPreprocessBuild(BuildReport report)
        {
            bool isManifestExist = File.Exists(ManifestFilePath);
            Debug.Log($"[ChimpCI-Package] AndroidManifest file exists: {isManifestExist}");
            if (!isManifestExist) { return; }

            var androidManifest = new AndroidManifest(ManifestFilePath);
            androidManifest.SetApplicationAttribute(
                "debuggable",
                UnityEditor.EditorUserBuildSettings.development ? "true" : "false"
            );
            androidManifest.Save();
        }
    }
}

