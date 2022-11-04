using System.IO;

using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

namespace TalusCI.Editor.iOS
{
    internal class ExemptFromEncryption
    {
        private const string _EncryptionKey = "ITSAppUsesNonExemptEncryption";
        private const string _EncryptionValue = "false";
        
        private const string _UserTrackingKey = "NSUserTrackingUsageDescription";
        private const string _UserTrackingValue = "Your data will be used for analytical purposes.";

        [PostProcessBuild]
        public static void OnPostProcessBuild(BuildTarget buildTarget, string pathToBuild)
        {
            if (buildTarget != BuildTarget.iOS && buildTarget != BuildTarget.tvOS) { return; }

            string plistPath = Path.Combine(pathToBuild, "Info.plist");
            Debug.Log($"[TalusCI-Package] Info.plist path: {plistPath}");

            var plist = new PlistDocument();
            plist.ReadFromFile(plistPath);

            PlistElementDict root = plist.root;
            root.SetString(_EncryptionKey, _EncryptionValue);
            root.SetString(_UserTrackingKey, _UserTrackingValue);

            File.WriteAllText(plistPath, plist.WriteToString());
        }
    }
}