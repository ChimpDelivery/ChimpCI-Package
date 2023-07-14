using System.IO;

using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

namespace ChimpCI.Editor.iOS
{
    internal class PlistPostProcessor
    {
        private const string _EncryptionKey = "ITSAppUsesNonExemptEncryption";
        private const string _EncryptionValue = "false";

#if SUPPORT_GA
        private const string _UserTrackingKey = "NSUserTrackingUsageDescription";
        private const string _UserTrackingValue = "Your data will be used for analytical purposes.";
#endif

        [PostProcessBuild]
        public static void OnPostProcessBuild(BuildTarget buildTarget, string pathToBuild)
        {
            if (buildTarget != BuildTarget.iOS && buildTarget != BuildTarget.tvOS) { return; }

            string plistPath = Path.Combine(pathToBuild, "Info.plist");
            Debug.Log($"[ChimpCI-Package] Info.plist path: {plistPath}");

            var plist = new PlistDocument();
            plist.ReadFromFile(plistPath);

            PlistElementDict root = plist.root;
            root.SetString(_EncryptionKey, _EncryptionValue);
#if SUPPORT_GA
            root.SetString(_UserTrackingKey, _UserTrackingValue);
#endif
            File.WriteAllText(plistPath, plist.WriteToString());
        }
    }
}