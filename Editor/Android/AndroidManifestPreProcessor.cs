using UnityEditor.Build;
using UnityEditor.Build.Reporting;

using UnityEngine;

namespace TalusCI.Android
{
    public class AndroidManifestPreProcessor : IPreprocessBuildWithReport
    {
        public int callbackOrder => 1;

        public void OnPreprocessBuild(BuildReport report)
        {
            Debug.Log($"[TalusCI-Package] AndroidManifest Pre Processing...");

            AndroidManifest androidManifest = new AndroidManifest("Assets/Plugins/Android/AndroidManifest.xml");

            androidManifest.SetApplicationAttribute("debuggable", UnityEditor.EditorUserBuildSettings.development ? "true" : "false");
            androidManifest.Save();
        }
    }
}

