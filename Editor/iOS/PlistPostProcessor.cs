using System.IO;

using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

// ReSharper disable ClassNeverInstantiated.Global

namespace TalusCI.Editor.iOS
{
    internal class PlistPostProcessor
    {
        private const string _EncryptionKey = "ITSAppUsesNonExemptEncryption";
        private const string _EncryptionValue = "false";
        
#if TALUS_GA
        private const string _UserTrackingKey = "NSUserTrackingUsageDescription";
#endif

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
#if TALUS_GA
            root.SetString(_UserTrackingKey, BuildSettingsHolder.instance.UserTrackingText);
#endif
            File.WriteAllText(plistPath, plist.WriteToString());
        }
    }
}