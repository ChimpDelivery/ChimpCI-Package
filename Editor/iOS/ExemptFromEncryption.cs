#if UNITY_IOS
using System.IO;

using UnityEditor;
using UnityEditor.Callbacks;

using UnityEditor.iOS.Xcode;

namespace TalusCI.Editor.iOS
{
    public class ExemptFromEncryption : UnityEditor.Editor
    {
        [PostProcessBuild(9999)]
        public static void OnPostProcessBuild(BuildTarget buildTarget, string pathToBuild)
        {
            if (buildTarget != BuildTarget.iOS && buildTarget != BuildTarget.tvOS) { return; }

            string plistPath = pathToBuild + "/Info.plist";

            var plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));

            PlistElementDict rootDict = plist.root;
            rootDict.SetString("ITSAppUsesNonExemptEncryption", "false");

            File.WriteAllText(plistPath, plist.WriteToString());
        }
    }
}
#endif