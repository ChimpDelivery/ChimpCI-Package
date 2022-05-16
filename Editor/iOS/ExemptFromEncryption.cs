using System.IO;

using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

namespace TalusCI.Editor.iOS
{
    internal class ExemptFromEncryption
    {
        private readonly static string _EncryptionKey = "ITSAppUsesNonExemptEncryption";
        private readonly static string _EncryptionValue = "false";

        [PostProcessBuild]
        public static void OnPostProcessBuild(BuildTarget buildTarget, string pathToBuild)
        {
            if (buildTarget != BuildTarget.iOS && buildTarget != BuildTarget.tvOS) { return; }

            string plistPath = Path.Combine(pathToBuild, "Info.plist");
            Debug.Log("[Unity-CI-Package] Info.plist path: " + plistPath);

            var plist = new PlistDocument();
            plist.ReadFromFile(plistPath);

            PlistElementDict root = plist.root;
            root.SetString(_EncryptionKey, _EncryptionValue);

            File.WriteAllText(plistPath, plist.WriteToString());
        }
    }
}